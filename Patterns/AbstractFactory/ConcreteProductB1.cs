namespace AbstractFactory
{
    // Конкретные Продукты создаются соответствующими Конкретными Фабриками.
    internal class ConcreteProductB1 : IAbstractProductB
    {
        public string MethodB()
        {
            return "The result of the product B1. Made by ConcreteFactory1.";
        }

        // Продукт B1 может корректно работать только с Продуктом A1. Тем не
        // менее, он принимает любой экземпляр Абстрактного Продукта А в
        // качестве аргумента.
        public string AnotherMethodB(IAbstractProductA collaborator)
        {
            var result = collaborator.MethodA();
            return $"The result of the B1 collaborating with the ({result})";
        }
    }
}
