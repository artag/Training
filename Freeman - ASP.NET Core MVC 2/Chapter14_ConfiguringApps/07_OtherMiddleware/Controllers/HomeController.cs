using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace OtherMiddleware.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index(bool throwException = false)
        {
            if (throwException)
            {
                throw new NullReferenceException();
            }

            var model = new Dictionary<string, string>
            {
                ["Message1"] = "This is the Index action",
                ["Message2"] = "Type next URL to see exception handling: " +
                              "/Home/Index?throwException=true",
                ["Message3"] = "Type error URL to see working method UseStatusCodePages()"
            };

            return View(model);
        }

        public ViewResult Error()
        {
            var model = new Dictionary<string, string>
            {
                ["Message"] = "This is the Error action"
            };

            return View(nameof(Index), model);
        }
    }
}
