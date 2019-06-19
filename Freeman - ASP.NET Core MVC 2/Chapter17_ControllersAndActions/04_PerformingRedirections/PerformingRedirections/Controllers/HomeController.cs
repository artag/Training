using Microsoft.AspNetCore.Mvc;

namespace PerformingRedirections.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index() => View();
    }
}
