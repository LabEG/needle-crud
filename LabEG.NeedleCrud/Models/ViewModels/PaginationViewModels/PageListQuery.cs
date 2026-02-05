using Newtonsoft.Json.Linq;

namespace LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;

public class PagedListQuery
{
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;

    public IList<PagedListQueryFilter> Filter { get; set; }
    public IList<PagedListQuerySort> Sort { get; set; }
    public JObject Graph { get; set; }
}