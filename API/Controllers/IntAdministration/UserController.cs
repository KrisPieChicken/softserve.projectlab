using Microsoft.AspNetCore.Mvc;
using API.Services.IntAdmin;
using softserve.projectlabs.Shared.DTOs.User;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.IntAdmin;

/// <summary>
/// API Controller for managing User operations.
/// </summary>
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    /// <summary>
    /// Constructor with dependency injection for IUserService.
    /// </summary>
    /// <param name="userService">The user service instance</param>
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="dto">User data transfer object containing creation information</param>
    /// <returns>HTTP response with the created user or error message</returns>
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
    {
        var result = await _userService.CreateUserAsync(dto);
        return result.IsSuccess
            ? Ok(result.Data)
            : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Retrieves a user by their unique ID.
    /// </summary>
    /// <param name="userId">Unique identifier of the user</param>
    /// <returns>HTTP response with the user or error message</returns>
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
    /// Retrieves all users.
    /// </summary>
    /// <returns>HTTP response with the list of users or error message</returns>
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
    /// Updates an existing user.
    /// </summary>
    /// <param name="userId">The ID of the user to update</param>
    /// <param name="userDto">The updated user data</param>
    /// <returns>HTTP response with the updated user or error message</returns>
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
    /// Removes a user by their unique ID. (Using soft delete)
    /// </summary>
    /// <param name="userId">Unique identifier of the user to remove</param>
    /// <returns>HTTP response indicating success or failure</returns>
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
    /// <param name="userId">The ID of the user to assign roles to</param>
    /// <param name="roleIds">List of role IDs to assign to the user</param>
    /// <returns>HTTP response indicating success or failure</returns>
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
    /// <param name="userId">The ID of the user whose password will be changed</param>
    /// <param name="newPassword">The new password</param>
    /// <returns>HTTP response indicating success or failure</returns>
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
