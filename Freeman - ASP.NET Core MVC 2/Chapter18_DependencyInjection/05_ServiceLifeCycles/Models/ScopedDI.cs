namespace ServiceLifeCycles.Models
{
    public class ScopedDI
    {
        private readonly IScopedDI _scopedDI;

        public ScopedDI(IScopedDI scopedDI)
        {
            _scopedDI = scopedDI;
        }

        public string Guid => _scopedDI.Guid;
    }
}
