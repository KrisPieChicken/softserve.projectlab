using API.Domain.Logistics;
using API.Implementations.Domain;
using API.Models.Logistics;
using API.Models.Logistics.Interfaces;
using API.Services;
using API.Services.Interfaces;
using API.Services.Logistics;
using API.Services.OrderService;
using Logistics.Models;
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
        services.AddScoped<IWarehouse, Warehouse>();
        services.AddScoped<WarehouseDomain>();
        return services;
    }

    public static IServiceCollection AddBranchServices(this IServiceCollection services)
    {
        // Register the branch services
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<IBranch, Branch>();
        services.AddScoped<BranchDomain>();
        return services;
    }

    public static IServiceCollection AddOrderServices(this IServiceCollection services)
    {
        // Register the order services
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrder, Order>();
        services.AddScoped<OrderDomain>();
        return services;
    }

    public static IServiceCollection AddSupplierServices(this IServiceCollection services)
    {
        // Register the supplier services
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<ISupplier, Supplier>();
        services.AddScoped<SupplierDomain>();
        return services;
    }

    public static IServiceCollection AddSupplierOrderServices(this IServiceCollection services)
    {
        // Register the supplier order services
        services.AddScoped<ISupplierOrderService, SupplierOrderService>();
        services.AddScoped<SupplierOrderDomain>();
        return services;
    }
}