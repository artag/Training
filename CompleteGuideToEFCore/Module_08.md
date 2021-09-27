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
    public User GetOneByFirstName(string firstName)
    {
        return ApplicationDbContext.Users
            .First(u => u.FirstName == firstName);
    }
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
    public User GetOneByFirstName(string firstName)
    {
        return ApplicationDbContext.Users
            .FirstOrDefault(u => u.FirstName == firstName);
    }
}
```

При таком запросе, если значение *не будет найдено* в БД, то будет возвращено значение `null`.

### Single

```csharp
public class UserRepository : Repository<User>, IUserRepository
{
    // ..
    public User GetOneByFirstName(string firstName)
    {
        return ApplicationDbContext.Users
            .Single(u => u.FirstName == firstName);
    }
}
```

`Single` - возвращает какое-либо значение, если оно *есть* в БД, и есть только *в одном экземпляре*.
Иначе - кидает исключение `InvalidOperationException`.

### SingleOrDefault

```csharp
public class UserRepository : Repository<User>, IUserRepository
{
    // ..
    public User GetOneByFirstName(string firstName)
    {
        return ApplicationDbContext.Users
            .SingleOrDefault(u => u.FirstName == firstName);
    }
}
```

При таком запросе, если значение *не будет найдено* в БД или оно будет *в нескольких экземплярах*,
то будет возвращено значение `null`.

### Find

```csharp
public class UserRepository : Repository<User>, IUserRepository
{
    // ..
    public User GetOneByFirstName(string firstName)
    {
        return ApplicationDbContext.Users
            .Find(firstName);
    }
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
    public User GetOneByFirstName(int id)
    {
        return ApplicationDbContext.Users
            .Find(id);
    }
}
```
