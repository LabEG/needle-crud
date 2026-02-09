using LabEG.NeedleCrud.Models.Entities;
using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;
using System.Text.Json.Nodes;

namespace LabEG.NeedleCrud.Repositories;

/// <summary>
/// Defines a generic CRUD (Create, Read, Update, Delete) repository interface for data access operations.
/// Provides standard operations for entity persistence including pagination, filtering, and graph loading.
/// </summary>
/// <typeparam name="TEntity">The entity type that implements <see cref="IEntity{TId}"/>. Must be a reference type with a parameterless constructor.</typeparam>
/// <typeparam name="TId">The type of the entity's identifier (e.g., int, Guid, string).</typeparam>
public interface ICrudRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>, new()
{
    /// <summary>
    /// Creates a new entity in the data store.
    /// </summary>
    /// <param name="entity">The entity to create. Must not be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created entity with its assigned identifier.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
    Task<TEntity> Create(TEntity entity);

    /// <summary>
    /// Deletes an entity from the data store by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="Models.Exceptions.ObjectNotFoundNeedleCrudException">Thrown when an entity with the specified <paramref name="id"/> is not found.</exception>
    Task Delete(TId id);

    /// <summary>
    /// Retrieves all entities from the data store.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of all entities.</returns>
    /// <remarks>
    /// Warning: This method may return a large dataset. Consider using <see cref="GetPaged"/> for better performance with large collections.
    /// </remarks>
    Task<TEntity[]> GetAll();

    /// <summary>
    /// Retrieves a single entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity with the specified identifier.</returns>
    /// <exception cref="Models.Exceptions.ObjectNotFoundNeedleCrudException">Thrown when an entity with the specified <paramref name="id"/> is not found.</exception>
    Task<TEntity> GetById(TId id);

    /// <summary>
    /// Updates an existing entity in the data store.
    /// </summary>
    /// <param name="id">The identifier of the entity to update.</param>
    /// <param name="entity">The entity containing updated values. Must not be null.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
    /// <exception cref="Models.Exceptions.ObjectNotFoundNeedleCrudException">Thrown when an entity with the specified <paramref name="id"/> is not found.</exception>
    Task Update(TId id, TEntity entity);

    /// <summary>
    /// Retrieves a paginated, filtered, and sorted list of entities.
    /// </summary>
    /// <param name="query">The query configuration containing pagination, filtering, sorting, and graph loading parameters. Must not be null.</param>
    /// <param name="data">Optional pre-filtered queryable data source. If null, uses the default entity set.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="PagedList{TEntity}"/> with the requested page of entities and pagination metadata.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="query"/> is null.</exception>
    Task<PagedList<TEntity>> GetPaged(PagedListQuery query, IQueryable<TEntity>? data = null);

    /// <summary>
    /// Retrieves a single entity by its identifier with related entities loaded based on the graph expression.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <param name="graph">The JSON object graph expression specifying which related entities to include (eager loading). Must not be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity with its specified related entities loaded.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="graph"/> is null.</exception>
    /// <exception cref="Models.Exceptions.ObjectNotFoundNeedleCrudException">Thrown when an entity with the specified <paramref name="id"/> is not found.</exception>
    Task<TEntity> GetGraph(TId id, JsonObject graph);
}