using API.Services.Logistics;
using API.Services.OrderService;
using API.Services.Interfaces;
using API.Services.IntAdmin;
using API.Implementations.Domain;
using API.Services;
using softserve.projectlabs.Shared.DTOs;
using API.Data.Repositories.IntAdministrationRepository.Interfaces;
using API.Data.Repositories.IntAdministrationRepository;
using API.Data.Repositories.LogisticsRepositories;
using API.Data.Repositories.LogisticsRepositories.Interfaces;
using API.Services.IntAdministration;
using softserve.projectlabs.Shared.Interfaces;

namespace API.Utils.Extensions;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddProjectServices(this IServiceCollection services)
    {
        // 1. Logistics services using extension methods
        services.AddWarehouseServices();
        services.AddBranchServices();
        services.AddOrderServices();
        services.AddSupplierServices();

        // 2. Domain services
        services.AddScoped<CustomerDomain>();
        services.AddScoped<BranchDomain>();
        services.AddScoped<OrderDomain>();
        services.AddScoped<SupplierDomain>();
        services.AddScoped<WarehouseDomain>();
        services.AddScoped<CatalogDomain>();
        services.AddScoped<CategoryDomain>();
        services.AddScoped<ItemDomain>();
        services.AddScoped<PermissionDomain>();
        services.AddScoped<RoleDomain>();
        services.AddScoped<UserDomain>();
        services.AddScoped<LineOfCreditDomain>();
        services.AddScoped<CartDomain>();
        services.AddScoped<PackageDomain>();
        services.AddScoped<TokenGenerator>();

        // 3. Interface services
        services.AddHttpContextAccessor();
        services.AddScoped<ICustomerService, API.Services.CustomerService>();
        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILineOfCreditService, LineOfCreditService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IPackageService, PackageService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICookieService, CookieService>();

        // 5. Repositories (Data layer)
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<ICatalogRepository, CatalogRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();

        // 6. Logistics DTOs
        services.AddScoped<WarehouseDto>();
        services.AddScoped<BranchDto>();
        services.AddScoped<OrderDto>();
        services.AddScoped<SupplierDto>();

        return services;
    }
}
