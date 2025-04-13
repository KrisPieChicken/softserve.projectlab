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

            // Map BusinessCustomer to CustomerEntity
            CreateMap<BusinessCustomer, CustomerEntity>()
                .IncludeBase<Customer, CustomerEntity>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CompanyName));

            // Map IndividualCustomer to CustomerEntity
            CreateMap<IndividualCustomer, CustomerEntity>()
                .IncludeBase<Customer, CustomerEntity>();

            // Map PremiumCustomer to CustomerEntity
            CreateMap<PremiumCustomer, CustomerEntity>()
                .IncludeBase<Customer, CustomerEntity>();

            //// Map LineOfCredit to LineOfCreditEntity
            //CreateMap<LineOfCredit, LineOfCreditEntity>()
            //    .ForMember(dest => dest.CreditLimit, opt => opt.MapFrom(src => src.CreditLimit))
            //    .ForMember(dest => dest.AnnualInterestRate, opt => opt.MapFrom(src => src.AnnualInterestRate))
            //    .ForMember(dest => dest.OpenDate, opt => opt.MapFrom(src => src.OpenDate))
            //    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            //// Map Cart to CartEntity
            //CreateMap<Cart, CartEntity>();

            //// Map Package to PackageEntity
            //CreateMap<Package, PackageEntity>()
            //    .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.Name)) // Adjusted to use PackageName
            //    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            //    .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.SaleDate))
            //    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}