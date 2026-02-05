using LabEG.NeedleCrud.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LabEG.NeedleCrud.Repositories;

public interface ICrudDbRepository<TDbContext, TEntity, TId> : ICrudRepository<TEntity, TId>
    where TDbContext : DbContext
    where TEntity : class, IEntity<TId>, new()
{
}