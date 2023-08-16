using Microsoft.EntityFrameworkCore;
using SiteProduct.Db;
using SiteProduct.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

// Чтение строки подключения из файла конфигурации.
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Repository>(options => options.UseSqlite(conn));

// Рекомендуется использовать время службы "Scoped" для работы с БД.
builder.Services.AddScoped<IProductData, SqlProductData>();
builder.Services.AddScoped<IProductTypeData, SqlProductTypeData>();

var app = builder.Build();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}");

app.Run();
