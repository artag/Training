using System;
using System.Threading;

namespace MutexOneRun
{
    class Program
    {
        static void Main(string[] args)
        {
            const string appName = "MyApp";
            Mutex mutex;

            try
            {
                mutex = Mutex.OpenExisting(appName);
                Console.WriteLine($"Sorry, {appName} is already running");
            }
            catch (WaitHandleCannotBeOpenedException e)
            {
                Console.WriteLine("We can run the program just fine");
                mutex = new Mutex(false, appName);
            }

            Console.WriteLine("Press any key to quit...");
            Console.ReadKey();
            mutex.ReleaseMutex();
        }
    }
}
