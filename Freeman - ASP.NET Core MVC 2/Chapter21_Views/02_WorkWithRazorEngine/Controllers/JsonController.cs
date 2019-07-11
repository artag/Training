using Microsoft.AspNetCore.Mvc;

namespace WorkWithRazorEngine.Controllers
{
    public class JsonController : Controller
    {
        public IActionResult Index() => View();
    }
}