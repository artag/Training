# Module 4. Working with multiple models - Navigation properties

## Lesson 20. Multiple Model Introduction

* Models can be connected in multiple ways (one to many, many to many etc.)
* Use navigation properties to connect multiple entities
* Connect an entity to another more than one time

## Lesson 21. Adding Navigation properties

Сделаем navigation property - один `ExpenseHeader` может содержать несколько `ExpenseLine`.
(Получается соотношение *one-to-many*).

### Добавление свойств для реализации отношения one-to-many

1. В `ExpenseHeader` добавляется свойство-ссылка на коллекцию `ExpenseLine`:

```csharp
public class ExpenseHeader
{
    // ...

    // Navigation property.
    // One-to-many. Один ExpenseHeader содержит ссылки на множество ExpenseLine.
    public List<ExpenseLine> ExpenseLines { get; set; }
}
```

2. В `ExpenseLine` добавляется свойство-ссылка только на один `ExpenseHeader`.
Дополнительно добавляется Foreign key из `ExpenseHeader`:

```csharp
public class ExpenseLine
{
    // ...

    // Foreign key to ExpenseHeader Id
    public int ExpenseHeaderId { get; set; }

    // Navigation property.
    // One-to-many. Один ExpenseHeader содержит ссылки на множество ExpenseLine.
    public ExpenseHeader ExpenseHeader { get; set; }
}
```

Рекомендуется добавлять navigation properties с обеих "сторон", как это было сделано чуть выше
(т.н. best practices). Но такое объявление зависимости не всегда может подходить (см. урок 23).

### Создание migration

Из Visual Studio, Package Manager Console:

```text
add-migration expenseheaderid
```

Или из VS Code. Запуск из директории, где лежит файл *.sln.

```text
dotnet ef migrations add ExpenseHeaderId -s efdemo/efdemo.csproj -p Model/Model.csproj
```

### Применение migration (update database)

Из Visual Studio, Package Manager Console:

```text
update-database
```

Или из VS Code. Запуск из startup project (проект efdemo):

```text
dotnet ef database update
```

### Изменения после migration. Что с таблицами в БД

* В таблице `ExpenseHeaders` нет никаких изменений.
* В таблице `ExpenseLines`.
  * Добавился столбец Foreign Key `ExpenseHeaderId`.
  * В Keys добавился Foreign Key (видно в видео, на MSSQL БД. В SQLite не видно).
  * В Constraints добавилось ограничение CASCADE DELETE (видно в видео, на MSSQL БД. В SQLite не видно).
  * В Indexes создан индекс (видно в видео, на MSSQL БД. В SQLite не видно).

CASCADE DELETE: Если `ExpenseHeader` будет удален, то он удалится вместе со всеми ссылающимися
на него `ExpenseLine`.

## Lesson 22. Entity Relationship definition

* **Dependent entity** - This is the entity that contains the foreign key properties. Sometimes
referred to as the "child" of the relationship. (Пример - `ExpenseLine`).
* **Principal (главная) entity** - This is the entity that contains the primary/alternate key
properties. Sometimes referred to as the 'parent' of the relationship. (Пример - `ExpenseHeader`).
* **Principal (главный) key** - The properties that uniquely identify the principal entity.
This may be the primary key or an alternate key. (Пример - `ExpenseHeader.Id`).
* **Foreign key** - The properties in the dependent entity that are used to store the principal
key values for the related (связанной) entity. (Пример - `ExpenseLine.ExpenseHeaderId`).
* **Navigation property** - a property defined on the principal and/or dependent entity that
  references the related entity.
  * **Collection navigation property** - A navigation property that contains references to
  many related entities. (Пример - `ExpenseHeader.ExpenseLines`).
  * **Reference navigation property** - A navigation property that holds a reference to a single
  related entity. (Пример - `ExpenseLine.ExpenseHeader`).
  * **Inverse navigation property** - When discussing a particular (определенный) navigation property,
  this term refers to the navigation property on the other end of the relationship.
  (Пример - `ExpenseLine.ExpenseHeader` is the inverse of `ExpenseHeader.ExpenseLine`).

