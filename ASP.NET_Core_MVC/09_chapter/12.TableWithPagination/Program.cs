using TablePage.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddSingleton<IPersons, Persons>();

var app = builder.Build();
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();
