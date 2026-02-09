using System.Text.Json.Nodes;

namespace LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;

/// <summary>
/// Represents a query configuration for paginated list retrieval with filtering, sorting, and graph loading capabilities.
/// </summary>
public class PagedListQuery
{
    /// <summary>
    /// Gets or sets the number of items per page.
    /// Default value is 10.
    /// </summary>
    public int PageSize { get; init; } = 10;

    /// <summary>
    /// Gets or sets the page number to retrieve (1-based index).
    /// Default value is 1.
    /// </summary>
    public int PageNumber { get; init; } = 1;

    /// <summary>
    /// Gets or sets the list of filter conditions to apply to the query.
    /// </summary>
    public PagedListQueryFilter[] Filter { get; init; } = [];

    /// <summary>
    /// Gets or sets the list of sort conditions to apply to the query.
    /// </summary>
    public PagedListQuerySort[] Sort { get; init; } = [];

    /// <summary>
    /// Gets or sets the graph expression as a JSON object for loading related entities (eager loading).
    /// </summary>
    public JsonObject? Graph { get; set; } = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedListQuery"/> class with specified pagination, filtering, sorting, and graph loading parameters.
    /// </summary>
    /// <param name="pageSize">The number of items per page. If null, uses the default page size.</param>
    /// <param name="pageNumber">The page number to retrieve (1-based). If null, defaults to the first page.</param>
    /// <param name="filter">A comma-separated string of filter expressions in the format: property~method~value.
    /// Example: "name~like~John,age~>=~18". If null or empty, no filtering is applied.</param>
    /// <param name="sort">A comma-separated string of sort expressions in the format: property~direction.
    /// Example: "name~asc,age~desc". Direction can be "asc" or "desc". If null or empty, default sorting is used.</param>
    /// <param name="graph">A JSON string representing the graph of related entities to load. If null or empty, no related entities are loaded.</param>
    /// <exception cref="Exceptions.NeedleCrudException">Thrown when an unknown filter method is encountered.</exception>
    public PagedListQuery(
        int? pageSize,
        int? pageNumber,
        string? filter,
        string? sort,
        string? graph
    )
    {
        PageSize = pageSize ?? PageSize;
        PageNumber = pageNumber ?? PageNumber;
        Filter = ParseFilters(filter);
        Sort = ParseSort(sort);
        Graph = ParseGraph(graph);
    }

    /// <summary>
    /// Parses a comma-separated string of filter expressions into an array of <see cref="PagedListQueryFilter"/>.
    /// </summary>
    /// <param name="filter">A comma-separated string of filter expressions in the format: property~method~value.</param>
    /// <returns>An array of parsed filters, or an empty array if the input is null or empty.</returns>
    private static PagedListQueryFilter[] ParseFilters(string? filter)
    {
        if (filter is not string filterString || string.IsNullOrEmpty(filterString))
        {
            return [];
        }

        ReadOnlySpan<char> filterSpan = filterString.AsSpan();
        int estimatedCount = filterSpan.Count(',') + 1;

        PagedListQueryFilter[] filters = new PagedListQueryFilter[estimatedCount];
        int count = 0;

        while (true)
        {
            int commaIndex = filterSpan.IndexOf(',');
            if (commaIndex == -1)
            {
                filters[count++] = new PagedListQueryFilter(filterSpan);
                break;
            }

            filters[count++] = new PagedListQueryFilter(filterSpan[..commaIndex]);
            filterSpan = filterSpan[(commaIndex + 1)..];
        }

        return filters;
    }

    /// <summary>
    /// Parses a comma-separated string of sort expressions into an array of <see cref="PagedListQuerySort"/>.
    /// </summary>
    /// <param name="sort">A comma-separated string of sort expressions in the format: property~direction.</param>
    /// <returns>An array of parsed sort conditions, or an empty array if the input is null or empty.</returns>
    private static PagedListQuerySort[] ParseSort(string? sort)
    {
        if (sort is not string sortString || string.IsNullOrEmpty(sortString))
        {
            return [];
        }

        ReadOnlySpan<char> sortSpan = sortString.AsSpan();
        int estimatedCount = sortSpan.Count(',') + 1;

        PagedListQuerySort[] sorts = new PagedListQuerySort[estimatedCount];
        int count = 0;

        while (true)
        {
            int commaIndex = sortSpan.IndexOf(',');
            if (commaIndex == -1)
            {
                sorts[count++] = new PagedListQuerySort(sortSpan);
                break;
            }

            sorts[count++] = new PagedListQuerySort(sortSpan[..commaIndex]);
            sortSpan = sortSpan[(commaIndex + 1)..];
        }

        return sorts;
    }

    /// <summary>
    /// Parses a JSON string into a <see cref="JsonObject"/> for graph loading configuration.
    /// </summary>
    /// <param name="graph">A JSON string representing the graph of related entities to load.</param>
    /// <returns>A parsed <see cref="JsonObject"/> if the input is valid JSON, otherwise null.</returns>
    private static JsonObject? ParseGraph(string? graph)
    {
        if (graph is not string graphString || string.IsNullOrEmpty(graphString))
        {
            return null;
        }

        try
        {
            return JsonNode.Parse(graphString)?.AsObject();
        }
        catch (System.Text.Json.JsonException)
        {
            return null;
        }
    }

}