using LabEG.NeedleCrud.Models.Entities;

namespace LabEG.NeedleCrud.Benchmarks.BLL.Entities;

/// <summary>
/// Book category
/// </summary>
public class Category : IEntity<Guid>
{
    /// <summary>
    /// Unique identifier for the category
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Category name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the category
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Order in which the category should be displayed
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Indicates whether the category is active and visible
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Collection of books in this category
    /// </summary>
    public ICollection<Book> Books { get; set; } = [];
}
