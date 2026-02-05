using LabEG.NeedleCrud.Models.Entities;
using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;
using LabEG.NeedleCrud.Repositories;
using Newtonsoft.Json.Linq;

namespace LabEG.NeedleCrud.Services;

/// <summary>
/// Provides a base implementation of <see cref="ICrudService{TEntity, TId}"/> that delegates operations to a repository.
/// This service acts as a business logic layer between controllers and repositories.
/// </summary>
/// <typeparam name="TEntity">The entity type that implements <see cref="IEntity{TId}"/>. Must be a reference type with a parameterless constructor.</typeparam>
/// <typeparam name="TId">The type of the entity's identifier (e.g., int, Guid, string).</typeparam>
public class CrudService<TEntity, TId> : ICrudService<TEntity, TId>
    where TEntity : class, IEntity<TId>, new()
{
    /// <summary>
    /// Gets the repository used for data access operations.
    /// </summary>
    protected ICrudRepository<TEntity, TId> Repository { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CrudService{TEntity, TId}"/> class.
    /// </summary>
    /// <param name="repository">The repository instance to use for data access. Must not be null.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="repository"/> is null.</exception>
    public CrudService(ICrudRepository<TEntity, TId> repository)
    {
        Repository = repository;
    }

    /// <inheritdoc />
    public async Task<TEntity> Create(TEntity entity)
    {
        return await Repository.Create(entity);
    }

    /// <inheritdoc />
    public async Task Delete(TId id)
    {
        await Repository.Delete(id);
    }

    /// <inheritdoc />
    public async Task<TEntity> GetById(TId id)
    {
        return await Repository.GetById(id);
    }

    /// <inheritdoc />
    public async Task<IList<TEntity>> GetAll()
    {
        return await Repository.GetAll();
    }

    /// <inheritdoc />
    public async Task Update(TId id, TEntity entity)
    {
        await Repository.Update(id, entity);
    }

    /// <inheritdoc />
    public async Task<PagedList<TEntity>> GetPaged(PagedListQuery query)
    {
        return await Repository.GetPaged(query);
    }

    /// <inheritdoc />
    public async Task<TEntity> GetGraph(TId id, JObject graph)
    {
        return await Repository.GetGraph(id, graph);
    }
}