using System.Collections.Generic;
using System.Linq;

namespace DijkstraAlgorithm
{
    public class Algorithm
    {
        private readonly Dictionary<string, int> _costs = new Dictionary<string, int>();
        private readonly Dictionary<string, string> _parents = new Dictionary<string, string>();
        private readonly HashSet<string> _processedNodes = new HashSet<string>();

        private Graph _inputGraph;
        private string _startNode;
        private string _endNode;

        public void DisplayMinimalCost(Graph inputGraph, string startNode, string endNode)
        {
            _inputGraph = inputGraph;
            _startNode = startNode;
            _endNode = endNode;

            ResetInnerCollections();

            AddAllNodesToInnerCollections();
            AddFirstNodeNeighborsToInnerCollections();

            FindMinimalCost();

            var path = GetMinimalCostPath();
            Display.MinimalCostPath(path);
            Display.MinimalCost(_costs[_endNode]);
        }

        private void ResetInnerCollections()
        {
            _costs.Clear();
            _parents.Clear();
            _processedNodes.Clear();
        }

        private void AddFirstNodeNeighborsToInnerCollections()
        {
            var nodesToAdd = _inputGraph.GetNeighbors(_startNode);
            foreach (var node in nodesToAdd)
            {
                _costs[node.Key] = node.Value;
                _parents[node.Key] = _startNode;
            }

            _processedNodes.Add(_startNode);
            _processedNodes.Add(_endNode);
        }

        private void AddAllNodesToInnerCollections()
        {
            foreach (var node in _inputGraph.GetNodes())
            {
                _costs.Add(node, int.MaxValue);
                _parents.Add(node, null);
            }
        }

        private void FindMinimalCost()
        {
            var processedNode = GetLowestCostNode();
            while (processedNode != string.Empty)
            {
                var neighbors = _inputGraph.GetNeighbors(processedNode);
                var cost = _costs[processedNode];
                foreach (var node in neighbors.Keys)
                {
                    var newCost = cost + neighbors[node];
                    if (_costs[node] > newCost)
                    {
                        _costs[node] = newCost;
                        _parents[node] = processedNode;
                    }
                }

                _processedNodes.Add(processedNode);
                processedNode = GetLowestCostNode();
            }
        }

        private string GetLowestCostNode()
        {
            var remainingNodes = _costs.Where(pair => !_processedNodes.Contains(pair.Key));
            return remainingNodes.Any()
                ? remainingNodes.OrderBy(pair => pair.Value).First().Key
                : string.Empty;
        }

        private Stack<string> GetMinimalCostPath()
        {
            var displayPath = new Stack<string>();

            displayPath.Push(_endNode);
            var currentNode = _parents[_endNode];
            while (currentNode != _startNode)
            {
                displayPath.Push(currentNode);
                currentNode = _parents[currentNode];
            }

            displayPath.Push(_startNode);
            return displayPath;
        }
    }
}
