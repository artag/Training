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

2. Добавить reference на проект `Model`:

```text
dotnet add reference ../Model/Model.csproj
```

3. В проект `Repository` добавляем generic класс `Repository<TEntity>`, который будет реализацией
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

## Lesson 42. Build application specific repository interfaces

Для entity, для которого требуется реализовать что-то особое можно сделать новый интерфейс,
специально под эту сущность и расширяющий базовый:

```csharp
public interface IUserRepository : IRepository<User>
{
    IEnumerable<User> GetByFirstName(string firstName);
}
```

## Lesson 43. Create application specific repository implementations

В проект `Repository` добавляется класс `UserRepository`:

```csharp
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public ApplicationDbContext ApplicationDbContext =>
        Context as ApplicationDbContext;

    public IEnumerable<User> GetByFirstName(string firstName) =>
        ApplicationDbContext.Users.Where(u => u.FirstName == firstName);
}
```

Особенности:

1. `UserRepository` расширяет generic `Repository<User>`.
2. Через конструктор задается `ApplicationDbContext`.
3. Доступ к `ApplicationDbContext` выполняется через свойство `Context`, находящемся в базовом
классе.

Таких specific repository implementations может быть очень много. Для их логического связывания
используется паттерн "Unit of Work".

## Lesson 44. Implementing a Unit of Work

Проект `Repository`. Сначала добавляем интерфейс:

```csharp
public interface IUnitOfWork : IDisposable
{
    // Подобные ссылки делаются для каждого репозитория entity.
    IUserRepository Users { get; }

    int Complete();
}
```

Особенности:

1. Здесь приводятся все ссылки на все repository всех entity. В данном примере ссылка только
на `IUserRepository`.
2. Ссылки делаются через интерфейсы.
3. `IDisposable` используется для закрытия соединения с БД.

Через один экземпляр `IUnitOfWork` мы можем работать со всеми entities из одного места.

Потом добавляется реализация:

```csharp
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Users = new UserRepository(_context);
    }

    public IUserRepository Users { get; private set; }

    public int Complete()
    {
        return _context != null
            ? _context.SaveChanges()
            : 0;        // Ничего не делаем.
    }

    public void Dispose()
    {
        if (_context != null)
            _context.Dispose();
    }
}
```

Метод `Complete` позволяет "одним махом" сохранить изменения, сделанные в нескольких сущностях.

## Lesson 45. Using the unit of work and repositories in your code

Проект `efdemo`.

1. Добавить reference на проект `Repository`:

```text
dotnet add reference ../Repository/Repository.csproj
```

2. Добавить `UnitOfWork` в `Startup` класс:

```csharp
public class Startup
{
    // ..

    public void ConfigureServices(IServiceCollection services)
    {
        // ..
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
```

3. Идем в `UserController`. Инжектируем `IUnitOfWork` через конструктор:

```csharp
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public UserController(ApplicationDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    // ..
}
```

4. Теперь можно использовать `UnitOfWork` в контроллере `UserController`:

```csharp
public class UserController : Controller
{
    // ..

    // GET: api/<controller>
    [HttpGet]
    public IEnumerable<User> Get()
    {
        var users = _unitOfWork.Users.GetAll().ToList();
        return users;
    }

    // GET: api/<controller>/5
    [HttpGet("{id}")]
    public User Get(int id)
    {
        var user = _unitOfWork.Users.Get(id);
        return user;
    }

    // ..
}
```

Используя Unit of Work мы отвязываемся от зависимостей:

* EF Core (при случае, можно безболезненно заменить на другой ORM).
* `DbContext`
