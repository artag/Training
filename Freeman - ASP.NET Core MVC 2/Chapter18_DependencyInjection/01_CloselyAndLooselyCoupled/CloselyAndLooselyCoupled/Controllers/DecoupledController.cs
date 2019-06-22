using CloselyAndLooselyCoupled.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloselyAndLooselyCoupled.Controllers
{
    public class DecoupledController : Controller
    {
        public IRepository Repository { get; set; } = new MemoryRepository();

        public ViewResult Index() =>
            View("List", Repository.Products);
    }
}
