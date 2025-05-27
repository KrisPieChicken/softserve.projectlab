using Microsoft.AspNetCore.Mvc;
using softserve.projectlabs.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using softserve.projectlabs.Shared.DTOs.Branch;

namespace API.Controllers.Logistics
{
    [ApiController]
    [Route("api/branches")]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        /// <summary>
        /// Adds a new branch.
        /// </summary>
        /// <param name="branchDto">The branch data to add.</param>
        /// <returns>The created branch if successful; otherwise, a bad request with an error message.</returns>
        [HttpPost]
        public async Task<IActionResult> AddBranch([FromBody] BranchDto branchDto)
        {
            var result = await _branchService.AddBranchAsync(branchDto);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Updates an existing branch.
        /// </summary>
        /// <param name="branchId">The ID of the branch to update.</param>
        /// <param name="branchDto">The updated branch data.</param>
        /// <returns>The updated branch if successful; otherwise, not found with an error message.</returns>
        [HttpPut("{branchId}")]
        [Authorize]
        public async Task<IActionResult> UpdateBranch(int branchId, [FromBody] BranchDto branchDto)
        {
            branchDto.BranchId = branchId;
            var result = await _branchService.UpdateBranchAsync(branchDto);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves a branch by its ID.
        /// </summary>
        /// <param name="branchId">The ID of the branch to retrieve.</param>
        /// <returns>The branch if found; otherwise, not found with an error message.</returns>
        [HttpGet("{branchId}")]
        public async Task<IActionResult> GetBranchById(int branchId)
        {
            var result = await _branchService.GetBranchByIdAsync(branchId);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves all branches.
        /// </summary>
        /// <returns>A list of all branches if successful; otherwise, a bad request with an error message.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllBranches()
        {
            var result = await _branchService.GetAllBranchesAsync();
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Removes a branch by its ID.
        /// </summary>
        /// <param name="branchId">The ID of the branch to remove.</param>
        /// <returns>True if the branch was removed; otherwise, not found with an error message.</returns>
        [HttpDelete("{branchId}")]
        [Authorize]
        public async Task<IActionResult> RemoveBranch(int branchId)
        {
            var result = await _branchService.RemoveBranchAsync(branchId);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }
    }
}
