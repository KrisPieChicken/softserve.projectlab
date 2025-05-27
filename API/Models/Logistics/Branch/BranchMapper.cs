using API.Data.Entities;
using softserve.projectlabs.Shared.DTOs.Branch;

namespace API.Models.Logistics.Branch
{
    public static class BranchMapper
    {
        // Existing mapping for BranchDto to Branch
        public static Branch ToDomain(this BranchDto dto)
            => new Branch(
                dto.BranchId,
                dto.BranchName,
                dto.BranchCity,
                dto.BranchRegion,
                dto.BranchAddress,
                dto.BranchContactNumber,
                dto.BranchContactEmail,
                dto.CreatedAt,
                dto.UpdatedAt,
                dto.IsDeleted);

        // NEW: mapping for BranchEntity to Branch
        public static Branch ToDomain(this BranchEntity entity)
            => new Branch(
                entity.BranchId,
                entity.BranchName,
                entity.BranchCity,
                entity.BranchRegion,
                entity.BranchAddress,
                entity.BranchContactNumber,
                entity.BranchContactEmail,
                entity.CreatedAt,
                entity.UpdatedAt,
                entity.IsDeleted
            );

        // Existing mapping for Branch to BranchDto
        public static BranchDto ToDto(this Branch branch)
            => new BranchDto
            {
                BranchId = branch.BranchId,
                BranchName = branch.BranchName,
                BranchCity = branch.BranchCity,
                BranchRegion = branch.BranchRegion,
                BranchAddress = branch.BranchAddress,
                BranchContactNumber = branch.BranchContactNumber,
                BranchContactEmail = branch.BranchContactEmail,
                CreatedAt = branch.CreatedAt,
                UpdatedAt = branch.UpdatedAt,
                IsDeleted = branch.IsDeleted
            };
    }
}
