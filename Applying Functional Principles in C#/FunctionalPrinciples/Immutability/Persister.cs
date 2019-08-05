using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Immutability
{
    /// <summary>
    /// Взаимодействие с файловой системой. Mutable.
    /// </summary>
    /// <remarks>
    /// 1. Как можно более простой класс.
    /// 2. Тестируется в Integration Tests (примерно 1-2 теста).
    /// </remarks>
    public class Persister
    {
        public FileContent ReadFile(string fileName)
        {
            return new FileContent(fileName, File.ReadAllLines(fileName));
        }

        public FileContent[] ReadDirectory(string directoryName)
        {
            return Directory
                .GetFiles(directoryName)
                .Select(file => ReadFile(file))
                .ToArray();
        }

        public void ApplyChanges(IEnumerable<FileAction> actions)
        {
            foreach (var action in actions)
            {
                switch (action.Type)
                {
                    case ActionType.Create:
                    case ActionType.Update:
                        File.WriteAllLines(action.FileName, action.Content);
                        continue;

                    case ActionType.Delete:
                        File.Delete(action.FileName);
                        continue;

                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public void ApplyChange(FileAction action)
        {
            ApplyChanges(new[] { action });
        }
    }
}
