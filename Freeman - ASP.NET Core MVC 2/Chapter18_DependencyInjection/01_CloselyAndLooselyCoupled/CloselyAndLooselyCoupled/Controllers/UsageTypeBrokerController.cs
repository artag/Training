using CloselyAndLooselyCoupled.Infrastructure;
using CloselyAndLooselyCoupled.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloselyAndLooselyCoupled.Controllers
{
    public class UsageTypeBrokerController : Controller
    {
        public IRepository Repository { get; } = TypeBroker.Repository;

        public ViewResult Index() =>
            View("List", Repository.Products);
    }
}
