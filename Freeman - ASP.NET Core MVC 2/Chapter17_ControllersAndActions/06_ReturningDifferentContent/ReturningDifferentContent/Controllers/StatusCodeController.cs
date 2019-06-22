using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReturningDifferentContent.Controllers
{
    public class StatusCodeController : Controller
    {
        public StatusCodeResult Index() =>
            StatusCode(StatusCodes.Status404NotFound);

        // То же что и Index, только лучше
        public StatusCodeResult MuchBetter() =>
            NotFound();
    }
}
