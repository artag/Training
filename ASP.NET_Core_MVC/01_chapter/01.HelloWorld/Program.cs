var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var env = app.Environment;
if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World! (Second version)");
    });
});

app.Run();
