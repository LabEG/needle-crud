using LabEG.NeedleCrud.Models.Entities;
using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;

namespace LabEG.NeedleCrud.Controllers;

public interface ICrudController<TEntity, TId>
    where TEntity : class, IEntity<TId>, new()
{
    Task<TEntity> Create(TEntity entity);

    Task Delete(TId id);

    Task<IList<TEntity>> GetAll();

    Task<TEntity> GetById(TId id);

    Task Update(TId id, TEntity entity);

    Task<PagedList<TEntity>> GetPaged(int pageSize, int pageNumber, string filter, string sort, string graph);

    Task<TEntity> GetGraph(TId id, string graph);
}