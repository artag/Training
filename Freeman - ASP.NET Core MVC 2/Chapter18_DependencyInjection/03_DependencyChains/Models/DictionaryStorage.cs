using System.Collections.Generic;

namespace DependencyChains.Models
{
    public class DictionaryStorage : IModelStorage
    {
        private Dictionary<string, Product> _items = new Dictionary<string, Product>();

        public Product this[string key]
        {
            get => _items[key];
            set => _items[key] = value;
        }

        public IEnumerable<Product> Items => _items.Values;

        public bool ContainsKey(string key) =>
            _items.ContainsKey(key);

        public void RemoveItem(string key) =>
            _items.Remove(key);
    }
}
