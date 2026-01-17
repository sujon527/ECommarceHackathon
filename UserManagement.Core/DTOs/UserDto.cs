using UserManagement.Core.Interfaces;

namespace UserManagement.Core.DTOs;

public class CreateUserDto : IUserValidationFields
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? DisplayName { get; set; }
    public string Password { get; set; } = string.Empty;

    string? IUserValidationFields.UserId => null;
}

public class UpdateUserDto : IUserValidationFields
{
    public string? UserId { get; set; } // Set by the service/controller
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? DisplayName { get; set; }

    // Read-only/Protected fields for validation reuse
    string IUserValidationFields.Email => string.Empty; // Not updatable, so uniqueness check should ignore if empty
    string? IUserValidationFields.Password => null;
}

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
}
