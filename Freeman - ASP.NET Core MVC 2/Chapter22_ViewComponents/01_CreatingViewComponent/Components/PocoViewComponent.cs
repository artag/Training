using System.Linq;
using CreatingViewComponent.Models;

namespace CreatingViewComponent.Components
{
    public class PocoViewComponent
    {
        private readonly ICityRepository _repository;

        public PocoViewComponent(ICityRepository repository)
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
