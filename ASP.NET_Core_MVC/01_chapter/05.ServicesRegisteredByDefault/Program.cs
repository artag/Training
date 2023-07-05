using System.Text;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.MapGet("/", async context =>
{
    context.Response.ContentType = "text/html;charset=utf-8";
    var html = new StringBuilder();
    foreach (var service in builder.Services)
    {
        html.AppendLine($"Тип сервиса: {service.ServiceType.FullName}<br/>");
        html.AppendLine($"Время жизни сервиса: {service.Lifetime}<br/>");
        html.AppendLine($"Тип реализации: {service.ImplementationType?.FullName}<br/><hr/>");
    }
    await context.Response.WriteAsync(html.ToString());
});

app.Run();
