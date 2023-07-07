using SiteProduct.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddScoped<IProductData, MockProductData>();

var app = builder.Build();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}");

app.Run();
