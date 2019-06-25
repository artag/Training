using System.Linq;

namespace UsingActionInjection.Models
{
    public class ProductTotalizer
    {
        private readonly IRepository _repository;

        public ProductTotalizer(IRepository repository)
        {
            _repository = repository;
        }

        public decimal Total => _repository.Products.Sum(product => product.Price);
    }
}
