using Microsoft.AspNetCore.Mvc;
using ServiceLifeCycles.Models;

namespace ServiceLifeCycles.Controllers
{
    public class ScopedController : Controller
    {
        private readonly IScopedDI _scopedDI;
        private readonly ScopedDI _concreteScoped;

        public ScopedController(IScopedDI scopedDI, ScopedDI concreteScoped)
        {
            _scopedDI = scopedDI;
            _concreteScoped = concreteScoped;
        }

        public IActionResult Index()
        {
            ViewBag.GuidFromIScopedDI = _scopedDI.Guid;
            ViewBag.GuidFromScopedDI = _concreteScoped.Guid;

            return View("List");
        }
    }
}