## Lesson 23. Removing a navigation property

Что надо сделать, если не хочется запретить каскадное удаление (урок 21): когда при удалении
`ExpenseHeader` удаляются все ссылающиеся на него `ExpenseLine`ы.

1. Revert database. Сначала надо отменить изменения в БД, сделанные в последней мирации (урок 21).

```text
update-database renameexpenseline
```

renameexpenseline - наименование миграции, на которую мы хотим откатить изменения в БД.

Аналогично, для VS Code:

```text
dotnet ef database update RenameExpenseLine
```

2. Удалить последнюю миграцию. Из Visual Studio:

```text
remove-migration
```

или в VS Code (запуск из директории, где лежит *.sln файл):

```text
dotnet ef migrations remove -s efdemo/efdemo.csproj -p Model/Model.csproj
```

>### Варианты поведения записи в child table (dependent entity) при удалении записи из parent table (principal entity)
>* *Cascade* - удаление из parent table приводит к удалению записи(ей) в child table.
>* *No Action* - удаление из parent table не оказывает эффекта на записи в child table.
>* *Restrict* - нельзя удалить запись из parent table, предварительно не удалив записи из child table.
>* *SetDefault* - при удалении записи из parent table, устанавливает значения по умолчанию в Foreign Key записи(ей) в child table.
>* *SetNull*

### Запрет на каскадное удаление записи(ей) в child table (dependent entity)

*Решение* - надо убрать navigation property со стороны `ExpenseLine` (navigation property остается только
в `ExpenseHeader`):

```csharp
public class ExpenseLine
{
    // // Foreign key to ExpenseHeader Id
    // public int ExpenseHeaderId { get; set; }

    // // Navigation property.
    // // One-to-many. Один ExpenseHeader содержит ссылки на множество ExpenseLine.
    // public ExpenseHeader ExpenseHeader { get; set; }
}
```

2. Сделать новую миграцию.

В миграции будет также будет создана связь между `ExpenseHeader` и `ExpenseLine`, с теми же самыми
изменениями, как в миграции из урока 21, за одним исключением:
вместо `ReferentialAction.Cascade` (каскадного удаления) теперь `ReferentialAction.Restrict` удаление.

3. Применить эту миграцию к БД.

4. Все же, рекомендуется делать navigation property в `ExpenseLine`, как это было
сделано ранее, в уроке 21.
Просто удобно иметь ссылки в enities друг на друга: можно строить более "удобные" запросы к БД.

Итак делаем очередную migration:

```text
add-migration addedinversenavigation
```

или

```text
dotnet ef migrations add AddedInverseNavigation -s efdemo/efdemo.csproj -p Model/Model.csproj
```

В этой миграции выдается предупреждение:

```text
An operation was scaffolded that may result in the loss of data. Please review the migration for accuracy.
```

Надо всегда проверять целостность данных в БД после применения подобных миграций.

5. В файле миграции заменяем "руками" строку

```text
onDelete: ReferentialAction.Cascade
```

на строку

```text
onDelete: ReferentialAction.Restrict
```

6. Применяем миграцию к БД.

## Lesson 24. Multiple navigation properties

Допустим, есть два пользователя, один из которых requester, а другой approver.
Оба могут запрашивать один или несколько экземпляров `ExpenseHeader`.
Как организовать такое взаимодействие?

1. Создаем entity `User` (в проекте `Model`):

```csharp
public class User
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
```

И создаем соответствующую таблицу в `ApplicationDbContext`:

```csharp
public class ApplicationDbContext : DbContext
{
    // ..
    public DbSet<User> Users { get; set; }
}
```

2. Далее, как обычно, создание и применение миграции.

