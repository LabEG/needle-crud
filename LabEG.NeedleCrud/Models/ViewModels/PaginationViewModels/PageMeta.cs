namespace LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;

public class PageMeta
{
    public long PageNumber { get; set; } = 0;
    public long PageSize { get; set; } = 0;
    public long ElementsInPage { get; set; } = 0;
    public long TotalPages { get; set; } = 0;
    public long TotalElements { get; set; } = 0;
}