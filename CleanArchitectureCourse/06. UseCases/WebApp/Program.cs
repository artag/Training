using DataAccess;
using DataAccess.Interfaces;
using DomainServices.Implementation;
using DomainServices.Interfaces;
using Email.Implementation;
using Email.Interfaces;
using MediatR;
using UseCases.Order.Commands.CreateOrder;
using UseCases.Order.Utils;
using Web.Interfaces;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Domain
builder.Services.AddScoped<IOrderDomainService, OrderDomainService>();

// Infrastructure
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddEntityFrameworkSqlite().AddDbContext<IDbContext, AppDbContext>();

// Application

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