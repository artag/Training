# Module 7. Repository layer

## Lesson 39. What is a repository layer

### Why Repository Pattern

* Remove dependency on persistence framework (развязка от ORM и типа используемых БД)
* Remove query duplication
* Improve unit testing

| Repository should contain | Repository should not contain |
|---------------------------|-------------------------------|
| GetAll()                  | SaveChanges()                 |
| GetById()                 | Update(Object)                |
| Add(Object)               |                               |
| Remove(Object)            |                               |
|                           |                               |
| Etc.                      |                               |

Не понял, почему репозиторий не должен включать операции сохранения (SaveChanges) и обновления
(Update). Невнятное объяснение.

### Is Entity Framework a Repository Pattern

| DbSet = Repository | DbContext = Unit of work |
|--------------------|--------------------------|
| Where()            | SaveChanges()            |
| Add(Object)        |                          |
| Remove(Object)     |                          |
|                    |                          |
| Etc.               |                          |

По своей сути, Entity Framework является Repository Pattern, а DbContext представляет Unit of work.
Но такой Repository Pattern не избавляет от дублирования query, поэтому рекомендуется вводить
еще один промежуточный слой Repository layer.

## Lesson 40. Creating a generic repository interface

Создается еще один проект - class library `Repository` для реализации Repository layer.

Из консоли, из корня solution:

```text
dotnet new classlib -o Repository
dotnet sln add Repository/Repository.csproj 
```

В этом проекте создается generic интерфейс `IRepository<TEntity>`, который будет применим ко всем
сущностям приложения.

```csharp
public interface IRepository<TEntity> where TEntity : class
{
    IEnumerable<TEntity> GetAll();
    TEntity Get(int id);

    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
}
```

Некоторые вместо `IEnumerable` используют `IQueryable`. Но это не рекомендуется делать, т.к.
использование `IQueryable` приводит в более запутанному коду (так утверждает автор).

## Lesson 41. Creating the generic repository implementation details

1. В проект `Repository` надо установить nuget пакет "Microsoft.EntityFrameworkCore"
(у меня версия 5.0.9).

```text
dotnet add package Microsoft.EntityFrameworkCore --version 5.0.9
```

2. В проект `Repository` добавляем generic класс `Repository<TEntity>`, который будет реализацией
`IRepository<TEntity>`.

```csharp
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly DbContext Context;

    public Repository(DbContext context)
    {
        Context = context;
    }

    public IEnumerable<TEntity> GetAll() =>
        Context.Set<TEntity>();

    public TEntity Get(int id) =>
        Context.Set<TEntity>().Find(id);

    public void Add(TEntity entity) =>
        Context.Set<TEntity>().Add(entity);

    public void AddRange(IEnumerable<TEntity> entities) =>
        Context.Set<TEntity>().AddRange(entities);

    public void Remove(TEntity entity) =>
        Context.Set<TEntity>().Remove(entity);

    public void RemoveRange(IEnumerable<TEntity> entities) =>
        Context.Set<TEntity>().RemoveRange(entities);
}
```

В класс `Repository` инжектируется через конструктор `DbContext`. Таким образом класс `Repository`
действительно generic и не зависит от нашего `ApplicationDbContext` и от типа нашей БД.

Все вышеприведенные свойства являются общими для всех entities. Но как быть с особенностями
какой-либо entity? Например, требуется получить `User` по номеру телефона,
а в другом entity такого поля нет?

В следующем уроке будет продемонстирован прием, позволяющий расширить generic repository класс.
