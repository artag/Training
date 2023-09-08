using Filters.Utils;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc(option =>
{
    // Регистрация фильтров в качестве глобальных.
    // (Применяются ко всем контроллерам и действиям).
    option.Filters.Add(new ActionFilter());
    option.Filters.Add(new AuthorizationFilter());
    option.Filters.Add(new ResourceFilter());
    option.Filters.Add(new ExceptionFilter());
    option.Filters.Add(new ResultFilter());
});

var app = builder.Build();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
