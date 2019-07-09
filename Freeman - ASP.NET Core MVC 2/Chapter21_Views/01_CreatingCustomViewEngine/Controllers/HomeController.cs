using System;
using Microsoft.AspNetCore.Mvc;

namespace CreatingCustomViewEngine.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            ViewBag.Message = "Hello, World";
            ViewBag.Time = DateTime.Now.ToString("HH:mm:ss");
            return View("DebugData");
        }

        public ViewResult List() => View();
    }
}
