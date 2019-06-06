# Создание разных внешних конфигурационных файлов
См. пример в `Program.BuildWebHost()`:
```cs
...
.ConfigureAppConfiguration(
    (hostingContext, config) =>
    {
        var env = hostingContext.HostingEnvironment;

        var configPath1 = "appsettings.json";
        var configPath2 = $"appsettings.{env.EnvironmentName}.json";

        config.AddJsonFile(configPath1, optional: true, reloadOnChange: true)
              .AddJsonFile(configPath2, optional: true, reloadOnChange: true);

        config.AddEnvironmentVariables();
        if (args != null)
        {
            config.AddCommandLine(args);
        }
    })
...

```

При загрузке данных конфигурации из файла, специфичного для платформы, все настройки из него
переопределяют любые существующие данные с теми же самыми именами.

Например:
Файл `appsettings.development.json` переопределит настройки файла `appsettings.json`, если
приложение будет выполняться в среде разработки (Development).
