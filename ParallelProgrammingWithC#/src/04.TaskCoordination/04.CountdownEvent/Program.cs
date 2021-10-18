using System.Threading.Tasks;
using System;
using System.Threading;

namespace CountdownEventDemo
{
    class Program
    {
        private static int taskCount = 5;   // Количество запускаемых задач.
        private static Random random = new Random();
        static CountdownEvent cte = new CountdownEvent(taskCount);

        static void Main(string[] args)
        {
            for (int i = 0; i < taskCount; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"Entering task {Task.CurrentId}");
                    Thread.Sleep(random.Next(3000));
                    // Регистрирует сигнал и уменьшает счетчик в CountdownEvent.
                    cte.Signal();
                    Console.WriteLine($"Exiting task {Task.CurrentId}");
                });
            }

            var finalTask = Task.Factory.StartNew(() => 
            {
                Console.WriteLine($"Waiting for other tasks to complete in {Task.CurrentId}");
                // Ожидает завершения всех задач. По окончании ожидания продолжает работу дальше.
                cte.Wait();
                Console.WriteLine("All tasks completed");
            });

            finalTask.Wait();
        }
    }
}
