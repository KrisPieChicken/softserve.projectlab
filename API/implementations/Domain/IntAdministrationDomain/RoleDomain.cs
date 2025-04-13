using API.Data;
using API.Data.Entities;
using API.Models;
using softserve.projectlabs.Shared.DTOs;
using API.Models.IntAdmin;
using API.Models.IntAdmin.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.Utilities;

namespace API.Implementations.Domain
{
    public class RoleDomain
    {
        private readonly ApplicationDbContext _context;

        public RoleDomain(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new role in the database.
        /// First, it saves the RoleEntity; then, if permission IDs are provided, it creates RolePermissionEntity rows.
        /// </summary>
        public async Task<Result<Role>> CreateRoleAsync(RoleDto roleDto)
        {
            try
            {
                // Create and save the base RoleEntity from the DTO.
                var roleEntity = new RoleEntity
                {
                    RoleName = roleDto.RoleName,
                    RoleDescription = roleDto.RoleDescription,
                    RoleStatus = roleDto.RoleStatus
                };

                _context.RoleEntities.Add(roleEntity);
                await _context.SaveChangesAsync();

                // If permission IDs are provided, add the corresponding RolePermissionEntities.
                if (roleDto.PermissionIds != null && roleDto.PermissionIds.Any())
                {
                    foreach (var permId in roleDto.PermissionIds)
                    {
                        var permissionEntity = await _context.PermissionEntities
                            .FirstOrDefaultAsync(p => p.PermissionId == permId);
                        if (permissionEntity != null)
                        {
                            var rolePermissionEntity = new RolePermissionEntity
                            {
                                RoleId = roleEntity.RoleId,
                                PermissionId = permId,
                                RoleName = roleEntity.RoleName,
                                PermissionName = permissionEntity.PermissionName
                            };
                            _context.RolePermissionEntities.Add(rolePermissionEntity);
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                // Map the saved entity to the Role model.
                var role = MapToRole(roleEntity);
                return Result<Role>.Success(role);
            }
            catch (Exception ex)
            {
                return Result<Role>.Failure($"Error creating role: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing role in the database.
        /// It updates the RoleEntity and syncs its permissions using RolePermissionEntities.
        /// </summary>
        public async Task<Result<Role>> UpdateRoleAsync(int roleId, RoleDto roleDto)
        {
            try
            {
                var roleEntity = await _context.RoleEntities
                    .Include(r => r.RolePermissionEntities)
                    .FirstOrDefaultAsync(r => r.RoleId == roleId);

                if (roleEntity == null)
                {
                    return Result<Role>.Failure("Role not found.");
                }

                // Update basic properties.
                roleEntity.RoleName = roleDto.RoleName;
                roleEntity.RoleDescription = roleDto.RoleDescription;
                roleEntity.RoleStatus = roleDto.RoleStatus;

                // Sync permissions: Remove those not in the new list and add missing ones.
                if (roleDto.PermissionIds != null)
                {
                    // Remove permissions that are no longer in the list.
                    var toRemove = roleEntity.RolePermissionEntities
                        .Where(rp => !roleDto.PermissionIds.Contains(rp.PermissionId))
                        .ToList();
                    _context.RolePermissionEntities.RemoveRange(toRemove);

                    // Determine which permissions to add.
                    var currentPermissionIds = roleEntity.RolePermissionEntities.Select(rp => rp.PermissionId).ToList();
                    var toAdd = roleDto.PermissionIds.Except(currentPermissionIds).ToList();
                    foreach (var permId in toAdd)
                    {
                        var permissionEntity = await _context.PermissionEntities
                            .FirstOrDefaultAsync(p => p.PermissionId == permId);
                        if (permissionEntity != null)
                        {
                            var newRolePermissionEntity = new RolePermissionEntity
                            {
                                RoleId = roleEntity.RoleId,
                                PermissionId = permId,
                                RoleName = roleEntity.RoleName,
                                PermissionName = permissionEntity.PermissionName
                            };
                            _context.RolePermissionEntities.Add(newRolePermissionEntity);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                var updatedRole = MapToRole(roleEntity);
                return Result<Role>.Success(updatedRole);
            }
            catch (Exception ex)
            {
                return Result<Role>.Failure($"Error updating role: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a role by its unique ID from the database.
        /// </summary>
        public async Task<Result<Role>> GetRoleByIdAsync(int roleId)
        {
            try
            {
                var roleEntity = await _context.RoleEntities
                    .Include(r => r.RolePermissionEntities)
                    .FirstOrDefaultAsync(r => r.RoleId == roleId);

                if (roleEntity == null)
                {
                    return Result<Role>.Failure("Role not found.");
                }

                var role = MapToRole(roleEntity);
                return Result<Role>.Success(role);
            }
            catch (Exception ex)
            {
                return Result<Role>.Failure($"Error retrieving role: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all roles from the database.
        /// </summary>
        public async Task<Result<List<Role>>> GetAllRolesAsync()
        {
            try
            {
                var roleEntities = await _context.RoleEntities
                    .Include(r => r.RolePermissionEntities)
                    .ToListAsync();
                var roles = roleEntities.Select(r => MapToRole(r)).ToList();
                return Result<List<Role>>.Success(roles);
            }
            catch (Exception ex)
            {
                return Result<List<Role>>.Failure($"Error retrieving roles: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a role from the database by its unique ID.
        /// </summary>
        public async Task<Result<bool>> DeleteRoleAsync(int roleId)
        {
            try
            {
                var roleEntity = await _context.RoleEntities
                    .FirstOrDefaultAsync(r => r.RoleId == roleId);
                if (roleEntity == null)
                {
                    return Result<bool>.Failure("Role not found.");
                }

                _context.RoleEntities.Remove(roleEntity);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error deleting role: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a permission to a role by creating a RolePermissionEntity.
        /// </summary>
        public async Task<Result<bool>> AddPermissionsToRoleAsync(int roleId, List<int> permissionIds)
        {
            try
            {
                var roleEntity = await _context.RoleEntities
                    .Include(r => r.RolePermissionEntities)
                    .FirstOrDefaultAsync(r => r.RoleId == roleId);
                if (roleEntity == null)
                {
                    return Result<bool>.Failure("Role not found.");
                }

                // Para cada ID en la lista, verifica si ya está asignado o no.
                foreach (var permId in permissionIds)
                {
                    bool alreadyAssigned = roleEntity.RolePermissionEntities
                        .Any(rp => rp.PermissionId == permId);

                    if (!alreadyAssigned)
                    {
                        var permissionEntity = await _context.PermissionEntities
                            .FirstOrDefaultAsync(p => p.PermissionId == permId);

                        if (permissionEntity != null)
                        {
                            var newRolePermission = new RolePermissionEntity
                            {
                                RoleId = roleId,
                                PermissionId = permId,
                                RoleName = roleEntity.RoleName,
                                PermissionName = permissionEntity.PermissionName
                            };
                            _context.RolePermissionEntities.Add(newRolePermission);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error adding permissions: {ex.Message}");
            }
        }


        /// <summary>
        /// Removes a permission from a role by deleting the corresponding RolePermissionEntity.
        /// </summary>
        public async Task<Result<bool>> RemovePermissionFromRoleAsync(int roleId, int permissionId)
        {
            try
            {
                var rolePermissionEntity = await _context.RolePermissionEntities
                    .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
                if (rolePermissionEntity == null)
                {
                    return Result<bool>.Failure("Permission not found in role.");
                }

                _context.RolePermissionEntities.Remove(rolePermissionEntity);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error removing permission: {ex.Message}");
            }
        }

        /// <summary>
        /// Maps a RoleEntity to the Role model.
        /// </summary>
        private Role MapToRole(RoleEntity entity)
        {
            var role = new Role
            {
                RoleId = entity.RoleId,
                RoleName = entity.RoleName,
                RoleDescription = entity.RoleDescription,
                RoleStatus = entity.RoleStatus,
                // Initialize the list of permissions (using Permission which implements IPermission)
                Permissions = new List<IPermission>()
            };

            if (entity.RolePermissionEntities != null && entity.RolePermissionEntities.Any())
            {
                foreach (var rp in entity.RolePermissionEntities)
                {
                    // Create a new Permission object for each RolePermissionEntity.
                    var permission = new Permission
                    {
                        PermissionId = rp.PermissionId,
                        PermissionName = rp.PermissionName
                    };
                    role.Permissions.Add(permission);
                }
            }
            return role;
        }
    }
}
