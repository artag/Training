using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PartyInvites.Models;

namespace PartyInvites.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            var hour = DateTime.Now.Hour;
            var greeting = hour < 12 ? "Good Morning" : "Good Afternoon";
            ViewBag.Greeting = greeting;

            return View("MyView");
        }

        [HttpGet]
        public ViewResult RsvpForm()
        {
            return View();
        }

        [HttpPost]
        public ViewResult RsvpForm(GuestResponse guestResponse)
        {
            if (ModelState.IsValid)
            {
                Repository.AddResponse(guestResponse);
                return View("Thanks", guestResponse);
            }

            return View();
        }

        public ViewResult ListResponses()
        {
            var responses = Repository.Responses.Where(r => r.WillAttend == true);
            return View(responses);
        }
    }
}
