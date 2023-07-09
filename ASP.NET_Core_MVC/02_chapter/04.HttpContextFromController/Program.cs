var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

var app = builder.Build();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Context}/{action=Index}");

app.Run();

/*
 Addresses:
 https://localhost:5001/context/headers                  - Вывести все заголовки запроса
 https://localhost:5001/context/method                   - Каким методом передан запрос на обработку
 https://localhost:5001/context/route                    - Вывести параметры маршрута
 https://localhost:5001/context/params/{id?}/par1/par2   - Получить любое множество параметров из запроса
*/
