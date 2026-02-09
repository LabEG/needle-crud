using LabEG.NeedleCrud.Models.Entities;

namespace LabEG.NeedleCrud.Benchmarks.BLL.Entities;

/// <summary>
/// Book in the library
/// </summary>
public class Book : IEntity<Guid>
{
    /// <summary>
    /// Unique identifier for the book
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Book title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// International Standard Book Number
    /// </summary>
    public string ISBN { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key to the book's author
    /// </summary>
    public Guid AuthorId { get; set; }

    /// <summary>
    /// Foreign key to the book's category
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Date when the book was published
    /// </summary>
    public DateTime PublicationDate { get; set; }

    /// <summary>
    /// Number of pages in the book
    /// </summary>
    public int PageCount { get; set; }

    /// <summary>
    /// Book publisher name
    /// </summary>
    public string Publisher { get; set; } = string.Empty;

    /// <summary>
    /// Language in which the book is written
    /// </summary>
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the book is available for loan
    /// </summary>
    public bool IsAvailable { get; set; }

    /// <summary>
    /// Navigation property to the book's author
    /// </summary>
    public Author Author { get; set; } = null!;

    /// <summary>
    /// Navigation property to the book's category
    /// </summary>
    public Category Category { get; set; } = null!;

    /// <summary>
    /// Collection of loans for this book
    /// </summary>
    public ICollection<Loan> Loans { get; set; } = [];

    /// <summary>
    /// Collection of reviews for this book
    /// </summary>
    public ICollection<Review> Reviews { get; set; } = [];
}
