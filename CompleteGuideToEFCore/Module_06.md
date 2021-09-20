# Module 6. Manipulating data

## Lesson 33. Setting up an API controller to perform CRUD operations

Добавляем REST контроллер для выполнения CRUD операций над entity `User`.

Работа в проекте "efdemo", директория "Controllers".

1. В Visual Studio создается новый API Controller Class, `UserController`:

```csharp
[Route("api/[controller]")]
public class UserController : Controller
{
    // GET: api/<controller>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET: api/<controller>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST: api/<controller>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT: api/<controller>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE: api/<controller>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
```

* `UserController` наследуется от базового класса `Controller`.
* Аннотация `[Route("api/[controller]")]` - настройка, которая задает путь, по которому контроллер
будет доступен. Например, при данном значении `UserController` будет доступен по адресу
"<сайт>/api/user". По этому адресу будут доступны для вызова методы, расположенные в данном
контроллере.

Доступные методы:

* `Get` - получить данные всех пользователей.
* `Get` by id - получить данные одного пользователя.
* `Post` - добавление новой записи (пользователя).
* `Put` by id - редактирование пользователя, находящегося в БД.
* `Delete` by id - удаление пользователя.

В Visual Studio Code такой API Controller надо набивать "ручками".

2. При помощи dependency injection добавим DbContext в контроллер. Можно конечно создавать
DbContext в каждом методе, но это не рекомендуется делать.

```csharp
[Route("api/[controller]")]
public class UserController : Controller
{
    private ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ..
}
```

3. В классе `Startup`, настройка dependency injection для класса `ApplicationDbContext`
уже была ранее задана:

```csharp
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // ..
        // To use with SQLite database
        services.AddEntityFrameworkSqlite().AddDbContext<ApplicationDbContext>();
    }

    // ..
}
```

## Lesson 34. Getting all data from a database table

Продолжение предыдущего урока - реализация GET метода в `UserController`:

```csharp
[Route("api/[controller]")]
public class UserController : Controller
{
    // ..

    // GET: api/<controller>
    [HttpGet]
    public IEnumerable<User> Get()
    {
        var users = _context.Users.ToList();
        return users;
    }
}
```

Перед запуском приложения, в классе `Startup`:

* В методе `Configure` комментируем строку `app.UseAuthorization()` (в данном приложении авторизация
не сконфигурирована и не используется).

* В метод `ConfigureServices` добавляется:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // ..
    services.AddControllers();
}
```

Запускаем приложение, в адресной строке браузера вводим адрес наподобие:

```text
https://localhost:5001/api/user
```

По `api/user` автоматически попадаем в контроллер `UserController`, в метод `Get()`.
Сразу происходит запрос в БД и вовзращения списка всех пользователей в виде JSON.

## Lesson 35. Getting a single record of data

Продолжение предыдущего - реализация GET метода, который возвращает определенного
пользователя. В `UserController`:

```csharp
[Route("api/[controller]")]
public class UserController : Controller
{
    // ..

    // GET: api/<controller>/5
    [HttpGet("{id}")]
    public User Get(int id)
    {
        var user = _context.Users.Find(id);
        return user;
    }
}
```

По адресу наподобие `https://localhost:5001/api/user/3` можно получить пользователя по его id.

Более подробно можно посмотреть в chrome developer tools (вызов по F12),
закладка Network -> Response.

При запросе id несуществующего пользователя, на закладке Response будет отображено:
"Failed to load response data". Необходимо добавить в метод `Get(int id)` обработку ошибок
при запросе несуществующего пользователя.
