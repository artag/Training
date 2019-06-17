using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace ReceivingContextData.Controllers
{
    public class DerivedController : Controller
    {
        public ViewResult Index() => View("Result", "This is a derived controller");

        public ViewResult Headers()
        {
            var result = Request.Headers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.First());

            return View("DictionaryResult", result);
        }
    }
}
