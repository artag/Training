using System;
using Microsoft.Extensions.Logging;

namespace Logging
{
    class FileLoggerProvider : ILoggerProvider
    {
        private readonly string _filename;

        public FileLoggerProvider(string filename)
        {
            _filename = filename;
        }

        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_filename);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
