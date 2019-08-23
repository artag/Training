using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Logging
{
    class FileLogger : ILogger
    {
        private readonly string _filename;
        private object _lock = new object();

        public FileLogger(string filename)
        {
            _filename = filename;
        }

        /// <summary>Begins a logical operation scope.</summary>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>An IDisposable that ends the logical operation scope on dispose.</returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the given <paramref name="logLevel" /> is enabled.
        /// </summary>
        /// <param name="logLevel">level to be checked.</param>
        /// <returns><c>true</c> if enabled.</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;        // Доступен всегда
        }

        /// <summary>Writes a log entry.</summary>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">
        /// Function to create a <c>string</c> message of the <paramref name="state" /> and
        /// <paramref name="exception" />.
        /// </param>
        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (formatter == null)
            {
                return;
            }

            lock (_lock)
            {
                File.AppendAllText(_filename, formatter(state, exception) + Environment.NewLine);
            }
        }
    }
}
