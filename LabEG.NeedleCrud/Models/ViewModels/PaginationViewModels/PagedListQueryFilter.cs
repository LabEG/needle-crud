namespace LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;

public class PagedListQueryFilter
{
    public string Property { get; set; }
    public string Value { get; set; }
    public PagedListQueryFilterMethod Method { get; set; }
}