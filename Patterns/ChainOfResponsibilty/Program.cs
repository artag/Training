using System;

namespace ChainOfResponsibilty
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client();

            // Другая часть клиентского кода создает саму цепочку.
            var monkey = new MonkeyHandler();
            var squirrel = new SquirrelHandler();
            var dog = new DogHandler();

            monkey.SetNext(squirrel).SetNext(dog);

            Console.WriteLine("Chain: Monkey > Squirrel > Dog");
            client.GiveFood(monkey, "Nut");
            client.GiveFood(monkey, "Banana");
            client.GiveFood(monkey, "Coffee");
            Console.WriteLine();

            // Клиент должен иметь возможность отправлять запрос любому
            // обработчику, а не только первому в цепочке.
            Console.WriteLine("Chain: Squirrel > Dog");
            client.GiveFood(squirrel, "MeatBall");
            client.GiveFood(squirrel, "Banana");
            client.GiveFood(squirrel, "Coffee");
        }
    }
}
