using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChildTasks
{
    class Main2
    {
        public static void Execute()
        {
            var parent = new Task(() =>
            {
                // Дочерняя задача.
                var child = new Task(() =>
                {
                    Console.WriteLine("Child task starting.");
                    Thread.Sleep(3000);
                    Console.WriteLine("Child task finishing.");
                    // throw new Exception();      // To test failure handler
                }, TaskCreationOptions.AttachedToParent);

                // Сработает если предыдущая дочерняя задача успешно завершится.
                var completionHandler = child.ContinueWith(t =>
                {
                    Console.WriteLine($"Hooray, task {t.Id}'s state is {t.Status}");
                },
                TaskContinuationOptions.AttachedToParent |
                TaskContinuationOptions.OnlyOnRanToCompletion);

                // Сработает если предыдущая дочерняя задача завершится с ошибкой.
                var failHandler = child.ContinueWith(t =>
                {
                    Console.WriteLine($"Oops, task {t.Id}'s state is {t.Status}");
                },
                TaskContinuationOptions.AttachedToParent |
                TaskContinuationOptions.OnlyOnFaulted);

                child.Start();
            });

            parent.Start();

            try
            {
                parent.Wait();
            }
            catch (AggregateException ae)
            {
                ae.Handle(e => true);
            }
        }
    }
}
