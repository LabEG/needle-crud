using LabEG.NeedleCrud.Models.Entities;

namespace LabEG.NeedleCrud.Benchmarks.BLL.Entities;

/// <summary>
/// Library user
/// </summary>
public class User : IEntity<Guid>
{
    /// <summary>
    /// Unique identifier for the user
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User's first name
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User's last name
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// User's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's phone number
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Date when the user registered in the library system
    /// </summary>
    public DateTime RegistrationDate { get; set; }

    /// <summary>
    /// Indicates whether the user account is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// User's physical address
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Collection of book loans associated with this user
    /// </summary>
    public ICollection<Loan> Loans { get; set; } = [];

    /// <summary>
    /// Collection of book reviews written by this user
    /// </summary>
    public ICollection<Review> Reviews { get; set; } = [];
}
