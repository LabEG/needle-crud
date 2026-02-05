namespace LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;

/// <summary>
/// Represents a single sort condition to be applied to a paginated query.
/// </summary>
public class PagedListQuerySort
{
    /// <summary>
    /// Gets or sets the name of the property to sort by.
    /// Property names are case-sensitive and should match entity property names.
    /// </summary>
    public required string Property { get; set; }

    /// <summary>
    /// Gets or sets the sort direction (ascending or descending).
    /// </summary>
    public required PagedListQuerySortDirection Direction { get; set; }
}