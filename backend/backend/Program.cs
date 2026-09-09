using backend.Adapters;
using backend.Data;
using backend.Interfaces;
using backend.IRepository;
using backend.Repositories;
using backend.Repository;
using backend.Services;
using backend.Services.Auth;
using GymStore.BuildingBlocks.Cqrs;
using GymStore.Common;
using GymStore.Common.Modules;
using GymStore.Modules.Cart;
using GymStore.Modules.Cart.Application.Abstractions;
using GymStore.Modules.Catalog;
using GymStore.Modules.Ordering;
using GymStore.Modules.Payments;
using GymStore.Modules.Reviews;
using GymStore.Modules.Reviews.Application.Abstractions;
using GymStore.Modules.Shipping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// The modular-monolith modules, registered uniformly via the common IModule mechanism.
var modules = new IModule[]
{
    new CartModule(),
    new CatalogModule(),
    new OrderingModule(),
    new PaymentsModule(),
    new ShippingModule(),
    new ReviewsModule()
};

// Add services to the container.
var mvcBuilder = builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
foreach (var module in modules)
{
    // Discover each module's controllers.
    mvcBuilder.AddApplicationPart(module.Assembly);
}

// Shared kernel + CQRS dispatcher, then each module's own services.
builder.Services.AddCommon();
builder.Services.AddCqrs();
foreach (var module in modules)
{
    module.Register(builder.Services, builder.Configuration);
}

// Host adapters that satisfy module ports.
// Lets the Cart module read product data (name/price/images) from Catalog.
builder.Services.AddScoped<IProductInfoProvider, ProductInfoProvider>();
// Lets the Reviews module resolve reviewer display names.
builder.Services.AddScoped<IReviewerInfoProvider, ReviewerInfoProvider>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IPreferenceService, PreferenceService>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IPreferenceRepository, PreferenceRepository>();


// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration.GetConnectionString("Redis");

    options.InstanceName = "GymStore:";
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<GymStoreContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"))
                    // The reconstructed DbContext intentionally omits the HasData seed
                    // (the seed is applied by the existing migration's InsertData calls),
                    // so ignore the resulting data-only pending-model-changes warning.
                    .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)));

var app = builder.Build();
app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable middleware to serve generated Swagger as a JSON endpoint
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // To serve the Swagger UI at application's root (e.g., http://localhost:<port>/)
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSession();

app.MapControllers();

app.Run();
