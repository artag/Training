using FiltersSample.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

// Регистрация фильтра в DI для его resolve при помощи ServiceFilter.
builder.Services.AddScoped<ConfigFilter>();

var app = builder.Build();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
