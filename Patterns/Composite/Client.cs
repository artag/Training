using System;

namespace Composite
{
    // Работа с компонентами:
    // 1. Через приватные поля (инициализация через конструктор)
    // 2. Через параметры методов (как сделано здесь)
    public class Client
    {
        // Клиентский код работает со всеми компонентами через базовый
        // интерфейс.
        public void ClientCode(Component component)
        {
            var result = component.Operation();
            Console.WriteLine($"RESULT: {result}");
            Console.WriteLine();
        }

        // Благодаря тому, что операции управления потомками объявлены в базовом
        // классе Компонента, клиентский код может работать как с простыми, так
        // и со сложными компонентами, вне зависимости от их конкретных классов.
        public void ClientCode2(Component component1, Component component2)
        {
            if (component1.IsComposite())
            {
                component1.Add(component2);
            }

            var result = component1.Operation();
            Console.WriteLine($"RESULT: {result}");
            Console.WriteLine();
        }
    }
}
