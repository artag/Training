namespace ChainOfResponsibilty
{
    // Поведение цепочки по умолчанию может быть реализовано внутри базового
    // класса обработчика.
    public abstract class AbstractHandler<T> : IHandler<T>
    {
        private IHandler<T> _nextHandler;

        public IHandler<T> SetNext(IHandler<T> handler)
        {
            _nextHandler = handler;

            // Возврат обработчика отсюда позволит связать обработчики простым
            // способом, вот так:
            // monkey.SetNext(squirrel).SetNext(dog);
            return handler;
        }

        public virtual T Handle(T request)
        {
            if (_nextHandler != null)
            {
                return _nextHandler.Handle(request);
            }

            // Обычно тут возвращается null (если конец цепочки), но лучше так не делать.
            return request;
        }
    }
}
