using System;

namespace Composite
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client();

            var leaf = new Leaf();

            var branch1 = new Composite();
            branch1.Add(new Leaf());
            branch1.Add(new Leaf());
            var branch2 = new Composite();
            branch2.Add(new Leaf());
            var tree = new Composite();
            tree.Add(branch1);
            tree.Add(branch2);

            Console.WriteLine("Client. Get Leaf:");
            client.ClientCode(leaf);

            Console.WriteLine("Client. Get Composite (tree):");
            client.ClientCode(tree);

            Console.WriteLine("Client. Get Composite (tree) and Leaf:");
            client.ClientCode2(tree, leaf);
        }
    }
}
