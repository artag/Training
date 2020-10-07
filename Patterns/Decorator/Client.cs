using System;

namespace Decorator
{
    internal class Client
    {
        // Клиентский код работает со всеми объектами, используя интерфейс
        // Компонента. Таким образом, он остаётся независимым от конкретных
        // классов компонентов, с которыми работает.
        public void Call(Component component)
        {
            Console.WriteLine("RESULT: " + component.Operation());
        }
    }
}
