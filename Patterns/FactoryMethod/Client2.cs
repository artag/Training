using System;

namespace FactoryMethod
{
    class Client2
    {
        // Используется создатель, передаваемый в метод.
        public void DoWork(Creator creator)
        {
            var result = creator.SomeOperation();
            Console.WriteLine(result);
        }
    }
}
