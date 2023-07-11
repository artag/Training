using Microsoft.AspNetCore.Mvc;

namespace Model_Models_ViewModel.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var message =
            "Доступные адреса:\n" +
            "https://loclahost:5001/movies/movie    - Передача одиночной модели 'Movie' в представление\n" +
            "https://loclahost:5001/movies/movies   - Передача коллекции моделей 'Movie' в представление\n" +
            "https://loclahost:5001/movies/group    - Передача моделей представления 'MovieViewModel' в представление\n";

        return Content(message);
    }
}
