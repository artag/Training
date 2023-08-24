# Использование интерфейса командной строки `LibMan` с ASP.NET Core

После загрузки проекта обновить фреймворки при помощи команды `libman restore`.

## Установка

**Источник: [learn.microsoft.com](https://learn.microsoft.com/ru-ru/aspnet/core/client-side/libman/libman-cli?view=aspnetcore-7.0)**

Чтобы установить LibMan CLI, выполните следующую команду:

```text
dotnet tool install -g Microsoft.Web.LibraryManager.Cli
```

Глобальное средство .NET Core устанавливается из пакета NuGet
[Microsoft.Web.LibraryManager.Cli](https://www.nuget.org/packages/Microsoft.Web.LibraryManager.Cli/)

Чтобы установить LibMan CLI из определенного источника пакета NuGet, выполните следующую команду:

```text
dotnet tool install -g Microsoft.Web.LibraryManager.Cli --version 1.0.94-g606058a278 --add-source C:\Temp\
```

В предыдущем примере глобальное средство .NET Core устанавливается из файла
`C:\Temp\Microsoft.Web.LibraryManager.Cli.1.0.94-g606058a278.nupkg` на локальном компьютере Windows.

## Использование

После установки доступна команда `libman`.

Узнать версию: `libman --version`

Вывести список доступных команд: libman --help`

## Инициализация LibMan в проекте

* Перейдите в корневой каталог проекта.

* Выполните следующую команду:

```text
libman init
```

*Введите имя поставщика по умолчанию или нажмите клавишу Enter,
 чтобы использовать поставщик CDNJS по умолчанию. Допустимы следующие значения:

* **cdnjs** - добавление из репозитория `https://cdnjs.com`;
* **filesystem** - добавление с локального диска;
* **jsdelivr** - добавление из репозиория `https://jsdelivr.com`;
* **unpkg** - добавление из репозитория `https://unpkg.com`.

В корневой каталог проекта добавляется файл `libman.json`` со следующим содержимым:

```json
{
  "version": "1.0",
  "defaultProvider": "cdnjs",
  "libraries": []
}
```

## Добавление файлов библиотеки

Примеры установки библиотек.

### Пример 1

Чтобы установить файл `jquery.min.js` `jQuery` версии `3.2.1` в директорию
`wwwroot/scripts/jquery` с использованием поставщика `CDNJS`, выполните следующую команду:

```text
libman install jquery@3.2.1 --provider cdnjs --destination wwwroot/scripts/jquery --files jquery.min.js
```

Библиотека будет добавлена в директорию и будет сделана запись в `libman.json`.

### Пример 2

Чтобы установить файлы `calendar.js` и `calendar.css` из папки `C:\temp\contosoCalendar\`
с использованием поставщика файловой системы, выполните следующую команду:

```text
libman install C:\temp\contosoCalendar\ --provider filesystem --files calendar.js --files calendar.css
```

## Восстановление файлов библиотек

Команда `libman restore` устанавливает файлы библиотеки, определенные в файле `libman.json`.

```text
libman restore [--verbosity]
libman restore [-h|--help]
```

## Удаление файлов библиотек

Команда `libman clean` удаляет файлы библиотек, восстановленные ранее с помощью LibMan.
Папки, которые становятся пустыми после выполнения этой операции, удаляются.

```text
libman clean [--verbosity]
libman clean [-h|--help]
```

## Удаление файлов библиотек

Команда `libman uninstall` делает следующее:

* удаляет все файлы, связанные с указанной библиотекой, из назначения в файле `libman.json`;
* удаляет связанную конфигурацию библиотеки из файла `libman.json`.

В следующих случаях возникает ошибка:

* файла libman.json нет в корневом каталоге проекта;
* указанная библиотека не существует.

Если установлено несколько библиотек с одним и тем же именем,
вам будет предложено выбрать одну из них.

```text
libman uninstall <LIBRARY> [--verbosity]
libman uninstall [-h|--help]
```

### Пример

Для удаления `jQuery` можно выполнить любую из следующих команд:

```text
libman uninstall jquery
libman uninstall jquery@3.3.1
```

## Обновление версии библиотеки

Команда `libman update` обновляет библиотеку, установленную с помощью LibMan, до указанной версии.

```text
libman update <LIBRARY> [-pre] [--to] [--verbosity]
libman update [-h|--help]
```

### Примеры

* Чтобы обновить jQuery до последней версии, выполните следующую команду:

```text
libman update jquery
```

* Чтобы обновить jQuery до версии 3.3.1, выполните следующую команду:

```text
libman update jquery --to 3.3.1
```

* Чтобы обновить jQuery до последней предварительной версии, выполните следующую команду:

```text
libman update jquery -pre
```

## Управление кэшем библиотек

Команда `libman cache` управляет кэшем библиотек LibMan.
Поставщик `filesystem` не использует кэш библиотек.

```text
libman cache clean [<PROVIDER>] [--verbosity]
libman cache list [--files] [--libraries] [--verbosity]
libman cache [-h|--help]
```

### Примеры

* Чтобы просмотреть имена кэшируемых библиотек для каждого поставщика,
выполните одну из следующих команд:

```text
libman cache list
libman cache list --libraries
```

* Чтобы просмотреть имена кэшируемых файлов библиотек для каждого поставщика,
выполните следующую команду:

```text
libman cache list --files
```

* Чтобы очистить кэш библиотек для поставщика `CDNJS`, выполните следующую команду:

```text
libman cache clean cdnjs
```

* Чтобы очистить кэш для всех поддерживаемых поставщиков, выполните следующую команду:

```text
libman cache clean
```
