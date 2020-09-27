using System;

namespace Bridge
{
    // За исключением этапа инициализации, когда объект Абстракции
    // связывается с определённым объектом Реализации, клиентский код должен
    // зависеть только от класса Абстракции. Таким образом, клиентский код
    // может поддерживать любую комбинацию абстракции и реализации.
    internal class Client : IClient
    {
        private readonly IAbstraction _abstraction;

        public Client(IAbstraction abstraction)
        {
            _abstraction = abstraction;
        }

        public void DoWork()
        {
            var result = _abstraction.Operation();
            Console.WriteLine($"Result: {result}");
        }
    }
}
