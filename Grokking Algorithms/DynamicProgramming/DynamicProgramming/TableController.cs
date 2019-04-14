using System.Collections.Generic;
using System.Linq;

namespace DynamicProgramming.Test
{
    public class TableController
    {
        private Item[] _items;
        private int _maxWeight;
        private WeightsService _weightsService;

        public TableController()
        {
        }

        public TableController(IEnumerable<Item> items, int maxWeight)
        {
            SetItemsAndInitTable(items, maxWeight);
        }

        public List<Item>[,] Table { get; private set; }

        public int RowCount => Table.GetLength(0);

        public int ColumnCount => Table.GetLength(1);

        private double[] Weights { get; set; }

        public IEnumerable<Item> GetResult() => Table[RowCount - 1, ColumnCount - 1];

        public void SetItemsAndInitTable(IEnumerable<Item> items, int maxWeight)
        {
            _items = items.ToArray();

            InitWeights(items, maxWeight);
            InitTable();
            FillTable();
        }

        private void InitWeights(IEnumerable<Item> items, int maxWeight)
        {
            var weights = items.Select(item => item.Weight);
            _weightsService = new WeightsService(weights, maxWeight);
            Weights = _weightsService.Weights.ToArray();
        }

        private void InitTable()
        {
            var rows = _items.Count();
            var cols = _weightsService.Weights.Count;

            Table = new List<Item>[rows, cols];

            for (var row = 0; row < rows; row++)
                for (var col = 0; col < cols; col++)
                    Table[row, col] = new List<Item>();
        }

        private void FillTable()
        {
            for (var row = 0; row < RowCount; row++)
                for (var col = 0; col < ColumnCount; col++)
                    if (row == 0)
                        TryAddCurrentItemToCellInFirstRow(row, col);
                    else
                        AddItemsToCellInOtherRows(col, row);
        }

        private void TryAddCurrentItemToCellInFirstRow(int row, int col)
        {
            if (_items[row].Weight <= Weights[col])
                Table[row, col].Add(_items[row]);
        }

        private void AddItemsToCellInOtherRows(int col, int row)
        {
            var index = _weightsService.GetRemainingWeightIndex(col, _items[row]);

            var addedItems = index >= 0
                ? AddPreviousItemsOrCurrentItemWithRemainingPreviousItems(row, col, index)
                : AddPreviousItemsOrCurrentItem(row, col);

            Table[row, col].AddRange(addedItems);
        }

        private IEnumerable<Item> AddPreviousItemsOrCurrentItemWithRemainingPreviousItems(
            int row, int col, int remainingWeightIdx)
        {
            var currentItem = _items[row];
            var previousItems = Table[row - 1, col];

            var previousRemainingItemsCost = Table[row - 1, remainingWeightIdx]
                                             .Select(item => item.Cost).Sum();

            var itemsToAdd = GetPreviousItemsCost(row, col) > previousRemainingItemsCost + currentItem.Cost
                ? previousItems
                : Table[row - 1, remainingWeightIdx].Append(_items[row]);

            return itemsToAdd;
        }

        private IEnumerable<Item> AddPreviousItemsOrCurrentItem(int row, int col)
        {
            IEnumerable<Item> itemsToAdd;

            var currentItem = _items[row];
            var previousItems = Table[row - 1, col];

            if (currentItem.Weight <= Weights[col])
                itemsToAdd = GetPreviousItemsCost(row, col) > currentItem.Cost
                    ? previousItems
                    : new List<Item> { _items[row] };
            else
                itemsToAdd = previousItems;

            return itemsToAdd;
        }

        private double GetPreviousItemsCost(int row, int col) =>
            Table[row - 1, col].Select(item => item.Cost).Sum();
    }
}
