using LabEG.NeedleCrud.Models.Entities;
using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;
using Newtonsoft.Json.Linq;

namespace LabEG.NeedleCrud.Repositories;

public interface ICrudRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>, new()
{
    Task<TEntity> Create(TEntity entity);

    Task Delete(TId id);

    Task<IList<TEntity>> GetAll();

    Task<TEntity> GetById(TId id);

    Task Update(TId id, TEntity entity);

    Task<PagedList<TEntity>> GetPaged(PagedListQuery query, IQueryable<TEntity> data = null);

    Task<TEntity> GetGraph(TId id, JObject graph);
}