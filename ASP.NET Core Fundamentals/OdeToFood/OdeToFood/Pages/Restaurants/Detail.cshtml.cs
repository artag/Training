using Microsoft.AspNetCore.Mvc.RazorPages;
using OdeToFood.Core;

namespace OdeToFood.Pages.Restaurants
{
    public class DetailModel : PageModel
    {
        public Restaurant Restaurant { get; set; }

        public void OnGet()
        {
            Restaurant = new Restaurant();
        }
    }
}