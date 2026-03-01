using LabEG.NeedleCrud.Models.Entities;
using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;
using LabEG.NeedleCrud.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;

namespace LabEG.NeedleCrud.Services;

/// <summary>
/// Provides a concrete implementation of <see cref="ICrudDbService{TDbContext, TEntity, TId}"/> for performing CRUD operations
/// using Entity Framework Core. This service wraps repository operations and manages database transactions through SaveChangesAsync.
/// </summary>
/// <typeparam name="TDbContext">The Entity Framework Core database context type. Must inherit from <see cref="DbContext"/>.</typeparam>
/// <typeparam name="TEntity">The entity type that implements <see cref="IEntity{TId}"/>. Must be a reference type with a parameterless constructor.</typeparam>
/// <typeparam name="TId">The type of the entity's identifier (e.g., int, Guid, string).</typeparam>
public class CrudDbService<TDbContext, TEntity, TId> : ICrudDbService<TDbContext, TEntity, TId>
where TDbContext : DbContext
where TEntity : class, IEntity<TId>, new()
{
    /// <summary>
    /// Gets the Entity Framework Core database context used for database operations and transaction management.
    /// </summary>
    protected TDbContext DBContext { get; }

    /// <summary>
    /// Gets the CRUD repository instance that performs the actual data access operations.
    /// </summary>
    protected ICrudDbRepository<TDbContext, TEntity, TId> Repository { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CrudDbService{TDbContext, TEntity, TId}"/> class.
    /// </summary>
    /// <param name="dbContext">The Entity Framework Core database context to use for database operations.</param>
    /// <param name="repository">The CRUD repository instance to delegate data access operations to.</param>
    public CrudDbService(TDbContext dbContext, ICrudDbRepository<TDbContext, TEntity, TId> repository)
    {
        DBContext = dbContext;
        Repository = repository;
    }

    /// <inheritdoc/>
    public virtual async Task<TEntity> Create(TEntity entity)
    {
        TEntity resultEntity = await Repository.Create(entity);
        await DBContext.SaveChangesAsync();
        return resultEntity;
    }

    /// <inheritdoc/>
    public virtual async Task<TEntity> GetById(TId id)
    {
        TEntity resultEntity = await Repository.GetById(id);
        return resultEntity;
    }

    /// <inheritdoc/>
    public virtual async Task<TEntity[]> GetAll()
    {
        return await Repository.GetAll();
    }

    /// <inheritdoc/>
    public virtual async Task Update(TId id, TEntity entity)
    {
        await Repository.Update(id, entity);
        await DBContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public virtual async Task Delete(TId id)
    {
        await Repository.Delete(id);
        await DBContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<PagedList<TEntity>> GetPaged(PagedListQuery query)
    {
        return await Repository.GetPaged(query);
    }

    /// <inheritdoc/>
    public virtual async Task<TEntity> GetGraph(TId id, JsonObject graph)
    {
        return await Repository.GetGraph(id, graph);
    }

}