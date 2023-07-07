var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();


var app = builder.Build();

// Добавление маршрутизации на основе соглашений.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// https://localhost:5001
// https://localhost:5001/home
// https://localhost:5001/home/index
// Напечатают: "Привет из контроллера HomeController."

// https://localhost:5001/home/index/123
// Напечатает: "Привет из контроллера HomeController! Передан id=123."

// https://localhost:5001/person/login
// Напечатает: "temp@mail.ru"

// https://localhost:5001/person/name
// Напечатает: "Иванов И.П."

// https://localhost:5001/person/
// https://localhost:5001/person/index
// Напечатают: "Привет из PersonController."