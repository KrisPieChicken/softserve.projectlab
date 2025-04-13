using API.Models.Logistics;
using API.Data.Entities;
using API.Models;
using softserve.projectlabs.Shared.Utilities;
using Microsoft.EntityFrameworkCore;
using API.Data;
using AutoMapper;
using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.DTOs;

namespace API.Implementations.Domain

{
    public class BranchDomain
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BranchDomain(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<Result<Branch>> CreateBranch(Branch branch)
        {
            try
            {
                // Map Branch to BranchEntity
                var branchEntity = _mapper.Map<BranchEntity>(branch);
                branchEntity.IsDeleted = false; // Ensure IsDeleted is set to false by default

                // Add to the database
                _context.BranchEntities.Add(branchEntity);
                await _context.SaveChangesAsync();

                // Map back to Branch and return
                var createdBranch = _mapper.Map<Branch>(branchEntity);
                return Result<Branch>.Success(createdBranch);
            }
            catch (Exception ex)
            {
                return Result<Branch>.Failure($"Failed to create branch: {ex.Message}");
            }
        }


        public async Task<Result<BranchEntity>> AddBranchAsync(BranchEntity branchEntity)
        {
            try
            {
                _context.BranchEntities.Add(branchEntity);
                await _context.SaveChangesAsync();

                return Result<BranchEntity>.Success(branchEntity);
            }
            catch (Exception ex)
            {
                return Result<BranchEntity>.Failure($"Failed to add branch: {ex.Message}", 500, ex.StackTrace);
            }
        }

        public async Task<BranchEntity?> GetBranchByNameAndCityAsync(string name, string city)
        {
            return await _context.BranchEntities
                .FirstOrDefaultAsync(b => b.BranchName == name && b.BranchCity == city && !b.IsDeleted);
        }


        public async Task<Result<Branch>> UpdateBranch(Branch branch)
        {
            try
            {
                // Retrieve the existing branch from the database
                var existingBranchEntity = await _context.BranchEntities
                    .FirstOrDefaultAsync(b => b.BranchId == branch.BranchId && !b.IsDeleted);

                if (existingBranchEntity == null)
                {
                    return Result<Branch>.Failure("Branch not found.");
                }

                // Update the properties of the existing branch
                existingBranchEntity.BranchName = branch.Name;
                existingBranchEntity.BranchCity = branch.City;
                existingBranchEntity.BranchRegion = branch.Region;
                existingBranchEntity.BranchContactNumber = branch.ContactNumber;
                existingBranchEntity.BranchContactEmail = branch.ContactEmail;
                existingBranchEntity.BranchAddress = branch.Address;

                // Save changes to the database
                _context.BranchEntities.Update(existingBranchEntity);
                await _context.SaveChangesAsync();

                // Map the updated entity back to the Branch model
                var updatedBranch = _mapper.Map<Branch>(existingBranchEntity);
                return Result<Branch>.Success(updatedBranch);
            }
            catch (Exception ex)
            {
                return Result<Branch>.Failure($"Failed to update branch: {ex.Message}");
            }
        }



        public async Task<Result<Branch>> GetBranchById(int branchId)
        {
            try
            {
                var branchEntity = await _context.BranchEntities
                    .FirstOrDefaultAsync(b => b.BranchId == branchId && !b.IsDeleted);

                if (branchEntity == null)
                    return Result<Branch>.Failure("Branch not found.");

                var branch = _mapper.Map<Branch>(branchEntity);
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
                var branchEntities = await _context.BranchEntities
                    .Where(b => !b.IsDeleted)
                    .ToListAsync();

                var branches = _mapper.Map<List<Branch>>(branchEntities);
                return Result<List<Branch>>.Success(branches);
            }
            catch (Exception ex)
            {
                return Result<List<Branch>>.Failure($"Failed to retrieve branches: {ex.Message}");
            }
        }


        public async Task<Result<bool>> RemoveBranch(int branchId)
        {
            try
            {
                var branchEntity = await _context.BranchEntities
                    .FirstOrDefaultAsync(b => b.BranchId == branchId);

                if (branchEntity == null)
                    return Result<bool>.Failure("Branch not found.");

                branchEntity.IsDeleted = true; 
                _context.BranchEntities.Update(branchEntity);
                await _context.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to remove branch: {ex.Message}");
            }
        }


    }
}
