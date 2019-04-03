using System.Collections.Generic;
using System.Linq;

namespace GreedyAlgorithm
{
    public class Algorithm
    {
        public static IEnumerable<Station> SelectBestStations(
            IEnumerable<Station> stations, IEnumerable<string> states)
        {
            var bestStations = new List<Station>();

            while (states.Any())
            {
                Station bestStation = null;
                IEnumerable<string> statesCovered = new List<string>();

                foreach (var station in stations)
                {
                    var covered = station.CoveredStates.Intersect(states);
                    if (covered.Count() > statesCovered.Count())
                    {
                        bestStation = station;
                        statesCovered = covered;
                    }
                }

                states = states.Except(statesCovered);
                bestStations.Add(bestStation);
            }

            return bestStations;
        }
    }
}
