using API.Models.IntAdmin;
using softserve.projectlabs.Shared.DTOs.User;
using softserve.projectlabs.Shared.Utilities;

namespace API.Services.IntAdmin;

public interface IUserService
{
    Task<Result<User>> CreateUserAsync(UserCreateDto dto);
    Task<Result<User>> GetUserByIdAsync(int userId);
    Task<Result<List<User>>> GetAllUsersAsync();
    Task<Result<User>> UpdateUserAsync(int userId, UserUpdateDto dto);
    Task<Result<bool>> DeleteUserAsync(int userId);
    Task<Result<bool>> UpdatePasswordAsync(int userId, string newPassword);
    Task<Result<bool>> AssignRolesAsync(int userId, List<int> roleIds);
}
