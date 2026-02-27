using LabEG.NeedleCrud.Controllers;
using LabEG.NeedleCrud.Services;
using LabEG.NeedleCrud.Settings;
using LabEG.NeedleCrud.TestsFixtures.BLL.Entities;
using LabEG.NeedleCrud.TestsFixtures.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LabEG.NeedleCrud.Sample.Controllers;

/// <summary>
/// CRUD operations for managing book authors
/// </summary>
/// <remarks>
/// Provides endpoints for managing author information including:
/// - Creating new author profiles
/// - Retrieving authors with filtering by country, name, etc.
/// - Updating author biographical information
/// - Deleting author records
/// - Viewing all books by a specific author through eager loading
/// </remarks>
[Route("api/authors")]
public class AuthorsController(
    ICrudDbService<LibraryDbContext, Author, Guid> service,
    IOptions<NeedleCrudSettings> settings
) : CrudController<Author, Guid>(service, settings)
{
}
