using System.Web;
using LabEG.NeedleCrud.Models.Exceptions;

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
    public string Property { get; init; }

    /// <summary>
    /// Gets or sets the value to compare against.
    /// The value will be compared to the property value using the specified <see cref="Method"/>.
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Gets or sets the comparison method to use for filtering.
    /// </summary>
    public PagedListQueryFilterMethod Method { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedListQueryFilter"/> struct by parsing a filter expression.
    /// </summary>
    /// <param name="filterItem">A ReadOnlySpan representing a filter expression in the format: property~method~value.</param>
    /// <exception cref="NeedleCrudException">Thrown when the method is unknown.</exception>
    public PagedListQueryFilter(ReadOnlySpan<char> filterItem)
    {
        int firstTilde = filterItem.IndexOf('~');
        int secondTilde = filterItem.Slice(firstTilde + 1).IndexOf('~');
        secondTilde += firstTilde + 1;

        ReadOnlySpan<char> property = filterItem.Slice(0, firstTilde);
        ReadOnlySpan<char> method = filterItem.Slice(firstTilde + 1, secondTilde - firstTilde - 1);
        ReadOnlySpan<char> value = filterItem.Slice(secondTilde + 1);

        Property = property.Length > 0
            ? string.Concat(char.ToUpperInvariant(property[0]).ToString(), property.Slice(1).ToString())
            : string.Empty;

        Method = ParseFilterMethod(method.ToString());
        Value = HttpUtility.UrlDecode(value.ToString());
    }

    /// <summary>
    /// Parses a filter method string and converts it to the corresponding <see cref="PagedListQueryFilterMethod"/> enum value.
    /// </summary>
    /// <param name="method">The filter method string. Supported values: "&lt;", "&lt;=", "&gt;=", "&gt;", "like", "ilike", "=".</param>
    /// <returns>The corresponding <see cref="PagedListQueryFilterMethod"/> enum value.</returns>
    /// <exception cref="NeedleCrudException">Thrown when the <paramref name="method"/> is not recognized.</exception>
    private static PagedListQueryFilterMethod ParseFilterMethod(string method)
    {
        return method switch
        {
            "<" => PagedListQueryFilterMethod.Less,
            "<=" => PagedListQueryFilterMethod.LessOrEqual,
            ">=" => PagedListQueryFilterMethod.GreatOrEqual,
            ">" => PagedListQueryFilterMethod.Great,
            "like" => PagedListQueryFilterMethod.Like,
            "ilike" => PagedListQueryFilterMethod.ILike,
            "=" => PagedListQueryFilterMethod.Equal,
            _ => throw new NeedleCrudException("Unknown filter method"),
        };
    }
}