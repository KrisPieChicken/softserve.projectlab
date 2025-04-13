using API.Data;
using API.Data.Entities;
using API.Models;
using API.Models.IntAdmin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.Utilities;

namespace API.Implementations.Domain
{
    public class PermissionDomain
    {
        private readonly ApplicationDbContext _context;

        public PermissionDomain(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new permission in the database.
        /// </summary>
        /// <param name="permission">Permission model to create</param>
        /// <returns>Result containing the created permission or an error message</returns>
        public async Task<Result<Permission>> CreatePermissionAsync(Permission permission)
        {
            try
            {
                // Create the base Permission entity
                var permissionEntity = new PermissionEntity
                {
                    PermissionName = permission.PermissionName,
                    PermissionDescription = permission.PermissionDescription
                };

                // Save the basic Permission entity first
                _context.PermissionEntities.Add(permissionEntity);
                await _context.SaveChangesAsync();

                // Assign the generated ID to the model
                permission.PermissionId = permissionEntity.PermissionId;

                return Result<Permission>.Success(permission);
            }
            catch (Exception ex)
            {
                return Result<Permission>.Failure($"Error creating permission: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing permission in the database.
        /// </summary>
        /// <param name="permission">Permission model with updated data</param>
        /// <returns>Result containing the updated permission or an error message</returns>
        public async Task<Result<Permission>> UpdatePermissionAsync(Permission permission)
        {
            try
            {
                var permissionEntity = await _context.PermissionEntities
                    .FirstOrDefaultAsync(p => p.PermissionId == permission.PermissionId);

                if (permissionEntity == null)
                {
                    return Result<Permission>.Failure("Permission not found.");
                }

                // Update the permission fields
                permissionEntity.PermissionName = permission.PermissionName;
                permissionEntity.PermissionDescription = permission.PermissionDescription;

                await _context.SaveChangesAsync();
                return Result<Permission>.Success(permission);
            }
            catch (Exception ex)
            {
                return Result<Permission>.Failure($"Error updating permission: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a permission by its unique ID from the database.
        /// </summary>
        /// <param name="permissionId">Unique identifier of the permission</param>
        /// <returns>Result containing the permission or an error message</returns>
        public async Task<Result<Permission>> GetPermissionByIdAsync(int permissionId)
        {
            try
            {
                // Get the basic Permission entity
                var permissionEntity = await _context.PermissionEntities
                    .FirstOrDefaultAsync(p => p.PermissionId == permissionId);

                if (permissionEntity == null)
                {
                    return Result<Permission>.Failure("Permission not found.");
                }

                // Map the entity to the Permission model
                var permission = MapToPermission(permissionEntity);
                return Result<Permission>.Success(permission);
            }
            catch (Exception ex)
            {
                return Result<Permission>.Failure($"Error retrieving permission: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all permissions stored in the database.
        /// </summary>
        /// <returns>Result containing a list of permissions or an error message</returns>
        public async Task<Result<List<Permission>>> GetAllPermissionsAsync()
        {
            try
            {
                var permissionEntities = await _context.PermissionEntities.ToListAsync();
                var permissions = new List<Permission>();

                // For each permission entity, map it to the model
                foreach (var entity in permissionEntities)
                {
                    permissions.Add(MapToPermission(entity));
                }

                return Result<List<Permission>>.Success(permissions);
            }
            catch (Exception ex)
            {
                return Result<List<Permission>>.Failure($"Error retrieving permissions: {ex.Message}");
            }
        }

        /// <summary>
        /// Removes a permission from the database by its unique ID.
        /// </summary>
        /// <param name="permissionId">Unique identifier of the permission to remove</param>
        /// <returns>Result indicating success or failure</returns>
        public async Task<Result<bool>> DeletePermissionAsync(int permissionId)
        {
            try
            {
                var permissionEntity = await _context.PermissionEntities
                    .FirstOrDefaultAsync(p => p.PermissionId == permissionId);

                if (permissionEntity == null)
                {
                    return Result<bool>.Failure("Permission not found.");
                }

                _context.PermissionEntities.Remove(permissionEntity);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error removing permission: {ex.Message}");
            }
        }

        /// <summary>
        /// Maps a PermissionEntity to the Permission model.
        /// </summary>
        /// <param name="entity">Instance of PermissionEntity</param>
        /// <returns>An instance of the Permission model</returns>
        private Permission MapToPermission(PermissionEntity entity)
        {
            return new Permission
            {
                PermissionId = entity.PermissionId,
                PermissionName = entity.PermissionName,
                PermissionDescription = entity.PermissionDescription
            };
        }
    }
}
