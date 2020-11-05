using System;

namespace FactoryMethod
{
    // Клиентский код работает с экземпляром конкретного создателя, хотя и
    // через его базовый интерфейс.
    class Client1
    {
        private readonly Creator _creator;

        public Client1(Creator creator)
        {
            _creator = creator;
        }

        // Используется создатель, заданный через конструктор.
        public void DoWork()
        {
            var result = _creator.SomeOperation();
            Console.WriteLine(result);
        }
    }
}
