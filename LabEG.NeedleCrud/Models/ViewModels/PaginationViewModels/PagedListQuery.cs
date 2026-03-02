using System.Text.Json;
using System.Text.Json.Nodes;
using LabEG.NeedleCrud.Models.Exceptions;
using LabEG.NeedleCrud.Settings;

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
    /// <param name="settings">The NeedleCrudSettings for validation and limits. If null, default values are used.</param>
    /// <exception cref="NeedleCrudException">Thrown when validation fails or an unknown filter method is encountered.</exception>
    public PagedListQuery(
        int? pageSize,
        int? pageNumber,
        string? filter,
        string? sort,
        string? graph,
        NeedleCrudSettings? settings = null
    )
    {
        settings ??= new NeedleCrudSettings();

        // Validate pagination parameters early
        if (pageSize > settings.MaxPageSize)
        {
            throw new NeedleCrudException($"Page size cannot exceed {settings.MaxPageSize}. Requested: {pageSize ?? PageSize}");
        }

        if (pageNumber < 1)
        {
            throw new NeedleCrudException($"Page number must be at least 1. Requested: {pageNumber ?? PageNumber}");
        }

        PageSize = pageSize ?? PageSize;
        PageNumber = pageNumber ?? PageNumber;
        Filter = ParseFilters(filter, settings);
        Sort = ParseSort(sort, settings);
        Graph = ParseGraph(graph, settings);
    }

    /// <summary>
    /// Parses a comma-separated string of filter expressions into an array of <see cref="PagedListQueryFilter"/>.
    /// </summary>
    /// <param name="filter">A comma-separated string of filter expressions in the format: property~method~value.</param>
    /// <param name="settings">The NeedleCrudSettings for validation and limits. If null, default values are used.</param>
    /// <returns>An array of parsed filters, or an empty array if the input is null or empty.</returns>
    /// <exception cref="NeedleCrudException">Thrown when filter count exceeds MaxFilterCount.</exception>
    private static PagedListQueryFilter[] ParseFilters(string? filter, NeedleCrudSettings settings)
    {
        if (filter is not string filterString || string.IsNullOrEmpty(filterString))
        {
            return [];
        }

        ReadOnlySpan<char> filterSpan = filterString.AsSpan();
        int estimatedCount = filterSpan.Count(',') + 1;

        // Validate before parsing to fail fast
        if (estimatedCount > settings.MaxFilterCount)
        {
            throw new NeedleCrudException($"Filter count cannot exceed {settings.MaxFilterCount}. Requested: {estimatedCount}");
        }

        PagedListQueryFilter[] filters = new PagedListQueryFilter[estimatedCount];
        int count = 0;

        while (true)
        {
            // Validate during parsing to catch issues early
            if (count >= settings.MaxFilterCount)
            {
                throw new NeedleCrudException($"Filter count cannot exceed {settings.MaxFilterCount}. Requested: {count + 1}");
            }

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
    /// <param name="settings">The NeedleCrudSettings for validation and limits. If null, default values are used.</param>
    /// <returns>An array of parsed sort conditions, or an empty array if the input is null or empty.</returns>
    /// <exception cref="NeedleCrudException">Thrown when sort count exceeds MaxSortCount.</exception>
    private static PagedListQuerySort[] ParseSort(string? sort, NeedleCrudSettings settings)
    {
        if (sort is not string sortString || string.IsNullOrEmpty(sortString))
        {
            return [];
        }

        ReadOnlySpan<char> sortSpan = sortString.AsSpan();
        int estimatedCount = sortSpan.Count(',') + 1;

        // Validate before parsing to fail fast
        if (estimatedCount > settings.MaxSortCount)
        {
            throw new NeedleCrudException($"Sort count cannot exceed {settings.MaxSortCount}. Requested: {estimatedCount}");
        }

        PagedListQuerySort[] sorts = new PagedListQuerySort[estimatedCount];
        int count = 0;

        while (true)
        {
            // Validate during parsing to catch issues early
            if (count >= settings.MaxSortCount)
            {
                throw new NeedleCrudException($"Sort count cannot exceed {settings.MaxSortCount}. Requested: {count + 1}");
            }

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
    /// Parses a JSON string into a <see cref="JsonObject"/> for graph loading configuration with depth validation.
    /// </summary>
    /// <param name="graph">A JSON string representing the graph of related entities to load.</param>
    /// <param name="settings">The NeedleCrudSettings for validation and limits.</param>
    /// <returns>A parsed <see cref="JsonObject"/> if the input is valid JSON and within depth limits, otherwise null.</returns>
    public static JsonObject? ParseGraph(string? graph, NeedleCrudSettings settings)
    {
        if (graph is not string graphString || string.IsNullOrEmpty(graphString))
        {
            return null;
        }

        try
        {
            JsonDocumentOptions documentOptions = new()
            {
                MaxDepth = settings.MaxGraphDepth
            };

            JsonNode? node = JsonNode.Parse(graphString, documentOptions: documentOptions);
            if (node is JsonObject obj)
            {
                return obj;
            }
            throw new NeedleCrudException(
                "Graph must be a JSON object with null values for leaf nodes. " +
                "Example format: {\"author\": {\"books\": {\"category\": null}}, \"category\": {\"books\": null}}");
        }
        catch (JsonException ex)
        {
            throw new NeedleCrudException($"Invalid JSON in graph parameter: {ex.Message}");
        }
    }
}