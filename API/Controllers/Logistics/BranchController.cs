using Microsoft.AspNetCore.Mvc;
using softserve.projectlabs.Shared.DTOs;
using softserve.projectlabs.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;

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
        [HttpPost]
        public async Task<IActionResult> AddBranch([FromBody] BranchDto branchDto)
        {
            var result = await _branchService.AddBranchAsync(branchDto);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Updates an existing branch.
        /// </summary>
        [HttpPut("{branchId}")]
        public async Task<IActionResult> UpdateBranch(int branchId, [FromBody] BranchDto branchDto)
        {
            branchDto.BranchId = branchId;
            var result = await _branchService.UpdateBranchAsync(branchDto);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves a branch by its ID.
        /// </summary>
        [HttpGet("{branchId}")]
        public async Task<IActionResult> GetBranchById(int branchId)
        {
            var result = await _branchService.GetBranchByIdAsync(branchId);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves all branches.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllBranches()
        {
            var result = await _branchService.GetAllBranchesAsync();
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Removes a branch by its ID.
        /// </summary>
        [HttpDelete("{branchId}")]
        public async Task<IActionResult> RemoveBranch(int branchId)
        {
            var result = await _branchService.RemoveBranchAsync(branchId);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }
    }
}
