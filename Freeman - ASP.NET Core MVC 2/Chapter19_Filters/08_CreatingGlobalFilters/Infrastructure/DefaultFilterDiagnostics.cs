using System.Collections.Generic;

namespace CreatingGlobalFilters.Infrastructure
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
