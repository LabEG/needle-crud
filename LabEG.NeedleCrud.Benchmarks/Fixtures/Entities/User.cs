using LabEG.NeedleCrud.Models.Entities;

namespace LabEG.NeedleCrud.Benchmarks.Fixtures.Entities;

/// <summary>
/// Library user
/// </summary>
public class User : IEntity<int>
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public DateTime RegistrationDate { get; set; }

    public bool IsActive { get; set; }

    public string Address { get; set; } = string.Empty;
}
