using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.DTOs.Branch;


namespace softserve.projectlabs.Shared.Interfaces
{
    public interface IBranchService
    {
        Task<Result<BranchDto>> AddBranchAsync(BranchDto branch); 
        Task<Result<BranchDto>> UpdateBranchAsync(BranchDto branch); 
        Task<Result<BranchDto>> GetBranchByIdAsync(int branchId); 
        Task<Result<List<BranchDto>>> GetAllBranchesAsync(); 
        Task<Result<bool>> RemoveBranchAsync(int branchId); 
    }
}
