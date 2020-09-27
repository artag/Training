namespace Builder
{
    // Директор отвечает только за выполнение шагов построения в определённой
    // последовательности. Это полезно при производстве продуктов в определённом
    // порядке или особой конфигурации. Строго говоря, класс Директор
    // необязателен, так как клиент может напрямую управлять строителями.
    //
    // Вариант Директора с установкой Строителя через конструктор.
    public class DirectorA
    {
        private IBuilder _builder;

        public DirectorA(IBuilder builder)
        {
            _builder = builder;
        }

        // Директор может строить несколько вариаций продукта, используя
        // одинаковые шаги построения.
        public void BuildMinimalProduct()
        {
            _builder.BuildPartA();
        }

        public void BuildFullProduct()
        {
            _builder.BuildPartA();
            _builder.BuildPartB();
            _builder.BuildPartC();
        }
    }
}
