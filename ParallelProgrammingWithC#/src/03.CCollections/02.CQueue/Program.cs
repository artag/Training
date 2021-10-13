using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CQueue
{
    class Program
    {
        private static ConcurrentQueue<int> queue = new ConcurrentQueue<int>();

        private static void Enqueue(int value)
        {
            queue.Enqueue(value);
            Console.WriteLine($"Add {value}");
        }

        private static void TryDequeue(string taskName)
        {
            var success = queue.TryDequeue(out var result);
            if (success)
                Console.WriteLine($"Task {taskName}: Dequeue value {result}");
            else
                Console.WriteLine($"Task {taskName}: Dequeue false");
        }

        private static void TryPeek(string taskName)
        {
            var success = queue.TryPeek(out var result);
            if (success)
                Console.WriteLine($"Task {taskName}: Peek value {result}");
            else
                Console.WriteLine($"Task {taskName}: Peek false");
        }

        private static void EnqueueParallel1()
        {
            var task1 = Task.Run(() => Enqueue(1));
            var task2 = Task.Run(() => Enqueue(2));
            var task3 = Task.Run(() => Enqueue(3));
            var task4 = Task.Run(() => Enqueue(4));

            Task.WaitAll(task1, task2, task3, task4);
        }

        private static void EnqueueParallel2()
        {
            var task1 = Task.Run(() => Enqueue(1));
            var task2 = Task.Run(() => Enqueue(2));

            Task.WaitAll(task1, task2);
        }

        public static void Main(string[] args)
        {
            EnqueueParallel1();

            var task1 = Task.Run(() => TryDequeue("1"));
            var task2 = Task.Run(() => TryDequeue("2"));
            var task3 = Task.Run(() => TryDequeue("3"));
            var task4 = Task.Run(() => TryDequeue("4"));
            var task5 = Task.Run(() => TryDequeue("5"));

            Task.WaitAll(task1, task2, task3, task4);

            EnqueueParallel2();

            task1 = Task.Run(() => TryPeek("1"));
            task2 = Task.Run(() => TryDequeue("2"));
            task3 = Task.Run(() => TryPeek("3"));
            task4 = Task.Run(() => TryDequeue("4"));
            task5 = Task.Run(() => TryPeek("5"));

            Task.WaitAll(task1, task2, task3, task4, task5);
        }
    }
}
