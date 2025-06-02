using API.Data;
using API.Data.Entities;
using API.Models.Customers;
using API.Models.IntAdmin;
using Microsoft.EntityFrameworkCore;
using softserve.projectlabs.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Implementations.Domain
{
    public class PackageDomain
    {
        private readonly ApplicationDbContext _context;

        public PackageDomain(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Package>> CreatePackageAsync(Package package, int customerId)
        {
            try
            {
                // Check if the customer exists
                var customerEntity = await _context.CustomerEntities.FindAsync(customerId);
                if (customerEntity == null)
                {
                    return Result<Package>.Failure("Customer not found");
                }

                // Create the package entity
                var packageEntity = new PackageEntity
                {
                    PackageName = package.Name,
                    CustomerId = customerId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                _context.PackageEntities.Add(packageEntity);
                await _context.SaveChangesAsync();

                // Assign the generated ID to the model
                package.Id = packageEntity.PackageId.ToString();
                package.Customer = new Customer { CustomerId = customerId };

                // If there are items in the package, add them
                if (package.Items != null && package.Items.Any())
                {
                    foreach (var item in package.Items)
                    {
                        var itemEntity = await _context.ItemEntities.FindAsync(item.Item.Sku);
                        if (itemEntity != null)
                        {
                            var packageItemEntity = new PackageItemEntity
                            {
                                PackageId = packageEntity.PackageId,
                                Sku = item.Item.Sku,
                                ItemQuantity = item.Quantity
                            };

                            _context.PackageItemEntities.Add(packageItemEntity);
                        }
                    }
                }

                // If there are notes in the package, add them
                if (package.Notes != null && package.Notes.Any())
                {
                    foreach (var note in package.Notes)
                    {
                        var packageNoteEntity = new PackageNoteEntity
                        {
                            Id = Guid.NewGuid().ToString(),
                            PackageId = packageEntity.PackageId,
                            Title = note.Title,
                            Content = note.Content,
                            CreatedBy = note.CreatedBy,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            IsDeleted = false
                        };

                        _context.PackageNoteEntities.Add(packageNoteEntity);
                    }
                }

                await _context.SaveChangesAsync();

                return Result<Package>.Success(package);
            }
            catch (Exception ex)
            {
                return Result<Package>.Failure($"Error creating package: {ex.Message}");
            }
        }

        public async Task<Result<Package>> GetPackageByIdAsync(string packageId)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<Package>.Failure("Invalid package ID");
                }

                var packageEntity = await _context.PackageEntities
                    .Include(p => p.PackageItemEntities)
                        .ThenInclude(pi => pi.SkuNavigation)
                    .Include(p => p.PackageNoteEntities)
                    .FirstOrDefaultAsync(p => p.PackageId == id && !p.IsDeleted);

                if (packageEntity == null)
                {
                    return Result<Package>.Failure("Package not found");
                }

                var package = MapToModel(packageEntity);
                return Result<Package>.Success(package);
            }
            catch (Exception ex)
            {
                return Result<Package>.Failure($"Error retrieving package: {ex.Message}");
            }
        }

        public async Task<Result<List<Package>>> GetPackagesByCustomerIdAsync(int customerId)
        {
            try
            {
                var customerEntity = await _context.CustomerEntities.FindAsync(customerId);
                if (customerEntity == null)
                    return Result<List<Package>>.Failure("Customer not found");

                var packageEntities = await _context.PackageEntities
                    .Where(p => p.CustomerId == customerId && !p.IsDeleted)
                    .Include(p => p.PackageItemEntities)
                        .ThenInclude(pi => pi.SkuNavigation)
                    .Include(p => p.PackageNoteEntities)
                    .ToListAsync();


                var packages = packageEntities.Select(MapToModel).ToList();

                return Result<List<Package>>.Success(packages);
            }
            catch (Exception ex)
            {
                return Result<List<Package>>.Failure($"Error retrieving packages: {ex.Message}");
            }
        }


        public async Task<Result<Package>> UpdatePackageAsync(Package package)
        {
            try
            {
                if (!int.TryParse(package.Id, out int id))
                {
                    return Result<Package>.Failure("Invalid package ID");
                }

                var packageEntity = await _context.PackageEntities.FindAsync(id);
                if (packageEntity == null)
                {
                    return Result<Package>.Failure("Package not found");
                }

                // Update basic properties
                packageEntity.PackageName = package.Name;
                packageEntity.UpdatedAt = DateTime.UtcNow;

                _context.PackageEntities.Update(packageEntity);
                await _context.SaveChangesAsync();

                return await GetPackageByIdAsync(package.Id);
            }
            catch (Exception ex)
            {
                return Result<Package>.Failure($"Error updating package: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeletePackageAsync(string packageId)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<bool>.Failure("Invalid package ID");
                }

                var packageEntity = await _context.PackageEntities.FindAsync(id);
                if (packageEntity == null)
                {
                    return Result<bool>.Failure("Package not found");
                }

                // Mark as deleted instead of physically deleting
                packageEntity.IsDeleted = true;
                packageEntity.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error deleting package: {ex.Message}");
            }
        }

        public async Task<Result<Package>> AddItemToPackageAsync(string packageId, int itemSku, int quantity)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<Package>.Failure("Invalid package ID");
                }

                if (quantity <= 0)
                {
                    return Result<Package>.Failure("Quantity must be positive");
                }

                // Check if the package exists
                var packageEntity = await _context.PackageEntities
                    .Include(p => p.PackageItemEntities)
                    .FirstOrDefaultAsync(p => p.PackageId == id && !p.IsDeleted);

                if (packageEntity == null)
                {
                    return Result<Package>.Failure("Package not found");
                }

                // Check if the item exists
                var itemEntity = await _context.ItemEntities.FindAsync(itemSku);
                if (itemEntity == null)
                {
                    return Result<Package>.Failure("Item not found");
                }

                // Check if the item is already in the package
                var existingItem = packageEntity.PackageItemEntities
                    .FirstOrDefault(pi => pi.Sku == itemSku);

                if (existingItem != null)
                {
                    // Update the quantity
                    existingItem.ItemQuantity += quantity;
                }
                else
                {
                    // Add new item to the package
                    var packageItemEntity = new PackageItemEntity
                    {
                        PackageId = id,
                        Sku = itemSku,
                        ItemQuantity = quantity
                    };

                    _context.PackageItemEntities.Add(packageItemEntity);
                }

                // Update the package's updated date
                packageEntity.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Get the updated package
                return await GetPackageByIdAsync(packageId);
            }
            catch (Exception ex)
            {
                return Result<Package>.Failure($"Error adding item to package: {ex.Message}");
            }
        }

        public async Task<Result<Package>> RemoveItemFromPackageAsync(string packageId, int itemSku)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<Package>.Failure("Invalid package ID");
                }

                // Check if the package exists
                var packageEntity = await _context.PackageEntities
                    .Include(p => p.PackageItemEntities)
                    .FirstOrDefaultAsync(p => p.PackageId == id && !p.IsDeleted);

                if (packageEntity == null)
                {
                    return Result<Package>.Failure("Package not found");
                }

                // Check if the item exists in the package
                var existingItem = packageEntity.PackageItemEntities
                    .FirstOrDefault(pi => pi.Sku == itemSku);

                if (existingItem == null)
                {
                    return Result<Package>.Failure($"Item with SKU {itemSku} not found in the package");
                }

                // Remove the item from the package
                _context.PackageItemEntities.Remove(existingItem);

                // Update the package's updated date
                packageEntity.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Get the updated package
                return await GetPackageByIdAsync(packageId);
            }
            catch (Exception ex)
            {
                return Result<Package>.Failure($"Error removing item from package: {ex.Message}");
            }
        }

        public async Task<Result<Package>> AddNoteToPackageAsync(string packageId, PackageNote note)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<Package>.Failure("Invalid package ID");
                }

                // Check if the package exists
                var packageEntity = await _context.PackageEntities.FindAsync(id);
                if (packageEntity == null)
                {
                    return Result<Package>.Failure("Package not found");
                }

                // Create the note
                var packageNoteEntity = new PackageNoteEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    PackageId = id,
                    Title = note.Title,
                    Content = note.Content,
                    CreatedBy = note.CreatedBy,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                _context.PackageNoteEntities.Add(packageNoteEntity);

                // Update the package's updated date
                packageEntity.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Get the updated package
                return await GetPackageByIdAsync(packageId);
            }
            catch (Exception ex)
            {
                return Result<Package>.Failure($"Error adding note to package: {ex.Message}");
            }
        }

        public async Task<Result<Package>> UpdatePackageStatusAsync(string packageId, string status, string updatedBy, string notes)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<Package>.Failure("Invalid package ID");
                }

                // Check if the package exists
                var packageEntity = await _context.PackageEntities.FindAsync(id);
                if (packageEntity == null)
                {
                    return Result<Package>.Failure("Package not found");
                }

                // Create a note for the status change if notes were provided
                if (!string.IsNullOrEmpty(notes))
                {
                    var packageNoteEntity = new PackageNoteEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        PackageId = id,
                        Title = $"Status Update: {status}",
                        Content = notes,
                        CreatedBy = updatedBy,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    };

                    _context.PackageNoteEntities.Add(packageNoteEntity);
                }

                // Update the package's updated date
                packageEntity.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Get the updated package
                return await GetPackageByIdAsync(packageId);
            }
            catch (Exception ex)
            {
                return Result<Package>.Failure($"Error updating package status: {ex.Message}");
            }
        }

        public async Task<Result<decimal>> CalculateTotalPriceAsync(string packageId)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<decimal>.Failure("Invalid package ID");
                }

                var packageResult = await GetPackageByIdAsync(packageId);
                if (!packageResult.IsSuccess)
                {
                    return Result<decimal>.Failure(packageResult.ErrorMessage);
                }

                var package = packageResult.Data;
                decimal totalPrice = package.CalculateTotalPrice();

                return Result<decimal>.Success(totalPrice);
            }
            catch (Exception ex)
            {
                return Result<decimal>.Failure($"Error calculating total price: {ex.Message}");
            }
        }

        public async Task<Result<decimal>> CalculateDiscountedPriceAsync(string packageId)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<decimal>.Failure("Invalid package ID");
                }

                var packageResult = await GetPackageByIdAsync(packageId);
                if (!packageResult.IsSuccess)
                {
                    return Result<decimal>.Failure(packageResult.ErrorMessage);
                }

                var package = packageResult.Data;
                decimal discountedPrice = package.CalculateDiscountedPrice();

                return Result<decimal>.Success(discountedPrice);
            }
            catch (Exception ex)
            {
                return Result<decimal>.Failure($"Error calculating discounted price: {ex.Message}");
            }
        }

        public async Task<Result<decimal>> CalculateTotalContractValueAsync(string packageId)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<decimal>.Failure("Invalid package ID");
                }

                var packageResult = await GetPackageByIdAsync(packageId);
                if (!packageResult.IsSuccess)
                {
                    return Result<decimal>.Failure(packageResult.ErrorMessage);
                }

                var package = packageResult.Data;
                decimal totalContractValue = package.CalculateTotalContractValue();

                return Result<decimal>.Success(totalContractValue);
            }
            catch (Exception ex)
            {
                return Result<decimal>.Failure($"Error calculating total contract value: {ex.Message}");
            }
        }

        public async Task<Result<bool>> IsContractActiveAsync(string packageId)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<bool>.Failure("Invalid package ID");
                }

                var packageResult = await GetPackageByIdAsync(packageId);
                if (!packageResult.IsSuccess)
                {
                    return Result<bool>.Failure(packageResult.ErrorMessage);
                }

                var package = packageResult.Data;
                bool isActive = package.IsContractActive();

                return Result<bool>.Success(isActive);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error checking if contract is active: {ex.Message}");
            }
        }

        public async Task<Result<TimeSpan>> GetRemainingContractTimeAsync(string packageId)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<TimeSpan>.Failure("Invalid package ID");
                }

                var packageResult = await GetPackageByIdAsync(packageId);
                if (!packageResult.IsSuccess)
                {
                    return Result<TimeSpan>.Failure(packageResult.ErrorMessage);
                }

                var package = packageResult.Data;
                TimeSpan remainingTime = package.GetRemainingContractTime();

                return Result<TimeSpan>.Success(remainingTime);
            }
            catch (Exception ex)
            {
                return Result<TimeSpan>.Failure($"Error getting remaining contract time: {ex.Message}");
            }
        }

        private Package MapToModel(PackageEntity entity)
        {
            var package = new Package
            {
                Id = entity.PackageId.ToString(),
                Name = entity.PackageName,
                Description = string.Empty, // Not available in the entity
                SaleDate = entity.CreatedAt, // Assume creation date as sale date
                Status = "Processing", // Default value as it's not in the entity
                ContractTermMonths = 12, // Default value
                ContractStartDate = entity.CreatedAt,
                MonthlyFee = 0, // Default value
                SetupFee = 0, // Default value
                DiscountAmount = 0, // Default value
                PaymentMethod = "Credit Card", // Default value
                ShippingAddress = string.Empty, // Default value
                IsRenewal = false, // Default value
                Items = new List<PackageItem>(),
                Notes = new List<PackageNote>()
            };

            // Map package items
            if (entity.PackageItemEntities != null)
            {
                foreach (var itemEntity in entity.PackageItemEntities)
                {
                    var item = new Item
                    {
                        Sku = itemEntity.Sku,
                        ItemName = itemEntity.SkuNavigation.ItemName,
                        ItemDescription = itemEntity.SkuNavigation.ItemDescription,
                        ItemUnitCost = itemEntity.SkuNavigation.ItemUnitCost,
                        ItemPrice = itemEntity.SkuNavigation.ItemPrice
                    };

                    package.Items.Add(new PackageItem
                    {
                        Item = item,
                        Quantity = itemEntity.ItemQuantity
                    });
                }
            }

            // Map package notes
            if (entity.PackageNoteEntities != null)
            {
                foreach (var noteEntity in entity.PackageNoteEntities)
                {
                    package.Notes.Add(new PackageNote
                    {
                        Id = noteEntity.Id,
                        Title = noteEntity.Title,
                        Content = noteEntity.Content,
                        CreatedBy = noteEntity.CreatedBy,
                        CreatedAt = noteEntity.CreatedAt
                    });
                }
            }

            return package;
        }
    }
}