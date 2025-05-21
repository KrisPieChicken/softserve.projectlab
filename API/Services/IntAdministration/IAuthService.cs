using softserve.projectlabs.Shared.DTOs.User;
using softserve.projectlabs.Shared.Utilities;
using API.Models.IntAdmin;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.DTOs.Auth;

namespace API.Services.IntAdmin;

public interface IAuthService
{
    Task<Result<AuthResponseDto>> LoginAsync(UserLoginDto dto);
    Task<Result<bool>> LogoutAsync();
}
