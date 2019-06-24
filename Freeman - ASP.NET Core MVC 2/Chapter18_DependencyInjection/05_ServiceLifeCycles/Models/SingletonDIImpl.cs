namespace ServiceLifeCycles.Models
{
    public class SingletonDIImpl : ISingletonDI
    {
        public SingletonDIImpl()
        {
            Guid = System.Guid.NewGuid().ToString();
        }

        public string Guid { get; }
    }
}
