using LabEG.NeedleCrud.Controllers;
using LabEG.NeedleCrud.Services;
using LabEG.NeedleCrud.Settings;
using LabEG.NeedleCrud.TestsFixtures.BLL.Entities;
using LabEG.NeedleCrud.TestsFixtures.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LabEG.NeedleCrud.Sample.Controllers;

/// <summary>
/// CRUD operations for managing library users
/// </summary>
/// <remarks>
/// Provides endpoints for creating, reading, updating, and deleting user records.
/// Supports advanced features like filtering, sorting, pagination, and eager loading of related entities.
/// </remarks>
[Route("api/users")]
public class UsersController(
    ICrudDbService<LibraryDbContext, User, Guid> service,
    IOptions<NeedleCrudSettings> settings
) : CrudController<User, Guid>(service, settings)
{
}
