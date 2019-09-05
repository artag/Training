using System.Collections.Generic;
using System.Linq;
using OdeToFood.Core;

namespace OdeToFood.Data
{
    public class InMemoryRestaurantData : IRestaurantData
    {
        private readonly List<Restaurant> _restaurants;

        public InMemoryRestaurantData()
        {
            _restaurants = new List<Restaurant>()
            {
                new Restaurant
                {
                    Id = 1,
                    Name = "Scott's Pizza",
                    Location = "Maryland",
                    Cuisine = CuisineType.Italian
                },
                new Restaurant
                {
                    Id = 2,
                    Name = "Cinnamon Club",
                    Location = "London",
                    Cuisine = CuisineType.Italian
                },
                new Restaurant
                {
                    Id = 3,
                    Name = "La Costa",
                    Location = "California",
                    Cuisine = CuisineType.Mexican
                },
            };
        }

        public IEnumerable<Restaurant> GetRestaurantsByName(string name = null)
        {
            return
                from r in _restaurants
                where string.IsNullOrEmpty(name) || r.Name.StartsWith(name)
                orderby r.Name
                select r;
        }
    }
}
