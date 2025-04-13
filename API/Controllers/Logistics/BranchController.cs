using API.Models.Logistics;
using API.Services.Logistics;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using softserve.projectlabs.Shared.Interfaces;
using softserve.projectlabs.Shared.DTOs;
using AutoMapper;

namespace API.Controllers.Logistics
{

    /// <summary>
    /// Controller for managing branch-related operations.
    /// </summary>
    [ApiController]
    [Route("api/branches")]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;
        private readonly IMapper _mapper;
        private readonly ILogger<BranchController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchController"/> class.
        /// </summary>
        /// <param name="branchService">Service for branch operations.</param>
        /// <param name="mapper">Mapper for object transformations.</param>
        /// <param name="logger">Logger for logging operations.</param>
        public BranchController(IBranchService branchService, IMapper mapper, ILogger<BranchController> logger)
        {
            _branchService = branchService;
            _mapper = mapper;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Adds a new branch.
        /// </summary>
        /// <param name="branchDto">The branch data transfer object.</param>
        /// <returns>Action result indicating the outcome of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> AddBranch([FromBody] BranchDto branchDto)
        {
            _logger.LogInformation("Starting AddBranch for Branch: {BranchName}", branchDto.BranchName);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for Branch: {BranchName}", branchDto.BranchName);
                return BadRequest(ModelState);
            }

            var result = await _branchService.AddBranchAsync(branchDto);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully added Branch: {BranchName}", branchDto.BranchName);
                return Ok(new { Message = "Branch successfully added.", Branch = result.Data });
            }
            else
            {
                _logger.LogError("Failed to add Branch: {BranchName}. Error: {ErrorMessage}", branchDto.BranchName, result.ErrorMessage);
                return BadRequest(result.ErrorMessage);
            }
        }

        /// <summary>
        /// Updates an existing branch.
        /// </summary>
        /// <param name="branchId">The ID of the branch to update.</param>
        /// <param name="branch">The updated branch data.</param>
        /// <returns>Action result indicating the outcome of the operation.</returns>
        [HttpPut("{branchId}")]
        public async Task<IActionResult> UpdateBranch(int branchId, [FromBody] Branch branch)
        {
            _logger.LogInformation("Starting UpdateBranch for BranchId: {BranchId}", branchId);

            branch.BranchId = branchId;

            // Map Branch to BranchDto
            var branchDto = _mapper.Map<BranchDto>(branch);

            var result = await _branchService.UpdateBranchAsync(branchDto);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully updated BranchId: {BranchId}", branchId);
                return Ok(result.Data);
            }
            else
            {
                _logger.LogError("Failed to update BranchId: {BranchId}. Error: {ErrorMessage}", branchId, result.ErrorMessage);
                return NotFound(result.ErrorMessage);
            }
        }

        /// <summary>
        /// Retrieves a branch by its ID.
        /// </summary>
        /// <param name="branchId">The ID of the branch to retrieve.</param>
        /// <returns>Action result containing the branch data.</returns>
        [HttpGet("{branchId}")]
        public async Task<IActionResult> GetBranchById(int branchId)
        {
            _logger.LogInformation("Fetching BranchId: {BranchId}", branchId);

            var result = await _branchService.GetBranchByIdAsync(branchId);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully fetched BranchId: {BranchId}", branchId);
                return Ok(result.Data);
            }
            else
            {
                _logger.LogError("Failed to fetch BranchId: {BranchId}. Error: {ErrorMessage}", branchId, result.ErrorMessage);
                return NotFound(result.ErrorMessage);
            }
        }

        /// <summary>
        /// Retrieves all branches.
        /// </summary>
        /// <returns>Action result containing a list of all branches.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllBranches()
        {
            _logger.LogInformation("Fetching all branches.");

            var result = await _branchService.GetAllBranchesAsync();

            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully fetched all branches.");
                return Ok(result.Data);
            }
            else
            {
                _logger.LogError("Failed to fetch branches. Error: {ErrorMessage}", result.ErrorMessage);
                return BadRequest(result.ErrorMessage);
            }
        }

        /// <summary>
        /// Removes a branch by its ID.
        /// </summary>
        /// <param name="branchId">The ID of the branch to remove.</param>
        /// <returns>Action result indicating the outcome of the operation.</returns>
        [HttpDelete("{branchId}")]
        public async Task<IActionResult> RemoveBranch(int branchId)
        {
            _logger.LogInformation("Removing BranchId: {BranchId}", branchId);

            var result = await _branchService.RemoveBranchAsync(branchId);

            if (result.IsNoContent)
            {
                _logger.LogInformation("Successfully removed BranchId: {BranchId}", branchId);
                return NoContent();
            }
            else if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully removed BranchId: {BranchId}", branchId);
                return Ok(result.Data);
            }
            else
            {
                _logger.LogError("Failed to remove BranchId: {BranchId}. Error: {ErrorMessage}", branchId, result.ErrorMessage);
                return NotFound(result.ErrorMessage);
            }
        }
    }
}
