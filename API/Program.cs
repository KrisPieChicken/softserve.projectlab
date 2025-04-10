using API.Models.Logistics.Interfaces;
using API.Models.Logistics;
using API.Services;
using API.Services.Logistics;
using API.Services.OrderService;
using Logistics.Models;
using API.Services.Interfaces;
using API.Services.IntAdmin;
using API.Implementations.Domain;
using API.Domain.Logistics;
using API.Utils.Extensions;
using Microsoft.EntityFrameworkCore;
using API.Data.Mapping;
using API.Data;
using softserve.projectlabs.Shared.Interfaces;
using AutoMapper;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddControllers()
       .AddJsonOptions(options =>
       {
           options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
           options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
       });

// Add services to the container using the extension method
//builder.Services.AddCustomerServices();
builder.Services.AddWarehouseServices();
builder.Services.AddBranchServices();
builder.Services.AddOrderServices();
builder.Services.AddSupplierServices();
builder.Services.AddSupplierOrderServices();

//// Register your services
builder.Services.AddScoped<API.Implementations.Domain.CustomerDomain>();
builder.Services.AddScoped<API.Services.Interfaces.ICustomerService, API.Services.CustomerService>();

builder.Services.AddScoped<IWarehouse, Warehouse>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IBranch, Branch>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrder, Order>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ISupplier, Supplier>();
builder.Services.AddScoped<BranchDomain>();
builder.Services.AddScoped<OrderDomain>();
builder.Services.AddScoped<SupplierDomain>();
builder.Services.AddScoped<SupplierOrderDomain>();
builder.Services.AddScoped<WarehouseDomain>();
builder.Services.AddScoped<CatalogDomain>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<CategoryDomain>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ItemDomain>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<PermissionDomain>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<RoleDomain>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<UserDomain>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISupplierOrderService, SupplierOrderService>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<LogisticsMapping>();
    cfg.AddProfile<CustomerMapping>();
});
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<LogisticsMapping>();
    cfg.AddProfile<CustomerMapping>();
});
mapperConfig.AssertConfigurationIsValid();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7153") // Replace with your Blazor WebAssembly frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
}

app.Run();
