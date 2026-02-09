using LabEG.NeedleCrud.Benchmarks.BLL;
using LabEG.NeedleCrud.Benchmarks.BLL.Entities;
using LabEG.NeedleCrud.Controllers;
using LabEG.NeedleCrud.Services;
using Microsoft.AspNetCore.Mvc;

namespace LabEG.NeedleCrud.Sample.Controllers;

/// <summary>
/// CRUD operations for managing book categories
/// </summary>
/// <remarks>
/// Provides endpoints for managing book categorization including:
/// - Creating new categories (Fiction, Non-Fiction, Science Fiction, etc.)
/// - Retrieving categories with sorting and filtering
/// - Updating category names and descriptions
/// - Deleting categories
/// - Viewing all books in a specific category through eager loading
/// </remarks>
[Route("api/categories")]
public class CategoriesController(ICrudDbService<LibraryDbContext, Category, Guid> service) : CrudController<Category, Guid>(service)
{
}
