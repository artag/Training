using System;
using System.Threading.Tasks;

namespace TaskCreateStart
{
    public static class Main3
    {
        public static void Execute()
        {
            var text1 = "testing";
            var text2 = "this";

            var task1 = new Task<int>(TextLength, text1);
            task1.Start();

            var task2 = Task.Factory.StartNew<int>(TextLength, text2);

            // Ожидание выполнения task.
            Console.WriteLine($"Length of '{text1}' is {task1.Result}");
            Console.WriteLine($"Length of '{text2}' is {task2.Result}");

            Console.WriteLine("Main program done.");
            Console.ReadKey();
        }

        public static int TextLength(object o)
        {
            Console.WriteLine($"\nTask with id {Task.CurrentId} processing object '{o}'...");
            return o.ToString().Length;
        }
    }
}
