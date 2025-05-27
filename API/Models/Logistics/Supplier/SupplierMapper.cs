using API.Data.Entities;
using softserve.projectlabs.Shared.DTOs.Supplier;

namespace API.Models.Logistics.Supplier
{
    public static class SupplierMapper
    {
        public static Supplier ToDomain(this SupplierDto dto)
            => new Supplier(
                dto.SupplierId,
                dto.Name,
                dto.ContactNumber,
                dto.ContactEmail,
                dto.Address,
                dto.IsDeleted,
                dto.CreatedAt,
                dto.UpdatedAt);

        public static SupplierDto ToDto(this Supplier supplier)
            => new SupplierDto
            {
                SupplierId = supplier.SupplierId,
                Name = supplier.Name,
                ContactNumber = supplier.ContactNumber,
                ContactEmail = supplier.ContactEmail,
                Address = supplier.Address,
                IsDeleted = supplier.IsDeleted,
                CreatedAt = supplier.CreatedAt,
                UpdatedAt = supplier.UpdatedAt
            };

        public static Supplier ToDomain(this SupplierEntity entity)
            => new Supplier(
                entity.SupplierId,
                entity.SupplierName,
                entity.SupplierContactNumber,
                entity.SupplierContactEmail,
                entity.SupplierAddress,
                entity.IsDeleted,
                entity.CreatedAt,
                entity.UpdatedAt);

        public static SupplierEntity ToEntity(this Supplier supplier)
            => new SupplierEntity
            {
                SupplierId = supplier.SupplierId,
                SupplierName = supplier.Name,
                SupplierContactNumber = supplier.ContactNumber,
                SupplierContactEmail = supplier.ContactEmail,
                SupplierAddress = supplier.Address,
                IsDeleted = supplier.IsDeleted,
                CreatedAt = supplier.CreatedAt,
                UpdatedAt = supplier.UpdatedAt
            };
    }
}
