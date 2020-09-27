namespace AbstractFactory
{
    internal class ConcreteProductB2 : IAbstractProductB
    {
        public string MethodB()
        {
            return "The result of the product B2. Made by ConcreteFactory2.";
        }

        public string AnotherMethodB(IAbstractProductA collaborator)
        {
            var result = collaborator.MethodA();
            return $"The result of the B2 collaborating with the ({result})";
        }
    }
}
