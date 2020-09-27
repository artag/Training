using System;

namespace AbstractFactory
{
    // Клиентский код работает с фабриками и продуктами только через абстрактные
    // типы: Абстрактная Фабрика и Абстрактный Продукт. Это позволяет передавать
    // любой подкласс фабрики или продукта клиентскому коду, не нарушая его.
    internal class Client : IClient
    {
        private readonly IAbstractFactory _factory;

        // Клиентский код может работать с любым конкретным классом фабрики.
        public Client(IAbstractFactory factory)
        {
            _factory = factory;
        }

        public void DoWork()
        {
            var productA = _factory.CreateProductA();
            var productB = _factory.CreateProductB();

            var resultCallMethodA = productA.MethodA();
            Console.WriteLine($"{resultCallMethodA}");

            var resultCallMethodB = productB.MethodB();
            Console.WriteLine($"{resultCallMethodB}");

            var resultCallAnotherMethodB2 = productB.AnotherMethodB(productA);
            Console.WriteLine($"{resultCallAnotherMethodB2}");
        }
    }
}
