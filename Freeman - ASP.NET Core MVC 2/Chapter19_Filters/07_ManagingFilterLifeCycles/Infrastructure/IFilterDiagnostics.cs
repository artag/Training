using System.Collections.Generic;

namespace ManagingFilterLifeCycles.Infrastructure
{
    public interface IFilterDiagnostics
    {
        IEnumerable<string> Messages { get; }
        void AddMessage(string message);
    }
}
