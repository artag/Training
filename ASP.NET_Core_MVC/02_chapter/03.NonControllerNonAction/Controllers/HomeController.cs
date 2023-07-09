using Microsoft.AspNetCore.Mvc;

namespace NonControllerNonAction.Controllers;

public class HomeController : Controller
{
    public ContentResult Index()
    {
        var str =
            "Addresses to test:\n" +
            "https://localhost:5001/person/name     - available action\n" +
            "https://localhost:5001/person/email    - renamed action\n\n" +
            "https://localhost:5001/person          - ignored action\n" +
            "https://localhost:5001/person/index    - ignored action\n\n" +
            "https://localhost:5001/unknown         - ignored controller\n" +
            "https://localhost:5001/unknown/index   - ignored controller and action";

        return Content(str);
    }
}
