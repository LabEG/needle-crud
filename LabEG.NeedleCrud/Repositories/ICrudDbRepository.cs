using LabEG.NeedleCrud.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LabEG.NeedleCrud.Repositories;

/// <summary>
/// Repository interface for database CRUD operations with Entity Framework DbContext
/// </summary>
/// <typeparam name="TDbContext">Type of the Entity Framework DbContext</typeparam>
/// <typeparam name="TEntity">Type of the entity</typeparam>
/// <typeparam name="TId">Type of the entity's primary key</typeparam>
public interface ICrudDbRepository<TDbContext, TEntity, TId> : ICrudRepository<TEntity, TId>
    where TDbContext : DbContext
    where TEntity : class, IEntity<TId>, new()
{
}