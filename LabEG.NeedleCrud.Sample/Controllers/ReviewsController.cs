using LabEG.NeedleCrud.Benchmarks.BLL;
using LabEG.NeedleCrud.Benchmarks.BLL.Entities;
using LabEG.NeedleCrud.Controllers;
using LabEG.NeedleCrud.Services;
using Microsoft.AspNetCore.Mvc;

namespace LabEG.NeedleCrud.Sample.Controllers;

/// <summary>
/// CRUD operations for managing book reviews
/// </summary>
/// <remarks>
/// Provides endpoints for managing user reviews and ratings including:
/// - Creating new book reviews with ratings
/// - Retrieving reviews with filtering by book, user, rating, etc.
/// - Updating review content and ratings
/// - Deleting reviews
/// - Eager loading of related User and Book information
/// - Sorting reviews by date, rating, or helpfulness
/// </remarks>
[Route("api/reviews")]
public class ReviewsController(ICrudDbService<LibraryDbContext, Review, Guid> service) : CrudController<Review, Guid>(service)
{
}
