using LabEG.NeedleCrud.Benchmarks.BLL;
using LabEG.NeedleCrud.Benchmarks.BLL.Entities;
using LabEG.NeedleCrud.Controllers;
using LabEG.NeedleCrud.Services;
using Microsoft.AspNetCore.Mvc;

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
public class AuthorsController(ICrudDbService<LibraryDbContext, Author, Guid> service) : CrudController<Author, Guid>(service)
{
}
