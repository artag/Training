using System.Linq;
using CreatingViewComponent.Models;
using Microsoft.AspNetCore.Mvc;

namespace CreatingViewComponent.Components
{
    public class CitySummaryView : ViewComponent
    {
        private readonly ICityRepository _repository;

        public CitySummaryView(ICityRepository repository)
        {
            _repository = repository;
        }

        public IViewComponentResult Invoke()
        {
            return View(new CityViewModel
            {
                Cities = _repository.Cities.Count(),
                Population = _repository.Cities.Sum(c => c.Population)
            });
        }
    }
}
