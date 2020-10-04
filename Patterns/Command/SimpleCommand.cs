using System;

namespace Command
{
    // Некоторые команды способны выполнять простые операции самостоятельно.
    internal class SimpleCommand : ICommand
    {
        private readonly string _payload;

        public SimpleCommand(string payload)
        {
            _payload = payload;
        }

        public void Execute()
        {
            Console.WriteLine("SimpleCommand. Execute. Begin.");
            Console.WriteLine($"Printing: {_payload}");
            Console.WriteLine("SimpleCommand. Execute. End.");
            Console.WriteLine();
        }
    }
}
