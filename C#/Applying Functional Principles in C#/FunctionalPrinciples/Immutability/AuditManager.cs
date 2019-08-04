using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Immutability
{
    public class AuditManager
    {
        private readonly int _maxEntriesPerFile;

        public AuditManager(int maxEntriesPerFile)
        {
            _maxEntriesPerFile = maxEntriesPerFile;
        }

        public FileAction AddRecord(FileContent currentFile, string visitorName, DateTime timeOfVisit)
        {
            var entries = Parse(currentFile.Content); 

            if (entries.Count < _maxEntriesPerFile)
            {
                entries.Add(new AuditEntry(entries.Count + 1, visitorName, timeOfVisit));
                var newContent = Serialize(entries);

                return new FileAction(currentFile.FileName, ActionType.Update, newContent);
            }
            else
            {
                var entry = new AuditEntry(1, visitorName, timeOfVisit);
                var newContent = Serialize(new[] {entry});
                var newFileName = GetNewFileName(currentFile.FileName);

                return new FileAction(newFileName, ActionType.Create, newContent);
            }
        }

        public IReadOnlyList<FileAction> RemoveMentionsAbout(string visitorName, FileContent[] directoryFiles)
        {
            return directoryFiles
                .Select(file => RemoveMentionsIn(file, visitorName))
                .ToList();

            ////foreach (var fileName in Directory.GetFiles(directoryName))
            ////{
            ////    var tempFile = Path.GetTempFileName();
            ////    var linesToKeep = File
            ////        .ReadLines(fileName)
            ////        .Where(line => !line.Contains(visitorName))
            ////        .ToList();

            ////    if (linesToKeep.Count == 0)
            ////    {
            ////        File.Delete(fileName);
            ////    }
            ////    else
            ////    {
            ////        File.WriteAllLines(tempFile, linesToKeep);
            ////        File.Delete(fileName);
            ////        File.Move(tempFile, fileName);
            ////    }
            ////}
        }

        private FileAction RemoveMentionsIn(FileContent file, string visitorName)
        {
            var entries = Parse(file.Content);

            var newContent = entries
                .Where(entry => entry.Visitor != visitorName)
                .Select((entry, index) => new AuditEntry(index + 1, entry.Visitor, entry.TimeOfVisit))
                .ToList();

            return new FileAction(file.FileName, ActionType.Update, Serialize(newContent));
        }

        private List<AuditEntry> Parse(IEnumerable<string> content)
        {
            var result = new List<AuditEntry>();

            foreach (var line in content)
            {
                var data = line.Split(";");
                result.Add(new AuditEntry(int.Parse(data[0]), data[1], DateTime.Parse(data[2])));
            }

            return result;
        }

        private string[] Serialize(IEnumerable<AuditEntry> entries)
        {
            return entries
                .Select(entry => entry.Number + ";" + entry.Visitor + ";" + entry.TimeOfVisit.ToString("s"))
                .ToArray();
        }

        private string GetNewFileName(string existingFileName)
        {
            var fileName = Path.GetFileNameWithoutExtension(existingFileName);
            var index = int.Parse(fileName.Split("_")[1]);
            return "Audit_" + (index + 1) + ".txt";
        }
    }
}
