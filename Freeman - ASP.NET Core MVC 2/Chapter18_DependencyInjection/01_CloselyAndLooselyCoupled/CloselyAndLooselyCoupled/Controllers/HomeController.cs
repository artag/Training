using Microsoft.AspNetCore.Mvc;

namespace CloselyAndLooselyCoupled.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index() => View();
    }
}
