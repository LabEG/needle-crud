using LabEG.NeedleCrud.Models.Entities;

namespace LabEG.NeedleCrud.Benchmarks.BLL.Entities;

/// <summary>
/// Book loan to user
/// </summary>
public class Loan : IEntity<int>
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }

    public DateTime LoanDate { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public bool IsReturned { get; set; }

    public decimal? LateFee { get; set; }

    public string Notes { get; set; } = string.Empty;
}
