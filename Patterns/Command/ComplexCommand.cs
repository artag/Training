using System;

namespace Command
{
    // Есть команды, которые делегируют более сложные операции другим
    // объектам, называемым "получателями".
    class ComplexCommand : ICommand
    {
        private readonly Receiver _receiver;

        // Данные о контексте, необходимые для запуска методов получателя.
        private readonly string _a;
        private readonly string _b;

        // Сложные команды могут принимать один или несколько объектов-
        // получателей вместе с любыми данными о контексте через конструктор.
        public ComplexCommand(Receiver receiver, string a, string b)
        {
            _receiver = receiver;
            _a = a;
            _b = b;
        }

        // Команды могут делегировать выполнение любым методам получателя.
        public void Execute()
        {
            Console.WriteLine("ComplexCommand. Execute. Begin");

            _receiver.DoSomething(_a);
            _receiver.DoSomethingElse(_b);

            Console.WriteLine("ComplexCommand. Execute. End");
            Console.WriteLine();
        }
    }
}
