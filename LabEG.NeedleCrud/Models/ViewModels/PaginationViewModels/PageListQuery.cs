using System.Web;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the page number to retrieve (1-based index).
    /// Default value is 1.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the list of filter conditions to apply to the query.
    /// </summary>
    public IList<PagedListQueryFilter>? Filter { get; set; } = null;

    /// <summary>
    /// Gets or sets the list of sort conditions to apply to the query.
    /// </summary>
    public IList<PagedListQuerySort>? Sort { get; set; } = null;

    /// <summary>
    /// Gets or sets the graph expression as a JSON object for loading related entities (eager loading).
    /// </summary>
    public JObject? Graph { get; set; } = null;

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
    /// <exception cref="BadHttpRequestException">Thrown when an unknown filter method is encountered.</exception>
    public PagedListQuery(
        int? pageSize,
        int? pageNumber,
        string? filter,
        string? sort,
        string? graph
    )
    {
        if (pageSize is int pageSizeInt)
        {
            PageSize = pageSizeInt;
        }

        if (pageNumber is int pageNumberInt)
        {
            PageNumber = pageNumberInt;
        }

        if (filter is string filterString)
        {
            Filter ??= [];

            if (!string.IsNullOrEmpty(filter))
            {
                string[] filterGroups = filterString.Split(',');
                foreach (string filterItem in filterGroups)
                {
                    string[] filterSeparated = filterItem.Split('~');
                    if (filterSeparated.Length == 3)
                    {
                        Filter.Add(new PagedListQueryFilter()
                        {
                            Property = filterSeparated[0].First().ToString().ToUpper() + string.Join("", filterSeparated[0].Skip(1)),
                            Method = ParseFilterMethod(filterSeparated[1]),
                            Value = HttpUtility.UrlDecode(filterSeparated[2])
                        });
                    }
                }
            }
        }

        if (sort is string sortString)
        {
            Sort ??= [];

            if (!string.IsNullOrEmpty(sort))
            {
                string[] sortGroups = sortString.Split(',');
                foreach (string sortItem in sortGroups)
                {
                    string[] sortSeparated = sortItem.Split('~');
                    if (sortSeparated.Length == 2)
                    {
                        Sort.Add(new PagedListQuerySort()
                        {
                            Property = sortSeparated[0].First().ToString().ToUpper() + string.Join("", sortSeparated[0].Skip(1)),
                            Direction = sortSeparated[1].ToLower() == "asc" ? PagedListQuerySortDirection.Asc : PagedListQuerySortDirection.Desc
                        });
                    }
                }
            }
        }

        if (graph is string graphString)
        {
            Graph = JsonConvert.DeserializeObject(graphString) as JObject;
        }
    }

    /// <summary>
    /// Parses a filter method string and converts it to the corresponding <see cref="PagedListQueryFilterMethod"/> enum value.
    /// </summary>
    /// <param name="method">The filter method string. Supported values: "&lt;", "&lt;=", "&gt;=", "&gt;", "like", "ilike", "=".</param>
    /// <returns>The corresponding <see cref="PagedListQueryFilterMethod"/> enum value.</returns>
    /// <exception cref="BadHttpRequestException">Thrown when the <paramref name="method"/> is not recognized.</exception>
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
            _ => throw new BadHttpRequestException("Unknown filter method"),
        };
    }
}