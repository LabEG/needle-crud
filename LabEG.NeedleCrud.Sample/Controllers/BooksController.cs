using LabEG.NeedleCrud.Benchmarks.BLL;
using LabEG.NeedleCrud.Benchmarks.BLL.Entities;
using LabEG.NeedleCrud.Controllers;
using LabEG.NeedleCrud.Services;
using Microsoft.AspNetCore.Mvc;

namespace LabEG.NeedleCrud.Sample.Controllers;

/// <summary>
/// CRUD operations for managing books in the library
/// </summary>
/// <remarks>
/// Provides endpoints for managing book catalog including:
/// - Creating new book entries
/// - Retrieving books with filtering by availability, page count, language, etc.
/// - Updating book information
/// - Deleting books from the catalog
/// - Eager loading of related entities (Author, Category, Loans, Reviews)
/// </remarks>
[Route("api/books")]
public class BooksController(ICrudDbService<LibraryDbContext, Book, Guid> service) : CrudController<Book, Guid>(service)
{
}
