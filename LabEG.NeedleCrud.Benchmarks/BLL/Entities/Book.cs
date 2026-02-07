using LabEG.NeedleCrud.Models.Entities;

namespace LabEG.NeedleCrud.Benchmarks.BLL.Entities;

/// <summary>
/// Book in the library
/// </summary>
public class Book : IEntity<int>
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string ISBN { get; set; } = string.Empty;

    public int AuthorId { get; set; }

    public int CategoryId { get; set; }

    public DateTime PublicationDate { get; set; }

    public int PageCount { get; set; }

    public string Publisher { get; set; } = string.Empty;

    public string Language { get; set; } = string.Empty;

    public bool IsAvailable { get; set; }
}
