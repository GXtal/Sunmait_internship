using Infrastructure.Database;
using Infrastructure.Repositories;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Web.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Web.AuthorizationData;
using Web.Authorization;
using Application.Interfaces;
using Web.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "a", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod(); ;
    });

});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = config[ConfigurationPaths.Issuer],
        ValidAudience = config[ConfigurationPaths.Audience],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config[ConfigurationPaths.Key]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddDbContext<ShopDbContext>(o => o.UseNpgsql(config.GetConnectionString("ShopDbConnection")));
// Add injections

builder.Services.AddSingleton(builder);
builder.Services.AddTransient<ITokenManager, TokenManager>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IBrandRepository, BrandRepository>();
builder.Services.AddTransient<ISectionRepository, SectionRepository>();
builder.Services.AddTransient<IAddressRepository, AddressRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ICategorySectionRepository, CategorySectionRepository>();
builder.Services.AddTransient<IContactRepository, ContactRepository>();
builder.Services.AddTransient<IOrderHistoryRepository, OrderHistoryRepository>();
builder.Services.AddTransient<IOrderProductRepository, OrderProductRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IReviewRepository, ReviewRepository>();
builder.Services.AddTransient<IRoleRepository, RoleRepository>();
builder.Services.AddTransient<IStatusRepository, StatusRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IImageRepository, ImageRepository>();
builder.Services.AddTransient<IReservedProductRepository, ReservedProductRepository>();

builder.Services.AddTransient<ShopDbContext>();

builder.Services.AddTransient<IBrandService, BrandService>();
builder.Services.AddTransient<IAddressService, AddressService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IContactService, ContactService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IReviewService, ReviewService>();
builder.Services.AddTransient<ISectionService, SectionService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<IReservationService, ReservationService>();

builder.Services.AddHostedService<ReservationBackgroundService>();

builder.Services.AddTransient<ErrorMiddleware>();

builder.Services.AddControllers();

builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorMiddleware>();

app.UseCors("a");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
