using API.Data.Entities;
using API.Models;
using API.Models.IntAdmin;
using System.Collections.Generic;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.DTOs;

namespace API.Services.IntAdmin
{
    /// <summary>
    /// Service interface for user operations.
    /// </summary>
    public interface IUserService
    {
        Task<Result<User>> CreateUserAsync(UserDto userDto);
        Task<Result<User>> UpdateUserAsync(int userId, UserDto userDto);
        Task<Result<User>> GetUserByIdAsync(int userId);
        Task<Result<List<User>>> GetAllUsersAsync();
        Task<Result<bool>> DeleteUserAsync(int userId);

        Task<Result<bool>> AuthenticateAsync(string email, string password);
        Task<Result<bool>> UpdatePasswordAsync(int userId, string newPassword);
        Task<Result<bool>> AssignRolesAsync(int userId, List<int> roleIds);
    }
}
