# Module 3. Applying migrations and tweaking the data model

## Lesson 11. Your first database migration

Migrations позволяют перенести все извменения из Model на структуру БД.
В Visual Studio управление миграциями выполняется в Package Manager Console.

### Настройки перед созданием migration

В Package Manager Console:

* Default Project -> Model (проект, где лежат модели БД, и где EFCore будет хранить Migration files).

В Solution Explorer:

* efdemo -> Set as Startup project (запускаемый проект, который будет применять migrations).

### Создание migration

Создание первой migration. В Package Manager Console:

```text
add-migration initial
```

* `add-migration` - команда
* `initial` - имя миграции

Создастся файл migration с именем `initial`, который наследует класс `Migration`.
Содержит два метода: `Up` и `Down`.

В `Up` методе содержатся действия, которые будут производиться с БД.
В примере показано, что в БД создается новая таблица, согласно модели.

Метод `Down` отменяет действие `Up`. Позволяет откатить migration. В примере показано, что в БД
удаляет таблицу из БД.

#### Создание migration из VS Code (.NET Core CLI)

1. Installing the tools
`dotnet ef` can be installed as either a global or local tool.
Most developers prefer installing dotnet ef as a global tool using the following command:

```text
dotnet tool install --global dotnet-ef
```

2. Update the tool using the following command:

```text
dotnet tool update --global dotnet-ef
```

3. Verify installation

```text
dotnet ef
```

4. Создание migration. Запуск из директории, где лежит файл *.sln.

```text
dotnet ef migrations add Initial -s efdemo/efdemo.csproj -p Model/Model.csproj
```

* `Initial` - имя миграции
* `-s efdemo/efdemo.csproj` - The startup project to use. Defaults to the current working directory.
* `-p Model/Model.csproj` - The project to use. Defaults to the current working directory.
(Именно здесь и будет создана migration).

### Выполнение migration (применение migration к БД)

В Package Manager Console:

```text
update-database --verbose
```

* `-v` или `--verbose` - необязательный параметр, позволяет увидеть более подробную информацию о том, что
происходит при выполнении команды.

#### Выполнение migration из VS Code (.NET Core CLI)

Запуск из startup project. В моем примере это проект efdemo.

```text
dotnet ef database update -v
```

Для SQLite файл БД `EFExpenseDemo.db` будет создан прямо в директории проекта efdemo.
