using Microsoft.AspNetCore.Mvc;

namespace CreatingPocoControllers.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index() => new ViewResult();
    }
}
