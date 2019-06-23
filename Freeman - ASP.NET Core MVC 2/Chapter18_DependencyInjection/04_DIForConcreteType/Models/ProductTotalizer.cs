using System.Linq;

namespace DIForConcreteType.Models
{
    public class ProductTotalizer
    {
        private readonly IRepository _repository;

        public ProductTotalizer(IRepository repository)
        {
            _repository = repository;
        }

        public IRepository Repository { get; set; }

        public decimal Total => _repository.Products.Sum(product => product.Price);
    }
}
