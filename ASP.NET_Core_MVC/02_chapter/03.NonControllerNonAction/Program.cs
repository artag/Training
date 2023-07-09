var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

var app = builder.Build();

app.MapControllerRoute(
     name: "default",
     pattern: "{controller=Home}/{action=Index}");

app.Run();

/*
 Addresses to test:
 https://localhost:5001/person/name     - available action
 https://localhost:5001/person/email    - renamed action

 https://localhost:5001/person          - ignored action
 https://localhost:5001/person/index    - ignored action

 https://localhost:5001/unknown         - ignored controller
 https://localhost:5001/unknown/index   - ignored controller and action
*/