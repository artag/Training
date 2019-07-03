using FilterOrder.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace FilterOrder.Controllers
{
    [Message("This is the Controller-Scoped Filter")]
    public class DefaultOrderController : Controller
    {
        [Message("This is the First Action-Scoped Filter")]
        [Message("This is the Second Action-Scoped Filter")]
        public IActionResult Index() =>
            View("Message", "This is default order controller");
    }
}
