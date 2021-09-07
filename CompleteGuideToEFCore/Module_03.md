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

*(Первые два этапа делаются только раз)*

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

## Lesson 12. Using a migration to modify your existing database tables

Что надо сделать если захотелось поменять что-то в модели?

### Изменение model

Работа в проекте `Model`. Например, меняем:

```csharp
public class ExpenseHeader
{
    // ..
    public DateTime ExpenseDate { get; set; }
}
```

на

```csharp
public DateTime? ExpenseDate { get; set; }
```

### Новая migration для изменения в model

Теперь надо сделать новую migration. В Package Manager Console:

1. Default Project -> Model
2. `efdemo` указан как Startup Project
3. Команда в Package Manager Console:

```text
add-migration expensedate-null
```

#### Новая migration из VS Code (.NET Core CLI)

Создание migration. Запуск из директории, где лежит файл *.sln.

```text
dotnet ef migrations add ExpenseDateNull -s efdemo/efdemo.csproj -p Model/Model.csproj
```

* `ExpenseDateNull` - имя миграции
* `-s efdemo/efdemo.csproj` - The startup project to use. Defaults to the current working directory.
* `-p Model/Model.csproj` - The project to use. Defaults to the current working directory.
(Именно здесь и будет создана migration).

### Применение migration к БД

В Package Manager Console:

```text
update-database
```

Та же команда, только в VS Code (.NET Core CLI). Запуск из корня запускаемого проекта `efdemo`:

```text
dotnet ef database update
```

#### Трудности с SQLite версии 3.x

Но, с SQLite миграция не прошла, в консоли вылетела ошибка:

```text
SQLite does not support this migration operation ('AlterColumnOperation') ...
```

Сайт ms сообщил, что операция 'AlterColumnOperation' для SQLite поддерживается начиная с EF Core
версии 5.

Поэтому:

* Все nuget-пакеты EF Core во всех проектах обновлены до последней версии 5.0.9
* Проект `Model` переведен на netstandard2.1, т.к. 5 версия не поддерживает netstandard2.0.

После этих манипуляции миграция прошла, БД обновилась.

## Lesson 13. Modifying a model with data annotations

Data annotations позволяют избирательно задать ограничения для полей таблицы.

1. Аннотации добавляются в POCO класс (проект `Model`, класс `ExpenseHeader`).
2. Используется namespace `System.ComponentModel.DataAnnotations`.

### Рассмотренные аннотации

* `Key` - говорит, что свойство явлюяется ключом. Для свойства с именем `Id` или `HeaderId`
задание данной аннотации необязательно - EF Core определит, что это ключ.
* `Required` - поле обязательно должно содержать какое-либо значение.
* `MaxLength(100)` - максимально допустимый размер данных в поле.
Например, максимальная длина строки - 100 символов.
* `MinLength(10)` - минимально допустимый размер данных в поле.

### Опять создание migration

Команды из Visual Studio, Package Manager Console:

```text
add-migration dataannotationexample
```

Или, для VS Code (запуск из директории, где лежит *.sln файл):

```text
dotnet ef migrations add DataAnnotationExample -s efdemo/efdemo.csproj -p Model/Model.csproj
```

## Lesson 14. Create custom error messages with data annotations

В аннотациях можно задать показ сообщений об ошибках пользователю (на Web Form и прочем),
если ограничение было нарушено.

Пример для аннотаций `Required` и `MaxLength` (проект `Model`, POCO класс `ExpenseHeader`):

```csharp
// ..
[Required(ErrorMessage = "{0} is required")]
[MaxLength(100, ErrorMessage = "{0} can not be more than 100 characters")]
public string Description { get; set; }
```

Для данного аттрибута вместо "{0}" будет поставлено наименование свойства "Description".

## Lesson 15. Removing data migrations

При добавлении migration для добавления error messages в БД видно, что ничего в такой migration
не будет происходить: методы `Up` и `Down` в созданном migration классе пустые.
Сообщения об ошибках из аннотаций сохраняются и показываются только на уровне приложения.

Такая миграция лишняя и хочется ее удалить. Просто удалить "руками" лишние созданные файлы миграции
нельзя, т.к. БД и модель данных может быстро рассинхронизоваться.

Надо удалить ненужную migration из консоли.

Команда из Visual Studio, Package Manager Console. Удаляет последнюю миграцию:

```text
remove-migration
```

Аналогично, для VS Code (запуск из директории, где лежит *.sln файл):

```text
dotnet ef migrations remove -s efdemo/efdemo.csproj -p Model/Model.csproj
```

## Lesson 16. Adding a second database table and overriding Entity Framework defaults

Добавление нового POCO класса для новой таблицы - проект `Model`, класс `ExpenseLine`.

### Еще аннотации

1. Аннотация для задания имени таблицы и имени столбца. Плюс, задание точности decimal числа.

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Таблица будет названа "ExpenseDetails"
[Table("ExpenseDetails")]
public class ExpenseLine
{
    public int ExpenseLineId { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }

    // "UnitPrice" - имя столбца в БД.
    // TypeName = "decimal(16, 2) - задание точности: 16 разрядов до запятой, 2 разряда после.
    [Column("UnitPrice", TypeName = "decimal(16, 2)")]
    public decimal UnitCost { get; set; }
}
```

### Попытка сделать migration

Если прямо попробовать сделать migration, то будет создана пустая миграция (методы `Up` и `Down` в
созданном migration классе будут пустыми).

Чтобы такого не было, необходимо добавить `DbSet` в `ApplicationDbContext`:

```csharp
public class ApplicationDbContext : DbContext
{
    // ..

    public DbSet<ExpenseLine> ExpenseLines { get; set; }
}
```

Тепеь миграция будет создана правильно. Для Visual Studio:

```text
add-migration addexpenselines
```

Или, тоже самое для VS Code (запуск из директории, где лежит *.sln файл):

```text
dotnet ef migrations add AddExpenseLines -s efdemo/efdemo.csproj -p Model/Model.csproj
```

Обновление БД как обычно. Для Visual Studio:

```text
update-database
```

Для VS Code (запуск из корня проекта `efdemo`)

```text
dotnet ef database update
```

### История миграций

Все примененные миграции расположены в БД, в таблице `__EFMigrationsHistory`.
