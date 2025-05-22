using Microsoft.AspNetCore.Mvc;
using API.Services.IntAdmin;
using softserve.projectlabs.Shared.DTOs.User;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.IntAdmin;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Creates a new User.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
    {
        var result = await _userService.CreateUserAsync(dto);
        return result.IsSuccess
            ? Ok(result.Data)
            : BadRequest(result.ErrorMessage);
    }


    /// <summary>
    /// Obtains a User by ID.
    /// </summary>
    [HttpGet("{userId}")]
    [Authorize]
    public async Task<IActionResult> GetUserById(int userId)
    {
        var result = await _userService.GetUserByIdAsync(userId);
        return result.IsSuccess 
            ? Ok(result.Data)
            : NotFound(result.ErrorMessage);
    }

    /// <summary>
    /// Obtains all Users.
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _userService.GetAllUsersAsync();
        return result.IsSuccess 
            ? Ok(result.Data) 
            : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Updates an existing User.
    /// </summary>
    [HttpPut("{userId}")]
    [Authorize]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserUpdateDto userDto)
    {
        var result = await _userService.UpdateUserAsync(userId, userDto);
        return result.IsSuccess 
            ? Ok(result.Data)
            : NotFound(result.ErrorMessage);
    }

    /// <summary>
    /// Deletes an existing User by ID. (Using soft delete)
    /// </summary>
    [HttpDelete("{userId}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var result = await _userService.DeleteUserAsync(userId);
        return result.IsSuccess ? NoContent() : NotFound(result.ErrorMessage);
    }

    /// <summary>
    /// Assigns roles to a user.
    /// </summary>
    [HttpPost("{userId}/roles")]
    [Authorize]
    public async Task<IActionResult> AssignRolesToUser(int userId, [FromBody] List<int> roleIds)
    {
        var result = await _userService.AssignRolesAsync(userId, roleIds);
        return result.IsSuccess 
            ? Ok("Roles assigned successfully.") 
            : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Changes the password of a user.
    /// </summary>
    [HttpPatch("{userId}/password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(int userId, [FromBody] string newPassword)
    {
        var result = await _userService.UpdatePasswordAsync(userId, newPassword);
        return result.IsSuccess 
            ? Ok("Password updated") 
            : BadRequest(result.ErrorMessage);
    }
}
