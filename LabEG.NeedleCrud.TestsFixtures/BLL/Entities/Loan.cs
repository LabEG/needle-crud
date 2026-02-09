using LabEG.NeedleCrud.Models.Entities;

namespace LabEG.NeedleCrud.Benchmarks.BLL.Entities;

/// <summary>
/// Book loan to user
/// </summary>
public class Loan : IEntity<Guid>
{
    /// <summary>
    /// Unique identifier for the loan
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key to the user who borrowed the book
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Foreign key to the borrowed book
    /// </summary>
    public Guid BookId { get; set; }

    /// <summary>
    /// Date when the book was loaned
    /// </summary>
    public DateTime LoanDate { get; set; }

    /// <summary>
    /// Date when the book should be returned
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Actual return date (null if not yet returned)
    /// </summary>
    public DateTime? ReturnDate { get; set; }

    /// <summary>
    /// Indicates whether the book has been returned
    /// </summary>
    public bool IsReturned { get; set; }

    /// <summary>
    /// Late fee charged if book was returned after due date (optional)
    /// </summary>
    public decimal? LateFee { get; set; }

    /// <summary>
    /// Additional notes about the loan
    /// </summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property to the user who borrowed the book
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Navigation property to the borrowed book
    /// </summary>
    public Book Book { get; set; } = null!;
}
