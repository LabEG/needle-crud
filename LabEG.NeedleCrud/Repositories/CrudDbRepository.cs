using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Nodes;
using LabEG.NeedleCrud.Models.Entities;
using LabEG.NeedleCrud.Models.Exceptions;
using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;
using Microsoft.EntityFrameworkCore;

namespace LabEG.NeedleCrud.Repositories;

/// <summary>
/// Repository implementation for database CRUD operations using Entity Framework
/// </summary>
/// <typeparam name="TDbContext">Type of the Entity Framework DbContext</typeparam>
/// <typeparam name="TEntity">Type of the entity</typeparam>
/// <typeparam name="TId">Type of the entity's primary key</typeparam>
public class CrudDbRepository<TDbContext, TEntity, TId> : ICrudDbRepository<TDbContext, TEntity, TId>
    where TDbContext : DbContext
    where TEntity : class, IEntity<TId>, new()
{
    private static readonly Dictionary<Type, TypeConverter> _converterCache = [];

    /// <summary>
    /// Gets the Entity Framework database context
    /// </summary>
    protected TDbContext DBContext { get; }

    /// <summary>
    /// Initializes a new instance of the repository
    /// </summary>
    /// <param name="dbContext">Entity Framework database context</param>
    public CrudDbRepository(TDbContext dbContext)
    {
        DBContext = dbContext;
    }

    /// <inheritdoc/>
    public virtual async Task<TEntity> Create(TEntity entity)
    {
        entity.Id = default;
        await DBContext.Set<TEntity>().AddAsync(entity);

        return entity;
    }

    /// <inheritdoc/>
    public virtual async Task<TEntity> GetById(TId id)
    {
        TEntity resultEntity = await DBContext
            .Set<TEntity>()
            .FirstOrDefaultAsync((entity) => entity.Id!.Equals(id)) ??
                throw new ObjectNotFoundNeedleCrudException($"{typeof(TEntity).Name} with ID '{id}' not found");

        return resultEntity;
    }

    /// <inheritdoc/>
    public virtual async Task<TEntity[]> GetAll()
    {
        return await DBContext.Set<TEntity>().ToArrayAsync();
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public virtual async Task Delete(TId id)
    {
        TEntity entity = await GetById(id);
        DBContext.Set<TEntity>().Remove(entity);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public virtual async Task<TEntity> GetGraph(TId id, JsonObject graph)
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

    /// <summary>
    /// Applies filter conditions to the queryable data
    /// </summary>
    /// <param name="queryableData">The queryable data to filter</param>
    /// <param name="filters">Array of filter conditions</param>
    /// <returns>Filtered queryable data</returns>
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

    /// <summary>
    /// Applies sorting to the queryable data
    /// </summary>
    /// <param name="queryableData">The queryable data to sort</param>
    /// <param name="sorts">Array of sort conditions</param>
    /// <returns>Sorted queryable data</returns>
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

    /// <summary>
    /// Extracts navigation property paths from the graph JSON object for eager loading
    /// </summary>
    /// <param name="graph">JSON object representing the graph structure</param>
    /// <param name="listOfProps">List of property paths (used for recursion)</param>
    /// <param name="previosProp">Previous property path (used for recursion)</param>
    /// <returns>List of navigation property paths</returns>
    protected IList<string> ExtractIncludes(JsonObject graph, List<string>? listOfProps = null, string? previosProp = null)
    {
        // Pre-allocate capacity to avoid multiple resizes (estimate: graph.Count * 2 for nested depth)
#pragma warning disable IDE0028 // Simplify collection initialization
        listOfProps ??= new List<string>(capacity: graph.Count * 2);
#pragma warning restore IDE0028 // Simplify collection initialization

        foreach (KeyValuePair<string, JsonNode?> prop in graph)
        {
            string camelKey = ToCamelCase(prop.Key);

            // Use pattern matching to check if Value is a JsonObject (nested graph)
            if (prop.Value is JsonObject nestedObject)
            {
                // Use string interpolation - faster than concatenation for 2-3 parts
                string deepProp = previosProp is not null ? $"{previosProp}.{camelKey}" : camelKey;
                ExtractIncludes(nestedObject, listOfProps, deepProp);
            }
            else if (prop.Value is null)
            {
                listOfProps.Add(previosProp is not null ? $"{previosProp}.{camelKey}" : camelKey);
            } // else ignore JsonValue nodes
        }

        return listOfProps;
    }

    /// <summary>
    /// Gets a member expression for a nested property path
    /// </summary>
    /// <param name="nestedProperty">Nested property path (e.g., "Author.Name")</param>
    /// <param name="param">Parameter expression</param>
    /// <param name="entityType">Entity type</param>
    /// <returns>Member expression or null if property not found</returns>
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

    /// <summary>
    /// Converts a string value to PascalCase
    /// </summary>
    /// <param name="value">String value to convert</param>
    /// <returns>PascalCase string</returns>
    protected string ToCamelCase(string value)
    {
        // Single allocation using string.Create instead of char + Substring (2 allocations)
        return string.Create(value.Length, value, static (chars, state) =>
        {
            state.AsSpan().CopyTo(chars);
            chars[0] = char.ToUpper(chars[0]);
        });
    }

    /// <summary>
    /// Converts a string value to the specified type
    /// </summary>
    /// <param name="value">String value to convert</param>
    /// <param name="type">Target type</param>
    /// <returns>Converted value</returns>
    protected object ToType(string value, Type type)
    {
        // Direct type checks in order of frequency for optimal performance
        // Most common types first to minimize average checks
#pragma warning disable IDE0011 // Add braces
        if (type == typeof(string)) return value;
        if (type == typeof(int)) return int.Parse(value);
        if (type == typeof(DateTime)) return DateTime.Parse(value);
        if (type == typeof(bool)) return bool.Parse(value);
        if (type == typeof(long)) return long.Parse(value);
        if (type == typeof(decimal)) return decimal.Parse(value);
        if (type == typeof(Guid)) return Guid.Parse(value);
        if (type == typeof(double)) return double.Parse(value);
        if (type == typeof(short)) return short.Parse(value);
        if (type == typeof(byte)) return byte.Parse(value);
        if (type == typeof(float)) return float.Parse(value);
        if (type == typeof(DateTimeOffset)) return DateTimeOffset.Parse(value);
        if (type == typeof(TimeSpan)) return TimeSpan.Parse(value);
        if (type == typeof(char)) return char.Parse(value);
        if (type == typeof(uint)) return uint.Parse(value);
        if (type == typeof(ulong)) return ulong.Parse(value);
        if (type == typeof(ushort)) return ushort.Parse(value);
        if (type == typeof(sbyte)) return sbyte.Parse(value);
#pragma warning restore IDE0011 // Add braces

        // For other types use cached converter
        if (!_converterCache.TryGetValue(type, out TypeConverter? converter))
        {
            converter = TypeDescriptor.GetConverter(type);
            lock (_converterCache)
            {
                _converterCache[type] = converter;
            }
        }

        return converter.ConvertFromInvariantString(value) ??
            throw new InvalidOperationException($"Failed to convert value '{value}' to type '{type.Name}'");
    }
}