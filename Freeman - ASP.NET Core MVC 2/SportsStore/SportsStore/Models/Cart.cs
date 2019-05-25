using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Models
{
    public class Cart
    {
        private readonly List<CartLine> _lineCollection = new List<CartLine>();

        public virtual IEnumerable<CartLine> Lines => _lineCollection;

        public virtual void AddItem(Product product, int quantity)
        {
            var selectedLine = _lineCollection
                .FirstOrDefault(line => line.Product.ProductID == product.ProductID);

            if (selectedLine == null)
            {
                var cartLine = new CartLine
                {
                    Product = product,
                    Quantity = quantity
                };

                _lineCollection.Add(cartLine);
            }
            else
            {
                selectedLine.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product) =>
            _lineCollection.RemoveAll(line => line.Product.ProductID == product.ProductID);

        public virtual decimal ComputeTotalValue() =>
            _lineCollection.Sum(line => line.Product.Price * line.Quantity);

        public virtual void Clear() => _lineCollection.Clear();
    }
}
