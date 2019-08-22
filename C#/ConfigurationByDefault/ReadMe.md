# Стандартный механизм конфигурации от Microsoft

*Источник: http://ts-soft.ru/blog/new-standard-net-libs-part-2*

Nuget пакеты:
* **Microsoft.Extensions.Configuration** - для чтения/записи настроек в config

* **Microsoft.Extensions.Configuration.Binder** - для чтения настроек в POCO-объект.
(Ключи конфига нечуствительны к регистру, «user:name» эквивалентно «User:Name»).

* **Microsoft.Extensions.Configuration.Json** - для чтения настроек из JSON-файла.

* **Microsoft.Extensions.Configuration.CommandLine** - для чтения настроек из аргументов
в командной строке.

* **Microsoft.Extensions.Configuration.EnvironmentVariables** - для чтения настроек из переменных
среды.


Смотреть методы:
* `CreateMemoryConfig` и `DisplayValuesFromMemoryConfig` - Чтение/запись настроек в config в памяти.

* `MapConfigToPocoObject(IConfiguration config)` - Чтение настроек в POCO-объект из памяти.

* `CreateJsonConfig()` - Загрузка настроек из JSON файла.
Сам файл находится в `\ConfigurationFiles\setting.json`. Кидать в директорию с исполняемым файлом.

* `CreateMemoryConfigAndReplaceByJsonData()` - Чтение настроек из памяти, их частичная замена
данными из JSON файла.

* `CreateCmdArgsConfig(string[] args)` - Загрузка настроек (не все поля) из аргументов командной
строки. Агрументы командной строки примерно такого вида:
```
"/App:Name=AppNameFromCmdArgs /User:Name=UserNameFromCmdArgs"
```

* `CreateMemoryConfigAndReplaceByCmdArgsData(string[] args)` - Чтение настроек из памяти,
их частичная замена данными из аргументов командной строки.

* `CreateEnvConfig()` - Загрузка настроек (не все поля) из переменных среды.
Переменные среды могут выглядеть примерно так:
```
"MyPrefix_User:Name": "UserNameFromEnv",
"MyPrefix_App:Name": "AppNameFromEnv"
```

* `CreateMemoryConfigAndReplaceByEnvData()` - Чтение настроек из памяти, их частичная замена
данными из переменных среды.
