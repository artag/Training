namespace DependencyInjection
{
    interface IGenericService<T>
    {
        T Create();
    }

    class GenericService<T> : IGenericService<T>
    {
        public T Create()
        {
            return default(T);
        }
    }
}
