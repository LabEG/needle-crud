using LabEG.NeedleCrud.Models.Entities;
using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LabEG.NeedleCrud.Controllers;

/// <summary>
/// Defines a generic CRUD (Create, Read, Update, Delete) controller interface for managing entities.
/// Provides standard operations for entity manipulation including pagination, filtering, and graph loading.
/// </summary>
/// <typeparam name="TEntity">The entity type that implements <see cref="IEntity{TId}"/>. Must be a reference type with a parameterless constructor.</typeparam>
/// <typeparam name="TId">The type of the entity's identifier (e.g., int, Guid, string).</typeparam>
public interface ICrudController<TEntity, TId>
    where TEntity : class, IEntity<TId>, new()
{
    /// <summary>
    /// Creates a new entity in the data store.
    /// </summary>
    /// <param name="entity">The entity to create. Must not be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created entity with its assigned identifier.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
    Task<ActionResult<TEntity>> Create(TEntity entity);

    /// <summary>
    /// Deletes an entity from the data store by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="ObjectNotFoundException">Thrown when an entity with the specified <paramref name="id"/> is not found.</exception>
    Task Delete(TId id);

    /// <summary>
    /// Retrieves all entities from the data store.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all entities.</returns>
    /// <remarks>
    /// Warning: This method may return a large dataset. Consider using <see cref="GetPaged"/> for better performance with large collections.
    /// </remarks>
    Task<IList<TEntity>> GetAll();

    /// <summary>
    /// Retrieves a single entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity with the specified identifier.</returns>
    /// <exception cref="ObjectNotFoundException">Thrown when an entity with the specified <paramref name="id"/> is not found.</exception>
    Task<TEntity> GetById(TId id);

    /// <summary>
    /// Updates an existing entity in the data store.
    /// </summary>
    /// <param name="id">The identifier of the entity to update.</param>
    /// <param name="entity">The entity containing updated values. Must not be null.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
    /// <exception cref="ObjectNotFoundException">Thrown when an entity with the specified <paramref name="id"/> is not found.</exception>
    Task<IActionResult> Update(TId id, TEntity entity);

    /// <summary>
    /// Retrieves a paginated, filtered, and sorted list of entities.
    /// </summary>
    /// <param name="pageSize">The number of entities to return per page. Must be greater than 0. If null, uses the default page size.</param>
    /// <param name="pageNumber">The page number to retrieve (1-based index). Must be greater than 0. If null, retrieves the first page.</param>
    /// <param name="filter">Optional filter expression to apply to the query. Pass null or empty string for no filtering.</param>
    /// <param name="sort">Optional sort expression to apply to the query. Pass null or empty string for default sorting.</param>
    /// <param name="graph">Optional graph expression to specify related entities to include (eager loading). Pass null or empty string to load only the main entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="PagedList{TEntity}"/> with the requested page of entities and pagination metadata.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="pageSize"/> or <paramref name="pageNumber"/> is provided and less than or equal to 0.</exception>
    Task<PagedList<TEntity>> GetPaged(int? pageSize, int? pageNumber, string? filter, string? sort, string? graph);

    /// <summary>
    /// Retrieves a single entity by its identifier with related entities loaded based on the graph expression.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <param name="graph">The graph expression specifying which related entities to include (eager loading). Pass null or empty string to load only the main entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity with its specified related entities loaded.</returns>
    /// <exception cref="ObjectNotFoundException">Thrown when an entity with the specified <paramref name="id"/> is not found.</exception>
    Task<TEntity> GetGraph(TId id, string graph);
}