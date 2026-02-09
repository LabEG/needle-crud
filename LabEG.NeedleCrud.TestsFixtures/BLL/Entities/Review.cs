using LabEG.NeedleCrud.Models.Entities;

namespace LabEG.NeedleCrud.Benchmarks.BLL.Entities;

/// <summary>
/// User review for a book
/// </summary>
public class Review : IEntity<Guid>
{
    /// <summary>
    /// Unique identifier for the review
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key to the user who wrote the review
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Foreign key to the reviewed book
    /// </summary>
    public Guid BookId { get; set; }

    /// <summary>
    /// Rating score (typically 1-5)
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// Review comment text
    /// </summary>
    public string Comment { get; set; } = string.Empty;

    /// <summary>
    /// Date when the review was written
    /// </summary>
    public DateTime ReviewDate { get; set; }

    /// <summary>
    /// Indicates whether the review has been verified by an administrator
    /// </summary>
    public bool IsVerified { get; set; }

    /// <summary>
    /// Navigation property to the user who wrote the review
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Navigation property to the reviewed book
    /// </summary>
    public Book Book { get; set; } = null!;
}
