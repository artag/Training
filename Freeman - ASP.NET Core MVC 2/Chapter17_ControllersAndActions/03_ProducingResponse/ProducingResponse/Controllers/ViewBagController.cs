using System;
using Microsoft.AspNetCore.Mvc;

namespace ProducingResponse.Controllers
{
    public class ViewBagController : Controller
    {
        public ViewResult SendViewBagToView()
        {
            ViewBag.Message = "Hello";
            ViewBag.Date = DateTime.Now;

            return View();
        }
    }
}
