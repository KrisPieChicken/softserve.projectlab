using AutoMapper;
using API.Models.IntAdmin;
using softserve.projectlabs.Shared.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Implementations.Domain;
using softserve.projectlabs.Shared.DTOs.User;

namespace API.Services.IntAdmin;

public class UserService : IUserService
{
    private readonly UserDomain _userDomain;
    private readonly IMapper _mapper;

    public UserService(UserDomain userDomain, IMapper mapper)
    {
        _userDomain = userDomain;
        _mapper = mapper;
    }

    public async Task<Result<User>> CreateUserAsync(UserCreateDto dto)
    {
        var domainModel = _mapper.Map<User>(dto);
        return await _userDomain.CreateUserAsync(domainModel, plainPassword: dto.UserPassword);
    }

    public async Task<Result<User>> GetUserByIdAsync(int userId)
    {
        return await _userDomain.GetUserByIdAsync(userId);
    }

    public async Task<Result<List<User>>> GetAllUsersAsync()
    {
        return await _userDomain.GetAllUsersAsync();
    }

    public async Task<Result<User>> UpdateUserAsync(int userId, UserUpdateDto dto)
    {
        var domainModel = _mapper.Map<User>(dto);
        return await _userDomain.UpdateUserAsync(userId, domainModel, plainPassword: dto.UserPassword);
    }

    public async Task<Result<bool>> DeleteUserAsync(int userId)
    {
        return await _userDomain.DeleteUserAsync(userId);
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
