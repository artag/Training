namespace FactoryMethod
{
    // Конкретные Продукты предоставляют различные реализации интерфейса
    // Продукта.
    class ConcreteProduct1 : IProduct
    {
        public string Operation()
        {
            return "{Result of ConcreteProduct1}";
        }
    }
}
