namespace MapControllerRoute.Controllers;

public class PersonController
{
    public string Login()
    {
        return "temp@mail.ru";
    }

    public string Name()
    {
        return "Иванов И.П.";
    }

    public string Index()
    {
        return $"Привет из {nameof(PersonController)}.";
    }
}
