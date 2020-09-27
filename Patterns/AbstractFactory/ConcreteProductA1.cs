namespace AbstractFactory
{
    // Конкретные продукты создаются соответствующими Конкретными Фабриками.
    internal class ConcreteProductA1 : IAbstractProductA
    {
        public string MethodA()
        {
            return "The result of the product A1. Made by ConcreteFactory1.";
        }
    }
}
