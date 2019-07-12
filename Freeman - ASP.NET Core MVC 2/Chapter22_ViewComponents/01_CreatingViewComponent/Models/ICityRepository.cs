using System.Collections.Generic;

namespace CreatingViewComponent.Models
{
    public interface ICityRepository
    {
        IEnumerable<City> Cities { get; }

        void AddCity(City newCity);
    }
}
