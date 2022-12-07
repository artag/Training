var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

var app = builder.Build();

// Redirects all HTTP requests to HTTPS
app.UseHttpsRedirection();
app.UseRouting();
// Add all endpoints in all controllers to MVC's route table
app.UseEndpoints(enpoints => enpoints.MapControllers());

app.Run();
