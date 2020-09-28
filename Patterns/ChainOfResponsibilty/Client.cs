using System;

namespace ChainOfResponsibilty
{
    public class Client
    {
        // Обычно клиентский код приспособлен для работы с единственным
        // обработчиком. В большинстве случаев клиенту даже неизвестно, что этот
        // обработчик является частью цепочки.
        public void GiveFood(AbstractHandler<string> handler, string food)
        {
            Console.WriteLine($"Give food: {food}");
            var result = handler.Handle(food);

            if (result != food)
            {
                Console.Write($"    {result}");
            }
            else
            {
                // Конец цепочки обработки.
                Console.WriteLine($"    {food} was left untouched.");
            }
        }
    }
}
