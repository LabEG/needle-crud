using LabEG.NeedleCrud.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LabEG.NeedleCrud.Services;

/// <summary>
/// Defines a generic CRUD service interface specifically for Entity Framework Core database operations.
/// Extends <see cref="ICrudService{TEntity, TId}"/> with database context-specific functionality.
/// </summary>
/// <typeparam name="TDbContext">The Entity Framework Core database context type. Must inherit from <see cref="DbContext"/>.</typeparam>
/// <typeparam name="TEntity">The entity type that implements <see cref="IEntity{TId}"/>. Must be a reference type with a parameterless constructor.</typeparam>
/// <typeparam name="TId">The type of the entity's identifier (e.g., int, Guid, string).</typeparam>
public interface ICrudDbService<TDbContext, TEntity, TId> : ICrudService<TEntity, TId>
    where TDbContext : DbContext
    where TEntity : class, IEntity<TId>, new()
{
}