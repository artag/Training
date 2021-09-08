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
