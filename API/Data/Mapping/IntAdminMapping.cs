using System.Linq;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using softserve.projectlabs.Shared.DTOs;
using API.Data.Entities;
using API.Models.IntAdmin;
using softserve.projectlabs.Shared.DTOs.Catalog;
using softserve.projectlabs.Shared.DTOs.Category;
using softserve.projectlabs.Shared.DTOs.User;
using softserve.projectlabs.Shared.DTOs.Item;
using softserve.projectlabs.Shared.DTOs.Permission;
using softserve.projectlabs.Shared.DTOs.Role;

namespace API.Mappings
{
    public class IntAdminMapping : Profile
    {
        public IntAdminMapping()
        {
            // =========================
            // Category | DTO ↔ Entity
            // =========================
            CreateMap<CategoryEntity, CategoryDto>()
                .IncludeBase<BaseEntity, BaseDto>()
                .ReverseMap()
                .IncludeBase<BaseDto, BaseEntity>()
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())                     // Ignoring the PK to avoid conflicts in Update
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CatalogCategoryEntities, opt => opt.Ignore());       // Intermediate relation handled from Catalog

            // =========================
            // Category | Entity ↔ Domain model
            // =========================
            CreateMap<CategoryEntity, Category>();
            CreateMap<Category, CategoryEntity>()
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())                    // Ignoring the PK to avoid conflicts in Update
                .ForMember(dest => dest.CatalogCategoryEntities, opt => opt.Ignore())       // Relation handled from Catalog
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            // =========================
            // Category | DTO ↔ Domain
            // =========================
            CreateMap<CategoryDto, Category>()
                .ReverseMap();

            // =========================
            // Create ↔ Domain
            // =========================
            CreateMap<CategoryCreateDto, Category>();

            // =========================
            // Update ↔ Domain
            // =========================
            CreateMap<CategoryUpdateDto, Category>();

            // =========================
            // Catalog | DTO ↔ Entity
            // =========================
            CreateMap<CatalogEntity, CatalogDto>()
                .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(
                    src => src.CatalogCategoryEntities != null
                        ? src.CatalogCategoryEntities.Select(cc => cc.CategoryId)
                        : Enumerable.Empty<int>()
                ))
                .ReverseMap()
                .ForMember(dest => dest.CatalogId, opt => opt.Ignore())                     // Ignoring the PK to avoid conflicts in Update
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CatalogCategoryEntities, opt =>
                {
                    opt.PreCondition(src => src.CategoryIds != null);
                    opt.UseDestinationValue();
                })
                .EqualityComparison((dto, entity) =>
                    (dto.CategoryIds != null && entity.CatalogCategoryEntities != null)
                        ? dto.CategoryIds.OrderBy(id => id).SequenceEqual(
                            entity.CatalogCategoryEntities.Select(cc => cc.CategoryId).OrderBy(id => id))
                        : (dto.CategoryIds == null && entity.CatalogCategoryEntities == null)
                );

            // Tell AutoMapper how to convert each int → CatalogCategoryEntity
            CreateMap<int, CatalogCategoryEntity>()
                .ConstructUsing(src => new CatalogCategoryEntity { CategoryId = src })
                .EqualityComparison((src, dest) => src == dest.CategoryId);

            // =========================
            // Catalog | Entity ↔ Domain model
            // =========================
            CreateMap<CatalogEntity, Catalog>()
                .ForMember(dest => dest.Categories,
                           opt => opt.MapFrom(src => src.CatalogCategoryEntities.Where(cc => !cc.Category.IsDeleted)));

            CreateMap<Catalog, CatalogEntity>()
                .ForMember(dest => dest.CatalogId, opt => opt.Ignore())
                .ForMember(dest => dest.CatalogCategoryEntities, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            // =========================
            // Catalog | DTO ↔ Domain
            // =========================
            CreateMap<CatalogDto, Catalog>().ReverseMap();

            // Create ↔ Domain
            CreateMap<CatalogCreateDto, Catalog>()
                .ForMember(dest => dest.Categories, opt => opt.Ignore()); // se cargan desde CategoryIds

            // Update ↔ Domain
            CreateMap<CatalogUpdateDto, Catalog>()
                .ForMember(dest => dest.Categories, opt => opt.Ignore());

            // Read (Domain → DTO)
            CreateMap<Catalog, CatalogDto>()
                .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(
                    src => src.Categories.Select(c => c.CategoryId)
                ));

            // =========================
            // Item | DTO ↔ Entity
            // =========================
            CreateMap<ItemEntity, ItemDto>()
                .IncludeBase<BaseEntity, BaseDto>()
                .ReverseMap()
                .IncludeBase<BaseDto, BaseEntity>()
                .ForMember(dest => dest.ItemId, opt => opt.Ignore())                     // Ignoring the PK to avoid conflicts in Update
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            // =========================
            // Item | Entity ↔ Domain model
            // =========================
            CreateMap<ItemEntity, Item>();
            CreateMap<Item, ItemEntity>()
                .ForMember(dest => dest.ItemId, opt => opt.Ignore())                     // Ignoring the PK to avoid conflicts in Update
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            // =========================
            // Item | DTO ↔ Domain
            // =========================
            CreateMap<ItemDto, Item>().ReverseMap();

            // =========================
            // Create ↔ Domain
            // =========================
            CreateMap<ItemCreateDto, Item>();

            // =========================
            // Update ↔ Domain
            // =========================
            CreateMap<ItemUpdateDto, Item>();

            // =========================
            // Permission | DTO ↔ Entity
            // =========================
            CreateMap<PermissionEntity, PermissionDto>()
                .IncludeBase<BaseEntity, BaseDto>()
                .ReverseMap()
                .IncludeBase<BaseDto, BaseEntity>()
                .ForMember(dest => dest.PermissionId, opt => opt.Ignore()); // Ignoring the PK to avoid conflicts in Update

            // =========================
            // Permission | Entity ↔ Domain model
            // =========================
            CreateMap<PermissionEntity, Permission>();
            CreateMap<Permission, PermissionEntity>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.RolePermissionEntities, opt => opt.Ignore());

            // =========================
            // Permission | DTO ↔ Domain
            // =========================
            CreateMap<PermissionDto, Permission>()
                .ReverseMap();

            // =========================
            // Create ↔ Domain
            // =========================
            CreateMap<PermissionCreateDto, Permission>();

            // =========================
            // Update ↔ Domain
            // =========================
            CreateMap<PermissionUpdateDto, Permission>();


            // =========================
            // Role | DTO ↔ Entity
            // =========================
            CreateMap<RoleEntity, RoleDto>()
                .IncludeBase<BaseEntity, BaseDto>()
                .ForMember(dest => dest.PermissionIds, opt => opt.MapFrom(
                    src => src.RolePermissionEntities != null
                        ? src.RolePermissionEntities.Select(rp => rp.PermissionId)
                        : Enumerable.Empty<int>()
                ))
                .ReverseMap()
                .IncludeBase<BaseDto, BaseEntity>()
                .ForMember(dest => dest.RoleId, opt => opt.Ignore())                     // ignore PK on updates
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.RolePermissionEntities, opt => {
                    opt.PreCondition(src => src.PermissionIds != null);
                    opt.UseDestinationValue();
                })
                .ForMember(dest => dest.UserRoleEntities, opt => opt.Ignore())
                .EqualityComparison((dto, entity) =>
                    (dto.PermissionIds != null && entity.RolePermissionEntities != null)
                        ? dto.PermissionIds.OrderBy(id => id).SequenceEqual(
                            entity.RolePermissionEntities.Select(rp => rp.PermissionId).OrderBy(id => id))
                        : (dto.PermissionIds == null && entity.RolePermissionEntities == null)
                );

            // =========================
            // Role | Entity ↔ Domain model
            // =========================
            CreateMap<RoleEntity, Role>();
            CreateMap<Role, RoleEntity>()
                .ForMember(dest => dest.RoleId, opt => opt.Ignore())                    // Ignoring the PK to avoid conflicts in Update
                .ForMember(dest => dest.RolePermissionEntities, opt => opt.Ignore())    // Ignoring intermediate relation
                .ForMember(dest => dest.UserRoleEntities, opt => opt.Ignore())          // Ignoring intermediate relation
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            // =========================
            // Role | DTO ↔ Domain
            // =========================
            CreateMap<RoleDto, Role>()
                .ReverseMap();

            // =========================
            // Create ↔ Domain
            // =========================
            CreateMap<RoleCreateDto, Role>();

            // =========================
            // Update ↔ Domain
            // =========================
            CreateMap<RoleUpdateDto, Role>();


            // =========================
            // User | DTO ↔ Entity
            // =========================
            CreateMap<UserEntity, UserDto>()
                .IncludeBase<BaseEntity, BaseDto>()
                .ForMember(dest => dest.RoleIds, opt => opt.MapFrom(
                    src => src.UserRoleEntities != null
                        ? src.UserRoleEntities.Select(ur => ur.RoleId)
                        : Enumerable.Empty<int>()
                ))
                .ReverseMap()
                .IncludeBase<BaseDto, BaseEntity>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())                      // Ignoring the PK to avoid conflicts in Update
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoleEntities, opt =>
                {
                    opt.PreCondition(src => src.RoleIds != null);
                    opt.UseDestinationValue();
                })
                .EqualityComparison((dto, entity) =>
                    (dto.RoleIds != null && entity.UserRoleEntities != null)
                        ? dto.RoleIds.OrderBy(id => id).SequenceEqual(
                            entity.UserRoleEntities.Select(ur => ur.RoleId).OrderBy(id => id))
                        : (dto.RoleIds == null && entity.UserRoleEntities == null)
                );

            // Tell AutoMapper how to convert each int → UserRoleEntity
            CreateMap<int, UserRoleEntity>()
                .ConstructUsing(src => new UserRoleEntity { RoleId = src })
                .EqualityComparison((src, dest) => src == dest.RoleId);

            // =========================
            // User | Entity ↔ Domain model
            // =========================
            CreateMap<UserEntity, User>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Roles are loaded manually or with Include()

            CreateMap<User, UserEntity>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())                    // Ignoring the PK to avoid conflicts in Update
                .ForMember(dest => dest.UserRoleEntities, opt => opt.Ignore())          // Ignoring intermediate relation
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            // =========================
            // User | DTO ↔ Domain
            // =========================
            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore()) // Roles are loaded from RoleIds
                .ReverseMap();

            // =========================
            // Create ↔ Domain
            // =========================
            CreateMap<UserCreateDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Hash generated in the domain
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
                .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Roles are loaded from RoleIds

            // =========================
            // Update ↔ Domain
            // =========================
            CreateMap<UserUpdateDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
                .ForMember(dest => dest.Roles, opt => opt.Ignore());
        }
    }
}
