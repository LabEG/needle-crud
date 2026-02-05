namespace LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;

public class PagedList<TEntity>
{
    public PageMeta PageMeta { get; set; } = new PageMeta();
    public IList<TEntity> Elements { get; set; } = [];
}