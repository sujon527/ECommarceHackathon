namespace UserManagement.Core.Interfaces;

public interface IUserValidationFields
{
    string Email { get; }
    string PhoneNumber { get; }
    string FirstName { get; }
    string LastName { get; }
    DateTime? DateOfBirth { get; }
    string? Password { get; } // Optional: only for Create
    string? UserId { get; } // Optional: only for Update (to exclude self from uniqueness checks)
}
