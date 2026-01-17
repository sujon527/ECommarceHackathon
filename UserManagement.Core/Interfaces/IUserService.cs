using UserManagement.Core.DTOs;

namespace UserManagement.Core.Interfaces;

public interface IUserService
{
    Task<UserDto> RegisterUserAsync(CreateUserDto createUserDto);
    Task<UserDto> UpdateUserAsync(string id, UpdateUserDto updateUserDto);
}
