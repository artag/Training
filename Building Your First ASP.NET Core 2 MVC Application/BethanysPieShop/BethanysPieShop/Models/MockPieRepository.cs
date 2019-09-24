using System.Collections.Generic;
using System.Linq;

namespace BethanysPieShop.Models
{
    public class MockPieRepository : IPieRepository
    {
        private List<Pie> _pies;

        public MockPieRepository()
        {
            if (_pies == null)
            {
                InitializePies();
            }
        }

        public IEnumerable<Pie> GetAllPies()
        {
            return _pies;
        }

        public Pie GetPieById(int pieId)
        {
            return _pies.FirstOrDefault(p => p.Id == pieId);
        }

        private void InitializePies()
        {
            _pies = new List<Pie>()
            {
                new Pie
                {
                    Id = 1,
                    Name = "Apple Pie",
                    Price = 12.95M,
                    ShortDescription = "Our famous apple pies!",
                    ImageThumbnailUrl = @"/blob/applepiesmall.jpg",
                },
                new Pie
                {
                    Id = 2,
                    Name = "Blueberry Cheese Cake",
                    Price = 18.95M,
                    ShortDescription = "You'll love it!",
                    ImageThumbnailUrl = @"/blob/blueberrycheesecakesmall.jpg",
                },
                new Pie
                {
                    Id = 3,
                    Name = "Cheese Cake",
                    Price = 18.95M,
                    ShortDescription = "Plain cheese cake. Plain pleasure.",
                    ImageThumbnailUrl = @"/blob/cheesecakesmall.jpg",
                },
                new Pie
                {
                    Id = 4,
                    Name = "Cherry Pie",
                    Price = 15.95M,
                    ShortDescription = "A summer classic!",
                    ImageThumbnailUrl = @"/blob/cherrypiesmall.jpg",
                }
            };
        }
    }
}
