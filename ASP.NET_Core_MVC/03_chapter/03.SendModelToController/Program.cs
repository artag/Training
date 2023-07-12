var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

var app = builder.Build();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();

/*

Доступные адреса:
https://localhost:5001/documents/use-properties - Передача данных в контроллер в виде набора полей
https://localhost:5001/documents/use-class      - Передача данных в контроллер в виде сложного объекта (класса)
https://localhost:5001/documents/use-classes    - Передача данных в контроллер в виде сложных объектов (массива классов)

*/