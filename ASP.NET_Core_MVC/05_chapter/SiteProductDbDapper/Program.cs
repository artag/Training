using SiteProduct.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

builder.Services.AddScoped<IDataSeed, DataSeed>();
builder.Services.AddScoped<IConnectionStringProvider, ConnectionStringProvider>();

// Рекомендуется использовать время службы "Scoped" для работы с БД.
builder.Services.AddScoped<IProductData, DapperProductData>();
builder.Services.AddScoped<IProductTypeData, DapperProductTypeData>();

var app = builder.Build();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}");

using (var scope = app.Services.CreateScope())
{
    var seed = scope.ServiceProvider.GetRequiredService<IDataSeed>();
    seed.EnsureDatabase();
}


app.Run();
