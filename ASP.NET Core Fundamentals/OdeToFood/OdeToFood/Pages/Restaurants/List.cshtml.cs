using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using OdeToFood.Core;
using OdeToFood.Data;

namespace OdeToFood.Pages.Restaurants
{
    public class ListModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IRestaurantData _restaurantData;

        public ListModel(IConfiguration configuration, IRestaurantData restaurantData)
        {
            _configuration = configuration;
            _restaurantData = restaurantData;
        }

        public IEnumerable<Restaurant> Restaurants { get; set; }

        public string Message { get; private set; }

        public string MessageFromConfig { get; private set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public void OnGet()
        {
            Restaurants = _restaurantData.GetRestaurantsByName(SearchTerm);

            Message = "Hello World!";
            MessageFromConfig = _configuration["Message"];
        }
    }
}
