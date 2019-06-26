using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace OdeToFood.Pages.Restaurants
{
    public class ListModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public ListModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Message { get; set; }

        public string MessageFromConfig { get; set; }

        public void OnGet()
        {
            Message = "Hello World!";
            MessageFromConfig = _configuration["Message"];

        }
    }
}