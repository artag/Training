using MonitorApp.Services;

var builder = WebApplication.CreateBuilder(args);
// Фоновой задаче могут быть доступны все сервисы из DI.
builder.Services.AddHostedService<TimedHostedService>();
builder.Services.AddSingleton<IMonitorPanel, MonitorPanel>();
builder.Services.AddMvc();

var app = builder.Build();
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
