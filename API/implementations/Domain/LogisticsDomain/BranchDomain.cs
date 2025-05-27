using API.Models.Logistics;
using API.Data.Entities;
using softserve.projectlabs.Shared.Utilities;
using API.Data.Repositories.LogisticsRepositories.Interfaces;
using API.Models.Logistics.Branch;
using softserve.projectlabs.Shared.DTOs.Branch;


namespace API.Implementations.Domain
{
    public class BranchDomain
    {
        private readonly IBranchRepository _branchRepository;

        public BranchDomain(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<Result<Branch>> CreateBranchAsync(BranchDto branchDto)
        {
            try
            {
                var branchEntity = new BranchEntity
                {
                    BranchName = branchDto.BranchName,
                    BranchCity = branchDto.BranchCity,
                    BranchRegion = branchDto.BranchRegion,
                    BranchContactNumber = branchDto.BranchContactNumber,
                    BranchContactEmail = branchDto.BranchContactEmail,
                    BranchAddress = branchDto.BranchAddress,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _branchRepository.AddAsync(branchEntity);

                var branch = branchEntity.ToDomain();

                return Result<Branch>.Success(branch);
            }
            catch (Exception ex)
            {
                return Result<Branch>.Failure($"Failed to create branch: {ex.Message}");
            }
        }

        public async Task<Result<Branch>> GetBranchById(int branchId)
        {
            try
            {
                var branchEntity = await _branchRepository.GetByIdAsync(branchId);
                if (branchEntity == null)
                    return Result<Branch>.Failure("Branch not found.");

                var branch = branchEntity.ToDomain();

                return Result<Branch>.Success(branch);
            }
            catch (Exception ex)
            {
                return Result<Branch>.Failure($"Failed to get branch: {ex.Message}");
            }
        }

        public async Task<Result<List<Branch>>> GetAllBranches()
        {
            try
            {
                var branchEntities = await _branchRepository.GetAllAsync();
                var branches = branchEntities.Select(be => be.ToDomain()).ToList();

                return Result<List<Branch>>.Success(branches);
            }
            catch (Exception ex)
            {
                return Result<List<Branch>>.Failure($"Failed to retrieve branches: {ex.Message}");
            }
        }

        public async Task<Result<Branch>> UpdateBranch(Branch branch)
        {
            try
            {
                var existingBranchEntity = await _branchRepository.GetByIdAsync(branch.BranchId);
                if (existingBranchEntity == null)
                    return Result<Branch>.Failure("Branch not found.");

                existingBranchEntity.BranchName = branch.BranchName;
                existingBranchEntity.BranchCity = branch.BranchCity;
                existingBranchEntity.BranchRegion = branch.BranchRegion;
                existingBranchEntity.BranchContactNumber = branch.BranchContactNumber;
                existingBranchEntity.BranchContactEmail = branch.BranchContactEmail;
                existingBranchEntity.BranchAddress = branch.BranchAddress;
                existingBranchEntity.UpdatedAt = DateTime.UtcNow;

                await _branchRepository.UpdateAsync(existingBranchEntity);

                var updatedBranch = existingBranchEntity.ToDomain();

                return Result<Branch>.Success(updatedBranch);
            }
            catch (Exception ex)
            {
                return Result<Branch>.Failure($"Failed to update branch: {ex.Message}");
            }
        }

        public async Task<Result<bool>> RemoveBranch(int branchId)
        {
            try
            {
                await _branchRepository.DeleteAsync(branchId);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to remove branch: {ex.Message}");
            }
        }
    }
}
