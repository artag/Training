# Module 8. LINQ

## Lesson 46. LINQ Introduction

Вначале рассматривается запрос из предыдущего модуля. Проект `Repository`, класс `UserRepository`:

```csharp
public class UserRepository : Repository<User>, IUserRepository
{
    // ..
    public IEnumerable<User> GetByFirstName(string firstName)
    {
        return ApplicationDbContext.Users
            .Where(u => u.FirstName == firstName);
    }
}
```

В запросе используется LINQ. В видео показан "more robust" запрос:

```csharp
// ..
return ApplicationDbContext.Users
    .Where(u => u.FirstName == firstName)
    .OrderBy(u => u.LastName);              // сортировка по возрастанию

// или

return ApplicationDbContext.Users
    .Where(u => u.FirstName == firstName)
    .OrderByDescending(u => u.LastName);    // сортировка по убыванию
```

## Lesson 47. Using LINQ to find single records in the database

Получение пользователя по имени.

### First

```csharp
public class UserRepository : Repository<User>, IUserRepository
{
    // ..
    public User GetOneByFirstName(string firstName) =>
        ApplicationDbContext.Users
            .First(u => u.FirstName == firstName);
}
```

Return the first element of the sequence that satisfies the specified condition.
Но при использовании LINQ запроса `First` надо быть уверенным, что хотя бы одно запрашиваемое имя
*есть* в БД. Иначе будет выброшено исключение `InvalidOperationException`.

В контроллере вызов этого метода будет выглядеть так:

```csharp
[Route("api/[controller]")]
public class UserController : Controller
{
    // ..
    [HttpGet("{firstName}")]
    public User Get(string firstName)
    {
        var user = _unitOfWork.Users.GetOneByFirstName(firstName);
        return user;
    }
}
```

### FirstOrDefault

Более элегантный запрос:

```csharp
public class UserRepository : Repository<User>, IUserRepository
{
    // ..
    public User GetOneByFirstName(string firstName) =>
        ApplicationDbContext.Users
            .FirstOrDefault(u => u.FirstName == firstName);
}
```

При таком запросе, если значение *не будет найдено* в БД, то будет возвращено значение `null`.

### Single

```csharp
public class UserRepository : Repository<User>, IUserRepository
{
    // ..
    public User GetOneByFirstName(string firstName) =>
        ApplicationDbContext.Users
            .Single(u => u.FirstName == firstName);
}
```

`Single` - возвращает какое-либо значение, если оно *есть* в БД, и есть только *в одном экземпляре*.
Иначе - кидает исключение `InvalidOperationException`.

### SingleOrDefault

```csharp
public class UserRepository : Repository<User>, IUserRepository
{
    // ..
    public User GetOneByFirstName(string firstName) =>
        ApplicationDbContext.Users
            .SingleOrDefault(u => u.FirstName == firstName);
}
```

При таком запросе, если значение *не будет найдено* в БД или оно будет *в нескольких экземплярах*,
то будет возвращено значение `null`.

### Find

```csharp
public class UserRepository : Repository<User>, IUserRepository
{
    // ..
    public User GetOneByFirstName(string firstName) =>
        ApplicationDbContext.Users
            .Find(firstName);
}
```

Запрос `Find` не использует lambda expression.
Проблема в том, что `Find` ищет только в значениях типа *primary key*, а поле `firstName` таким
не является.

Вот более правильное использование `Find`:

```csharp
public class UserRepository : Repository<User>, IUserRepository
{
    // ..
    public User GetOneByFirstName(int id) =>
        ApplicationDbContext.Users
            .Find(id);
}
```

## Lesson 48. Any, Count, Min and Max

Рассматривается несколько запросов.

```csharp
public class UserRepository : Repository<User>, IUserRepository
{
    // ...

    // true - если хотя бы один пользователь есть в таблице Users,
    // false - если в таблице Users нет пользователей.
    public bool HasAny() =>
        ApplicationDbContext.Users
            .Any();

    // Считает количество записей с FirstName = "John".
    // В production обычно нет hardcoded values - этот пример чисто для демонстрации.
    public int CountOfFirstNameJohn() =>
        ApplicationDbContext.Users
            .Count(u => u.FirstName == "John");

    // Считает количество записей с firstName.
    public int CountMatchingFirstName(string firstName) =>
        ApplicationDbContext.Users
            .Count(u => u.FirstName == firstName);

    // Получает id с максимальным значением.
    public int GetMaximumUserId() =>
        ApplicationDbContext.Users
            .Max(u => u.UserId);

    // Получает id с минимальным значением.
    public int GetMinimumUserId() =>
        ApplicationDbContext.Users
            .Min(u => u.UserId);
}
```

## Lesson 49. Using LINQ Select

Пример, добавленный в `UserRepository`:

```csharp
public class UserRepository : Repository<User>, IUserRepository
{
    // ..
    public IEnumerable GetAllFirstNames() =>
        ApplicationDbContext.Users
            .Select(u => u.FirstName)
            .ToList();
}
```

Его использование в контроллере `UserController`:

```csharp
[Route("api/[controller]")]
public class UserController : Controller
{
    // GET: api/<controller>
    [HttpGet]
    public IEnumerable Get()
    {
        IEnumerable users = _unitOfWork.Users.GetAllFirstNames();
        return users;
    }
}
```

Запрос `localhost:5001/api/user` вернет JSON, содержащий имена. Наподобие такого:

```json
["Scott", "Sam", "John", "John"]
```
