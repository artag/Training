# Конфигурирование регистрации в журнале
Пример конфигурирования см. в `Program.BuildWebHost()`:
```cs
...
.ConfigureLogging(
    (hostingContext, logging) =>
    {
        logging.AddConfiguration(
            hostingContext.Configuration.GetSection("Logging"));
        logging.AddConsole();
        logging.AddDebug();
    })
...
```

`ConfigureLogging()` - настраивает систему логирования.
Два аргумента: `WebHostBuilderContext` и объект `ILoggingBuilder`.

Методы для `ILoggingBuilder`:
* `AddConfiguration()` - конфигурация логирования с данными, полученными либо из `appsettings.json`,
либо из командной строки, либо из переменных среды.
* `AddConsole()` - отправляет логи на консоль (удобно при запуске приложения через `dotnet run`).
* `AddDebug()` - отправляет логи в окно `Debug` (когда используется отладчик Visual Studio).
* `AddEventLog()` - отправляет логи в журнал событий Windows.


## Подробности о AddConfiguration()
См. пример файла `appsettings.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  }
}
```

`Logging` - раздел конфигурации.
`Default` - элемент, который устанавливает порог для отображения логов. Будут отбражаться сообщения
с уровнем `Debug` и выше.
`System` - элемент, который устанавливает порог для отображения логов в пространстве имен System.
`Microsoft` - элемент, который устанавливает порог для отображения логов в пространстве имен Microsoft.

Элементы `System` и `Microsoft` переопределяют элемент `Default` - в этих пространствах имен будут
отбражаться сообщения с уровнем `Information` и выше.


### Уровни отладочной информации ASP.NET
* `Trace` - сообщения, полезные только в процессе разработки.
* `Debug` - детализированные соообщения, для разработчиков. Используются при отладке.
* `Information` - описывают общее функционирования приложения.
* `Warning` - сообщения о непредвиденных событиях, которые не приводят к вылету приложения.
* `Error` - сообщения об ошибках событиях, которые приводят к вылету приложения.
* `Critical` - сообщения о катастрофических отказах.
* `None` - отключение регистрации журнальных сообщений.


## Создание специальных журнальных сообщений
Пример создания журнальной записи из `Controllers/HomeController.cs`:
```cs
...
_logger.LogDebug($"Handled {Request.Path} at uptime {_uptimeService.Uptime}");
...
```

В кострукторе контроллера, один из параметров:
`ILogger` определяет функциональность, требующуюся для создания журнальных записей.
`ILogger<HomeController>` - параметр типа позволяет использовать имя класса в журнальных сообщениях.

Предусмотрены методы создания каждого из уровней отладочной информации ASP.NET:
* `LogTrace()`
* `LogDebug()`
* `LogInformation()`
* `LogWarning()`
* `LogError()`
* `LogCritical()`
