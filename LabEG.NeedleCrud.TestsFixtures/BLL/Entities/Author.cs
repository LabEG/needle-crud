using LabEG.NeedleCrud.Models.Entities;

namespace LabEG.NeedleCrud.Benchmarks.BLL.Entities;

/// <summary>
/// Book author
/// </summary>
public class Author : IEntity<Guid>
{
    /// <summary>
    /// Unique identifier for the author
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Author's first name
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Author's last name
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Author's date of birth (optional)
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Country where the author is from
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// Biography or description of the author
    /// </summary>
    public string Biography { get; set; } = string.Empty;

    /// <summary>
    /// Collection of books written by this author
    /// </summary>
    public ICollection<Book> Books { get; set; } = [];
}
