using System.Linq;
using CreatingViewComponent.Models;
using Microsoft.AspNetCore.Mvc;

namespace CreatingViewComponent.Components
{
    public class CitySummary : ViewComponent
    {
        private readonly ICityRepository _repository;

        public CitySummary(ICityRepository repository)
        {
            _repository = repository;
        }

        public string Invoke()
        {
            return $"{_repository.Cities.Count()} cities, " +
                   $"{_repository.Cities.Sum(c => c.Population)} people";
        }
    }
}
