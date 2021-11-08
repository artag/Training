using System;
using System.Threading.Tasks;

namespace AsyncInitPattern
{
    public  interface IAsyncInit
    {
        Task InitTask { get; }
    }

    public class MyClass : IAsyncInit
    {
        public MyClass()
        {
            Console.WriteLine($"Begin ctor {nameof(MyClass)}.");
            InitTask = InitAsync();
            Console.WriteLine($"End ctor {nameof(MyClass)}.");
        }

        public Task InitTask { get; }

        private async Task InitAsync()
        {
            Console.WriteLine($"Begin init {nameof(MyClass)}.");
            await Task.Delay(1000);
            Console.WriteLine($"End init {nameof(MyClass)}.");
        }
    }

        public class MyOtherClass : IAsyncInit
    {
        private readonly MyClass _myClass;

        public MyOtherClass(MyClass myClass)
        {
            Console.WriteLine($"Begin ctor {nameof(MyOtherClass)}.");

            _myClass = myClass;
            InitTask = InitAsync();

            Console.WriteLine($"End ctor {nameof(MyOtherClass)}.");
        }

        public Task InitTask { get; }

        private async Task InitAsync()
        {
            Console.WriteLine($"Begin init {nameof(MyOtherClass)}.");

            if (_myClass is IAsyncInit ai)
                await ai.InitTask;

            await Task.Delay(1000);

            Console.WriteLine($"End init {nameof(MyOtherClass)}.");
        }
    }

    public static class Program
    {
        private static async Task Main()
        {
            var myClass = new MyClass();
            var oc = new MyOtherClass(myClass);
            await oc.InitTask;

            // Display:
            // Begin ctor MyClass.
            // Begin init MyClass.
            // End ctor MyClass.
            // Begin ctor MyOtherClass.
            // Begin init MyOtherClass.
            // End ctor MyOtherClass.
            // End init MyClass.
            // End init MyOtherClass.
        }
    }
}
