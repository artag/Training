using System;

namespace DependencyInjection
{
    static class ConsoleEx
    {
        public static void WaitingForKeyPressing()
        {
            Console.WriteLine("\nPress \"Enter\" to continue");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
