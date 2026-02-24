using LabEG.NeedleCrud.Benchmarks.BLL;
using LabEG.NeedleCrud.Benchmarks.BLL.Entities;
using LabEG.NeedleCrud.Controllers;
using LabEG.NeedleCrud.Services;
using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Authorization;

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
// ── Authorization example ────────────────────────────────────────────────────
// NeedleCrud controllers are regular ASP.NET Core controllers, so you can
// protect them with any [Authorize] attribute combination.
//
// Uncomment ONE of the examples below (and enable auth in Program.cs) to try:
//
// • Require any authenticated user for all endpoints in this controller:
// [Authorize]
//
// • Allow anonymous read access but require auth for write operations —
//   override individual methods instead of annotating the class:
// [Authorize]                                 ← applied to class
// [AllowAnonymous] on GET overrides            ← applied to method
//
// • Require a specific role (e.g. "Admin" or "Librarian"):
// [Authorize(Roles = "Admin,Librarian")]
//
// • Require a named policy defined in Program.cs via AddAuthorization():
// [Authorize(Policy = "LibraryStaff")]
//
// You are free to implement any authentication scheme (JWT, cookies, API key,
// etc.) — NeedleCrud does not interfere with the ASP.NET Core auth pipeline.
// ────────────────────────────────────────────────────────────────────────────
public class BooksController(ICrudDbService<LibraryDbContext, Book, Guid> service) : CrudController<Book, Guid>(service)
{
}
