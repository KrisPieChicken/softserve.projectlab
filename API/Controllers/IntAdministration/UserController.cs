using softserve.projectlabs.Shared.DTOs;
using API.Models.IntAdmin;
using API.Services.IntAdmin;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.IntAdmin
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            var result = await _userService.CreateUserAsync(userDto);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserDto userDto)
        {
            var result = await _userService.UpdateUserAsync(userId, userDto);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var result = await _userService.GetUserByIdAsync(userId);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        [HttpPut("assign-roles/{userId}")]
        public async Task<IActionResult> AssignRoles(int userId, [FromBody] List<int> roleIds)
        {
            var result = await _userService.AssignRolesAsync(userId, roleIds);
            return result.IsSuccess ? Ok("Roles assigned successfully.") : BadRequest(result.ErrorMessage);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserDto userDto)
        {
            var result = await _userService.AuthenticateAsync(userDto.UserEmail, userDto.UserPassword);
            return result.IsSuccess ? Ok("Authenticated") : Unauthorized(result.ErrorMessage);
        }

        [HttpPut("update-password/{userId}")]
        public async Task<IActionResult> UpdatePassword(int userId, [FromBody] string newPassword)
        {
            var result = await _userService.UpdatePasswordAsync(userId, newPassword);
            return result.IsSuccess ? Ok("Password updated") : BadRequest(result.ErrorMessage);
        }
    }
}
