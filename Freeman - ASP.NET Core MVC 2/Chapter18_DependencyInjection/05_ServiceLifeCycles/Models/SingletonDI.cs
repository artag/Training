namespace ServiceLifeCycles.Models
{
    public class SingletonDI
    {
        private readonly ISingletonDI _singletonDI;

        public SingletonDI(ISingletonDI singletonDi)
        {
            _singletonDI = singletonDi;
        }

        public string Guid => _singletonDI.Guid;
    }
}
