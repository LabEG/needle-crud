using System.Web;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;

public class PagedListQuery
{
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;

    public IList<PagedListQueryFilter>? Filter { get; set; } = null;
    public IList<PagedListQuerySort>? Sort { get; set; } = null;
    public JObject? Graph { get; set; } = null;

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