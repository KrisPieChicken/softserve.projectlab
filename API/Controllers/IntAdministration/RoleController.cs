using API.Models.IntAdmin;
using API.Services.IntAdmin;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.DTOs.Role;

namespace API.Controllers.IntAdmin
{
    /// <summary>
    /// API Controller for managing Role operations.
    /// </summary>
    [ApiController]
    [Route("api/roles")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        /// <summary>
        /// Constructor with dependency injection for IRoleService.
        /// </summary>
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Adds a new role.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleDto roleDto)
        {
            // Llamas directamente a CreateRoleAsync(roleDto)
            var result = await _roleService.CreateRoleAsync(roleDto);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Updates an existing role.
        /// </summary>
        [HttpPut("{roleId}")]
        public async Task<IActionResult> UpdateRole(int roleId, [FromBody] RoleDto roleDto)
        {
            var result = await _roleService.UpdateRoleAsync(roleId, roleDto);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves a role by its unique ID.
        /// </summary>
        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRoleById(int roleId)
        {
            var result = await _roleService.GetRoleByIdAsync(roleId);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves all roles.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _roleService.GetAllRolesAsync();
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Removes a role by its unique ID.
        /// </summary>
        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            var result = await _roleService.DeleteRoleAsync(roleId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }

            return NoContent();
        }

        /// <summary>
        /// Adds a permission to a role.
        /// </summary>
        [HttpPost("{roleId}/permissions")]
        public async Task<IActionResult> AddPermissionsToRole(int roleId, [FromBody] List<int> permissionIds)
        {
            var result = await _roleService.AddPermissionsToRoleAsync(roleId, permissionIds);
            return result.IsSuccess ? Ok("Permissions added successfully.") : BadRequest(result.ErrorMessage);
        }


        /// <summary>
        /// Removes a permission from a role by its permission ID.
        /// </summary>
        [HttpDelete("{roleId}/permissions/{permissionId}")]
        public async Task<IActionResult> RemovePermissionFromRole(int roleId, int permissionId)
        {
            var result = await _roleService.RemovePermissionFromRoleAsync(roleId, permissionId);
            return result.IsSuccess ? Ok("Permission removed successfully.") : NotFound(result.ErrorMessage);
        }
    }
}
