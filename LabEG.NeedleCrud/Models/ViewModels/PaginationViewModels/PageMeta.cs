namespace LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;

/// <summary>
/// Contains metadata information about a paginated result set.
/// </summary>
public class PageMeta
{
    /// <summary>
    /// Gets or sets the current page number (1-based index).
    /// Default value is 0.
    /// </summary>
    public long PageNumber { get; set; } = 0;

    /// <summary>
    /// Gets or sets the number of items per page.
    /// Default value is 0.
    /// </summary>
    public long PageSize { get; set; } = 0;

    /// <summary>
    /// Gets or sets the actual number of elements in the current page.
    /// This may be less than <see cref="PageSize"/> for the last page.
    /// Default value is 0.
    /// </summary>
    public long ElementsInPage { get; set; } = 0;

    /// <summary>
    /// Gets or sets the total number of pages available.
    /// Default value is 0.
    /// </summary>
    public long TotalPages { get; set; } = 0;

    /// <summary>
    /// Gets or sets the total number of elements across all pages.
    /// Default value is 0.
    /// </summary>
    public long TotalElements { get; set; } = 0;
}