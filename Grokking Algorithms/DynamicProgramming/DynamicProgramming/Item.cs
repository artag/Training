using System;

namespace DynamicProgramming
{
    public class Item
    {
        public Item(string name, double cost, double weight, string costName, string weightName)
        {
            Name = name;

            Cost = cost;
            CostName = costName;

            Weight = weight;
            WeightName = weightName;
        }

        public string Name { get; }

        public double Cost { get; }

        public string CostName { get; }

        public double Weight { get; }

        public string WeightName { get; }

        public override string ToString()
        {
            return string.Format($"{Name}: {Cost} {CostName}, Weight: {Weight} {WeightName}");
        }

        public override bool Equals(object obj)
        {
            var item = obj as Item;
            if (item == null)
                return false;

            return Name.Equals(item.Name) &&
                   Math.Abs(Cost - item.Cost) < double.Epsilon &&
                   Math.Abs(Weight - item.Weight) < double.Epsilon &&
                   CostName.Equals(item.CostName) &&
                   WeightName.Equals(item.WeightName);
        }

        public static bool operator!=(Item item1, Item item2)
        {
            return !(item1 == item2);
        }

        public static bool operator==(Item item1, Item item2)
        {
            if (ReferenceEquals(item1, null) && ReferenceEquals(item2, null))
                return true;

            if (ReferenceEquals(item1, null) || ReferenceEquals(item2, null))
                return false;

            return item1.Equals(item2);
        }
    }
}
