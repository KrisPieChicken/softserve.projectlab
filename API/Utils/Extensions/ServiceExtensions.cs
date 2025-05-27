using API.Implementations.Domain;
using API.Services.Logistics;
using API.Services.OrderService;
using softserve.projectlabs.Shared.Interfaces;

namespace API.Utils.Extensions;

/// <summary>
/// Extension methods for configuring services.
/// </summary>
public static class ServiceExtensions
{
    public static IServiceCollection AddWarehouseServices(this IServiceCollection services)
    {
        // Register the warehouse services
        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<WarehouseDomain>();
        return services;
    }

    public static IServiceCollection AddBranchServices(this IServiceCollection services)
    {
        // Register the branch services
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<BranchDomain>();
        return services;
    }

    public static IServiceCollection AddOrderServices(this IServiceCollection services)
    {
        // Register the order services
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<OrderDomain>();
        return services;
    }

    public static IServiceCollection AddSupplierServices(this IServiceCollection services)
    {
        // Register the supplier services
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<SupplierDomain>();
        return services;
    }
}