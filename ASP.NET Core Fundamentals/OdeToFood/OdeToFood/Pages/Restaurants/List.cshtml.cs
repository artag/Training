using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OdeToFood.Core;
using OdeToFood.Data;

namespace OdeToFood.Pages.Restaurants
{
    public class ListModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly IRestaurantData _restaurantData;
        private readonly ILogger<ListModel> _logger;

        public ListModel(
            IConfiguration config,
            IRestaurantData restaurantData,
            ILogger<ListModel> logger)
        {
            _config = config;
            _restaurantData = restaurantData;
            _logger = logger;
        }

        public string Message { get; set; }

        public string MessageFromConfig { get; set; }

        public IEnumerable<Restaurant> Restaurants { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public void OnGet()
        {
            _logger.LogError("Executing ListModel");

            Message = "Hello, World!";
            MessageFromConfig = _config["Message"];
            Restaurants = _restaurantData.GetRestaurantsByName(SearchTerm);
        }
    }
}