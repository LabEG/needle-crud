using LabEG.NeedleCrud.Models.Entities;

namespace LabEG.NeedleCrud.Benchmarks.Fixtures.Entities;

/// <summary>
/// Book author
/// </summary>
public class Author : IEntity<int>
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime? BirthDate { get; set; }

    public string Country { get; set; } = string.Empty;

    public string Biography { get; set; } = string.Empty;
}
