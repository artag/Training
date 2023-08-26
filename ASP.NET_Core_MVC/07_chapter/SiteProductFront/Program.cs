using System.Globalization;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using SiteProduct.Db;
using SiteProduct.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

// Чтение строки подключения из файла конфигурации.
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Repository>(options => options.UseSqlite(conn));

// Рекомендуется использовать время службы "Scoped" для работы с БД.
// builder.Services.AddScoped<IProductData, SqlProductData>();
// builder.Services.AddScoped<IProductTypeData, SqlProductTypeData>();
builder.Services.AddSingleton<IProductData, MockProductData>();
builder.Services.AddSingleton<IProductTypeData, MockProductTypeData>();

var app = builder.Build();

// Загрузка статических файлов (.css и .js).
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}");

// Установка культуры для отображения цены (выключено - плохо работает с валидацией)
// var cultureInfo = new CultureInfo("ru-RU");
// //cultureInfo.NumberFormat.CurrencySymbol = "Руб";
// CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
// CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

app.Run();
