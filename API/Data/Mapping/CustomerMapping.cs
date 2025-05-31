using System;
using System.Linq;
using API.Data.Entities;
using API.Models.Customers;
using AutoMapper;
using softserve.projectlabs.Shared.DTOs.Package;
using softserve.projectlabs.Shared.DTOs.Customer;
using softserve.projectlabs.Shared.DTOs.Cart;
using softserve.projectlabs.Shared.DTOs;
using API.Models.IntAdmin;
using softserve.projectlabs.Shared.DTOs.Category;

namespace API.Data.Mapping
{
    public class CustomerMapping : Profile
    {
        public CustomerMapping()
        {
            // =========================
            // CustomerEntity ↔ CustomerDto
            // =========================
            CreateMap<CustomerEntity, CustomerDto>()
                .IncludeBase<BaseEntity, BaseDto>()
                .ReverseMap()
                .IncludeBase<BaseDto, BaseEntity>()
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CartEntities, opt => opt.Ignore())
                .ForMember(dest => dest.OrderEntities, opt => opt.Ignore());

            // =========================
            // CustomerEntity ↔ Customer (Domain)
            // =========================
            CreateMap<CustomerEntity, Customer>();
            CreateMap<Customer, CustomerEntity>()
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                .ForMember(dest => dest.CartEntities, opt => opt.Ignore())
                .ForMember(dest => dest.OrderEntities, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            // =========================
            // CustomerDto ↔ Customer (Domain)
            // =========================
            CreateMap<CustomerDto, Customer>()
                .ReverseMap();

            // =========================
            // Create ↔ Domain
            // =========================
            CreateMap<CustomerCreateDto, Customer>();

            // =========================
            // CartEntity ↔ CartDto
            // =========================
            CreateMap<CartEntity, CartDto>()
                .ReverseMap()
                .ForMember(e => e.CartId, opt => opt.Ignore())
                .ForMember(e => e.CreatedAt, opt => opt.Ignore())
                .ForMember(e => e.UpdatedAt, opt => opt.Ignore())
                .ForMember(e => e.IsDeleted, opt => opt.Ignore())
                .ForMember(e => e.Customer, opt => opt.Ignore())
                .ForMember(e => e.CartItemEntities, opt => opt.Ignore());

            // =========================
            // CartEntity ↔ Cart (Dominio)
            // =========================
            CreateMap<CartEntity, Cart>()
                .ReverseMap()
                .ForMember(e => e.CartId, opt => opt.Ignore())
                .ForMember(e => e.IsDeleted, opt => opt.Ignore())
                .ForMember(e => e.Customer, opt => opt.Ignore())
                .ForMember(e => e.CartItemEntities, opt => opt.Ignore());

            // =========================
            // CartItemEntity ↔ CartItemDto
            // =========================
            CreateMap<CartItemEntity, CartItemDto>()
                .ReverseMap()
                .ForMember(e => e.Cart, opt => opt.Ignore())
                .ForMember(e => e.SkuNavigation, opt => opt.Ignore());

            // =========================
            // CartItemEntity ↔ CartItem (Dominio)
            // =========================
            CreateMap<CartItemEntity, Cart>()
                .ReverseMap()
                .ForMember(e => e.Cart, opt => opt.Ignore())
                .ForMember(e => e.SkuNavigation, opt => opt.Ignore());

            // =========================
            // PackageEntity ↔ PackageDto
            // =========================
            CreateMap<PackageEntity, PackageDto>()
                .ReverseMap()
                .ForMember(e => e.PackageId, opt => opt.Ignore())
                .ForMember(e => e.PackageName, opt => opt.Ignore())
                .ForMember(e => e.IsDeleted, opt => opt.Ignore())
                .ForMember(e => e.PackageItemEntities, opt => opt.Ignore())
                .ForMember(e => e.PackageNoteEntities, opt => opt.Ignore());

            // =========================
            // PackageEntity ↔ Package
            // =========================
            CreateMap<PackageEntity, Package>()
                .ReverseMap()
                .ForMember(e => e.PackageId, opt => opt.Ignore())
                .ForMember(e => e.PackageName, opt => opt.Ignore())
                .ForMember(e => e.IsDeleted, opt => opt.Ignore())
                .ForMember(e => e.PackageItemEntities, opt => opt.Ignore())
                .ForMember(e => e.PackageNoteEntities, opt => opt.Ignore());

            // =========================
            // PackageItemEntity ↔ PackageItemDto
            // =========================
            CreateMap<PackageItemEntity, PackageItemDto>()
                .ReverseMap()
                .ForMember(e => e.Package, opt => opt.Ignore())
                .ForMember(e => e.SkuNavigation, opt => opt.Ignore());

            // =========================
            // PackageItemEntity ↔ PackageItem
            // =========================
            CreateMap<PackageItemEntity, PackageItem>()
                .ReverseMap()
                .ForMember(e => e.Package, opt => opt.Ignore())
                .ForMember(e => e.SkuNavigation, opt => opt.Ignore());

            // =========================
            // PackageNoteEntity ↔ PackageNoteDto
            // =========================
            CreateMap<PackageNoteEntity, PackageNoteDto>()
                .ReverseMap()
                .ForMember(e => e.Package, opt => opt.Ignore())
                .ForMember(e => e.IsDeleted, opt => opt.Ignore())
                .ForMember(e => e.UpdatedAt, opt => opt.Ignore());

            // =========================
            // PackageNoteEntity ↔ PackageNote
            // =========================
            CreateMap<PackageNoteEntity, PackageNote>()
                .ReverseMap()
                .ForMember(e => e.Package, opt => opt.Ignore())
                .ForMember(e => e.IsDeleted, opt => opt.Ignore())
                .ForMember(e => e.UpdatedAt, opt => opt.Ignore());
        }
    }
}
