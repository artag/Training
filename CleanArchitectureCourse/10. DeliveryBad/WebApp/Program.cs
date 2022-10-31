using ApplicationServices.Implementation;
using ApplicationServices.Interfaces;
using DataAccess;
using DataAccess.Interfaces;
using Delivery.DeliveryCompany;
using Delivery.Interfaces;
using DomainServices.Implementation;
using DomainServices.Interfaces;
using Email.Implementation;
using Email.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Mobile.UseCases.Order.Commands.CreateOrder;
using Mobile.UseCases.Order.Utils;
using Web.Interfaces;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Entities
builder.Services.AddScoped<IOrderDomainService, OrderDomainService>();

// Infrastructure
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();
builder.Services.AddEntityFrameworkSqlite().AddDbContext<IDbContext, AppDbContext>();

// Application
builder.Services.AddScoped<ISecurityService, SecurityService>();

// Frameworks
builder.Services
    .AddControllers().PartManager.ApplicationParts
    .Add(new AssemblyPart(typeof(Web.Controllers.DummyController).Assembly));

builder.Services
    .AddControllers().PartManager.ApplicationParts
    .Add(new AssemblyPart(typeof(Mobile.Controllers.OrdersController).Assembly));

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