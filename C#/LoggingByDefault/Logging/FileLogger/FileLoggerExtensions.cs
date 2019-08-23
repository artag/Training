using Microsoft.Extensions.Logging;

namespace Logging
{
    static class FileLoggerExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory, string filename)
        {
            factory.AddProvider(new FileLoggerProvider(filename));
            return factory;
        }
    }
}
