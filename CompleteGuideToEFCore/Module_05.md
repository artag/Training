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

## Lesson 27. Computing string columns with the fluent API

Пример. Для entity пользователя `User` необходимо сделать поле, которое является "суммой" его
свойств `FirstName` и `LastName`.

1. Назовем это свойство `FullName`:

```csharp
public class User
{
    // ..

    // (Было определено ранее)
    public string FirstName { get; set; }

    // (Было определено ранее)
    public string LastName { get; set; }

    // Вычисляемое свойство
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public string FullName { get; set; }
}
```

2. В `ApplicationDbContext`, в метод `OnModelCreating` необходимо добавить:

```csharp
public class ApplicationDbContext : DbContext
{
    // ..

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ..

        builder.Entity<User>()
            .Property(p => p.FullName)
            .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");
    }
```

Инструкции для fluent API можно определять в любом порядке.

3. Опять создание migration и обновление БД:

```text
add-migration fullname
или
dotnet ef migrations add FullName -s efdemo/efdemo.csproj -p Model/Model.csproj

update-database
или
dotnet ef database update -s efdemo/efdemo.csproj
```

### Возможные проблемы с вычисляемыми свойствами в SQLite

У меня SQLite на Linux не воспринял последние изменения.
Значения в столбце `FullName` "не вычисляются" - пишется значение 0.

## Lesson 28. Setting data types using the fluent API

Можно настраивать поля в entity не только при помощи аннотаций, но и при помощи fluent API.

Например, добавим поле `UsdExchangeRate` в entity `ExpenseHeader`:

```csharp
public class ExpenseHeader
{
    // ..

    public decimal UsdExchangeRate { get; set; }
}
```

А вместо аннотаций можно добавить строки конфигурации в `ApplicationDbContext`,
метод `OnModelCreating`:

```csharp
public class ApplicationDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // ..

        builder.Entity<ExpenseHeader>()
            .Property(e => e.UsdExchangeRate)
            .HasColumnType("decimal(13,4)")
            .IsRequired(true);
        }
    }
}
```

Эти два способа конфигурирования полей entity с точки зрения получения итогового результата
одинаковы.

Как обычно, добавление миграции и обновление БД:

```text
add-migration exchangerate
или
dotnet ef migrations add ExchangeRate -s efdemo/efdemo.csproj -p Model/Model.csproj

update-database
или
dotnet ef database update -s efdemo/efdemo.csproj
```

## Lesson 29. One to many relationships

Здесь будет показано как задать отношение one-to-many, используя fluent API.

Пример, который был реализован выше через Data annotations. Один `User` может быть связан
с несколькими `ExpenseHeader`:

```csharp

public class ExpenseHeader
{
    // ..

    // Id пользователя, который будет запрашивать этот expense.
    public int RequesterId { get; set; }

    // Navigation property к пользователю, который будет запрашивать этот expense.
    public User Requester { get; set; }
}

public class User
{
    // ..

    // Navigation property. Для одного/нескольких ExpenseHeader.
    public List<ExpenseHeader> RequesterExpenseHeaders { get; set; }

    // Navigation property. Для одного/нескольких ExpenseHeader.
    public List<ExpenseHeader> ApproverExpenseHeaders { get; set; }
}
```

Связь через fluent API описывается так:

```csharp
public class ApplicationDbContext : DbContext
{
    // ..

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // ..

        builder.Entity<ExpenseHeader>()
            .HasOne(e => e.Requester)
            .WithMany(e => e.RequesterExpenseHeaders)
            .HasForeignKey(e => e.RequesterId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(true);
        }
    }
```

Добавление миграции и обновление БД:

```text
add-migration fluentapirequester
или
dotnet ef migrations add FluentApiRequester -s efdemo/efdemo.csproj -p Model/Model.csproj

update-database
или
dotnet ef database update -s efdemo/efdemo.csproj
```

Из кода миграции видно, что настройки из fluent API перезаписывают аннотации в entities.
*(Мое примечание - а если вначале применить на БД настройки из fluent API,*
*а потом data annotations, что в итоге получится?)*

