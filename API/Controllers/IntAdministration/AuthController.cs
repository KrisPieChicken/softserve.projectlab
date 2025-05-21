using Microsoft.AspNetCore.Mvc;
using softserve.projectlabs.Shared.DTOs.User;
using API.Services.IntAdmin;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.IntAdmin;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Obtains a new access token by using the credentials of the user.
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        var res = await _authService.LoginAsync(dto);
        return res.IsSuccess
            ? Ok(res.Data)
            : Unauthorized(res.ErrorMessage);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var res = await _authService.LogoutAsync();
        return res.IsSuccess ? NoContent() : BadRequest(res.ErrorMessage);
    }
}
