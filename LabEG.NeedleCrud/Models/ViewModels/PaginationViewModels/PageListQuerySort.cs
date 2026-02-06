namespace LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;

/// <summary>
/// Represents a single sort condition to be applied to a paginated query.
/// </summary>
public struct PagedListQuerySort
{
    /// <summary>
    /// Gets or sets the name of the property to sort by.
    /// Property names are case-sensitive and should match entity property names.
    /// </summary>
    public string Property { get; init; }

    /// <summary>
    /// Gets or sets the sort direction (ascending or descending).
    /// </summary>
    public PagedListQuerySortDirection Direction { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedListQuerySort"/> struct by parsing a sort expression.
    /// </summary>
    /// <param name="sortItem">A ReadOnlySpan representing a sort expression in the format: property~direction.</param>
    public PagedListQuerySort(ReadOnlySpan<char> sortItem)
    {
        int tildeIndex = sortItem.IndexOf('~');

        ReadOnlySpan<char> property = sortItem.Slice(0, tildeIndex);
        ReadOnlySpan<char> direction = sortItem.Slice(tildeIndex + 1);

        Property = property.Length > 0
            ? string.Concat(char.ToUpperInvariant(property[0]).ToString(), property.Slice(1).ToString())
            : string.Empty;

        bool isAsc = direction.Length == 3 &&
                    (direction[0] == 'a' || direction[0] == 'A') &&
                    (direction[1] == 's' || direction[1] == 'S') &&
                    (direction[2] == 'c' || direction[2] == 'C');

        Direction = isAsc ? PagedListQuerySortDirection.Asc : PagedListQuerySortDirection.Desc;
    }
}