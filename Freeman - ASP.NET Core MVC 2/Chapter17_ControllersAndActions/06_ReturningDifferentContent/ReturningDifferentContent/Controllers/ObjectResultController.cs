using Microsoft.AspNetCore.Mvc;

namespace ReturningDifferentContent.Controllers
{
    public class ObjectResultController : Controller
    {
        public ObjectResult Index() =>
            Ok(new string[] { "Alice", "Bob", "Joe" });
    }
}
