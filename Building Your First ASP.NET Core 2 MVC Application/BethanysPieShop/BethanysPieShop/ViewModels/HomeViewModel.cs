using System.Collections.Generic;
using BethanysPieShop.Models;

namespace BethanysPieShop.ViewModels
{
    public class HomeViewModel
    {
        public string Title { get; set; }
        public IEnumerable<Pie> Pies { get; set; }
    }
}
