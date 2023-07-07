using Microsoft.AspNetCore.Mvc;

namespace MapControllerByAttribute.Controllers;

[Route("[controller]")]
public class PersonController
{
    [Route("[action]")]
    public string Login()
    {
        return "temp@mail.ru";
    }

    [Route("[action]")]
    public string Name()
    {
        return "Иванов И.П.";
    }

    [Route("")]             // <- Маршрут по умолчанию: [controller]
    [Route("[action]")]     // <- Маршрут: [controller]/index
    public string Index()
    {
        return $"Привет из {nameof(PersonController)}.";
    }
}
