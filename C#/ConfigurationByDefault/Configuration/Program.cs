using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;

namespace Configuration
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateMemoryConfig();

            // Чтение/запись настроек в config в памяти.
            var config = CreateMemoryConfig();
            DisplayValuesFromMemoryConfig(config);

            // Чтение настроек в POCO-объект из памяти.
            var pocoConfig = MapConfigToPocoObject(config);
            Helpers.DisplayValuesFromPoco(
                pocoConfig,
                "Чтение настроек в POCO-объект");

            // Загрузка настроек (не все поля) из JSON файла.
            var jsonConfig = CreateJsonConfig();
            pocoConfig = MapConfigToPocoObject(jsonConfig);
            Helpers.DisplayValuesFromPoco(
                pocoConfig,
                "Загрузка настроек (не все поля) из JSON файла");

            // Чтение настроек в POCO-объект из памяти.
            // Загрузка настроек (не все поля) из JSON файла.
            // Частичная перезапись данных по ключам.
            jsonConfig = CreateMemoryConfigAndReplaceByJsonData();
            pocoConfig = MapConfigToPocoObject(jsonConfig);
            Helpers.DisplayValuesFromPoco(
                pocoConfig,
                "Загрузка настроек из памяти и их частичная перезапись данными из JSON файла");

            // Загрузка настроек (не все поля) из аргументов командной строки.
            var cmdArgsConfig = CreateCmdArgsConfig(args);
            pocoConfig = MapConfigToPocoObject(cmdArgsConfig);
            Helpers.DisplayValuesFromPoco(
                pocoConfig,
                "Загрузка настроек (не все поля) из аргументов командной строки");

            // Чтение настроек в POCO-объект из памяти.
            // Загрузка настроек (не все поля) из аргументов командной строки.
            // Частичная перезапись данных по ключам.
            cmdArgsConfig = CreateMemoryConfigAndReplaceByCmdArgsData(args);
            pocoConfig = MapConfigToPocoObject(cmdArgsConfig);
            Helpers.DisplayValuesFromPoco(
                pocoConfig,
                "Загрузка настроек из памяти и их частичная перезапись аргументами командной строки");

            // Загрузка настроек (не все поля) из переменных среды.
            var envConfig = CreateEnvConfig();
            pocoConfig = MapConfigToPocoObject(envConfig);
            Helpers.DisplayValuesFromPoco(
                pocoConfig,
                "Загрузка настроек (не все поля) из переменных среды");

            // Чтение настроек в POCO-объект из памяти.
            // Загрузка настроек (не все поля) из переменных среды.
            // Частичная перезапись данных по ключам.
            envConfig = CreateMemoryConfigAndReplaceByEnvData();
            pocoConfig = MapConfigToPocoObject(envConfig);
            Helpers.DisplayValuesFromPoco(
                pocoConfig,
                "Загрузка настроек из памяти и их частичная перезапись данными из переменных среды");

            Console.WriteLine("\nPress \"Enter\" to quit.");
            Console.ReadLine();
        }

        private static IConfigurationRoot CreateMemoryConfig()
        {
            return new ConfigurationBuilder()
                .Add(new MemoryConfigurationSource
                {
                    InitialData = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("App:Name", "MySuperApp"),
                        new KeyValuePair<string, string>("App:BuildDate", "2017-07-13T10:02:02"),
                        new KeyValuePair<string, string>("User:Name", "Julious"),
                        new KeyValuePair<string, string>("User:BirthDate", "2017-07-12T11:00:00"),
                    }
                })
                .Build();
        }

        private static void DisplayValuesFromMemoryConfig(IConfiguration config)
        {
            var appName = config.GetSection("App")["Name"];
            var userName = config["User:Name"];

            Console.WriteLine($"App Name: {appName}");                  // MySuperApp
            Console.WriteLine($"User Name: {userName}");                // Julious

            var unknownValue1 = config.GetSection("Site")["Url"];
            var unknownValue2 = config["Id:Unknown"];

            Console.WriteLine($"Unknown value 1: {unknownValue1}");     // Ничего не выведет (unknownValue1 == null)
            Console.WriteLine($"Unknown value 2: {unknownValue2}");     // Ничего не выведет (unknownValue2 == null)
            Console.WriteLine("----------------------------------------------------\n");
        }

        private static AppConfiguration MapConfigToPocoObject(IConfiguration config)
        {
            return config.Get<AppConfiguration>();
        }

        private static IConfiguration CreateJsonConfig()
        {
            // SetBasePath - устанавливает, где искать файлы, если пути относительные.
            // Флаг optional - если false и файл не найден, то генерируется исключение.

            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json", optional: true)
                .Build();
        }

        private static IConfiguration CreateMemoryConfigAndReplaceByJsonData()
        {
            var inMemoryConfig = Helpers.CreateInMemorySettings();

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfig)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json")
                .Build();
        }

        private static IConfiguration CreateCmdArgsConfig(string[] args)
        {
            // Аргументы будут примерно такие (со слешами и без пробелов):
            // /App:Name=AppNameFromCmdArgs /User:Name=UserNameFromCmdArgs

            return new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();
        }

        private static IConfiguration CreateMemoryConfigAndReplaceByCmdArgsData(string[] args)
        {
            var inMemoryConfig = Helpers.CreateInMemorySettings();

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfig)
                .AddCommandLine(args)
                .Build();

        }

        private static IConfiguration CreateEnvConfig()
        {
            // Указание prefix ("MyPrefix_") для имен переменных среды опционально.

            return new ConfigurationBuilder()
                .AddEnvironmentVariables("MyPrefix_")
                .Build();
        }

        private static IConfiguration CreateMemoryConfigAndReplaceByEnvData()
        {
            var inMemoryConfig = Helpers.CreateInMemorySettings();

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfig)
                .AddEnvironmentVariables("MyPrefix_")
                .Build();

        }
    }
}
