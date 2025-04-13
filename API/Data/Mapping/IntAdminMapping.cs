using AutoMapper;
using softserve.projectlabs.Shared.DTOs;
using API.Data.Entities;
using System.Linq;

namespace API.Mappings
{
    public class IntAdminMapping : Profile
    {
        public IntAdminMapping()
        {
            // Mapeo de CategoryDto a CategoryEntity
            CreateMap<CategoryDto, CategoryEntity>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId.HasValue ? src.CategoryId.Value : 0))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.CategoryStatus, opt => opt.MapFrom(src => src.CategoryStatus));

            // Mapeo de CatalogDto a CatalogEntity
            CreateMap<CatalogDto, CatalogEntity>()
                .ForMember(dest => dest.CatalogId, opt => opt.MapFrom(src => src.CatalogId.HasValue ? src.CatalogId.Value : 0))
                .ForMember(dest => dest.CatalogName, opt => opt.MapFrom(src => src.CatalogName))
                .ForMember(dest => dest.CatalogDescription, opt => opt.MapFrom(src => src.CatalogDescription))
                .ForMember(dest => dest.CatalogStatus, opt => opt.MapFrom(src => src.CatalogStatus))
                // Si deseas mapear los CategoryIds a la colección de CatalogCategoryEntities,
                // necesitarás implementar una transformación o resolver personalizado.
                .ForMember(dest => dest.CatalogCategoryEntities, opt => opt.Ignore());

            // Mapeo de ItemDto a ItemEntity
            CreateMap<ItemDto, ItemEntity>()
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId.HasValue ? src.ItemId.Value : 0))
                .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Sku))
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.ItemName))
                .ForMember(dest => dest.ItemDescription, opt => opt.MapFrom(src => src.ItemDescription))
                .ForMember(dest => dest.OriginalStock, opt => opt.MapFrom(src => src.OriginalStock))
                .ForMember(dest => dest.CurrentStock, opt => opt.MapFrom(src => src.CurrentStock))
                .ForMember(dest => dest.ItemCurrency, opt => opt.MapFrom(src => src.ItemCurrency))
                .ForMember(dest => dest.ItemUnitCost, opt => opt.MapFrom(src => src.ItemUnitCost))
                .ForMember(dest => dest.ItemMarginGain, opt => opt.MapFrom(src => src.ItemMarginGain))
                .ForMember(dest => dest.ItemDiscount, opt => opt.MapFrom(src => src.ItemDiscount))
                .ForMember(dest => dest.ItemAdditionalTax, opt => opt.MapFrom(src => src.ItemAdditionalTax))
                .ForMember(dest => dest.ItemPrice, opt => opt.MapFrom(src => src.ItemPrice))
                .ForMember(dest => dest.ItemStatus, opt => opt.MapFrom(src => src.ItemStatus))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.ItemImage, opt => opt.MapFrom(src => src.ItemImage));

            // Mapeo de PermissionDto a PermissionEntity
            CreateMap<PermissionDto, PermissionEntity>()
                .ForMember(dest => dest.PermissionName, opt => opt.MapFrom(src => src.PermissionName))
                .ForMember(dest => dest.PermissionDescription, opt => opt.MapFrom(src => src.PermissionDescription));

            // Mapeo de RoleDto a RoleEntity
            CreateMap<RoleDto, RoleEntity>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
                .ForMember(dest => dest.RoleDescription, opt => opt.MapFrom(src => src.RoleDescription))
                .ForMember(dest => dest.RoleStatus, opt => opt.MapFrom(src => src.RoleStatus))
                // Para la relación de permisos, se puede necesitar mapear la lista de IDs
                // a la colección de RolePermissionEntities; en este ejemplo se omite ese mapping
                .ForMember(dest => dest.RolePermissionEntities, opt => opt.Ignore())
                // Igualmente, se ignoran otras propiedades de relación que se gestionan aparte.
                .ForMember(dest => dest.UserRoleEntities, opt => opt.Ignore());

            // Mapeo de UserDto a UserEntity
            CreateMap<UserDto, UserEntity>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.HasValue ? src.UserId.Value : 0))
                .ForMember(dest => dest.UserFirstName, opt => opt.MapFrom(src => src.UserFirstName))
                .ForMember(dest => dest.UserLastName, opt => opt.MapFrom(src => src.UserLastName))
                // La propiedad UserEmail en el DTO se mapea a UserContactEmail en la entidad
                .ForMember(dest => dest.UserContactEmail, opt => opt.MapFrom(src => src.UserEmail))
                // Similarmente, UserPhone se mapea a UserContactNumber
                .ForMember(dest => dest.UserContactNumber, opt => opt.MapFrom(src => src.UserPhone))
                .ForMember(dest => dest.UserPassword, opt => opt.MapFrom(src => src.UserPassword))
                .ForMember(dest => dest.UserStatus, opt => opt.MapFrom(src => src.UserStatus))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
                // El mapeo de las relaciones de roles (UserRoleEntities) se deja pendiente de resolver
                .ForMember(dest => dest.UserRoleEntities, opt => opt.Ignore());
        }
    }
}
