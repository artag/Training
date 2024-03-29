using Authorization.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IUsersPortalRepository, UsersPortalRepository>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
    options.IdleTimeout = TimeSpan.FromMinutes(20)
);


var app = builder.Build();
app.UseSession();
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
