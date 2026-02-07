using LabEG.NeedleCrud.Models.Exceptions;

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
    /// <exception cref="NeedleCrudException">Thrown when the sort format is invalid.</exception>
    public PagedListQuerySort(ReadOnlySpan<char> sortItem)
    {
        int tildeIndex = sortItem.IndexOf('~');
        if (tildeIndex == -1)
        {
            throw new NeedleCrudException($"Invalid sort format. Missing delimiter '~'. Expected 'property~direction'. Sort: '{sortItem}'");
        }

        ReadOnlySpan<char> property = sortItem.Slice(0, tildeIndex);
        ReadOnlySpan<char> direction = sortItem.Slice(tildeIndex + 1);

        if (property.IsEmpty)
        {
            throw new NeedleCrudException($"Invalid sort format. Property name is required. Sort: '{sortItem}'");
        }

        if (direction.IsEmpty)
        {
            throw new NeedleCrudException($"Invalid sort format. Direction (asc/desc) is required. Sort: '{sortItem}'");
        }

        Property = string.Concat(char.ToUpperInvariant(property[0]).ToString(), property.Slice(1).ToString());

        if (direction.Equals("asc", StringComparison.OrdinalIgnoreCase))
        {
            Direction = PagedListQuerySortDirection.Asc;
        }
        else if (direction.Equals("desc", StringComparison.OrdinalIgnoreCase))
        {
            Direction = PagedListQuerySortDirection.Desc;
        }
        else
        {
            throw new NeedleCrudException($"Invalid sort direction. Expected 'asc' or 'desc', but found '{direction}'. Sort: '{sortItem}'");
        }
    }
}