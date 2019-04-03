using System.Collections.Generic;

namespace GreedyAlgorithm
{
    public class Station
    {
        public Station(string name, IEnumerable<string> coveredStates)
        {
            Name = name;
            CoveredStates = new HashSet<string>();

            foreach (var state in coveredStates)
            {
                CoveredStates.Add(state);
            }
        }

        public string Name { get; }

        public HashSet<string> CoveredStates { get; }
    }
}
