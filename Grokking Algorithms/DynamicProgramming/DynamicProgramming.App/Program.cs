using System.Collections.Generic;
using DynamicProgramming.Test;

namespace DynamicProgramming.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var tableController = new TableController();

            Test1(tableController);
            Test2(tableController);
            Test3(tableController);
            Test4(tableController);
            Test5(tableController);
        }

        private static void Test1(TableController tableController)
        {
            Display.Clear();

            var items = new List<Item>()
            {
                new Item("Tape player", 3000, 4, "$", "pounds"),
                new Item("Notebook", 2000, 3, "$", "pounds"),
                new Item("Guitar", 1500, 1, "$", "pounds"),
            };
            var maxWeight = 4;

            Display.Items(items, maxWeight);

            tableController.SetItemsAndInitTable(items, maxWeight);

            var itemsToGet = tableController.GetResult();
            Display.ItemsToGet(itemsToGet, maxWeight);

            Display.PauseAndClear();
        }

        private static void Test2(TableController tableController)
        {
            var items = new List<Item>()
            {
                new Item("Tape player", 3000, 4, "$", "pounds"),
                new Item("Notebook", 2000, 3, "$", "pounds"),
                new Item("Guitar", 1500, 1, "$", "pounds"),
                new Item("IPhone", 2000, 1, "$", "pounds"),
            };
            var maxWeight = 4;

            Display.Items(items, maxWeight);

            tableController.SetItemsAndInitTable(items, maxWeight);

            var itemsToGet = tableController.GetResult();
            Display.ItemsToGet(itemsToGet, maxWeight);

            Display.PauseAndClear();
        }

        private static void Test3(TableController tableController)
        {
            var items = new List<Item>()
            {
                new Item("Tape player", 3000, 4, "$", "pounds"),
                new Item("Notebook", 2000, 3, "$", "pounds"),
                new Item("Guitar", 1500, 1, "$", "pounds"),
                new Item("IPhone", 2000, 1, "$", "pounds"),
                new Item("MP3 player", 1000, 1, "$", "pounds"),
            };
            var maxWeight = 4;

            Display.Items(items, maxWeight);

            tableController.SetItemsAndInitTable(items, maxWeight);

            var itemsToGet = tableController.GetResult();
            Display.ItemsToGet(itemsToGet, maxWeight);

            Display.PauseAndClear();
        }

        private static void Test4(TableController tableController)
        {
            var items = new List<Item>()
            {
                new Item("Westminster Abbey", 7, 0.5,"points", "days"),
                new Item("The Globe Theatre", 6, 0.5, "points", "days"),
                new Item("National Gallery", 9, 1, "points", "days"),
                new Item("British Museum", 9, 2, "points", "days"),
                new Item("St Paul's Cathedral", 8, 0.5, "points", "days"),
            };
            var maxWeight = 2;

            Display.Items(items, maxWeight);

            tableController.SetItemsAndInitTable(items, maxWeight);

            var itemsToGet = tableController.GetResult();
            Display.ItemsToGet(itemsToGet, maxWeight);

            Display.PauseAndClear();
        }

        private static void Test5(TableController tableController)
        {
            var items = new List<Item>()
            {
                new Item("Water", 10, 3,"points", "pounds"),
                new Item("Book", 3, 1, "points", "pounds"),
                new Item("Food", 9, 2, "points", "pounds"),
                new Item("Jacket", 5, 2, "points", "pounds"),
                new Item("Camera", 6, 1, "points", "pounds"),
            };
            var maxWeight = 6;

            Display.Items(items, maxWeight);

            tableController.SetItemsAndInitTable(items, maxWeight);

            var itemsToGet = tableController.GetResult();
            Display.ItemsToGet(itemsToGet, maxWeight);

            Display.PauseAndClear();
        }
    }
}
