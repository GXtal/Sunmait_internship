using Infrastructure.Database;
using Infrastructure.Repositories;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Web.Middlewares;
using Microsoft.EntityFrameworkCore.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ShopDbContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("ShopDbConnection")));
// Add injections

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

builder.Services.AddTransient<ShopDbContext>();

builder.Services.AddTransient<IBrandService, BrandService>();
builder.Services.AddTransient<IAddressService, AddressService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IContactService, ContactService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IReviewService, ReviewService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<ISectionService, SectionService>();
builder.Services.AddTransient<IStatusService, StatusService>();
builder.Services.AddTransient<IUserService, UserService>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
