var builder = WebApplication.CreateBuilder(args);

// Подключение сервисов MVC.
// Добавление сервисов MVC с опцией отключения использования конечных точек маршрутизации.
// Это необходимо сделать, если установлена маршрутизация по умолчанию.
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

var app = builder.Build();
// MVC с настройками маршрутизации по умолчанию.
app.UseMvcWithDefaultRoute();
app.Run();
