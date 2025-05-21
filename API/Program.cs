using API.Services.Logistics;
using API.Services.OrderService;
using API.Services.Interfaces;
using API.Services.IntAdmin;
using API.Implementations.Domain;
using API.Utils.Extensions;
using Microsoft.EntityFrameworkCore;
using API.Data.Mapping;
using AutoMapper.EquivalencyExpression;
using API.Data;
using softserve.projectlabs.Shared.Interfaces;
using System.Text.Json.Serialization;
using System.Text.Json;
using API.Data.Repositories.IntAdministrationRepository.Interfaces;
using API.Data.Repositories.IntAdministrationRepository;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using API.Services;
using softserve.projectlabs.Shared.DTOs;
using API.Data.Repositories.LogisticsRepositories;
using API.Data.Repositories.LogisticsRepositories.Interfaces;
using API.Mappings;
using API.Data.Repositories.BaseRepository;
using API.Services.IntAdministration;

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
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

var tokenString = builder.Configuration["AppSettings:Token"]
                  ?? throw new InvalidOperationException("JWT Token is missing from configuration.");

var keyBytes = Convert.FromBase64String(tokenString);
var key = new SymmetricSecurityKey(keyBytes);


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = false,
            ValidateAudience = false
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var tokenInCookie = context.Request.Cookies["jwt"];
                if (!string.IsNullOrWhiteSpace(tokenInCookie))
                {
                    context.Token = tokenInCookie;
                }
                return Task.CompletedTask;
            }
        };
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
              .AllowAnyMethod()
              .AllowCredentials();
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

// 2. Domain services
builder.Services.AddScoped<CustomerDomain>();
builder.Services.AddScoped<BranchDomain>();
builder.Services.AddScoped<OrderDomain>();
builder.Services.AddScoped<SupplierDomain>();
builder.Services.AddScoped<WarehouseDomain>();
builder.Services.AddScoped<CatalogDomain>();
builder.Services.AddScoped<CategoryDomain>();
builder.Services.AddScoped<ItemDomain>();
builder.Services.AddScoped<PermissionDomain>();
builder.Services.AddScoped<RoleDomain>();
builder.Services.AddScoped<UserDomain>();
builder.Services.AddScoped<LineOfCreditDomain>();
builder.Services.AddScoped<CartDomain>();
builder.Services.AddScoped<PackageDomain>();
builder.Services.AddScoped<TokenGenerator>();

// 3. Interface services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICustomerService, API.Services.CustomerService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILineOfCreditService, LineOfCreditService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICookieService, CookieService>();


// 4. Model implementations
//builder.Services.AddScoped<IWarehouse, Warehouse>();
//builder.Services.AddScoped<IBranch, Branch>();
//builder.Services.AddScoped<IOrder, Order>();
//builder.Services.AddScoped<ISupplier, Supplier>();

// 5a. Generic repositories (Base layer)
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(ISoftRepository<>), typeof(SoftRepository<>));

// 5. Repositories (Data layer)
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();

//6. Logistics DTOs
builder.Services.AddScoped<WarehouseDto>();
builder.Services.AddScoped<BranchDto>();
builder.Services.AddScoped<OrderDto>();
builder.Services.AddScoped<SupplierDto>();


//-------------------------------------------------------------------------------
// AutoMapper Configuration
//-------------------------------------------------------------------------------
var assemblies = AppDomain.CurrentDomain.GetAssemblies();

builder.Services.AddAutoMapper(cfg =>
{ 
    cfg.AddCollectionMappers();

    cfg.AddProfile<CustomerMapping>();
    cfg.AddProfile<IntAdminMapping>();
    cfg.AddProfile<LogisticsMapping>();
}, assemblies);

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
    //app.UseDeveloperExceptionPage();
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