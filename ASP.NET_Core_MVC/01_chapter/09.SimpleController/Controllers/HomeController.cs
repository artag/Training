namespace SimpleController.Controllers;

public class HomeController
{
    public string Index()
    {
        return $"Привет из контроллера {nameof(HomeController)}";
    }
}
