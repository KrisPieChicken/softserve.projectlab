using API.Utils.Extensions;
using Microsoft.EntityFrameworkCore;
using API.Data.Mapping;
using AutoMapper.EquivalencyExpression;
using API.Data;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

builder.Services.AddProjectServices();


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

// Middleware configuration order
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