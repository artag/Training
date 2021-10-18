using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChildTasks
{
    class Main1
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
                }, TaskCreationOptions.AttachedToParent);

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
