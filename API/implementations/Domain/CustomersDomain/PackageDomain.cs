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
                // Verificar si el cliente existe
                var customerEntity = await _context.CustomerEntities.FindAsync(customerId);
                if (customerEntity == null)
                {
                    return Result<Package>.Failure("Cliente no encontrado");
                }

                // Crear la entidad de paquete
                var packageEntity = new PackageEntity
                {
                    PackageName = package.Name,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                _context.PackageEntities.Add(packageEntity);
                await _context.SaveChangesAsync();

                // Asignar el ID generado al modelo
                package.Id = packageEntity.PackageId.ToString();
                package.Customer = new Customer { CustomerId = customerId };

                // Si hay elementos en el paquete, agregarlos
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

                // Si hay notas en el paquete, agregarlas
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
                return Result<Package>.Failure($"Error al crear el paquete: {ex.Message}");
            }
        }

        public async Task<Result<Package>> GetPackageByIdAsync(string packageId)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<Package>.Failure("ID de paquete inválido");
                }

                var packageEntity = await _context.PackageEntities
                    .Include(p => p.PackageItemEntities)
                        .ThenInclude(pi => pi.SkuNavigation)
                    .Include(p => p.PackageNoteEntities)
                    .FirstOrDefaultAsync(p => p.PackageId == id && !p.IsDeleted);

                if (packageEntity == null)
                {
                    return Result<Package>.Failure("Paquete no encontrado");
                }

                var package = MapToModel(packageEntity);
                return Result<Package>.Success(package);
            }
            catch (Exception ex)
            {
                return Result<Package>.Failure($"Error al obtener el paquete: {ex.Message}");
            }
        }

        public async Task<Result<List<Package>>> GetPackagesByCustomerIdAsync(int customerId)
        {
            try
            {
                // Verificar si el cliente existe
                var customerEntity = await _context.CustomerEntities.FindAsync(customerId);
                if (customerEntity == null)
                {
                    return Result<List<Package>>.Failure("Cliente no encontrado");
                }

                // En este caso, asumimos que necesitamos un método adicional para obtener los paquetes de un cliente
                // Esto podría requerir una relación adicional en la base de datos o una consulta personalizada
                // Para este ejemplo, vamos a devolver una lista vacía
                var packages = new List<Package>();

                return Result<List<Package>>.Success(packages);
            }
            catch (Exception ex)
            {
                return Result<List<Package>>.Failure($"Error al obtener los paquetes: {ex.Message}");
            }
        }

        public async Task<Result<Package>> UpdatePackageAsync(Package package)
        {
            try
            {
                if (!int.TryParse(package.Id, out int id))
                {
                    return Result<Package>.Failure("ID de paquete inválido");
                }

                var packageEntity = await _context.PackageEntities.FindAsync(id);
                if (packageEntity == null)
                {
                    return Result<Package>.Failure("Paquete no encontrado");
                }

                // Actualizar propiedades básicas
                packageEntity.PackageName = package.Name;
                packageEntity.UpdatedAt = DateTime.UtcNow;

                _context.PackageEntities.Update(packageEntity);
                await _context.SaveChangesAsync();

                return await GetPackageByIdAsync(package.Id);
            }
            catch (Exception ex)
            {
                return Result<Package>.Failure($"Error al actualizar el paquete: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeletePackageAsync(string packageId)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<bool>.Failure("ID de paquete inválido");
                }

                var packageEntity = await _context.PackageEntities.FindAsync(id);
                if (packageEntity == null)
                {
                    return Result<bool>.Failure("Paquete no encontrado");
                }

                // Marcar como eliminado en lugar de eliminar físicamente
                packageEntity.IsDeleted = true;
                packageEntity.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error al eliminar el paquete: {ex.Message}");
            }
        }

        public async Task<Result<Package>> AddItemToPackageAsync(string packageId, int itemSku, int quantity)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<Package>.Failure("ID de paquete inválido");
                }

                if (quantity <= 0)
                {
                    return Result<Package>.Failure("La cantidad debe ser positiva");
                }

                // Verificar si el paquete existe
                var packageEntity = await _context.PackageEntities
                    .Include(p => p.PackageItemEntities)
                    .FirstOrDefaultAsync(p => p.PackageId == id && !p.IsDeleted);

                if (packageEntity == null)
                {
                    return Result<Package>.Failure("Paquete no encontrado");
                }

                // Verificar si el item existe
                var itemEntity = await _context.ItemEntities.FindAsync(itemSku);
                if (itemEntity == null)
                {
                    return Result<Package>.Failure("Item no encontrado");
                }

                // Verificar si el item ya está en el paquete
                var existingItem = packageEntity.PackageItemEntities
                    .FirstOrDefault(pi => pi.Sku == itemSku);

                if (existingItem != null)
                {
                    // Actualizar la cantidad
                    existingItem.ItemQuantity += quantity;
                }
                else
                {
                    // Añadir nuevo item al paquete
                    var packageItemEntity = new PackageItemEntity
                    {
                        PackageId = id,
                        Sku = itemSku,
                        ItemQuantity = quantity
                    };

                    _context.PackageItemEntities.Add(packageItemEntity);
                }

                // Actualizar la fecha de actualización del paquete
                packageEntity.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Obtener el paquete actualizado
                return await GetPackageByIdAsync(packageId);
            }
            catch (Exception ex)
            {
                return Result<Package>.Failure($"Error al añadir item al paquete: {ex.Message}");
            }
        }

        public async Task<Result<Package>> RemoveItemFromPackageAsync(string packageId, int itemSku)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<Package>.Failure("ID de paquete inválido");
                }

                // Verificar si el paquete existe
                var packageEntity = await _context.PackageEntities
                    .Include(p => p.PackageItemEntities)
                    .FirstOrDefaultAsync(p => p.PackageId == id && !p.IsDeleted);

                if (packageEntity == null)
                {
                    return Result<Package>.Failure("Paquete no encontrado");
                }

                // Verificar si el item existe en el paquete
                var existingItem = packageEntity.PackageItemEntities
                    .FirstOrDefault(pi => pi.Sku == itemSku);

                if (existingItem == null)
                {
                    return Result<Package>.Failure($"Item con SKU {itemSku} no encontrado en el paquete");
                }

                // Eliminar el item del paquete
                _context.PackageItemEntities.Remove(existingItem);

                // Actualizar la fecha de actualización del paquete
                packageEntity.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Obtener el paquete actualizado
                return await GetPackageByIdAsync(packageId);
            }
            catch (Exception ex)
            {
                return Result<Package>.Failure($"Error al eliminar item del paquete: {ex.Message}");
            }
        }

        public async Task<Result<Package>> AddNoteToPackageAsync(string packageId, PackageNote note)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<Package>.Failure("ID de paquete inválido");
                }

                // Verificar si el paquete existe
                var packageEntity = await _context.PackageEntities.FindAsync(id);
                if (packageEntity == null)
                {
                    return Result<Package>.Failure("Paquete no encontrado");
                }

                // Crear la nota
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

                // Actualizar la fecha de actualización del paquete
                packageEntity.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Obtener el paquete actualizado
                return await GetPackageByIdAsync(packageId);
            }
            catch (Exception ex)
            {
                return Result<Package>.Failure($"Error al añadir nota al paquete: {ex.Message}");
            }
        }

        public async Task<Result<Package>> UpdatePackageStatusAsync(string packageId, string status, string updatedBy, string notes)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<Package>.Failure("ID de paquete inválido");
                }

                // Verificar si el paquete existe
                var packageEntity = await _context.PackageEntities.FindAsync(id);
                if (packageEntity == null)
                {
                    return Result<Package>.Failure("Paquete no encontrado");
                }

                // Crear una nota para el cambio de estado si se proporcionaron notas
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

                // Actualizar la fecha de actualización del paquete
                packageEntity.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Obtener el paquete actualizado
                return await GetPackageByIdAsync(packageId);
            }
            catch (Exception ex)
            {
                return Result<Package>.Failure($"Error al actualizar el estado del paquete: {ex.Message}");
            }
        }

        public async Task<Result<decimal>> CalculateTotalPriceAsync(string packageId)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<decimal>.Failure("ID de paquete inválido");
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
                return Result<decimal>.Failure($"Error al calcular el precio total: {ex.Message}");
            }
        }

        public async Task<Result<decimal>> CalculateDiscountedPriceAsync(string packageId)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<decimal>.Failure("ID de paquete inválido");
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
                return Result<decimal>.Failure($"Error al calcular el precio con descuento: {ex.Message}");
            }
        }

        public async Task<Result<decimal>> CalculateTotalContractValueAsync(string packageId)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<decimal>.Failure("ID de paquete inválido");
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
                return Result<decimal>.Failure($"Error al calcular el valor total del contrato: {ex.Message}");
            }
        }

        public async Task<Result<bool>> IsContractActiveAsync(string packageId)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<bool>.Failure("ID de paquete inválido");
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
                return Result<bool>.Failure($"Error al verificar si el contrato está activo: {ex.Message}");
            }
        }

        public async Task<Result<TimeSpan>> GetRemainingContractTimeAsync(string packageId)
        {
            try
            {
                if (!int.TryParse(packageId, out int id))
                {
                    return Result<TimeSpan>.Failure("ID de paquete inválido");
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
                return Result<TimeSpan>.Failure($"Error al obtener el tiempo restante del contrato: {ex.Message}");
            }
        }

        private Package MapToModel(PackageEntity entity)
        {
            var package = new Package
            {
                Id = entity.PackageId.ToString(),
                Name = entity.PackageName,
                Description = string.Empty, // No disponible en la entidad
                SaleDate = entity.CreatedAt, // Asumimos la fecha de creación como fecha de venta
                Status = "Processing", // Valor predeterminado ya que no está en la entidad
                ContractTermMonths = 12, // Valor predeterminado
                ContractStartDate = entity.CreatedAt,
                MonthlyFee = 0, // Valor predeterminado
                SetupFee = 0, // Valor predeterminado
                DiscountAmount = 0, // Valor predeterminado
                PaymentMethod = "Credit Card", // Valor predeterminado
                ShippingAddress = string.Empty, // Valor predeterminado
                IsRenewal = false, // Valor predeterminado
                Items = new List<PackageItem>(),
                Notes = new List<PackageNote>()
            };

            // Mapear los items del paquete
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

            // Mapear las notas del paquete
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