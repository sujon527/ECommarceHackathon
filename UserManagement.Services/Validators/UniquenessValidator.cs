using UserManagement.Core.Interfaces;

namespace UserManagement.Services.Validators;

public class UniquenessValidator : IValidator<IUserValidationFields>
{
    private readonly IUserRepository _userRepository;

    public UniquenessValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<(bool IsValid, string? ErrorMessage)> ValidateAsync(IUserValidationFields context)
    {
        var normalizedEmail = context.Email?.Trim().ToLowerInvariant();
        var normalizedPhone = context.PhoneNumber;

        // 1. Email Uniqueness (Only if Email is provided, which is only during Create)
        if (!string.IsNullOrEmpty(normalizedEmail))
        {
            var existingByEmail = await _userRepository.GetByEmailIncludingDeletedAsync(normalizedEmail);
            if (existingByEmail != null && (string.IsNullOrEmpty(context.UserId) || existingByEmail.Id != context.UserId))
            {
                return (false, "Registration blocked. Email already exists (may be inactive). Please contact admin.");
            }
        }

        // 2. Phone Uniqueness (Always check if provided)
        if (!string.IsNullOrEmpty(normalizedPhone))
        {
            var existingByPhone = await _userRepository.GetByPhoneNumberIncludingDeletedAsync(normalizedPhone);
            if (existingByPhone != null && (string.IsNullOrEmpty(context.UserId) || existingByPhone.Id != context.UserId))
            {
                return (false, "Update/Registration blocked. Phone number already exists (may be inactive). Please contact admin.");
            }
        }

        return (true, null);
    }
}
