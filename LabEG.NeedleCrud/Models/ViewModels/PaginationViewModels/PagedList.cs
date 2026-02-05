namespace LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;

/// <summary>
/// Represents a paginated list of entities with metadata about the pagination.
/// </summary>
/// <typeparam name="TEntity">The type of entities contained in the list.</typeparam>
public class PagedList<TEntity>
{
    /// <summary>
    /// Gets or sets the pagination metadata including page number, size, and total counts.
    /// </summary>
    public PageMeta PageMeta { get; set; } = new PageMeta();

    /// <summary>
    /// Gets or sets the collection of entities for the current page.
    /// </summary>
    public IList<TEntity> Elements { get; set; } = [];
}