using System.Collections.Generic;

namespace Flyweight
{
    // Клиент
    public class Forest
    {
        private readonly List<Tree> _trees = new List<Tree>();

        private readonly TreeTypeFactory _treeTypeFactory;
        private readonly string _surface;

        public Forest(TreeTypeFactory treeTypeFactory, string surface)
        {
            _treeTypeFactory = treeTypeFactory;
            _surface = surface;
        }

        public void PlantTree(int x, int y, string name, string color)
        {
            var treeType = _treeTypeFactory.GetTreeType(name, color);
            var tree = new Tree(x, y, treeType);
            _trees.Add(tree);
        }

        public void Draw()
        {
            _trees.ForEach(tree => tree.Draw(_surface));
        }
    }
}
