using System.Linq;
using BethanysPieShop.Models;
using BethanysPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPieRepository _pieRepository;

        public HomeController(IPieRepository pieRepository)
        {
            _pieRepository = pieRepository;
        }

        public IActionResult Index()
        {
            var pies = _pieRepository.GetAllPies().OrderBy(p => p.Name);

            var viewModel = new HomeViewModel()
            {
                Title = "Welcome to Bethany's Pie Shop",
                Pies = pies,
            };

            return View(viewModel);
        }
    }
}
