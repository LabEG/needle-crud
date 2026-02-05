using LabEG.NeedleCrud.Models.Entities;
using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;
using LabEG.NeedleCrud.Repositories;
using Newtonsoft.Json.Linq;

namespace LabEG.NeedleCrud.Services;

public class CrudService<TEntity, TId> : ICrudService<TEntity, TId>
    where TEntity : class, IEntity<TId>, new()
{
    protected ICrudRepository<TEntity, TId> Repository { get; }

    public CrudService(ICrudRepository<TEntity, TId> repository)
    {
        Repository = repository;
    }

    public async Task<TEntity> Create(TEntity entity)
    {
        return await Repository.Create(entity);
    }

    public async Task Delete(TId id)
    {
        await Repository.Delete(id);
    }

    public async Task<TEntity> GetById(TId id)
    {
        return await Repository.GetById(id);
    }

    public async Task<IList<TEntity>> GetAll()
    {
        return await Repository.GetAll();
    }

    public async Task Update(TId id, TEntity entity)
    {
        await Repository.Update(id, entity);
    }

    public async Task<PagedList<TEntity>> GetPaged(PagedListQuery query)
    {
        return await Repository.GetPaged(query);
    }

    public async Task<TEntity> GetGraph(TId id, JObject graph)
    {
        return await Repository.GetGraph(id, graph);
    }
}