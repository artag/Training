using Microsoft.AspNetCore.Mvc;

namespace TrainingCourses.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
