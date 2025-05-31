using AutoMapper;
using API.Data.Entities;
using API.Models.IntAdmin;
using softserve.projectlabs.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Repositories.IntAdministrationRepository.Interfaces;
using API.Utils;

namespace API.Implementations.Domain;

public class UserDomain
{
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;

    public UserDomain(IUserRepository userRepo, IMapper mapper)
    {
        _userRepo = userRepo;
        _mapper = mapper;
    }

    public async Task<Result<User>> CreateUserAsync(User model, string plainPassword)
    {
        try
        {
            PasswordHelper.CreatePasswordHash(plainPassword, out byte[] hash, out byte[] salt);

            var entity = _mapper.Map<UserEntity>(model);
            entity.PasswordHash = hash;
            entity.PasswordSalt = salt;
            entity.UserStatus = true;
            entity.CreatedAt = DateTime.UtcNow;

            var savedEntity = await _userRepo.AddAsync(entity);

            foreach (var role in model.Roles)
            {
                await _userRepo.AddUserRoleAsync(new UserRoleEntity
                {
                    UserId = savedEntity.UserId,
                    RoleId = role.RoleId
                });
            }

            var fullEntity = await _userRepo.GetByIdAsync(savedEntity.UserId);
            return Result<User>.Success(_mapper.Map<User>(fullEntity!));
        }
        catch (Exception ex)
        {
            return Result<User>.Failure($"Error al crear usuario: {ex.Message}");
        }
    }

    public async Task<Result<User>> UpdateUserAsync(int id, User model, string? plainPassword = null)
    {
        try
        {
            var entity = await _userRepo.GetByIdAsync(id);
            if (entity == null)
                return Result<User>.Failure("Usuario no encontrado.");

            _mapper.Map(model, entity);
            entity.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(plainPassword))
            {
                PasswordHelper.CreatePasswordHash(plainPassword, out var hash, out var salt);
                entity.PasswordHash = hash;
                entity.PasswordSalt = salt;
            }

            await _userRepo.UpdateAsync(entity);

            var existing = await _userRepo.GetUserRolesAsync(id);
            var existingIds = existing.Select(ur => ur.RoleId).ToHashSet();
            var incomingIds = model.Roles.Select(r => r.RoleId).ToHashSet();

            foreach (var ur in existing.Where(ur => !incomingIds.Contains(ur.RoleId)))
                await _userRepo.RemoveUserRoleAsync(ur);

            foreach (var roleId in incomingIds.Except(existingIds))
                await _userRepo.AddUserRoleAsync(new UserRoleEntity
                {
                    UserId = id,
                    RoleId = roleId
                });

            var updated = await _userRepo.GetByIdAsync(id);
            return Result<User>.Success(_mapper.Map<User>(updated!));
        }
        catch (Exception ex)
        {
            return Result<User>.Failure($"Error al actualizar usuario: {ex.Message}");
        }
    }

    public async Task<Result<User>> GetUserByIdAsync(int id)
    {
        try
        {
            var entity = await _userRepo.GetByIdAsync(id);
            return entity == null
                ? Result<User>.Failure("Usuario no encontrado.")
                : Result<User>.Success(_mapper.Map<User>(entity));
        }
        catch (Exception ex)
        {
            return Result<User>.Failure($"Error al obtener usuario: {ex.Message}");
        }
    }

    public async Task<Result<List<User>>> GetAllUsersAsync()
    {
        try
        {
            var entities = await _userRepo.GetAllAsync();
            return Result<List<User>>.Success(_mapper.Map<List<User>>(entities));
        }
        catch (Exception ex)
        {
            return Result<List<User>>.Failure($"Error al listar usuarios: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteUserAsync(int id)
    {
        try
        {
            var exists = await _userRepo.ExistsAsync(id);
            if (!exists)
                return Result<bool>.Failure("Usuario no encontrado.");

            await _userRepo.DeleteAsync(id);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Error al eliminar usuario: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UpdatePasswordAsync(int id, string newPassword)
    {
        try
        {
            var entity = await _userRepo.GetByIdAsync(id);
            if (entity == null)
                return Result<bool>.Failure("Usuario no encontrado.");

            PasswordHelper.CreatePasswordHash(newPassword, out var hash, out var salt);
            entity.PasswordHash = hash;
            entity.PasswordSalt = salt;
            entity.UpdatedAt = DateTime.UtcNow;

            await _userRepo.UpdateAsync(entity);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Error al actualizar contraseña: {ex.Message}");
        }
    }

    public async Task<Result<bool>> AssignRolesAsync(int id, List<int> roleIds)
    {
        try
        {
            var existing = await _userRepo.GetUserRolesAsync(id);
            var existingIds = existing.Select(r => r.RoleId).ToHashSet();

            foreach (var ur in existing.Where(r => !roleIds.Contains(r.RoleId)))
                await _userRepo.RemoveUserRoleAsync(ur);

            foreach (var roleId in roleIds.Except(existingIds))
                await _userRepo.AddUserRoleAsync(new UserRoleEntity
                {
                    UserId = id,
                    RoleId = roleId
                });

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Error al asignar roles: {ex.Message}");
        }
    }

    public async Task<Result<User>> AuthenticateAsync(string email, string password)
    {
        try
        {
            var entity = await _userRepo.GetByEmailAsync(email);
            if (entity == null)
                return Result<User>.Failure("Invalid credentials");

            var isValid = PasswordHelper.VerifyPasswordHash(password, entity.PasswordHash, entity.PasswordSalt);
            if (!isValid)
                return Result<User>.Failure("Invalid credentials");

            return Result<User>.Success(_mapper.Map<User>(entity));
        }
        catch (Exception ex)
        {
            return Result<User>.Failure($"Error authenticating user: {ex.Message}");
        }
    }
}
