using System;

namespace Flyweight
{
    // Класс-легковес с уникальными полями.
    // Также содержит другой класс-легковес с общими полями.
    public class Tree : IDrawableTree
    {
        private readonly int _x;
        private readonly int _y;
        private readonly TreeType _treeType;

        public Tree(int x, int y, TreeType treeType)
        {
            _x = x;
            _y = y;
            _treeType = treeType;
        }

        public void Draw(string surface)
        {
            Console.Write($"Tree at ({_x}; {_y}). ");
            _treeType.Draw(surface);
        }
    }
}