## Lesson 30. Using separate fluent API configuration files

Работа идет в проекте `Model`.

При наличии множества entity и использовании конфигурирования через fluent API, метод
`OnModelCreating` в `ApplicationDbContext` очень быстро превращается в трудночитаемый.

Существует способ избавиться от такого беспорядка - разнести конфигурацию по нескольким файлам.

Пример. Вынесем конфигурацию для entity `User` из `ApplicationDbContext.OnModelCreating`
в отдельный файл `UserConfigiration` (папка EntityConfigurations).

```csharp
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FullName)
            .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");
    }
}
```

Класс для конфигурирования должен реализовывать `IEntityTypeConfiguration<T>`, где `T` - это
конфигурируемая entity.

Изменения в `ApplicationDbContext`, методе `OnModelCreating`:

```csharp
public class ApplicationDbContext : DbContext
{
    // ..

    protected override void OnModelCreating(ModelBuilder builder)
    {

            // Конфигурация перенесена в класс "UserConfiguration".
            // builder.Entity<User>()
            //     .Property(p => p.FullName)
            //     .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");

            // Применение файла конфигурации "UserConfiguration".
            builder.ApplyConfiguration(new UserConfiguration());
    }
}
```

Если создать migration, то там не будет никаких изменений - перенос конфигурации в отдельный
файл не влияет на настройки entity и БД.

## Lesson 31. Pulling fluent API configuration information from the assembly

Работа идет в проекте `Model`.

Можно выносить описания/конфигурации entity в отдельные файлы, как это было показано на предущем
уроке (lesson 30). Но всегда существует вероятность забыть применить один из файлов конфигурации
в методе `OnModelCreating`.

Существует способ применить все объекты типа `IEntityTypeConfiguration<T>` разом путем
объявления их загрузки из сборки:

```csharp
public class ApplicationDbContext : DbContext
{
    // ..

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Эта строка применит все объекты типа `IEntityTypeConfiguration<T>`,
        // существующие в текущей сборке.
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Применение файла конфигурации "UserConfiguration".
        // Заменено строкой выше: builder.ApplyConfigurationsFromAssembly(...)
        // builder.ApplyConfiguration(new UserConfiguration());
    }
}
```

Если создать migration, то по сравнению с предыдущим состоянием не будет никаких изменений.

## Lesson 32. Generating default dates using the fluent API

Здесь описывается как сделать автоматическую подстановку текущей даты в БД,
например при добавлении и/или изменении какого-либо поля с помощью fluent API.

Это хорошая практика, когда требуется отслеживать изменения в БД.

1. Добавление двух свойств типа `DateTime` в entity `User`:

```csharp
public class User
{
    // ..

    // Добавленные поля для tracking changes in the database.
    public DateTime CreatedDate { get; set; }
    public DateTime LastModified { get; set; }
}
```

2. Добавление настроек fluent API в файл конфигурации `UserConfiguration`:

```csharp
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // ..

        // Генерация даты при создании новой записи в таблице. Для MSSQL.
        // Для SQLite не получилось сделать.
        builder.Property(u => u.CreatedDate).ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETUTCDATE()");

        // Генерация даты при добавлении/обновлении записи в таблице. Для MSSQL.
        // Для SQLite не получилось сделать.
        builder.Property(u => u.LastModified).ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
```

`HasDefaultValueSql` задает значение по умолчанию, которое будет создано для свойства.
В качестве аргумента здесь передается SQL-инструкция в виде строки.

3. Добавление миграции и обновление БД:

```text
add-migration createddate
или
dotnet ef migrations add CreatedDate -s efdemo/efdemo.csproj -p Model/Model.csproj

update-database
или
dotnet ef database update -s efdemo/efdemo.csproj
```

В видео было показано, что столбцы `CreatedDate` и `LastModified` правильно создаются при добавлении
новой записи, но при обновлении записи никакого изменения в `LastModified` не наблюдается.
Автор пообещал, что далее в лекциях эта проблема будет решена.
