# Module 5. Manipulating the database using the fluent API

## Lesson 26. Managing your database with the fluent API

EF позволяет более точно настраивать характеристики создаваемых сущностей. Даже более точно,
чем с аннотациями.

Это можно сделать здесь:

```csharp
public class ApplicationDbContext : DbContext
{
    // В этом методе можно настроить поведение всех описываемых entity
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ..
    }
```

### Генерация значений для свойств при помощи БД

В entity `ExpenseLine` было добавлено вычисляемое свойство:

```csharp
public class ExpenseLine
{
    // ..

    // (Было определено раньше)
    [Range(1, 10, ErrorMessage = "{0} must be between 1 and 10")]
    public int Quantity { get; set; }

    // (Было определено раньше)
    [Column(TypeName = "decimal(16, 2)")]
    [Range(0.01, 100.0, ErrorMessage = "Unit Cost must be between 0.01 and 100.00")]
    public decimal UnitCost { get; set; }

    // Вычисляемое значение. Quantity * UnitCost
    [Column(TypeName = "decimal(16, 2)")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public decimal TotalCost { get; set; }
}
```

Атрибут `DatabaseGenerated` определяет/задает как БД генерирует значение для свойства.
Этот атрибут может принимать следующие значения:

* `DatabaseGeneratedOption.Identity` - БД генерирует значение, когда вставляется строка.
Генерация значений для столбцов типа identity.
* `DatabaseGeneratedOption.Computed` - БД генерирует значение, когда строка вставляется или удаляется.
* `DatabaseGeneratedOption.None` - БД не генерирует значение (поведение по умолчанию).

Вычисление значения для свойства происходит во время создания `DbContext`, в методе `OnModelCreating`.
Пример задания вычисляемого значения для свойства `TotalCost`:

```csharp
public class ApplicationDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ExpenseLine>()
            .Property(e => e.TotalCost)
            .HasComputedColumnSql("[Quantity] * [UnitCost]");
    }
}
```

### Создание migration и обновление БД

```text
add-migration totalcost
или
dotnet ef migrations add TotalCost -s efdemo/efdemo.csproj -p Model/Model.csproj

update-database
или
dotnet ef database update -s efdemo/efdemo.csproj
```

### Возможные проблемы с SQL browser

У меня в Linux, DB Browser for SQLite не показывает вычисляемые поля. После применения последней
миграции в БД перестала показываться структура таблицы `ExpenseLines`.

Другой браузер SQLite из VS Code эту же таблицу смог отобразить.

Зато, после добавления записи в таблицу `ExpenseLines`, DB Browser for SQLite опять стал ее
отображать.
