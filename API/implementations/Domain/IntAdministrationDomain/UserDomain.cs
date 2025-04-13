using API.Data;
using API.Data.Entities;
using API.Models;
using softserve.projectlabs.Shared.DTOs;
using API.Models.IntAdmin;
using API.Models.IntAdmin.Interfaces;
using Microsoft.EntityFrameworkCore;
using softserve.projectlabs.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Implementations.Domain
{
    public class UserDomain
    {
        private readonly ApplicationDbContext _context;
        public UserDomain(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new user in the database.
        /// Inserts a UserEntity and, if RoleIds are provided, creates UserRoleEntity entries.
        /// </summary>
        public async Task<Result<User>> CreateUserAsync(UserDto userDto)
        {
            try
            {
                // Create the base UserEntity from the DTO.
                var userEntity = new UserEntity
                {
                    UserFirstName = userDto.UserFirstName,
                    UserLastName = userDto.UserLastName,
                    UserContactEmail = userDto.UserEmail,
                    UserContactNumber = userDto.UserPhone,
                    UserPassword = userDto.UserPassword,
                    UserStatus = userDto.UserStatus,
                    BranchId = userDto.BranchId
                };
                _context.UserEntities.Add(userEntity);
                await _context.SaveChangesAsync();

                // If RoleIds are provided, add UserRoleEntity rows.
                if (userDto.RoleIds != null && userDto.RoleIds.Any())
                {
                    foreach (var roleId in userDto.RoleIds)
                    {
                        var roleEntity = await _context.RoleEntities.FirstOrDefaultAsync(r => r.RoleId == roleId);
                        if (roleEntity != null)
                        {
                            var userRole = new UserRoleEntity
                            {
                                UserId = userEntity.UserId,
                                RoleId = roleId
                            };
                            _context.UserRoleEntities.Add(userRole);
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                // Map the inserted user to the domain model.
                var user = await MapToUser(userEntity.UserId);
                return Result<User>.Success(user);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"Error creating user: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing user, including synchronizing the assigned roles.
        /// </summary>
        public async Task<Result<User>> UpdateUserAsync(int userId, UserDto userDto)
        {
            try
            {
                var userEntity = await _context.UserEntities
                    .Include(u => u.UserRoleEntities)
                    .FirstOrDefaultAsync(u => u.UserId == userId);
                if (userEntity == null)
                    return Result<User>.Failure("User not found.");

                // Update basic properties.
                userEntity.UserFirstName = userDto.UserFirstName;
                userEntity.UserLastName = userDto.UserLastName;
                userEntity.UserContactEmail = userDto.UserEmail;
                userEntity.UserContactNumber = userDto.UserPhone;
                userEntity.UserPassword = userDto.UserPassword;
                userEntity.UserStatus = userDto.UserStatus;
                userEntity.BranchId = userDto.BranchId;

                // Synchronize roles if provided.
                if (userDto.RoleIds != null)
                {
                    // Remove roles not in the new list.
                    var toRemove = userEntity.UserRoleEntities
                        .Where(ur => !userDto.RoleIds.Contains(ur.RoleId))
                        .ToList();
                    _context.UserRoleEntities.RemoveRange(toRemove);

                    // Determine which roles to add.
                    var currentRoleIds = userEntity.UserRoleEntities.Select(ur => ur.RoleId).ToList();
                    var toAdd = userDto.RoleIds.Except(currentRoleIds).ToList();
                    foreach (var roleId in toAdd)
                    {
                        var roleEntity = await _context.RoleEntities.FirstOrDefaultAsync(r => r.RoleId == roleId);
                        if (roleEntity != null)
                        {
                            var newUserRole = new UserRoleEntity
                            {
                                UserId = userEntity.UserId,
                                RoleId = roleId
                            };
                            _context.UserRoleEntities.Add(newUserRole);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                var updatedUser = await MapToUser(userEntity.UserId);
                return Result<User>.Success(updatedUser);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"Error updating user: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a user by ID, including its assigned roles.
        /// </summary>
        public async Task<Result<User>> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await MapToUser(userId);
                if (user == null)
                    return Result<User>.Failure("User not found.");
                return Result<User>.Success(user);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"Error retrieving user: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        public async Task<Result<List<User>>> GetAllUsersAsync()
        {
            try
            {
                var users = new List<User>();
                var userEntities = await _context.UserEntities.ToListAsync();
                foreach (var ue in userEntities)
                {
                    var user = await MapToUser(ue.UserId);
                    if (user != null)
                        users.Add(user);
                }
                return Result<List<User>>.Success(users);
            }
            catch (Exception ex)
            {
                return Result<List<User>>.Failure($"Error retrieving users: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        public async Task<Result<bool>> DeleteUserAsync(int userId)
        {
            try
            {
                var userEntity = await _context.UserEntities.FirstOrDefaultAsync(u => u.UserId == userId);
                if (userEntity == null)
                    return Result<bool>.Failure("User not found.");
                _context.UserEntities.Remove(userEntity);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error deleting user: {ex.Message}");
            }
        }

        /// <summary>
        /// Authenticates a user by email and password.
        /// </summary>
        public async Task<Result<bool>> AuthenticateAsync(string email, string password)
        {
            try
            {
                var userEntity = await _context.UserEntities
                    .FirstOrDefaultAsync(u => u.UserContactEmail == email && u.UserPassword == password);
                if (userEntity != null)
                    return Result<bool>.Success(true);
                return Result<bool>.Failure("Invalid credentials");
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error authenticating user: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the user's password.
        /// </summary>
        public async Task<Result<bool>> UpdatePasswordAsync(int userId, string newPassword)
        {
            try
            {
                var userEntity = await _context.UserEntities.FirstOrDefaultAsync(u => u.UserId == userId);
                if (userEntity == null)
                    return Result<bool>.Failure("User not found.");
                userEntity.UserPassword = newPassword;
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error updating password: {ex.Message}");
            }
        }

        /// <summary>
        /// Assigns roles to a user by synchronizing the UserRoleEntity table.
        /// </summary>
        public async Task<Result<bool>> AssignRolesAsync(int userId, List<int> roleIds)
        {
            try
            {
                var userEntity = await _context.UserEntities
                    .Include(u => u.UserRoleEntities)
                    .FirstOrDefaultAsync(u => u.UserId == userId);
                if (userEntity == null)
                    return Result<bool>.Failure("User not found.");

                // Remove roles not in the new list.
                var toRemove = userEntity.UserRoleEntities
                    .Where(ur => !roleIds.Contains(ur.RoleId))
                    .ToList();
                _context.UserRoleEntities.RemoveRange(toRemove);

                // Determine roles to add.
                var currentRoleIds = userEntity.UserRoleEntities.Select(ur => ur.RoleId).ToList();
                var toAdd = roleIds.Except(currentRoleIds).ToList();
                foreach (var roleId in toAdd)
                {
                    var roleEntity = await _context.RoleEntities.FirstOrDefaultAsync(r => r.RoleId == roleId);
                    if (roleEntity != null)
                    {
                        var newUserRole = new UserRoleEntity
                        {
                            UserId = userEntity.UserId,
                            RoleId = roleId
                        };
                        _context.UserRoleEntities.Add(newUserRole);
                    }
                }
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error assigning roles: {ex.Message}");
            }
        }

        // Helper: Map UserEntity (including roles) to the domain model User.
        private async Task<User> MapToUser(int userId)
        {
            var userEntity = await _context.UserEntities
                .Include(u => u.UserRoleEntities)
                .FirstOrDefaultAsync(u => u.UserId == userId);
            if (userEntity == null)
                return null;

            var user = new User
            {
                UserId = userEntity.UserId,
                UserEmail = userEntity.UserContactEmail,
                UserFirstName = userEntity.UserFirstName,
                UserLastName = userEntity.UserLastName ?? string.Empty,
                UserPhone = userEntity.UserContactNumber,
                UserPassword = userEntity.UserPassword,
                UserStatus = userEntity.UserStatus,
                BranchId = userEntity.BranchId,
                UserImage = string.Empty, // Ajusta según los datos de la entidad, si existen
                Roles = new List<IRole>()
            };

            // Map roles assigned to the user.
            var userRoles = await _context.UserRoleEntities
                .Where(ur => ur.UserId == userId)
                .ToListAsync();
            foreach (var ur in userRoles)
            {
                var roleEntity = await _context.RoleEntities.FirstOrDefaultAsync(r => r.RoleId == ur.RoleId);
                if (roleEntity != null)
                {
                    var role = new Role
                    {
                        RoleId = roleEntity.RoleId,
                        RoleName = roleEntity.RoleName,
                        RoleDescription = roleEntity.RoleDescription,
                        RoleStatus = roleEntity.RoleStatus
                    };
                    user.Roles.Add(role);
                }
            }
            return user;
        }
    }
}
