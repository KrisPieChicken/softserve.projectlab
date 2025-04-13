using API.Models.Logistics.Interfaces;
using softserve.projectlabs.Shared.Utilities;
namespace API.Models.Logistics
{
    public class Branch : IBranch
    {
        public int BranchId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public string Address { get; set; }

        public Branch() { }

        public Branch(int branchId, string name, string city, string region, string contactNumber)
        {
            BranchId = branchId;
            Name = name;
            City = city;
            Region = region;
            ContactNumber = contactNumber;
        }

     
        public Result<IBranch> AddBranch(IBranch branch)
        {
            return Result<IBranch>.Success(branch);
        }

       
        public Result<IBranch> UpdateBranch(IBranch branch)
        {
            return Result<IBranch>.Success(branch);
        }

       
        public Result<IBranch> GetBranchById(int branchId)
        {
            var branch = new Branch(branchId, "Main Branch", "New York", "NY", "555-1234");
            return Result<IBranch>.Success(branch);
        }

       
        public Result<List<IBranch>> GetAllBranches()
        {
            var branches = new List<IBranch>
                {
                    new Branch(1, "Main Branch", "New York", "NY", "555-1234"),
                    new Branch(2, "Secondary Branch", "Los Angeles", "CA", "555-5678")
                };
            return Result<List<IBranch>>.Success(branches);
        }

       
        public Result<bool> RemoveBranch(int branchId)
        {
            return Result<bool>.Success(true);  // Assume the branch was removed successfully
        }
    }
}
