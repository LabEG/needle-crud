using LabEG.NeedleCrud.Controllers;
using LabEG.NeedleCrud.Services;
using LabEG.NeedleCrud.Settings;
using LabEG.NeedleCrud.TestsFixtures.BLL.Entities;
using LabEG.NeedleCrud.TestsFixtures.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LabEG.NeedleCrud.Sample.Controllers;

/// <summary>
/// CRUD operations for managing book loans
/// </summary>
/// <remarks>
/// Provides endpoints for managing book lending transactions including:
/// - Creating new loan records when books are borrowed
/// - Retrieving loan history with filtering by user, book, dates, etc.
/// - Updating loan status (active, returned, overdue)
/// - Deleting loan records
/// - Eager loading of related User and Book information
/// </remarks>
[Route("api/loans")]
public class LoansController(
    ICrudDbService<LibraryDbContext, Loan, Guid> service,
    IOptions<NeedleCrudSettings> settings
) : CrudController<Loan, Guid>(service, settings)
{
}
