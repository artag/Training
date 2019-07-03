using FilterOrder.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace FilterOrder.Controllers
{
    [Message("This is the Controller-Scoped Filter", Order = 10)]
    public class CustomOrderController : Controller
    {
        [Message("This is the First Action-Scoped Filter", Order = 1)]
        [Message("This is the Second Action-Scoped Filter", Order = -1)]
        public IActionResult Index() =>
            View("Message", "This is custom order controller");
    }
}
