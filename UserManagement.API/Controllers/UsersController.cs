using Microsoft.AspNetCore.Mvc;
using UserManagement.Core.DTOs;
using UserManagement.Core.Interfaces;

namespace UserManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
    {
        try
        {
            var user = await _userService.RegisterUserAsync(createUserDto);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDto updateUserDto)
    {
        try
        {
            var user = await _userService.UpdateUserAsync(id, updateUserDto);
            return Ok(user);
        }
        catch (Exception ex)
        {
            if (ex.Message == "User not found.")
                return NotFound(new { message = ex.Message });
            
            return BadRequest(new { message = ex.Message });
        }
    }
}
