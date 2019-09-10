using Microsoft.EntityFrameworkCore;
using OdeToFood.Core;

namespace OdeToFood.Data
{
    public class OdeToFoodContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }
    }
}
