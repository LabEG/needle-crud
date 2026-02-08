using LabEG.NeedleCrud.Models.Entities;

namespace LabEG.NeedleCrud.Benchmarks.BLL.Entities;

/// <summary>
/// Book in the library
/// </summary>
public class Book : IEntity<Guid>
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string ISBN { get; set; } = string.Empty;

    public Guid AuthorId { get; set; }

    public Guid CategoryId { get; set; }

    public DateTime PublicationDate { get; set; }

    public int PageCount { get; set; }

    public string Publisher { get; set; } = string.Empty;

    public string Language { get; set; } = string.Empty;

    public bool IsAvailable { get; set; }

    // Navigation properties
    public Author Author { get; set; } = null!;

    public Category Category { get; set; } = null!;

    public ICollection<Loan> Loans { get; set; } = [];

    public ICollection<Review> Reviews { get; set; } = [];
}