```text
add-migration adduser
или
dotnet ef migrations add AddUser -s efdemo/efdemo.csproj -p Model/Model.csproj
```

```text
update-database
или
dotnet ef database update
```

3. В `ExpenseHeader` добавляем navigation property для пользователя requester и для пользователя
approver:

```csharp
 public class ExpenseHeader
{
    // ..

    // Id пользователя, который будет запрашивать этот expense.
    public int RequesterId { get; set; }
    // Navigation property к пользователю, который будет запрашивать этот expense.
    public User Requester { get; set; }

    // Id пользователя, который будет approve (подтверждать) этот expense.
    public int ApproverId { get; set; }
    // Navigation property к пользователю, который будет approve этот expense.
    public User Approver { get; set; }
}
```

EF достаточно умен, чтобы привязать `RequesterId` к `Requester`, а `ApproverId` к `Approver`.
Но для непохожих имен можно использовать аннотацию `ForeignKey(name)`:

```csharp
public class ExpenseHeader
{
    // ..

    // Id пользователя, который будет запрашивать этот expense.
    [ForeignKey("Requester")]
    public int RequesterId { get; set; }

    // Navigation property к пользователю, который будет запрашивать этот expense.
    // Inverse property указывает на свойство User.RequesterExpenseHeaders для связывания.
    [InverseProperty("RequesterExpenseHeaders")]
    public User Requester { get; set; }


    // Id пользователя, который будет approve (подтверждать) этот expense.
    [ForeignKey("Approver")]
    public int ApproverId { get; set; }

    // Navigation property к пользователю, который будет approve этот expense.
    // Inverse property указывает на свойство User.ApproverExpenseHeaders для связывания.
    [InverseProperty("ApproverExpenseHeaders")]
    public User Approver { get; set; }

}
```

Рекомендуется помечать атрибутом `ForeignKey` для лучшей читаемости. Даже в таких очевидных случаях.

Атрибут `InverseProperty` устанавливает связь с navigation property в `User` entity.

4. Создание navigation property со стороны `User`. Для Requester и Approver:

```csharp
public class User
{
    // ..

    // Navigation property. Для одного/нескольких ExpenseHeader.
    public List<ExpenseHeader> RequesterExpenseHeaders { get; set; }

    // Navigation property. Для одного/нескольких ExpenseHeader.
    public List<ExpenseHeader> ApproverExpenseHeaders { get; set; }
}
```

5. Создание migration и обновление БД

```text
add-migration usertoexpenseheader
или
dotnet ef migrations add UserToExpenseHeader -s efdemo/efdemo.csproj -p Model/Model.csproj

update-database
или
dotnet ef database update -s efdemo/efdemo.csproj
```

При попытке обновления БД ничего не выйдет: проблема существовании constraint на каскадное удаление
полей `FK_ExpenseHeaders_Users_ApproverId` и `FK_ExpenseHeaders_Users_RequesterId`
и возникающей циклической зависимости. Решение - поправить эти constraint в файле миграции:

эти строки

```csharp
onDelete: ReferentialAction.Cascade
```

исправить на

```csharp
ReferentialAction.NoAction
```

Теперь обновление БД должно успешно завершиться.

У меня и еще одного чувака с курса БД все равно не желала обновляться:

I was getting the following error when trying to update my database after we added
the `InverseProperty` stuff to the `ExpenseHeader.cs` file.

The ALTER TABLE statement conflicted with the FOREIGN KEY constraint "FK_ExpenseHeaders_Users_ApproverId".
The conflict occurred in database `EfExpenseDemo`, table `dbo.Users`, column `UserId`.

To **resolve** this, go to the `ExpenseHeaders` table in the SQL Server Object Explorer
and right-click to View Data. Delete any records you have in there by highlighting
the row and pressing delete on your keyboard or right-click then Delete from your menu.

Если кратко, то надо удалить все записи из таблицы `ExpenseHeaders` в БД.
