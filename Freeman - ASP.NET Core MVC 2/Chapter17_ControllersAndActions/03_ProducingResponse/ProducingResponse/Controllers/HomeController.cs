using Microsoft.AspNetCore.Mvc;

namespace ProducingResponse.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index() => View();
    }
}
