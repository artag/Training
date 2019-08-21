using System;

namespace DependencyInjection
{
    interface IDisposeMeTo : IDisposable
    {
    }

    class DisposeMe : IDisposable
    {
        private static int _increment;
        private readonly IDisposeMeTo _child;
        private readonly int _number;

        public DisposeMe(IDisposeMeTo child)
        {
            _number = ++_increment;
            Console.WriteLine($"DisposeMe {_number} created");
            _child = child;
        }

        public void Dispose()
        {
            Console.WriteLine($"DisposeMe {_number} disposed");
        }

        public void DoIt()
        {
            Console.WriteLine($"DisposeMe {_number} did it");
        }
    }

    class DisposeMeTo : IDisposeMeTo
    {
        private static int _increment;
        private readonly int _number;

        public DisposeMeTo()
        {
            _number = ++_increment;
            Console.WriteLine($"DisposeMeTo {_number} created");
        }

        public void Dispose()
        {
            Console.WriteLine($"DisposeMeTo {_number} disposed");
        }
    }
}
