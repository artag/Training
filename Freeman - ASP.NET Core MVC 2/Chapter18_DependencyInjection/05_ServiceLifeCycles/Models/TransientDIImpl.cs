namespace ServiceLifeCycles.Models
{
    public class TransientDIImpl : ITransientDI
    {
        public TransientDIImpl()
        {
            Guid = System.Guid.NewGuid().ToString();
        }

        public string Guid { get; }
    }
}
