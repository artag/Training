using Microsoft.AspNetCore.Mvc;
using WorkWithAreas.Areas.Admin.Models;

namespace WorkWithAreas.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private Person[] _data = new Person[]
        {
            new Person { Name = "Alice", City = "London" },
            new Person { Name = "Bob", City = "Paris" },
            new Person { Name = "Joe", City = "New York" },
        };

        public ViewResult Index() => View(_data);
    }
}
