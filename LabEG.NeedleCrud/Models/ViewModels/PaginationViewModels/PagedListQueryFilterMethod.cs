namespace LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;

/// <summary>
/// Specifies the comparison method to use when filtering query results.
/// </summary>
public enum PagedListQueryFilterMethod
{
    /// <summary>
    /// Less than comparison (&lt;).
    /// The property value must be less than the filter value.
    /// </summary>
    Less,

    /// <summary>
    /// Less than or equal to comparison (&lt;=).
    /// The property value must be less than or equal to the filter value.
    /// </summary>
    LessOrEqual,

    /// <summary>
    /// Equality comparison (=).
    /// The property value must exactly match the filter value.
    /// </summary>
    Equal,

    /// <summary>
    /// Greater than or equal to comparison (&gt;=).
    /// The property value must be greater than or equal to the filter value.
    /// </summary>
    GreatOrEqual,

    /// <summary>
    /// Greater than comparison (&gt;).
    /// The property value must be greater than the filter value.
    /// </summary>
    Great,

    /// <summary>
    /// Case-sensitive pattern matching (LIKE).
    /// The property value must match the filter pattern. Supports wildcards.
    /// </summary>
    Like,

    /// <summary>
    /// Case-insensitive pattern matching (ILIKE).
    /// The property value must match the filter pattern, ignoring case. Supports wildcards.
    /// </summary>
    ILike
}