using Microsoft.AspNetCore.Mvc;
using softserve.projectlabs.Shared.DTOs.User;
using API.Services.IntAdmin;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.IntAdmin;

/// <summary>
/// API Controller for managing authentication operations.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Constructor with dependency injection for IAuthService.
    /// </summary>
    /// <param name="authService">The authentication service instance</param>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Authenticates a user and issues an access token.
    /// </summary>
    /// <param name="dto">Login credentials containing email and password</param>
    /// <returns>HTTP response with authentication token or error message</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        var res = await _authService.LoginAsync(dto);
        return res.IsSuccess
            ? Ok(res.Data)
            : Unauthorized(res.ErrorMessage);
    }

    /// <summary>
    /// Logs out the currently authenticated user.
    /// </summary>
    /// <returns>HTTP response indicating success or failure</returns>
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var res = await _authService.LogoutAsync();
        return res.IsSuccess
            ? NoContent()
            : BadRequest(res.ErrorMessage);
    }
}
