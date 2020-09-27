using System;
using System.Collections.Generic;

namespace Builder
{
    // Имеет смысл использовать паттерн Строитель только тогда, когда ваши
    // продукты достаточно сложны и требуют обширной конфигурации.
    //
    // В отличие от других порождающих паттернов, различные конкретные строители
    // могут производить несвязанные продукты. Другими словами, результаты
    // различных строителей  могут не всегда следовать одному и тому же
    // интерфейсу.
    public class Product
    {
        private readonly List<string> _parts = new List<string>();

        public void Add(string part)
        {
            _parts.Add(part);
        }

        public void Show()
        {
            Console.WriteLine("*** Product Parts ***");
            foreach (var part in _parts)
                Console.WriteLine(part);
        }
    }
}
