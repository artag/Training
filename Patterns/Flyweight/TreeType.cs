using System;
using System.Text;

namespace Flyweight
{
    // Класс-легковес. Содержит общие поля.
    // Ссылаемся на него из множества отдельных деревьев.
    public class TreeType : IDrawableTree
    {
        private readonly string _name;
        private readonly string _color;

        public TreeType(string name, string color)
        {
            _name = name;
            _color = color;
        }

        public void Draw(string surface)
        {
            Console.WriteLine($"Draw {_name} tree with {_color} color on {surface}.");
        }

        public override string ToString()
        {
            var tree = new StringBuilder();
            tree.Append($"Name: {_name}; ");
            tree.Append($"Color: {_color};");
            return tree.ToString();
        }
    }
}
