using RazorRuntimeCompilation.Services;

var builder = WebApplication.CreateBuilder(args);

// Важно. Регистрация RazorRuntimeCompilation для изменения Razor без перекомпиляции и перезапуска приложения.
builder.Services.AddMvc().AddRazorRuntimeCompilation();
builder.Services.AddScoped<IAlert, AlertService>();

var app = builder.Build();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();
