using System.Collections.Generic;

namespace DependencyChains.Models
{
    public interface IModelStorage
    {
        IEnumerable<Product> Items { get; }

        Product this[string key] { get; set; }

        bool ContainsKey(string key);

        void RemoveItem(string key);
    }
}
