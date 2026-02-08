using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using LabEG.NeedleCrud.Models.Entities;
using LabEG.NeedleCrud.Models.Exceptions;
using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace LabEG.NeedleCrud.Repositories;

public class CrudDbRepository<TDbContext, TEntity, TId> : ICrudDbRepository<TDbContext, TEntity, TId>
    where TDbContext : DbContext
    where TEntity : class, IEntity<TId>, new()
{
    protected TDbContext DBContext { get; }

    public CrudDbRepository(TDbContext dbContext)
    {
        DBContext = dbContext;
    }

    public virtual async Task<TEntity> Create(TEntity entity)
    {
        entity.Id = default;
        await DBContext.Set<TEntity>().AddAsync(entity);

        return entity;
    }

    public virtual async Task<TEntity> GetById(TId id)
    {
        TEntity resultEntity = await DBContext
            .Set<TEntity>()
            .FirstOrDefaultAsync((entity) => entity.Id!.Equals(id)) ??
                throw new ObjectNotFoundNeedleCrudException($"{typeof(TEntity).Name} with ID '{id}' not found");

        return resultEntity;
    }

    public virtual async Task<IList<TEntity>> GetAll()
    {
        return await DBContext.Set<TEntity>().ToListAsync();
    }

    public virtual async Task Update(TId id, TEntity entity)
    {
        bool exists = await DBContext.Set<TEntity>().AnyAsync(e => e.Id!.Equals(id));
        if (!exists)
        {
            throw new ObjectNotFoundNeedleCrudException($"{typeof(TEntity).Name} with ID '{id}' not found");
        }

        entity.Id = id;
        DBContext.Set<TEntity>().Update(entity);
    }

    public virtual async Task Delete(TId id)
    {
        TEntity entity = await GetById(id);
        DBContext.Set<TEntity>().Remove(entity);
    }

    public virtual async Task<PagedList<TEntity>> GetPaged(PagedListQuery query, IQueryable<TEntity>? qData = null)
    {
        PagedList<TEntity> resultEntity = new();
        resultEntity.PageMeta.PageNumber = query.PageNumber;
        resultEntity.PageMeta.PageSize = query.PageSize;

        IQueryable<TEntity> queryableData = qData ?? DBContext.Set<TEntity>();

        if (query.Graph is not null)
        {
            IList<string> listOfProps = ExtractIncludes(query.Graph);
            foreach (string prop in listOfProps)
            {
                queryableData = queryableData.Include(prop);
            }
        }

        queryableData = AddFilter(queryableData, query.Filter);
        queryableData = AddSort(queryableData, query.Sort);

        // count total elements
        int countPage = await queryableData.CountAsync();
        resultEntity.PageMeta.TotalElements = countPage;

        // count total pages
        int extraCount = countPage % query.PageSize > 0 ? 1 : 0;
        resultEntity.PageMeta.TotalPages = (countPage < query.PageSize) ? 1 : (countPage / query.PageSize) + extraCount;

        // get elements
        int countFrom = query.PageNumber == 1 ? 0 : (query.PageSize * query.PageNumber) - query.PageSize;
        resultEntity.Elements = await queryableData
            .Skip(countFrom)
            .Take(query.PageSize)
            .ToListAsync();

        resultEntity.PageMeta.ElementsInPage = resultEntity.Elements.LongCount();

        return resultEntity;
    }

    public virtual async Task<TEntity> GetGraph(TId id, JObject graph)
    {
        IQueryable<TEntity> graphQuery = DBContext.Set<TEntity>();

        IList<string> listOfProps = ExtractIncludes(graph);

        foreach (string prop in listOfProps)
        {
            graphQuery = graphQuery.Include(prop);
        }

        TEntity? resultEntity = await graphQuery.FirstOrDefaultAsync((entity) => entity.Id!.Equals(id)) ??
            throw new ObjectNotFoundNeedleCrudException($"{typeof(TEntity).Name} with ID '{id}' not found");

        return resultEntity;
    }

    protected IQueryable<TEntity> AddFilter(IQueryable<TEntity> queryableData, PagedListQueryFilter[] filters)
    {
        for (int i = 0; i < filters.Length; i++)
        {
            ref readonly PagedListQueryFilter filter = ref filters[i];

            ParameterExpression param = Expression.Parameter(typeof(TEntity), "TEntity");
            Expression? memberExpression = GetMemberExpression(filter.Property, param, typeof(TEntity));

            if (memberExpression is null)
            {
                continue;
            }

            Expression body = filter.Method switch
            {
                PagedListQueryFilterMethod.Less => Expression.LessThan(
                    memberExpression,
                    Expression.Constant(ToType(filter.Value, memberExpression.Type))
                ),
                PagedListQueryFilterMethod.LessOrEqual => Expression.LessThanOrEqual(
                    memberExpression,
                    Expression.Constant(ToType(filter.Value, memberExpression.Type))
                ),
                PagedListQueryFilterMethod.Equal => Expression.Equal(
                    memberExpression,
                    Expression.Constant(ToType(filter.Value, memberExpression.Type))
                ),
                PagedListQueryFilterMethod.GreatOrEqual => Expression.GreaterThanOrEqual(
                    memberExpression,
                    Expression.Constant(ToType(filter.Value, memberExpression.Type))
                ),
                PagedListQueryFilterMethod.Great => Expression.GreaterThan(
                    memberExpression,
                    Expression.Constant(ToType(filter.Value, memberExpression.Type))
                ),
                PagedListQueryFilterMethod.Like => Expression.Call(
                    memberExpression,
                    typeof(string).GetMethod("Contains", [typeof(string)])!,
                    Expression.Constant(ToType(filter.Value, memberExpression.Type))
                ),
                PagedListQueryFilterMethod.ILike => Expression.GreaterThanOrEqual(
                    Expression.Call(
                        memberExpression,
                        "IndexOf",
                        null,
                        Expression.Constant(filter.Value, typeof(string)),
                        Expression.Constant(StringComparison.InvariantCultureIgnoreCase)
                    ),
                    Expression.Constant(0)
                ),
                _ => throw new ArgumentOutOfRangeException(nameof(filter.Method), filter.Method, $"Unsupported filter method: {filter.Method}")
            };

            Expression<Func<TEntity, bool>> lambda = Expression.Lambda<Func<TEntity, bool>>(body, param);
            queryableData = queryableData.Where(lambda);
        }

        return queryableData;
    }

    protected IQueryable<TEntity> AddSort(IQueryable<TEntity> queryableData, PagedListQuerySort[] sorts)
    {
        int sortIndex = 0;
        for (int i = 0; i < sorts.Length; i++)
        {
            ref readonly PagedListQuerySort sort = ref sorts[i];

            ParameterExpression param = Expression.Parameter(typeof(TEntity), "TEntity");
            Expression? memberExpression = GetMemberExpression(sort.Property, param, typeof(TEntity));

            if (memberExpression is not MemberExpression)
            {
                continue;
            }

            LambdaExpression selector = Expression.Lambda(memberExpression, param);
            string methodName = sort.Direction == PagedListQuerySortDirection.Asc
                ? (sortIndex == 0 ? "OrderBy" : "ThenBy")
                : (sortIndex == 0 ? "OrderByDescending" : "ThenByDescending");

            MethodCallExpression call = Expression.Call(
                typeof(Queryable),
                methodName,
                [typeof(TEntity), selector.Body.Type],
                queryableData.Expression,
                selector
            );

            queryableData = (IQueryable<TEntity>)queryableData.Provider.CreateQuery(call);
            sortIndex++;
        }

        return queryableData;
    }

    protected IList<string> ExtractIncludes(JObject graph, IList<string>? listOfProps = null, string? previosProp = null)
    {
        listOfProps ??= [];

        foreach (KeyValuePair<string, JToken?> prop in graph)
        {
            if (prop.Value?.ToObject<object>() is object)
            {
                string deepProp = previosProp == null ? ToCamelCase(prop.Key) : (previosProp + "." + ToCamelCase(prop.Key));
                ExtractIncludes((JObject)prop.Value, listOfProps, deepProp);
            }
            else if (prop.Value?.ToObject<object>() == null)
            {
                listOfProps.Add((previosProp is string ? previosProp + "." : "") + ToCamelCase(prop.Key));
            } // else ignore
        }

        return listOfProps;
    }

    protected Expression? GetMemberExpression(string nestedProperty, ParameterExpression param, Type entityType)
    {
        // https://stackoverflow.com/questions/16208214/construct-lambdaexpression-for-nested-property-from-string

        Type elementType = entityType;
        Expression memberExpression = param;
        string[] members = nestedProperty.Split('.');

        for (int i = 0; i < members.Length; i++)
        {
            string propName = ToCamelCase(members[i]);
            PropertyInfo? sortableProperty = elementType.GetProperty(propName);

            if (sortableProperty is not null)
            {
                memberExpression = Expression.PropertyOrField(memberExpression, propName);
                elementType = sortableProperty.PropertyType;
            }
            else
            {
                return null;
            }
        }

        return memberExpression;
    }

    protected string ToCamelCase(string value)
    {
        return value.First().ToString().ToUpper() + string.Join("", value.Skip(1));
    }

    protected object? ToType(string value, Type type)
    {
        return TypeDescriptor.GetConverter(type).ConvertFromInvariantString(value);
    }
}