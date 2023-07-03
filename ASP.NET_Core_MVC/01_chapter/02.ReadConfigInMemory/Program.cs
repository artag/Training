using HelloConfig;

var builder = WebApplication.CreateBuilder(args);
// Подключение конфигурации.
builder.WebHost.UseConfiguration(new Settings().Config);

var app = builder.Build();

var configuration = app.Configuration;

// Чтение старых значений из конфигурации.
var oldCountry = $"Страна: {configuration["Country"]}<br/>";
var oldCity = $"Город: {configuration["City"]}<br/>";

// Изменение и отображение параметров конфигурации.
configuration["Country"] = "Норвегия";
configuration["City"] = "Осло";
var country = $"Страна (изменено): {configuration["Country"]}<br/>";
var city = $"Город (изменено): {configuration["City"]}<br/>";
var html = $"<!DOCTYPE html>" +
           $"<html>" +
           $"<head><meta charset=utf-8></head>" +
           $"<body>" +
           $"<p>{oldCountry + oldCity}</p>" +
           $"<p>{country + city}</p>" +
           $"</body>" +
           $"</html";
app.MapGet("/", async context =>
{
    await context.Response.WriteAsync(html);
});

app.Run();
