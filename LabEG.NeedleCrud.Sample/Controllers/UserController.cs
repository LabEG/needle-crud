using LabEG.NeedleCrud.Benchmarks.BLL;
using LabEG.NeedleCrud.Benchmarks.BLL.Entities;
using LabEG.NeedleCrud.Controllers;
using LabEG.NeedleCrud.Services;
using Microsoft.AspNetCore.Mvc;

namespace LabEG.NeedleCrud.Sample.Controllers;

/// <summary>
/// CRUD operations for managing library users
/// </summary>
/// <remarks>
/// Provides endpoints for creating, reading, updating, and deleting user records.
/// Supports advanced features like filtering, sorting, pagination, and eager loading of related entities.
/// </remarks>
[Route("api/users")]
public class UsersController(ICrudDbService<LibraryDbContext, User, Guid> service) : CrudController<User, Guid>(service)
{
}
