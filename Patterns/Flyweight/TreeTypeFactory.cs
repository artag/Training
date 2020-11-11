using System;
using System.Collections.Generic;

namespace Flyweight
{
    // Фабрика классов-легковесов (с общими полями).
    // Решает, когда нужно создать новый легковес, а когда можно обойтись существующим.
    public class TreeTypeFactory 
    {
        private readonly Dictionary<string, TreeType> _treeTypes =
            new Dictionary<string, TreeType>();

        public TreeType GetTreeType(string name, string color)
        {
            var treeType = new TreeType(name, color);
            var key = treeType.ToString();
            if (!_treeTypes.ContainsKey(key))
            {
                Console.WriteLine($"Create new tree type {key}");
                _treeTypes[key] = treeType;
            }
            else
            {
                Console.WriteLine($"Get existing tree type {key}");
            }

            return _treeTypes[key];
        }
    }
}
