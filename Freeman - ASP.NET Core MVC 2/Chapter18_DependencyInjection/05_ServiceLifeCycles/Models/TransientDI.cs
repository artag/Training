namespace ServiceLifeCycles.Models
{
    public class TransientDI
    {
        private readonly ITransientDI _transientDi;

        public TransientDI(ITransientDI transientDI)
        {
            _transientDi = transientDI;
        }

        public string Guid => _transientDi.Guid;
    }
}
