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
builder.Services.AddTransient<ShopDbContext>();
builder.Services.AddTransient<IBrandService, BrandService>();
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
