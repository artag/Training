using System;
using System.IO;
using System.Linq;

namespace Immutability
{
    /// <summary>
    /// Связующее звено между <see cref="AuditManager"/> и <see cref="Persister"/>.
    /// </summary>
    /// <remarks>
    /// 1. Как можно более простой класс.
    /// 2. Тестируется в Integration Tests (примерно 1-2 теста).
    /// </remarks>
    public class ApplicationService
    {
        private readonly string _directoryName;
        private readonly AuditManager _auditManager;
        private readonly Persister _persister;

        public ApplicationService(string directoryName)
        {
            _directoryName = directoryName;
        }

        public void RemoveMentionsAbout(string visitorName)
        {
            var files = _persister.ReadDirectory(_directoryName);
            var actions = _auditManager.RemoveMentionsAbout(visitorName, files);
            _persister.ApplyChanges(actions);
        }

        public void AddRecord(string visitorName, DateTime timeOfVisit)
        {
            var fileInfo = new DirectoryInfo(_directoryName)
                .GetFiles()
                .OrderByDescending(f => f.LastWriteTime)
                .First();

            var file = _persister.ReadFile(fileInfo.Name);
            var action = _auditManager.AddRecord(file, visitorName, timeOfVisit);
            _persister.ApplyChange(action);
        }
    }
}
