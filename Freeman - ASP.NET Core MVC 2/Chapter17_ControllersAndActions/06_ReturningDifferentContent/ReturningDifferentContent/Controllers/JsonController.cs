using Microsoft.AspNetCore.Mvc;

namespace ReturningDifferentContent.Controllers
{
    public class JsonController : Controller
    {
        public JsonResult Index() => Json(new[] { "Alice", "Bob", "Joe" });
    }
}
