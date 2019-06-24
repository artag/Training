namespace ServiceLifeCycles.Models
{
    public class ScopedDIImpl : IScopedDI
    {
        public ScopedDIImpl()
        {
            Guid = System.Guid.NewGuid().ToString();
        }

        public string Guid { get; }
    }
}
