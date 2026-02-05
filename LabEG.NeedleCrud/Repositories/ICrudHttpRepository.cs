using LabEG.NeedleCrud.Models.Entities;

namespace LabEG.NeedleCrud.Repositories;

public interface ICrudHttpRepository<TEntity, TId> : ICrudRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>, new()
{
}