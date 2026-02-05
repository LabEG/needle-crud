namespace LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;

/// <summary>
/// Specifies the direction for sorting query results.
/// </summary>
public enum PagedListQuerySortDirection
{
    /// <summary>
    /// Sort in ascending order (A to Z, 0 to 9, earliest to latest).
    /// </summary>
    Asc,

    /// <summary>
    /// Sort in descending order (Z to A, 9 to 0, latest to earliest).
    /// </summary>
    Desc
}