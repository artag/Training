using System.Configuration;
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
using Hangfire;
using Hangfire.SQLite;
using MediatR;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Mobile.UseCases.Order.BackgroundJobs;
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
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<IDbContext, AppDbContext>(opts => opts.UseSqlite(connectionString));

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
builder.Services.AddHangfire(cfg => cfg.UseSQLiteStorage(connectionString));
builder.Services.AddHangfireServer();

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

app.UseHangfireDashboard();

RecurringJob.AddOrUpdate<UpdateDeliveryStatusJob>(
    recurringJobId: "UpdateDeliveryStatusJob",
    methodCall: job => job.ExecuteAsync(),
    cronExpression: Cron.Daily);

app.Run();