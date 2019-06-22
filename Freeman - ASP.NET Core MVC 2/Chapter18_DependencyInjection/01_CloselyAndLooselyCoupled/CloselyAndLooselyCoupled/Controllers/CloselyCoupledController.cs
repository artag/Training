using CloselyAndLooselyCoupled.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloselyAndLooselyCoupled.Controllers
{
    public class CloselyCoupledController : Controller
    {
        public ViewResult Index() =>
            View("List", new MemoryRepository().Products);
    }
}
