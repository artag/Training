var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// localhost[:port]/?age=19
app.UseMiddleware<AgeMiddleware.Components.AgeMiddleware>();
app.MapGet("/", () => "Your age is over 18 years old!");
app.Run();
