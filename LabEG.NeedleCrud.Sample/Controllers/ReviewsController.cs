using LabEG.NeedleCrud.Controllers;
using LabEG.NeedleCrud.Services;
using LabEG.NeedleCrud.Settings;
using LabEG.NeedleCrud.TestsFixtures.BLL.Entities;
using LabEG.NeedleCrud.TestsFixtures.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
public class ReviewsController(
    ICrudDbService<LibraryDbContext, Review, Guid> service,
    IOptions<NeedleCrudSettings> settings
) : CrudController<Review, Guid>(service, settings)
{
}
