namespace MapControllerRoute.Controllers;

public class HomeController
{
    public string Index(string? id)
    {
        if (string.IsNullOrEmpty(id))
            return $"Привет из контроллера {nameof(HomeController)}.";

        return $"Привет из контроллера {nameof(HomeController)}! Передан id={id}.";
    }
}
