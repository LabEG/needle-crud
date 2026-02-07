using LabEG.NeedleCrud.Models.Entities;
using LabEG.NeedleCrud.Models.Exceptions;
using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;
using LabEG.NeedleCrud.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LabEG.NeedleCrud.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
public class CrudController<TEntity, TId> : ControllerBase, ICrudController<TEntity, TId>
    where TEntity : class, IEntity<TId>, new()
{
    protected ICrudService<TEntity, TId> Service { get; }

    public CrudController(ICrudService<TEntity, TId> service)
    {
        Service = service;
    }

    [HttpPost]
    public virtual async Task<ActionResult<TEntity>> Create([FromBody] TEntity entity)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return await Service.Create(entity);
    }

    [HttpGet]
    public virtual async Task<IList<TEntity>> GetAll()
    {
        return await Service.GetAll();
    }

    [HttpGet("{id}")]
    public virtual async Task<TEntity> GetById(TId id)
    {
        TEntity entity = await Service.GetById(id);

        return entity;
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(TId id, [FromBody] TEntity entity)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await Service.Update(id, entity);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public virtual async Task Delete(TId id)
    {
        await Service.Delete(id);
    }

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

    [HttpGet("{id}/graph")]
    public virtual async Task<TEntity> GetGraph(TId id, [FromQuery] string graph)
    {
        if (string.IsNullOrEmpty(graph))
        {
            throw new NeedleCrudException("Parameter 'graph' cannot be null or empty");
        }

        JObject graphObject = JsonConvert.DeserializeObject(graph) as JObject ??
            throw new NeedleCrudException("Invalid JSON in 'graph' parameter");

        TEntity graphResult = await Service.GetGraph(id, graphObject);

        return graphResult;
    }

}