using System.Collections.Generic;

namespace ResolvingFilterDependencies.Infrastructure
{
    public class DefaultFilterDiagnostics : IFilterDiagnostics
    {
        private List<string> _messages = new List<string>();

        public IEnumerable<string> Messages => _messages;

        public void AddMessage(string message)
        {
            _messages.Add(message);
        }
    }
}
