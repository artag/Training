var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

var app = builder.Build();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();

/*
Доступные адреса:
https://loclahost:5001/movies/movie    - Передача одиночной модели 'Movie' в представление
https://loclahost:5001/movies/movies   - Передача коллекции моделей 'Movie' в представление
https://loclahost:5001/movies/group    - Передача моделей представления 'MovieViewModel' в представление
*/
