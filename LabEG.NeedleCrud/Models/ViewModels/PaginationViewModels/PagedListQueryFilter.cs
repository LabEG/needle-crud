namespace LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;

/// <summary>
/// Represents a single filter condition to be applied to a paginated query.
/// </summary>
public struct PagedListQueryFilter
{
    /// <summary>
    /// Gets or sets the name of the property to filter on.
    /// Property names are case-sensitive and should match entity property names.
    /// </summary>
    public required string Property { get; init; }

    /// <summary>
    /// Gets or sets the value to compare against.
    /// The value will be compared to the property value using the specified <see cref="Method"/>.
    /// </summary>
    public required string Value { get; init; }

    /// <summary>
    /// Gets or sets the comparison method to use for filtering.
    /// </summary>
    public required PagedListQueryFilterMethod Method { get; init; }
}