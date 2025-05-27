using API.Implementations.Domain;
using API.Models.Logistics;
using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.Interfaces;
using API.Models.Logistics.Branch;
using softserve.projectlabs.Shared.DTOs.Branch;

namespace API.Services.Logistics
{
    public class BranchService : IBranchService
    {
        private readonly BranchDomain _branchDomain;

        public BranchService(BranchDomain branchDomain)
        {
            _branchDomain = branchDomain ?? throw new ArgumentNullException(nameof(branchDomain));
        }

        public async Task<Result<BranchDto>> AddBranchAsync(BranchDto branchDto)
        {
            var result = await _branchDomain.CreateBranchAsync(branchDto);

            if (!result.IsSuccess)
                return Result<BranchDto>.Failure(result.ErrorMessage, result.ErrorCode);

            var branchData = result.Data.ToDto();

            return Result<BranchDto>.Success(branchData);
        }

        public async Task<Result<BranchDto>> UpdateBranchAsync(BranchDto branchDto)
        {
            var branch = branchDto.ToDomain();

            var result = await _branchDomain.UpdateBranch(branch);

            if (!result.IsSuccess)
                return Result<BranchDto>.Failure(result.ErrorMessage);

            var updatedBranchData = result.Data.ToDto();

            return Result<BranchDto>.Success(updatedBranchData);
        }

        public async Task<Result<BranchDto>> GetBranchByIdAsync(int branchId)
        {
            var result = await _branchDomain.GetBranchById(branchId);

            if (!result.IsSuccess)
                return Result<BranchDto>.Failure(result.ErrorMessage);

            var branchData = result.Data.ToDto();

            return Result<BranchDto>.Success(branchData);
        }

        public async Task<Result<List<BranchDto>>> GetAllBranchesAsync()
        {
            var result = await _branchDomain.GetAllBranches();

            if (!result.IsSuccess)
                return Result<List<BranchDto>>.Failure(result.ErrorMessage);

            var branchDtos = result.Data.Select(branch => branch.ToDto()).ToList();

            return Result<List<BranchDto>>.Success(branchDtos);
        }

        public async Task<Result<bool>> RemoveBranchAsync(int branchId)
        {
            return await _branchDomain.RemoveBranch(branchId);
        }
    }
}
