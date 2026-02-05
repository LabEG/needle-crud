using LabEG.NeedleCrud.Models.Entities;
using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;
using LabEG.NeedleCrud.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace LabEG.NeedleCrud.Services;

public class CrudDbService<TDbContext, TEntity, TId> : ICrudDbService<TDbContext, TEntity, TId>
where TDbContext : DbContext
where TEntity : class, IEntity<TId>, new()
{
    protected TDbContext DBContext { get; }
    protected ICrudDbRepository<TDbContext, TEntity, TId> Repository { get; }

    public CrudDbService(TDbContext dbContext, ICrudDbRepository<TDbContext, TEntity, TId> repository)
    {
        DBContext = dbContext;
        Repository = repository;
    }

    public virtual async Task<TEntity> Create(TEntity entity)
    {
        TEntity resultEntity = await Repository.Create(entity);
        await SaveContext();
        return resultEntity;
    }

    public virtual async Task<TEntity> GetById(TId id)
    {
        TEntity resultEntity = await Repository.GetById(id);
        await SaveContext();
        return resultEntity;
    }

    public virtual async Task<IList<TEntity>> GetAll()
    {
        IList<TEntity> resultEntities = await Repository.GetAll();
        return resultEntities;
    }

    public virtual async Task Update(TId id, TEntity entity)
    {
        try
        {
            await Repository.Update(id, entity);
            await SaveContext();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public virtual async Task Delete(TId id)
    {
        await Repository.Delete(id);
        await SaveContext();
    }

    public virtual async Task<PagedList<TEntity>> GetPaged(PagedListQuery query)
    {
        return await Repository.GetPaged(query);
    }

    public virtual async Task<TEntity> GetGraph(TId id, JObject graph)
    {
        return await Repository.GetGraph(id, graph);
    }

    protected virtual async Task SaveContext()
    {
        await DBContext.SaveChangesAsync();
    }
}