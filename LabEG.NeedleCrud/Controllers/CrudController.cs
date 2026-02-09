using LabEG.NeedleCrud.Models.Entities;
using LabEG.NeedleCrud.Models.Exceptions;
using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;
using LabEG.NeedleCrud.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace LabEG.NeedleCrud.Controllers;

/// <summary>
/// Base CRUD controller providing standard Create, Read, Update, Delete operations
/// </summary>
/// <typeparam name="TEntity">Entity type implementing IEntity</typeparam>
/// <typeparam name="TId">Type of the entity's primary key</typeparam>
/// <remarks>
/// This controller provides a complete set of RESTful endpoints for entity management:
/// - POST / - Create new entity
/// - GET / - Get all entities
/// - GET /{id} - Get entity by ID
/// - PUT /{id} - Update entity
/// - DELETE /{id} - Delete entity
/// - GET /paged - Get paginated list with filtering, sorting, and eager loading
/// - GET /{id}/graph - Get entity with related data (eager loading)
/// </remarks>
[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class CrudController<TEntity, TId> : ControllerBase, ICrudController<TEntity, TId>
    where TEntity : class, IEntity<TId>, new()
{
    /// <summary>
    /// Gets the CRUD service instance used for business logic operations.
    /// </summary>
    protected ICrudService<TEntity, TId> Service { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CrudController{TEntity, TId}"/> class.
    /// </summary>
    /// <param name="service">The CRUD service instance to use for entity operations.</param>
    public CrudController(ICrudService<TEntity, TId> service)
    {
        Service = service;
    }

    /// <summary>
    /// Create a new entity
    /// </summary>
    /// <param name="entity">Entity to create</param>
    /// <returns>Created entity with generated ID</returns>
    /// <response code="200">Returns the newly created entity</response>
    /// <response code="400">If the entity data is invalid</response>
    [HttpPost]
    public virtual async Task<TEntity> Create([FromBody] TEntity entity)
    {
        return await Service.Create(entity);
    }

    /// <summary>
    /// Get all entities
    /// </summary>
    /// <returns>Array of all entities</returns>
    /// <response code="200">Returns all entities</response>
    /// <remarks>
    /// Warning: Use with caution on large datasets. Consider using the /paged endpoint for better performance.
    /// </remarks>
    [HttpGet]
    public virtual async Task<TEntity[]> GetAll()
    {
        return await Service.GetAll();
    }

    /// <summary>
    /// Get a specific entity by ID
    /// </summary>
    /// <param name="id">Entity ID</param>
    /// <returns>Entity with the specified ID</returns>
    /// <response code="200">Returns the entity</response>
    /// <response code="404">If the entity is not found</response>
    [HttpGet("{id}")]
    public virtual async Task<TEntity> GetById(TId id)
    {
        TEntity entity = await Service.GetById(id);

        return entity;
    }

    /// <summary>
    /// Update an existing entity
    /// </summary>
    /// <param name="id">Entity ID to update</param>
    /// <param name="entity">Updated entity data</param>
    /// <response code="200">Entity updated successfully</response>
    /// <response code="404">If the entity is not found</response>
    /// <response code="400">If the entity data is invalid</response>
    [HttpPut("{id}")]
    public virtual async Task Update(TId id, [FromBody] TEntity entity)
    {
        await Service.Update(id, entity);
    }

    /// <summary>
    /// Delete an entity
    /// </summary>
    /// <param name="id">Entity ID to delete</param>
    /// <response code="200">Entity deleted successfully</response>
    /// <response code="404">If the entity is not found</response>
    [HttpDelete("{id}")]
    public virtual async Task Delete(TId id)
    {
        await Service.Delete(id);
    }

    /// <summary>
    /// Get paginated list of entities with optional filtering, sorting, and eager loading
    /// </summary>
    /// <param name="pageSize">Number of items per page (default: 10)</param>
    /// <param name="pageNumber">Page number starting from 1 (default: 1)</param>
    /// <param name="filter">Filter expression. Format: "Property~Operator~Value[,Property~Operator~Value...]"
    /// Operators: =, >, >=, &lt;, &lt;=, like, ilike
    /// Example: "Name~like~John,Age~>=~18"</param>
    /// <param name="sort">Sort expression. Format: "Property~Direction[,Property~Direction...]"
    /// Direction: asc, desc
    /// Example: "Name~asc,Age~desc"</param>
    /// <param name="graph">JSON object defining related entities to include (eager loading).
    /// Example: {"RelatedEntity":null} or {"Parent":{"Child":null}}</param>
    /// <returns>Paginated list with metadata</returns>
    /// <response code="200">Returns paginated results</response>
    /// <response code="400">If query parameters are invalid</response>
    /// <remarks>
    /// Example usage:
    /// GET /api/entity/paged?pageSize=20&amp;pageNumber=1&amp;filter=IsActive~=~true&amp;sort=Name~asc&amp;graph={"Author":null}
    /// </remarks>
    [HttpGet("paged")]
    public virtual async Task<PagedList<TEntity>> GetPaged(
        [FromQuery] int? pageSize,
        [FromQuery] int? pageNumber,
        [FromQuery] string? filter,
        [FromQuery] string? sort,
        [FromQuery] string? graph
    )
    {
        PagedListQuery query = new(pageSize, pageNumber, filter, sort, graph);
        PagedList<TEntity> pagedResult = await Service.GetPaged(query);

        return pagedResult;
    }

    /// <summary>
    /// Get entity by ID with related entities (eager loading)
    /// </summary>
    /// <param name="id">Entity ID</param>
    /// <param name="graph">JSON object defining related entities to include.
    /// Example: {"Author":null,"Category":null} to load Author and Category relations</param>
    /// <returns>Entity with loaded related data</returns>
    /// <response code="200">Returns the entity with related data</response>
    /// <response code="400">If the graph parameter is invalid or missing</response>
    /// <response code="404">If the entity is not found</response>
    /// <remarks>
    /// The graph parameter uses JSON format where keys are navigation property names.
    /// Nested relationships: {"Parent":{"Child":null}}
    /// Multiple relationships: {"Relation1":null,"Relation2":null}
    /// </remarks>
    [HttpGet("{id}/graph")]
    public virtual async Task<TEntity> GetGraph(TId id, [FromQuery] string graph)
    {
        if (string.IsNullOrEmpty(graph))
        {
            throw new NeedleCrudException("Parameter 'graph' cannot be null or empty");
        }

        JsonObject graphObject = JsonNode.Parse(graph)?.AsObject() ??
            throw new NeedleCrudException("Invalid JSON in 'graph' parameter");

        TEntity graphResult = await Service.GetGraph(id, graphObject);

        return graphResult;
    }

}