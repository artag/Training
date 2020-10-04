using System;

namespace Command
{
    // Объект Вызывающего класса может быть связан с одной или несколькими командами.
    // Он отправляет запрос команде/командам.
    internal class Invoker : IInvoker
    {
        private readonly ICommand _startCommand;
        private readonly ICommand _finishCommand;

        public Invoker(ICommand startCommand, ICommand finishCommand)
        {
            _startCommand = startCommand;
            _finishCommand = finishCommand;
        }

        // Вызывающий объект не зависит от классов конкретных команд и получателей.
        // Вызывающий объект передаёт запрос получателю косвенно, выполняя команду.
        public void Invoke()
        {
            Console.WriteLine("Invoker. Execute start command");
            _startCommand.Execute();

            Console.WriteLine("Invoker: Execute finish command");
            _finishCommand.Execute();
        }
    }
}
