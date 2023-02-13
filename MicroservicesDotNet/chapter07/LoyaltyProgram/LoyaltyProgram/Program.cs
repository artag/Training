// (1) Adds MVC controller and helper services to the service collection
// (2) Redirects all HTTP requests to HTTPS
// (3) Adds all endpoints in all controllers to MVCs route table

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();      // (1)
var app = builder.Build();

app.UseHttpsRedirection();                                  // (2)
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());  // (3)

app.Run();
