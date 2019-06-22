using Microsoft.AspNetCore.Mvc;

namespace ReturningDifferentContent.Controllers
{
    public class ContentController : Controller
    {
        public ContentResult Index() =>
            Content("[\"Alice\", \"Bob\", \"Joe\"]", "application/json");
    }
}
