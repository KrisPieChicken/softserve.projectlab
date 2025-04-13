using API.Data.Entities;
using API.Implementations.Domain;
using API.Models.IntAdmin;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;
using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.DTOs;

namespace API.Services.IntAdmin
{
    /// <summary>
    /// Service class for user operations. Delegates business logic to the UserDomain.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserDomain _userDomain;
        public UserService(UserDomain userDomain)
        {
            _userDomain = userDomain;
        }
        public async Task<Result<User>> CreateUserAsync(UserDto userDto)
        {
            return await _userDomain.CreateUserAsync(userDto);
        }
        public async Task<Result<User>> UpdateUserAsync(int userId, UserDto userDto)
        {
            return await _userDomain.UpdateUserAsync(userId, userDto);
        }
        public async Task<Result<User>> GetUserByIdAsync(int userId)
        {
            return await _userDomain.GetUserByIdAsync(userId);
        }
        public async Task<Result<List<User>>> GetAllUsersAsync()
        {
            return await _userDomain.GetAllUsersAsync();
        }
        public async Task<Result<bool>> DeleteUserAsync(int userId)
        {
            return await _userDomain.DeleteUserAsync(userId);
        }
        public async Task<Result<bool>> AuthenticateAsync(string email, string password)
        {
            return await _userDomain.AuthenticateAsync(email, password);
        }
        public async Task<Result<bool>> UpdatePasswordAsync(int userId, string newPassword)
        {
            return await _userDomain.UpdatePasswordAsync(userId, newPassword);
        }
        public async Task<Result<bool>> AssignRolesAsync(int userId, List<int> roleIds)
        {
            return await _userDomain.AssignRolesAsync(userId, roleIds);
        }
    }
}
