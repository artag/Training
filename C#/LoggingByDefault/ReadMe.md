# Стандартный механизм логирования от Microsoft

*Источники:*

*http://ts-soft.ru/blog/new-standard-net-libs-part-3* (все, кроме использования `NLog`)

*https://metanit.com/sharp/aspnet5/2.10.php* (только создание провайдера логгирования)


Nuget пакеты:

* **Microsoft.Extensions.Logging**

* **Microsoft.Extensions.Logging.Console** (опционально) - provider package sends log output to
the console.

* **Microsoft.Extensions.Logging.Debug** (опционально) - provider package writes log output by
using the `System.Diagnostics.Debug` class (`Debug.WriteLine` method calls).


Смотреть методы:
* `LogInfo()` - вывод логов (различные уровни логирования) на консоль и отладочую консоль.

* `LogUsingScopes()` - попытка (неудачная) воспроизвести влияние Scope на логгеры.


Смотреть файлы в директории `FileLogger` - добавление провайдера для записи логов в файл.

## Уровни логирования:

* Trace = 0
For information that's typically valuable only for debugging. These messages may contain sensitive 
application data and so shouldn't be enabled in a production environment. Disabled by default.

* Debug = 1
For information that may be useful in development and debugging. Example: Entering method Configure
with flag set to true. Enable Debug level logs in production only when troubleshooting,
due to the high volume of logs.

* Information = 2
For tracking the general flow of the app. These logs typically have some long-term value.
Example: Request received for path /api/todo

* Warning = 3
For abnormal or unexpected events in the app flow. These may include errors or other conditions
that don't cause the app to stop but might need to be investigated.
Handled exceptions are a common place to use the Warning log level.
Example: FileNotFoundException for file quotes.txt.

* Error = 4
For errors and exceptions that cannot be handled. These messages indicate a failure in the current
activity or operation (such as the current HTTP request), not an app-wide failure.
Example log message: Cannot insert record due to duplicate key violation.

* Critical = 5
For failures that require immediate attention. Examples: data loss scenarios, out of disk space.


## Некоторые методы `ConsoleLogger`

Для вывода соответствующего уровня информации определены соответствующие методы расширения:

* `LogDebug()`
* `LogTrace()`
* `LogInformation()`
* `LogWarning()`
* `LogError()`
* `LogCritical()`