using System.Collections.Generic;

namespace ResolvingFilterDependencies.Infrastructure
{
    public interface IFilterDiagnostics
    {
        IEnumerable<string> Messages { get; }
        void AddMessage(string message);
    }
}
