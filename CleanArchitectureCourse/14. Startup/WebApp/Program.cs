using DataAccess;
using DataAccess.Interfaces;
using Delivery.Company;
using Delivery.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCases.Order.Commands.CreateOrder;
using UseCases.Order.Utils;
using WebApp.Extensions;
using WebApp.Interfaces;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<IDbContext, AppDbContext>(opts => opts.UseSqlite(connectionString));

// Frameworks
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddMediatR(typeof(CreateOrderCommand));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandlerMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();