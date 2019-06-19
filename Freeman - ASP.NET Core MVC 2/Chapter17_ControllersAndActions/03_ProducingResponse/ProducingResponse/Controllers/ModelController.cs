using System;
using Microsoft.AspNetCore.Mvc;

namespace ProducingResponse.Controllers
{
    public class ModelController : Controller
    {
        public ViewResult SendToUntypedView() => View(DateTime.Now);

        public ViewResult SendToTypedView() => View(DateTime.Now);

        public ViewResult SendStringToView() => View((object)"Hello, World");
    }
}
