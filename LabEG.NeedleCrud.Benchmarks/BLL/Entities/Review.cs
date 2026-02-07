using LabEG.NeedleCrud.Models.Entities;

namespace LabEG.NeedleCrud.Benchmarks.BLL.Entities;

/// <summary>
/// User review for a book
/// </summary>
public class Review : IEntity<int>
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; } = string.Empty;

    public DateTime ReviewDate { get; set; }

    public bool IsVerified { get; set; }
}
