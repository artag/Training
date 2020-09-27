namespace Builder
{
    // Интерфейс Строителя объявляет создающие методы для различных частей
    // объектов Продуктов.
    public interface IBuilder
    {
        void BuildPartA();
        void BuildPartB();
        void BuildPartC();

        Product GetResult();

        // Этот метод может отсутствовать в некоторых реализациях Builder.
        void Reset();
    }
}
