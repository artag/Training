using Microsoft.AspNetCore.Mvc;

namespace CreatingPocoControllers.Controllers
{
    public class DerivedController : Controller
    {
        public ViewResult Index() => View("Result", "This is a Derived controller");
    }
}
