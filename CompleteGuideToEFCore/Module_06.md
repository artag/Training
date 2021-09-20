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

## Lesson 36. Adding a record to the database

Здесь реализация добавления записи в БД. В `UserController`:

```csharp
[Route("api/[controller]")]
public class UserController : Controller
{
    // ..

    // POST: api/<controller>
    [HttpPost]
    public ActionResult<User> Post([FromBody] User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = user.UserId }, user);
    }
}
```

При вызове `Add(user)` происходит добавление пользователя в БД и начало tracking этого значения.
После этого необходимо сохранить это изменение в БД - команда `SaveChanges()`.
В `SaveChanges` не указывается таблица - БД сохраняет все tracking записи.

Можно добавить сразу несколько пользователей - использовать несколько вызовов `Add` и один
вызов `SaveChanges`.

`CreatedAtAction` - возвращает HTTP статус 201 (Created). После записи в БД мы считываем
только что записанного `User` с новым id, полученным после его сохранения в БД.

Для нового пользователя надо определить поля:

* `FirstName`
* `LastName`

Все остальные поля (в особенности `UserId`) генерируются БД автоматически и не требуют определения.

Для теста воспользуемся программой *Postman*.

1. "New Request" -> Окно "Save request":

* Request name: "postuser"
* All collections: "efdemo"

Команда "Save".

*Примечание*: у меня версия Postman несколько отличается от показанной на видео: там несколько
по другому создается и сохраняется коллекция и запрос.

2. "Request" -> Вкладка "Body" -> raw -> выпадающий список "Text" перевести на "JSON"

Задать body в виде JSON:

```json
{
    "FirstName": "John",
    "LastName": "Adams"
}
```

3. Задать адрес для POST: `https://localhost:5001/api/user`

4. При работающем приложении нажать кнопку "Send". Если все сделано верно, то в нижней части
Postman можно увидеть содержимое ответа в виде JSON - сохраненные данные в БД.
