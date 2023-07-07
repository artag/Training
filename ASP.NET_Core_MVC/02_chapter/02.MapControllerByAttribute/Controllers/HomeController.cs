using Microsoft.AspNetCore.Mvc;

namespace MapControllerByAttribute.Controllers;

public class HomeController
{
    public string Ping()
    {
        return "Pong";
    }

    public string Index(string? id)
    {
        if (string.IsNullOrEmpty(id))
            return $"Привет из контроллера {nameof(HomeController)}.";

        return $"Привет из контроллера {nameof(HomeController)}! Передан id={id}.";
    }
}
