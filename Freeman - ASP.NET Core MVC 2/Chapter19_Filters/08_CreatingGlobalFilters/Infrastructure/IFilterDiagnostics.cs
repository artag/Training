using System.Collections.Generic;

namespace CreatingGlobalFilters.Infrastructure
{
    public interface IFilterDiagnostics
    {
        IEnumerable<string> Messages { get; }
        void AddMessage(string message);
    }
}
