using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Logging
{
    class Program
    {
        static void Main(string[] args)
        {
            LogInfo();
            LogUsingScopes();
            LogUsingFile();

            Console.ReadLine();
        }

        private static void LogInfo()
        {
            // Создание фабрики логов LoggerFactory.
            // В случае AddConsole передали лямбду, которая принимает строку сообщения и
            // уровень протоколирования и возвращает bool.
            // В AddDebug передали минимально возможный уровень.

            var loggerFactory = new LoggerFactory()
                .AddConsole((log, logLevel) => true)        // вывод на консоль
                .AddDebug(LogLevel.Debug);                  // вывод в отладочную консоль

            var logger = loggerFactory.CreateLogger<Program>();

            logger.LogTrace("Test trace");
            logger.LogDebug("Test debug");
            logger.LogInformation("Test info");
        }

        private static void LogUsingScopes()
        {
            // Странно, не получилось как в статье, чтобы Scope влияли на логгеры.

            var loggerFactory = new LoggerFactory()
                .AddConsole();

            var logger = loggerFactory.CreateLogger<Program>();

            logger.LogInformation("Not in scope");

            using (logger.BeginScope("outer"))
            {
                logger.LogInformation("On outer scope");
                using (logger.BeginScope("inner"))
                {
                    loggerFactory.CreateLogger(categoryName: "123").LogInformation("On inner scope another logger");
                    logger.LogInformation("On inner scope");
                }
            }
        }

        private static void LogUsingFile()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var filename = Path.Combine(currentDirectory, "log.txt");

            var loggerFactory = new LoggerFactory()
                .AddFile(filename);

            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation($"{DateTime.Now}: test info for file logger.");
        }
    }
}
