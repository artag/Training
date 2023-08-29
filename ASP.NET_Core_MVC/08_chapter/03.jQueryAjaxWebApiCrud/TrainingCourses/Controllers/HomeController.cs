using Microsoft.AspNetCore.Mvc;

namespace TrainingCourses.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewBag.Title = "Документ";
        return View();
    }

    // public IActionResult IndexAjax()
    // {
    //     return View();
    // }
    //
    // public IActionResult IndexMethods()
    // {
    //     return View();
    // }
    //
    // public IActionResult IndexParam()
    // {
    //     return View();
    // }
}