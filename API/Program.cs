using API.Models.Logistics.Interfaces;
using API.Models.Logistics;
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
using System.Text.Json.Serialization;
using System.Text.Json;
using API.Mappings;

var builder = WebApplication.CreateBuilder(args);

//-------------------------------------------------------------------------------
// Basic configuration and API documentation
//-------------------------------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Include XML comments for API documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});
builder.Services.AddRazorPages();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

//-------------------------------------------------------------------------------
// Controller configuration and JSON serialization (unified)
//-------------------------------------------------------------------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

//-------------------------------------------------------------------------------
// CORS Configuration
//-------------------------------------------------------------------------------
var frontendOrigin = "https://localhost:7135";
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(frontendOrigin)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

//-------------------------------------------------------------------------------
// Cookie Configuration
//-------------------------------------------------------------------------------
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
});

//-------------------------------------------------------------------------------
// Service registration grouped by category
//-------------------------------------------------------------------------------

// 1. Logistics services using extension methods
builder.Services.AddWarehouseServices();
builder.Services.AddBranchServices();
builder.Services.AddOrderServices();
builder.Services.AddSupplierServices();
builder.Services.AddSupplierOrderServices();

// 2. Domain services
builder.Services.AddScoped<CustomerDomain>();
builder.Services.AddScoped<BranchDomain>();
builder.Services.AddScoped<OrderDomain>();
builder.Services.AddScoped<SupplierDomain>();
builder.Services.AddScoped<SupplierOrderDomain>();
builder.Services.AddScoped<WarehouseDomain>();
builder.Services.AddScoped<CatalogDomain>();
builder.Services.AddScoped<CategoryDomain>();
builder.Services.AddScoped<ItemDomain>();
builder.Services.AddScoped<PermissionDomain>();
builder.Services.AddScoped<RoleDomain>();
builder.Services.AddScoped<UserDomain>();

// 3. Interface services
builder.Services.AddScoped<ICustomerService, API.Services.CustomerService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ISupplierOrderService, SupplierOrderService>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();

// 4. Model implementations
builder.Services.AddScoped<IWarehouse, Warehouse>();
builder.Services.AddScoped<IBranch, Branch>();
builder.Services.AddScoped<IOrder, Order>();
builder.Services.AddScoped<ISupplier, Supplier>();

//-------------------------------------------------------------------------------
// AutoMapper Configuration
//-------------------------------------------------------------------------------
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<LogisticsMapping>();
    cfg.AddProfile<CustomerMapping>();
    cfg.AddProfile<IntAdminMapping>();
    cfg.AddProfile<BaseMapping>();
});

//-------------------------------------------------------------------------------
// HttpClient Configuration for API communication
//-------------------------------------------------------------------------------
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("https://localhost:7153");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
});

//-------------------------------------------------------------------------------
// Database Configuration
//-------------------------------------------------------------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

//-------------------------------------------------------------------------------
// Application building and configuration
//-------------------------------------------------------------------------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Diagnostic middleware to help debugging
app.Use(async (context, next) =>
{
    Console.WriteLine($"Incoming request: {context.Request.Path}");
    await next();
});

// Middleware configuration in correct order
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowFrontend");
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Database initialization if needed
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
}

app.Run();