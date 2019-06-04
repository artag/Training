# Конфигурирование приложения
Данные конфигурации помимо класса `Startup` (см. главу 14) можно грузить из:
* Переменные среды
* Аргументы командной строки
* Файлы формата JSON

В классе `Program` в конфигурации по умолчанию обычно включены.
В данном примере `Program` находится в "укороченной" версии и надо добавить следующее
(в `WebHostBuilder`):
```cs
...
.ConfigureAppConfiguration(
    (hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        config.AddEnvironmentVariables();
        if (args != null)
        {
            config.AddCommandLine(args);
        }
    })
...
```

`ConfigureAppConfiguration()` применяется для обработки данных конфигурации.
Его аргументы: `WebHostBuilderContext` и `IConfigurationBuilder`.


## WebHostBuilderContext
Определены свойства:
* `HostingEnvironment` - возвращает объект, реализующий `IHostingEnvironment`. Предоставляет
информацию о среде размещения (см. главу 14).

* `Configuration` - возвращает объект, реализующий `IConfiguration`. Предоставляет доступ по чтению
данных конфигурации.


## IConfigurationBuilder
Определены свойства:
* `AddJsonFile()` - загрузка данных конфигурации из файла JSON (`appsettings.json`).
* `AddEnvironmentVariables()` - загрузка данных конфигурации из переменных среды.
* `AddCommandLine()` - загрузка данных конфигурации из аргументов командной строки.

Для `AddJsonFile()`:
```
config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
```
Указывается:
* Имя файла
* Является ли файл необязательным
* Должны ли данные конфигурации загружаться повторно, если файл изменяется

*Внимание*: Не рекомендуется динамически редактировать конфигурационные файлы в производственных
системах.


## Пример конфигурационного файла JSON
Пример задания конфигурационных данных см. в файле `appsettings.json`.
Пример чтения конфигурационных данных см. в `Startup`.


### Члены, определяемые `IConfiguration`
* `[key]` - индексатор для получения строкового значения, соответствующего указанному ключу `key`.
* `GetSection(name)` - возвращает объект `IConfiguration`, который представляет раздел данных
конфигурации.
* `GetChildren()` - возвращает перечисление объектов `IConfiguration`, каждый из которых
представляет раздел данных.


### Расширяющие методы для `IConfiguration`
* `GetValue<T>(keyName)` - получает значение, ассоциированное с ключом `` и пытается преобразовать
его тип `T`.
* `GetValue<T>(keyName, defaultValue)` - получает значение, ассоциированное с ключом `` и пытается
преобразовать его тип `T`. Если значение для ключа отсутствует, то используется стандартное значение
`defaultValue`.
