using System.Reflection;
using ReadConfigFromJson;

var builder = WebApplication.CreateBuilder(args);

// Подключение конфигурации.
var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
builder.WebHost.UseConfiguration(new Settings(path!).Config);

var app = builder.Build();

// Отображение параметров конфигурации.
var configuration = app.Configuration;
var section = configuration.GetSection("Country");
var city = $"Город: {section["City"]}<br/>";
var html = $"<!DOCTYPE html>" +
           $"<html>" +
           $"<head><meta charset=utf-8></head>" +
           $"<body>" +
           $"<p style='color:{section["Status"]}'>{city}</p>" +
           $"</body>" +
           $"</html>";

app.MapGet("/", async context =>
{
    await context.Response.WriteAsync(html);
});

app.Run();
