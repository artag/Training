using ApplicationServices.Implementation;
using ApplicationServices.Interfaces;
using DataAccess;
using DataAccess.Interfaces;
using Delivery.Company;
using Delivery.Interfaces;
using DomainServices.Implementation;
using DomainServices.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mobile.Telemetry.Implementation;
using Mobile.Telemetry.Interfaces;
using Mobile.UseCases.Order.Commands.CreateOrder;
using Mobile.UseCases.Order.Utils;
using Web.ApplicationServices.Implementation;
using Web.ApplicationServices.Interfaces;
using WebApp.Extensions;
using WebApp.Interfaces;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Domain
builder.Services.AddScoped<IOrderDomainService, OrderDomainService>();

// UseCases & Application
builder.Services.AddScoped<ISecurityService, SecurityService>();

// Infrastructure
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<IDbContext, AppDbContext>(opts => opts.UseSqlite(connectionString));

// Mobile Infrastructure
builder.Services.AddScoped<ITelemetryService, TelemetryService>();

// Web Infrastructure
builder.Services.AddScoped<IWebAppService, WebAppService>();

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