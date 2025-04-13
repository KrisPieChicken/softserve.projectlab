using API.Implementations.Domain;
using API.Models.Logistics;
using API.Data.Entities;
using API.Models;
using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.Interfaces;
using softserve.projectlabs.Shared.DTOs;
using AutoMapper; 

namespace API.Services.Logistics
{
    public class BranchService : IBranchService
    {
        private readonly BranchDomain _branchDomain;
        private readonly IMapper _mapper;
        private readonly ILogger<BranchService> _logger;

        public BranchService(BranchDomain branchDomain, IMapper mapper, ILogger<BranchService> logger)
        {
            _branchDomain = branchDomain ?? throw new ArgumentNullException(nameof(branchDomain));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<BranchDto>> AddBranchAsync(BranchDto branchDto)
        {
            // Validate duplicate branch
            var existingBranch = await _branchDomain.GetBranchByNameAndCityAsync(branchDto.BranchName, branchDto.BranchCity);
            if (existingBranch != null)
            {
                return Result<BranchDto>.Failure($"A branch with the name '{branchDto.BranchName}' already exists in the city '{branchDto.BranchCity}'.");
            }

            // Map BranchDto to BranchEntity
            var branchEntity = _mapper.Map<BranchEntity>(branchDto);
            branchEntity.IsDeleted = false; // Set IsDeleted to false by default

            var result = await _branchDomain.AddBranchAsync(branchEntity);

            return result.IsSuccess
                ? Result<BranchDto>.Success(_mapper.Map<BranchDto>(result.Data))
                : Result<BranchDto>.Failure(result.ErrorMessage, result.ErrorCode);
        }


        public async Task<Result<BranchDto>> UpdateBranchAsync(BranchDto branchDto)
        {
            _logger.LogInformation("Starting UpdateBranch for BranchId: {BranchId}", branchDto.BranchId);

            var branch = _mapper.Map<Branch>(branchDto);
            var result = await _branchDomain.UpdateBranch(branch);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully updated BranchId: {BranchId}", branchDto.BranchId);
                return Result<BranchDto>.Success(_mapper.Map<BranchDto>(result.Data));
            }
            else
            {
                _logger.LogError("Failed to update BranchId: {BranchId}. Error: {ErrorMessage}", branchDto.BranchId, result.ErrorMessage);
                return Result<BranchDto>.Failure(result.ErrorMessage);
            }
        }


        public async Task<Result<BranchDto>> GetBranchByIdAsync(int branchId)
        {
            _logger.LogInformation("Fetching BranchId: {BranchId}", branchId);

            var result = await _branchDomain.GetBranchById(branchId);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully fetched BranchId: {BranchId}", branchId);
                return Result<BranchDto>.Success(_mapper.Map<BranchDto>(result.Data));
            }
            else
            {
                _logger.LogError("Failed to fetch BranchId: {BranchId}. Error: {ErrorMessage}", branchId, result.ErrorMessage);
                return Result<BranchDto>.Failure(result.ErrorMessage);
            }
        }


        public async Task<Result<List<BranchDto>>> GetAllBranchesAsync()
        {
            _logger.LogInformation("Fetching all branches.");

            var result = await _branchDomain.GetAllBranches();

            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully fetched all branches.");
                return Result<List<BranchDto>>.Success(_mapper.Map<List<BranchDto>>(result.Data));
            }
            else
            {
                _logger.LogError("Failed to fetch branches. Error: {ErrorMessage}", result.ErrorMessage);
                return Result<List<BranchDto>>.Failure(result.ErrorMessage);
            }
        }


        public async Task<Result<bool>> RemoveBranchAsync(int branchId)
        {
            _logger.LogInformation("Removing BranchId: {BranchId}", branchId);

            var result = await _branchDomain.RemoveBranch(branchId);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully removed BranchId: {BranchId}", branchId);
            }
            else
            {
                _logger.LogError("Failed to remove BranchId: {BranchId}. Error: {ErrorMessage}", branchId, result.ErrorMessage);
            }

            return result;
        }

    }
}
