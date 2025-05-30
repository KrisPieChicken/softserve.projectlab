using API.Data.Entities;
using API.Models.Customers;
using AutoMapper;

namespace API.Data.Mapping
{
    public class CustomerMapping : Profile
    {
        public CustomerMapping()
        {
            // Map Customer to CustomerEntity
            CreateMap<Customer, CustomerEntity>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.ZipCode))
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.RegistrationDate));

            // Map Cart to CartEntity
            CreateMap<Cart, CartEntity>()
                .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Customer.Id))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

            // Map CartItem to CartItemEntity
            CreateMap<CartItem, CartItemEntity>()
                .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Item.Sku))
                .ForMember(dest => dest.ItemQuantity, opt => opt.MapFrom(src => src.Quantity));

            // Map Package to PackageEntity
            CreateMap<Package, PackageEntity>()
                .ForMember(dest => dest.PackageId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.Name));

            // Map PackageItem to PackageItemEntity
            CreateMap<PackageItem, PackageItemEntity>()
                .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Item.Sku))
                .ForMember(dest => dest.ItemQuantity, opt => opt.MapFrom(src => src.Quantity));

            // Map PackageNote to PackageNoteEntity
            CreateMap<PackageNote, PackageNoteEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}