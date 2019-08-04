using System;
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

        public void AddRecord(string currentFile, string visitorName, DateTime timeOfVisit)
        {
            var lines = File.ReadAllLines(currentFile);

            if (lines.Length < _maxEntriesPerFile)
            {
                var lastIndex = int.Parse(lines.Last().Split(';')[0]);
                var newLine = (lastIndex + 1) + ';' + visitorName + ';' + timeOfVisit.ToString("s");
                File.AppendAllLines(currentFile, new[] { newLine });
            }
            else
            {
                var newLine = "1;" + visitorName + ";" + timeOfVisit.ToString("s");
                var newFileName = GetNewFileName(currentFile);
                File.WriteAllLines(newFileName, new[] { newLine });
            }
        }

        public void RemoveMentionsAbout(string visitorName, string directoryName)
        {
            foreach (var fileName in Directory.GetFiles(directoryName))
            {
                var tempFile = Path.GetTempFileName();
                var linesToKeep = File
                    .ReadLines(fileName)
                    .Where(line => !line.Contains(visitorName))
                    .ToList();

                if (linesToKeep.Count == 0)
                {
                    File.Delete(fileName);
                }
                else
                {
                    File.WriteAllLines(tempFile, linesToKeep);
                    File.Delete(fileName);
                    File.Move(tempFile, fileName);
                }
            }
        }

        private string GetNewFileName(string existingFileName)
        {
            var fileName = Path.GetFileNameWithoutExtension(existingFileName);
            var index = int.Parse(fileName.Split('_')[1]);
            return "Audit_" + (index + 1) + ".txt";
        }
    }
}
