using ContentFormatting.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContentFormatting.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;

        public HomeController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index() => View(_repository.Reservations);

        public IActionResult AddReservation(Reservation reservation)
        {
            _repository.AddReservation(reservation);
            return RedirectToAction(nameof(Index));
        }
    }
}
