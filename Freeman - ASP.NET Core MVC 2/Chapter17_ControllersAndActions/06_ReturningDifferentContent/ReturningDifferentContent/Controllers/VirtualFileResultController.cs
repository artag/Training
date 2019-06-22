using Microsoft.AspNetCore.Mvc;

namespace ReturningDifferentContent.Controllers
{
    public class VirtualFileResultController : Controller
    {
        public VirtualFileResult Index() =>
            File("/Files/Text.txt", "text/plain");
    }
}
