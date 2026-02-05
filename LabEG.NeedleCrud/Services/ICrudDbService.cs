using LabEG.NeedleCrud.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LabEG.NeedleCrud.Services;

public interface ICrudDbService<TDbContext, TEntity, TId> : ICrudService<TEntity, TId>
    where TDbContext : DbContext
    where TEntity : class, IEntity<TId>, new()
{
}