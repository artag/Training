namespace ChainOfResponsibilty
{
    // Интерфейс Обработчика объявляет метод построения цепочки обработчиков.
    // Он также объявляет метод для выполнения запроса.
    public interface IHandler<T>
    {
        IHandler<T> SetNext(IHandler<T> handler);

        T Handle(T request);
    }
}
