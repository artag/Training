using Microsoft.AspNetCore.Mvc;
using ServiceLifeCycles.Models;

namespace ServiceLifeCycles.Controllers
{
    public class SingletonController : Controller
    {
        private readonly ISingletonDI _singletonDI;
        private readonly SingletonDI _concreteSingleton;

        public SingletonController(ISingletonDI singletonDi, SingletonDI concreteSingleton)
        {
            _singletonDI = singletonDi;
            _concreteSingleton = concreteSingleton;
        }

        public IActionResult Index()
        {
            ViewBag.GuidFromISingletonDI = _singletonDI.Guid;
            ViewBag.GuidFromSingletonDI = _concreteSingleton.Guid;

            return View("List");
        }
    }
}
