var builder = WebApplication.CreateBuilder(args);

// Сеансы (сессии) работают поверх объекта IDistributedCache.
builder.Services.AddDistributedMemoryCache();

// Включение сеансов (сессий)
builder.Services.AddSession(option =>
{
    // Установка времени сессии (значение по умолчанию 20 минут после последнего запроса).
    option.IdleTimeout = TimeSpan.FromMinutes(1);
});

builder.Services.AddMvc();

var app = builder.Build();

// Включение session state для приложения.
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
