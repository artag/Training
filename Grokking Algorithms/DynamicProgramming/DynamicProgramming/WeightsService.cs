using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicProgramming
{
    public class WeightsService
    {
        public WeightsService(IEnumerable<double> weights, double specifiedWeight)
        {
            FillWeights(weights, specifiedWeight);
        }

        public List<double> Weights { get; } = new List<double>();

        public double GetRemainingWeight(int currentIndex, Item item)
        {
            return GetRemainingWeight(currentIndex, item.Weight);
        }

        public int GetRemainingWeightIndex(int currentIndex, Item item)
        {
            return GetRemainingWeightIndex(currentIndex, item.Weight);
        }

        public int GetRemainingWeightIndex(int currentIndex, IEnumerable<Item> items)
        {
            var itemsWeight = items.Select(item => item.Weight).Sum();

            return GetRemainingWeightIndex(currentIndex, itemsWeight);
        }

        private int GetRemainingWeightIndex(int currentIndex, double currentWeight)
        {
            var remainingWeight = GetRemainingWeight(currentIndex, currentWeight);
            if (Math.Abs(remainingWeight) < double.Epsilon)
                return -1;

            foreach (var w in Weights)
                if (w >= remainingWeight)
                    return Weights.IndexOf(w);

            return -1;
        }

        private double GetRemainingWeight(int currentIndex, double currentWeight)
        {
            var weightAtIndex = Weights.ElementAt(currentIndex);
            if (currentWeight > weightAtIndex)
                return 0;

            return weightAtIndex - currentWeight;
        }

        private void FillWeights(IEnumerable<double> weights, double maxWeight)
        {
            Weights.Clear();

            var affordableWeights = weights
                                    .Where(weight => weight <= maxWeight)
                                    .Append(maxWeight);

            var min = GreatestCommonDivisor.Find(affordableWeights);
            var max = affordableWeights.Max();

            if (min == max)
            {
                Weights.Add(min);
                return;
            }

            var tmpWeight = min;
            var i = 1;

            while (tmpWeight < max)
            {
                tmpWeight = min * i;
                var roundWeight = Math.Round(tmpWeight, 2);
                Weights.Add(roundWeight);
                i++;
            }
        }
    }
}
