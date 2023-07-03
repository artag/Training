using AlertService.Services;

var builder = WebApplication.CreateBuilder(args);

// Подключение службы.
// builder.Services.AddSingleton<IAlert, JsonAlert>();
builder.Services.AddSingleton<IAlert, XmlAlert>();

var app = builder.Build();

app.MapGet("/", async context =>
{
    var alert = context.RequestServices.GetRequiredService<IAlert>();
    var msg = alert.GetMessage();
    context.Response.ContentType = "text/html;charset=utf-8";
    await context.Response.WriteAsync(msg);
});

app.Run();
