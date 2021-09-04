# Module 2. Starting with Entity Framework

## Lesson 4. Why Entity Framework

### What is persistence layer

```text
Application layer ---> Persistence Framework ---> Database
```

* A database is used to persist data over time.
* A persistence layer handles reading data and writing data to the database.
* Separates business logic from data access.

### Traditional method of handling persistence

* Lots of Stored Procedures
  * Наиболее быстрый доступ
  * Для запросов используется SQL
  * Repetitive Plumbing code (повторяющийся проникающий(?) код)
* ADO.NET
  * A lot of repetitive boilerplate code

Для stored procedures и SQL запросов:

* Not easy to source control
  * Using third party tools

### Advantages of Entity Framework

* Removes the need for stored procedures and ADO.NET
* Work with database using C# code (на 99%)
* Provides better source control through code first migrations
* Helps map database tablets to objects in code. (ORM - object relational mapping system)
* Allows developers to focus more on business logic than plumbing (repetitive code)

Entity Framework немного медленнее по сравнению с хранимыми процедурами и прямыми SQL запросами
к БД, но для большинства приложений эта разница не столь заметна.

## Lesson 6. Create your first Entity Framework application

Пример использует ASP.NET Core Web Application, .NET Core SDK не ниже 3 версии.

* Имя solution: efdemo
* Тип проекта: Web Application (Model-View-Controller)

Команды для VS Code:

1. Создание solution. Название "efdemo":

```text
dotnet new sln -n efdemo
```

2. Создание проекта  ASP.NET Core Web Application (MVC). Имя проекта "efdemo":

```text
dotnet new mvc -n efdemo
```

3. Добавление проекта в solution:

```text
dotnet sln add efdemo/efdemo.csproj
```

4. Добавление проекта class library. Имя проекта "Model":

```text
dotnet new classlib -n Model
dotnet sln add Model/Model.csproj
```

Плюс, я поменял строку в `Model.csproj` с

```xml
<TargetFramework>net5.0</TargetFramework>
```

на

```xml
<TargetFramework>netstandard2.0</TargetFramework>
```

5. Добавление проекта "Model" как reference в проект "efdemo":

```text
dotnet add reference ../Model/Model.csproj
```

6. (Только для VS Code). Добавление конфигурации для build всего solution. Файл `.vscode/tasks.json`:

```json
{
    "version": "2.0.0",
    "tasks": [
        {
            "label": "build",
            "command": "dotnet",
            "type": "process",
            "args": [
                "build",
                "${workspaceFolder}/efdemo.sln"
            ],
            "problemMatcher": "$msCompile"
        }
    ]
}
```

### Создание модели для использования в БД

Работа в проекте `Model`. Создаем POCO class `ExpenseHeader`. *POCO* class - plain old CLR object.

Здесь название класса - это название таблицы в БД, а свойства - названия полей в таблице.
По соглашениям - название класса такого типа дается в единственном числе.

```csharp
public class ExpenseHeader
{
    // Имя Id - соглашение об наименовании Primary key в таблице.
    // EF - автоматически распознает это свойство как Primary key.
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime ExpenseDate { get; set; }
}
```

## Lesson 7. Create a database context

Работа в проекте `Model`. Теперь надо создать database context - позволит "общаться" с БД.

### Добавление nuget пакетов

* Microsoft.EntityFrameworkCore - установка в проекты "efcore" и "Model". В видео версия 3.1.6.

Для VS Code. Я поставил не самую свежую версию 3.1.18, т.к. она поддерживает netstandard2.0:

```text
dotnet add package Microsoft.EntityFrameworkCore --version 3.1.18
```

### Добавление database context

Добавляется класс `ApplicationDbContext`. Рекомендуется оставлять окончание "Context" в названиях
такого рода классах. Добавленный класс расширяет `DbContext`:

```csharp
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ExpenseHeader> ExpenseHeaders { get; set
}
```

Конструктор принимает опции - в них задается провайдер к нужной БД и connectionString
(строка, описывающая настройки соединения).

`DbSet` описывает таблицы, которые будут созданы и к которым будет происходить соединение.

Название таблицы по соглашению принято назвать множественным числом от POCO class.

## Lesson 8. Setup your database configuration

### Добавление connectionString

Работа в проекте `efdemo`, файл `appsettings.json`. Добавлена секция для работы с локальной БД
MSSQL:

```json
"ConnectionStrings": {
      "DefaultConnection": "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=EFExpenseDemo;Integrated Security=True"
  },
```

Таких строк соединений в файле может быть несколько.

Т.к. я буду использовать SQLite, то мне сюда добавлять ничего не надо.

Зато мне надо добавить (в отличие от видео) строку в класс `ApplicationDbContext`,
в метод `OnConfiguring`:

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder options)
{
    options.UseSqlite("Filename=EFExpenseDemo.db");
}
```

### Добавление nuget пакетов - провайдеров для БД и tooling

* Microsoft.EntityFrameworkCore.SqlServer - установка в проекты "efcore" и "Model".
В видео версия 3.1.6. Для БД MSSql - в видео используется локальная БД - LocalDB.

* Microsoft.EntityFrameworkCore.Tools - установка в проекты "efcore" и "Model".
В видео версия 3.1.6.

Я вместо MSSQL буду использовать SQLite - мне вместо SqlServer надо поставить nuget:

* Microsoft.EntityFrameworkCore.Sqlite

```text
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 3.1.18
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 3.1.18
```

### Добавление в `Startup`

Работа в проекте `efdemo`, файл `Startup.cs`.
В метод `ConfigureServices`, для MSSQL добавляется:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // ...
    var dbConnectionString = Configuration.GetConnectionString("DefaultConnection");
    services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer());
}
```

Мне, для SQLite надо добавить:

```cs
public void ConfigureServices(IServiceCollection services)
{
    // ..
    services.AddEntityFrameworkSqlite().AddDbContext<ApplicationDbContext>()
}
```
