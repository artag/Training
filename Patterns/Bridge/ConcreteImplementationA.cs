namespace Bridge
{
    // Каждая Конкретная Реализация соответствует определённой платформе и
    // реализует интерфейс Реализации с использованием API этой платформы.
    internal class ConcreteImplementationA : IImplementation
    {
        public string OperationImplementation()
        {
            return "The result in ConcreteImplementationA.\n";
        }
    }
}
