using Microsoft.AspNetCore.Mvc;

namespace ReturningDifferentContent.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index() => View("Index");
    }
}
