using Microsoft.AspNetCore.Mvc;
using ServiceLifeCycles.Models;

namespace ServiceLifeCycles.Controllers
{
    public class TransientController : Controller
    {
        private readonly ITransientDI _transientDI;
        private readonly ITransientFactoryDI _transientFactoryDI;
        private readonly TransientDI _concreteTransient;

        public TransientController(
            ITransientDI transientDI,
            ITransientFactoryDI transientFactoryDI,
            TransientDI concreteTransient)
        {
            _transientDI = transientDI;
            _transientFactoryDI = transientFactoryDI;
            _concreteTransient = concreteTransient;
        }

        public IActionResult Index()
        {
            ViewBag.GuidFromITransientDI = _transientDI.Guid;
            ViewBag.GuidFromTransientDI = _concreteTransient.Guid;
            ViewBag.GuidFromITransientFactoryDI = _transientFactoryDI.Guid;

            return View("List");
        }
    }
}
