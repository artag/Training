var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// localhost[:port]/index.html
// Использовать статические файлы из директории wwwroot
app.UseStaticFiles();
app.MapGet("/", () => "Hello World!\n" +
                      "Наберите localhost[:port]/index.html");

app.Run();
