using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.DTOs;


namespace softserve.projectlabs.Shared.Interfaces
{
    public interface IBranchService
    {
        Task<Result<BranchDto>> AddBranchAsync(BranchDto branch); // Updated to async
        Task<Result<BranchDto>> UpdateBranchAsync(BranchDto branch); // Updated to async
        Task<Result<BranchDto>> GetBranchByIdAsync(int branchId); // Updated to async
        Task<Result<List<BranchDto>>> GetAllBranchesAsync(); // Updated to async
        Task<Result<bool>> RemoveBranchAsync(int branchId); // Updated to async
    }
}
