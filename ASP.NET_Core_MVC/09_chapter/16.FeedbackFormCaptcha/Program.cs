using Feedback.Services;

var builder = WebApplication.CreateBuilder(args);

// Включение сессии. Параметры сессии по умолчанию.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddMvc();
builder.Services.AddScoped<ICaptchaFactory, CaptchaFactory>();
builder.Services.AddScoped<IEmail, MockEmail>();

var app = builder.Build();

// Включение сессий.
app.UseSession();

app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();
