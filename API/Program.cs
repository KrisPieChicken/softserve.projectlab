using API.Models.Logistics.Interfaces;
using API.Models.Logistics;
using API.Services;
using API.Services.Logistics;
using API.Services.WareHouseService;
using API.Services.IntAdmin; // Aseg�rate de incluir el namespace correcto para CatalogService
using API.Implementations.Domain; // Para CatalogDomain

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register your services
builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddScoped<IWarehouse, Warehouse>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();

// Register Catalog services
builder.Services.AddScoped<CatalogDomain>();
builder.Services.AddScoped<ICatalogService, CatalogService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
