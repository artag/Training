var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
// Добавление служб Blazor Server.
builder.Services.AddServerSideBlazor();

var app = builder.Build();

// Для статических файлов
// (для обработки запросов к скрипту ~/_framework/blazor.server.js).
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

// Для связи клиентской части приложения с сервером при помощи SignalR.
app.MapBlazorHub();

app.Run();
