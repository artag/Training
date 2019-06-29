using System;
using Microsoft.AspNetCore.Mvc;
using UsingExceptionFilters.Infrasructure;

namespace UsingExceptionFilters.Controllers
{
    [RangeException]
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult GenerateException(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            else if (id > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return View("Message", $"The value is {id}");
        }
    }
}
