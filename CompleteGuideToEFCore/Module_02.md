# Module 2. Starting with Entity Framework

## Lesson 4. Why Entity Framework

### What is persistence layer.

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
