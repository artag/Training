var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

var app = builder.Build();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();

/*
 Адреса:
 https://localhost:5001/model/anemic    - Пример анемичной (тонкой) модели (класс Auto)
 https://localhost:5001/model/full      - Пример полной (богатой) модели (класс ThickAuto)
*/
