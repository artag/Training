using System.Collections.Generic;
using System.Linq;

namespace DijkstraAlgorithm
{
    public class Graph
    {
        private new Dictionary<string, Dictionary<string, int>> _graph =
            new Dictionary<string, Dictionary<string, int>>();

        public void AddNode(string name)
        {
            _graph.Add(name, new Dictionary<string, int>());
        }

        public void AddEdge(string startNode, string endNode, int weight)
        {
            _graph[startNode].Add(endNode, weight);
        }

        public IEnumerable<string> GetNodes()
        {
            return _graph.Select(node => node.Key);
        }

        public IDictionary<string, int> GetNeighbors(string node)
        {
            return _graph[node];
        }
    }
}
