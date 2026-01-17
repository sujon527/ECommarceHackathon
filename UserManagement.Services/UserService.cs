using UserManagement.Core.DTOs;
using UserManagement.Core.Entities;
using UserManagement.Core.Interfaces;
using BCrypt.Net;
using System.Text.RegularExpressions;

namespace UserManagement.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IEnumerable<IValidator<IUserValidationFields>> _validators;

    public UserService(IUserRepository userRepository, IEnumerable<IValidator<IUserValidationFields>> validators)
    {
        _userRepository = userRepository;
        _validators = validators;
    }

    public async Task<UserDto> RegisterUserAsync(CreateUserDto createUserDto)
    {
        // 1. Normalization
        createUserDto.Email = createUserDto.Email.Trim().ToLowerInvariant();
        createUserDto.PhoneNumber = NormalizePhoneNumber(createUserDto.PhoneNumber);

        // 2. Modular Validation
        await RunValidatorsAsync(createUserDto);

        // 3. User Creation & Hashing
        var user = new User
        {
            Username = createUserDto.Username,
            Email = createUserDto.Email,
            PhoneNumber = createUserDto.PhoneNumber,
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            DateOfBirth = createUserDto.DateOfBirth,
            DisplayName = createUserDto.DisplayName ?? $"{createUserDto.FirstName} {createUserDto.LastName}",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        await _userRepository.AddAsync(user);

        return MapToDto(user);
    }

    public async Task<UserDto> UpdateUserAsync(string id, UpdateUserDto updateUserDto)
    {
        // 1. Fetch User
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        // 2. Normalization
        updateUserDto.UserId = id;
        updateUserDto.PhoneNumber = NormalizePhoneNumber(updateUserDto.PhoneNumber);

        // 3. Modular Validation (Reuses Name, Age, Uniqueness validators)
        await RunValidatorsAsync(updateUserDto);

        // 4. Update Fields (Protecting Email and Password)
        user.FirstName = updateUserDto.FirstName;
        user.LastName = updateUserDto.LastName;
        user.PhoneNumber = updateUserDto.PhoneNumber;
        user.DateOfBirth = updateUserDto.DateOfBirth;
        user.DisplayName = updateUserDto.DisplayName ?? $"{updateUserDto.FirstName} {updateUserDto.LastName}";

        await _userRepository.UpdateAsync(user);

        return MapToDto(user);
    }

    private async Task RunValidatorsAsync(IUserValidationFields fields)
    {
        foreach (var validator in _validators)
        {
            var (isValid, errorMessage) = await validator.ValidateAsync(fields);
            if (!isValid)
            {
                throw new Exception(errorMessage);
            }
        }
    }

    private UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id!,
            Username = user.Username,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            FullName = $"{user.FirstName} {user.LastName}",
            DisplayName = user.DisplayName
        };
    }

    private string NormalizePhoneNumber(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return string.Empty;
        var digits = Regex.Replace(phone, @"[^\d]", "");
        if (digits.StartsWith("01") && digits.Length == 11) return "+88" + digits;
        if (digits.StartsWith("8801") && digits.Length == 13) return "+" + digits;
        return "+" + digits;
    }
}
