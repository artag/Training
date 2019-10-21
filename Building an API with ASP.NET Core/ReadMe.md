# Building an API with ASP.NET Core

О чем курс:
* Creating an API with ASP.NET Core
* Creating API Controllers
* Querying and Modifying Data
* Using Association Controllers
* Defining Operational APIs
* Versioning APIs with MVC 6

## 02. Pragmatic Rest

### 02-02. How Does HTTP Work?

От клиента серверу идет `Request` (запрос), который содержит:
1. `verb` - что собираемся делать
2. `headers` - дополнительная информация
3. `content` - содержимое (может отсутствовать)

Пример `Request`:
* (verb) - `POST`
* (headers) - `Content Length: 11`
* (content) - `Hello World`

От сервера клиенту возвращается `Response` (ответ). Структура:
1. `status code` - статус операции
2. `headers` - дополнительная информация
3. `content` - содержимое (может отсутствовать)

Пример `Response`:
* (status code) - `201`
* (headers) - `Content Type: text`
* (content) - `Hello World`

Режим работы сервера **Stateless** (не имеет состояния). Для сервера
каждый раз надо передавать всю необходимую информацию.

#### Основные виды запросов:

* `GET` - Retrieve a resource.

* `POST` - Add a new resource.

* `PUT` - Update an existing resource. Замещение ресурса целиком.

* `PATCH` - Update an existing resource with set of changes. Замещение части данных ресурса.
Используется более редко чем `PUT`.

* `DELETE` - Remove the existing resource

Есть еще другие запросы, но эти основные.


### 02-03. What Is REST

Расшифровывается как **REpresentational State Transfer** - передача состояния представления.

Из вики:
```
Это архитектурный стиль взаимодействия компонентов распределённого приложения в сети.
REST представляет собой согласованный набор ограничений, учитываемых при проектировании 
распределённой гипермедиа-системы. В определённых случаях (интернет-магазины, поисковые системы,
прочие системы, основанные на данных) это приводит к повышению производительности и
упрощению архитектуры.

В сети Интернет вызов удалённой процедуры может представлять собой обычный HTTP-запрос
(обычно «GET» или «POST»; такой запрос называют «REST-запрос»),
а необходимые данные передаются в качестве параметров запроса.

Для веб-служб, построенных с учётом REST (то есть не нарушающих накладываемых им ограничений),
применяют термин «RESTful».

В отличие от веб-сервисов (веб-служб) на основе SOAP, не существует «официального»
стандарта для RESTful веб-API. Дело в том, что REST является архитектурным стилем,
в то время как SOAP является протоколом.
```

Concepts include:
* Separation of Client and Server
* Server Requests are Stateless
* Cacheable Requests
* Uniform Interface (Единый интерфейс)


### 02-04. What Are Resources

Ресурсы это не только Enity:
* People
* Invoices
* Payments
* Products

Но и:
* Entity + окружающий Context
* Наборы (коллекции) из нескольких Entity - Entities
* Несколько взаимосвязанных сущностей (например, отчет)


### 02-05. What Are URIs

URI - **Uniform Resource Identifier** — унифицированный (единообразный) идентификатор ресурса.

URIs are just paths to Resources. Пример: `api.yourserver.com/people`.

Query Strings for non-data elements: например для format, sorting, searching, etc.


### 02-06. Designing the URI

Учебный пример.
1. Есть Camp'ы.
2. У каждого Camp есть свой Location.
3. В каждом Camp несколько Talk.
4. У каждого Talk есть свой Speaker.

Необходимые URI.

Работа с ресурсами:
* `http://.../api/camps` - список всех Camp'ов.
* `http://.../api/camps/ATL2018` - определенный Camp.
* `http://.../api/camps/ATL2018/talks` - список всех Talk'ов в определенном Camp.
* `http://.../api/camps/ATL2018/talks?topic=database` - список Talk'ов определенной тематики.
* `http://.../api/camps/ATL2018/talks/1` - определенный Talk.
* `http://.../api/camps/ATL2018/talks/1/speaker` - спикер для выбранного Talk.

Работа с функциональностью системы
* `http://.../api/reloadconfig` - перезагрузка конфигурации.


### 02-07,08. Getting the Starting Project. Using Postman

**1.** Для работы будет использоваться тулза `Postman`.

Небольшие настройки Postman:
```
Settings:
Two-pane view -> On
Automatically follow redirects -> Off
```

**2.** Настройки учебного проекта в VS
В свойствах проекта, закладка `Debug`:
* Снять флажок с `Launch browser`
* В `App URL` указать адрес с фиксированным портом. Например: `http://localhost:6600/`.
 

При вводе в Postman такого адреса (GET):
```
http://localhost:6600/api/values
```
От сервера прийдет ответ:
```
[
    "Hello",
    "From",
    "Pluralsight"
]
```


### 02-09. Trip Around the Project

Рассказывается немного о проекте:
* Program.cs
* Startup.cs
* appsettings.json (ConnectionStings для БД)

В `appsettings.json` интересна строка:
```json
  "ConnectionStrings": {
    "CodeCamp": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PSCodeCamp;
                 Integrated Security=True;Connect Timeout=30;"
```
Здесь используется:
* Название соединения "CodeCamp".
* БД `localdb` (входит в состав VS).
* Название БД будет "PSCodeCamp".

Для создания БД и применения миграции выполнить команду (в консоли):
```
dotnet ef database update
```
Примечание. Для .NET Core 3.0 Entity Framework "no longer part of the .NET Core SDK"

Чтобы его включить надо:
1. This change allows us to ship dotnet ef as a regular .NET CLI tool that can be installed
as either a global or local tool. For example, to be able to manage migrations or scaffold
a DbContext, install dotnet ef as a global tool typing the following command:
```
dotnet tool install -g dotnet-ef --version 3.0.0-*
```
У меня получилось поставить с помощью команды:
```
dotnet tool install -g dotnet-ef
```

2. You might need to add the following NuGet packages to your project:
```
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.Tools
```
У меня завелось без добавления NuGet пакетов (возможно из-за того, что проект был создан в
.NET Core 2).

Рекомендуемые **Extensions для VS**:
1. Open Command Line (by Mads Kristensen)
2. Add New File (by Mads Kristensen)


## 03. Building Your First API

### 03-01. Introduction

Как работает доступ к API.
1. Приходит Request

2. С помощью механизма Route определяется маршрут
```
http://.../api/customer -> /api/customer
``` 

3. Производится поиск маршрута по всем Route'ам.

4. Найденный маршрут соответствует определенном `Controller` и `Action`.

5. Execute Action. (Выполняется Action).

6. Отправляется `Response`.


### 03-02. Creating an Action

Создание нового контроллера `/Controllers/CampsController.cs`.
* Задается атрибут `Route`:
  * Можно так: `[Route("api/camps")]`
  * Но лучше (более надежно) так: `[Route("api/[controller]")]`
* Наследуется от класса `ControllerBase`.
* Создание action `Get()`, возвращающего (временно) object, анонимный объект.

Итого:
```csharp
[Route("api/[controller]")]
public class CampsController : ControllerBase
{
    public object Get()
    {
        return new {Moniker = "ATL2018", Name = "Atlanta Code Camp" };
    }
}
```

В Postman:
```
http://localhost:6600/api/camps
```
Выведет анонимный объект в виде JSON.


### 03-03. Status Codes

* 200 - OK
* 201 - Created 
* 202 - Accepted 
* 302 - Found 
* 304 - Not Modified 
* 307 - Temp Redirect 
* 308 - Perm Redirect 
* 400 - Bad Request
* 401 - Not Authorized
* 403 - Forbidden
* 404 - Not Found
* 405 - Method Not Allowed
* 409 - Conflict
* 500 - Internal Error

**Минимальный набор** используемых Status Codes
* 200 - OK ("It Worked")
* 400 - Bad Request ("You did bad")
* 500 - Internal Error ("We did bad")

И еще полезные коды для использования:
* 201 - Created
* 304 - Not Modified
* 404 - Not Found
* 401 - Unauthorized
* 403 - Forbidden


### 03-04. Using Status Codes

В action `CampsController.Get()` надо добавить возвращаемые ответы:
```csharp
public IActionResult Get()
{
    if (...)
    {
        // Что-то пошло не так - status code 400
        return BadRequest("Bad stuff happens");
    }

    if (...)
    {
        // Что-то пошло не так - status code 404
        return NotFound("Message not found");
    }

    // Все нормально - status code 200
    return Ok(new {Moniker = "ATL2018", Name = "Atlanta Code Camp" });
}
```

Атрибут `[HttpGet]` над action четко описывает назначение этого action'а.
Рекомендуется добавлять этот атрибут.
Также можно переименовать метод (для лучшей читаемости, например).

В нашем случае результат будет (пока) такой:
```csharp
[HttpGet]
public IActionResult GetCamps()
{
    return Ok(new {Moniker = "ATL2018", Name = "Atlanta Code Camp" });
}
```
В данном случае не добавляются другие возвращаемые Status Codes, кроме 200 (Ok).


### 03-05 Using GET for Collections

*Использование GET для получения коллекции из БД.*

Шаги:
1. Добавление в контроллер `CampsController` через конструктор reference на `ICampRepository`.

2. Добавление в метод `Get` следующего кода:
```csharp
[HttpGet]
public async Task<IActionResult> Get()
{
    try
    {
        var results = await _repository.GetAllCampsAsync();
        return Ok(results);
    }
    catch (Exception)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
    }
}
```
Т.к. нет отдельного метода для возврата Status Code 500, то код 500 возвращается через вызов
метода `StatusCode(StatusCodes.Status500InternalServerError, ...)`,
где `StatusCodes` enum.

3. Т.к. метод `GetAllCampsAsync()` из репозитория асинхронный, то добавляется конструкция
async-await и тип возвращаемого значения меняется с `IActionResult` на `Task<IActionResult>`.

Запрос в Postman:
```
http://localhost:6600/api/camps
```
Возвращает коллекцию `Camp` в виде JSON.


### 03-06. Returning Models Instead of Entities

Как правило, пользователю не надо возвращать/показывать весь Entity целиком, поэтому
надо создать EnityModel только с требуемыми полями и использовать его для возврата пользователю.

Why Models instead of Entities?
- Payload is a contract with your users
- Likely want to filter data for security too
- Surrogate Keys are useful too

Шаги. Для создания Model для entity `Camp`:
1. Создание `/Models/CampModel` (содержит только некоторые поля из entity):
```csharp
public class CampModel
{
    public string Name { get; set; }
    public string Moniker { get; set; }
    public DateTime EventDate { get; set; } = DateTime.MinValue;
    public int Length { get; set; } = 1;
}
```

2. Можно создать models в методе `CampsController.Get()`, используя LINQ, for, foreach:
```csharp
var results = await _repository.GetAllCampsAsync(includeTalks);
CampModel[] models = results...;
```
Но в данном примере будет использоваться automapper:

2.1. Ставится NuGet пакет `AutoMapper.Extensions.Microsoft.DependencyInjection`.

2.2. В `Startup.ConfigureServices()` добавляется новый сервис:
```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    // В видео
    services.AddAutoMapper();
    // У меня (в версии automapper, которую я использовал)
    services.AddAutoMapper(typeof(Startup));
    ...
}
```

2.3. Добавляется `/Data/CampProfile` который задает mapping между классами `Camp` и `CampModel`.
```csharp
public class CampProfile : Profile
{
    public CampProfile()
    {
        CreateMap<Camp, CampModel>();
    }
}
```
Особенности:
* Базовый класс `Profile`.
* В конструкторе задается map между нужными классами.

2.4. Через конструктор класса `CampsController` передается ссылка на `IMapper`.

2.5. В метод `CampsController.Get()` создание `CampModel[]` происходит так:
```csharp
public async Task<IActionResult> Get()
{
    ...
    var results = await _repository.GetAllCampsAsync(includeTalks);
    CampModel[] models = _mapper.Map<CampModel[]>(results);
    return Ok(models);
    ...
}
```

3. При запросе в Postman:
```
http://localhost:6600/api/camps
```
будет возвращаться только нужные данные из entity `Camp`

**Итоговый action для GET**

Для большей ясности кода автор преобразует `CampsController.Get()` в следующее:
```csharp
public async Task<ActionResult<CampModel[]>> Get()
{
    try
    {
        var results = await _repository.GetAllCampsAsync();
        return _mapper.Map<CampModel[]>(results);
    }
    catch (Exception)
    {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
    }
}
```
* `Task<ActionResult<CampModel[]>>` автоматом возвращает статус Ok.
* Поэтому можно просто вернуть `_mapper.Map<CampModel[]>(results)`.


### 03-07. Getting an Individual Item

Пользователь будет получать требуемый `Camp` по значению его `Moniker`

Добавленный метод в `CampsController`:
```csharp
[HttpGet("{moniker}")]
public async Task<ActionResult<CampModel>> Get(string moniker)
{
    try
    {
        var result = await _repository.GetCampAsync(moniker);

        if (result == null)
            return NotFound();    // Status Code 404

        return _mapper.Map<CampModel>(result);
    }
    catch (Exception)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
    }
}
```
Особенности:
* Добавление дополнительного пути для routing - `[HttpGet("{moniker}")]`. Запрос к action'у будет
выглядеть так:
```
http://localhost:6600/api/camps/ATL2018
```

* Если не найдено (null), то возвращается код `NotFound`.

* Если бы использовался тип int для входного параметра, то атрибут задания доп. пути 
для routing помимо названия еще можно было ограничить его типом int:
```csharp
[HttpGet("{moniker:int}")]
public async Task<ActionResult<CampModel>> Get(int moniker)
{
    ...
}
```


### 03-08. Returning Related Data

В entity `Camp` есть ссылка на другой entity - `Location`.

Есть несколько способов включить `Location` в состав показываемого `CampModel`:
* Целиком, как это делается в `Camp`.
* Можем скопировать часть свойств из `Location` прямо в `CampModel`.
* Или можно опционально показывать `Location` в составе `CampModel`.

В примере показывается копирование нескольких свойств из Location прямо в `CampModel`.

Шаги:
1. Копирование некоторых свойств из `Location` в `CampModel`:
```csharp
public class CampModel
{
    ...
    public string LocationVenueName { get; set; }
    public string LocationAddress1 { get; set; }
    public string LocationAddress2 { get; set; }
    public string LocationAddress3 { get; set; }
    public string LocationCityTown { get; set; }
    public string LocationStateProvince { get; set; }
    public string LocationPostalCode { get; set; }
    public string LocationCountry { get; set; }
}
```
Префикс `Location` в добавленных свойствах автоматически задает mapping для свойств
из entity `Location` для automapper'а.

2. Для mapping'a переименованного свойства:

Пример. Mapping свойства `LocationVenueName` как `Venue`
```csharp
public class CampModel
{
    ...
    public string Venue { get; set; }
    ...
}
```

2.1. В классе `CampProfile` создается следующий mapping:
```csharp
public CampProfile()
{
    CreateMap<Camp, CampModel>()
        .ForMember(c => c.Venue, o => o.MapFrom(m => m.Location.VenueName));
}
```
* Первая лямбда задает целевое свойство для mapping'а.

* Вторая лямбда позволяет задать:
  * AllowNull
  * ExplicitExpansion
  * Ignore
  * ...
  * MapFrom - откуда производится mapping

* Третья лямда задает исходное свойство для mapping'а.

Запрос в Postman:
```
http://localhost:6600/api/camps/ATL2018
```
Покажет содержимое одного CampModel с выборочной информацией из `Location` entity.


### 03-09. Using Query Strings

Включим получение коллекции `Talk` в составе `CampModel`.

#### Получение Talks в составе Camps 

1. Получение коллекции из `Talk` опциональное: метод `_repository.GetAllCampsAsync()`
содержит опциональный параметр `bool includeTalks = false`.

Метод Get в `CampsController` изменится следующим образом:
```csharp
[HttpGet]
public async Task<ActionResult<CampModel[]>> Get(bool includeTalks = false)
{
    try
    {
        var results = await _repository.GetAllCampsAsync(includeTalks);
        return _mapper.Map<CampModel[]>(results);
    }
    ...
}
```
Без указания параметра или с параметром `false` возвращается `CampModel` без `Talks`.

2. Создание TalkModel в `/Models/TalkModels`. Скопированы из `/Data/Entities/Talk`
некоторые поля:
```csharp
public class TalkModel
{
    public string Title { get; set; }
    public string Abstract { get; set; }
    public int Level { get; set; }
}
```

3. Включение коллекции полей в `CampModel` (еще один способ включения данных из entity,
см. ранее, для `Location`):
```csharp
public class CampModel
{
    ...
    public ICollection<TalkModel> Talks { get; set; }
}
```

4. В видео сказано о этом не было и не создавалось, но у меня не работало без создания
`/Data/TalkProfile` - класса, который задает mapping между классами `Talk` и `TalkModel`:
```csharp
public class TalkProfile : Profile
{
    public TalkProfile()
    {
        CreateMap<Talk, TalkModel>();
    }
}
```

Запрос:
```
http://localhost:6600/api/camps
```
Покажет Camps в виде JSON с пустыми полями talks: `"talks": []`.

Запрос:
```
http://localhost:6600/api/camps?includeTalks=true
```
Покажет Camps в виде JSON с заполненными полями talks.


#### Получение Speaker в составе TalkModel

Шаги.

1. Создание `/Models/SpeakerModel`:
```csharp
public class SpeakerModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string Company { get; set; }
    public string CompanyUrl { get; set; }
    public string BlogUrl { get; set; }
    public string Twitter { get; set; }
    public string GitHub { get; set; }
}
```

2. Добавление `SpeakerModel` в `TalkModel`:
```csharp
public class TalkModel
{
    ...
    public SpeakerModel Speaker { get; set; }
}
```

3. В видео сказано о этом не было и не создавалось, но у меня не работало без создания
`/Data/SpeakerProfile` - класса, который задает mapping между классами `Speaker` и
`SpeakerModel`:
```csharp
public class SpeakerProfile : Profile
{
    public SpeakerProfile()
    {
        CreateMap<Speaker, SpeakerModel>();
    }
}
```

Запрос:
```
http://localhost:6600/api/camps?includeTalks=true
```
Покажет Camps в виде JSON с заполненными полями talks. Для каждого поля talk будет свой speaker.


#### Дополнительное добавление полей в `TalkModel` и `SpeakerModel`

В последний момент в эти классы моделей добавляются свойства с id (взяты из entity),
для идентификации talk и speaker:
`TalkId` и `SpeakerId`.


### 03-10. Implementing Searching

*Добавление поиска Camps по дате.*

Шаг 1. В контроллер `CampsController` добавляется следующий метод:
```csharp
[HttpGet("search")]
public async Task<ActionResult<CampModel[]>> SearchByDate(
    DateTime theDate, bool includeTalks = false)
{
    try
    {
        var result = await _repository.GetAllCampsByEventDate(theDate, includeTalks);

        if (!result.Any())
        {
            return NotFound();
        }

        return _mapper.Map<CampModel[]>(result);
    }
    catch (Exception)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
    }
}
```
Похоже на все то, что использовалось ранее. Особенности:
* Атрибут `[HttpGet("search")]` добавляет к Route "дополнительный" маршрут:
```
/api/camps/search?queryString1&queryString2
```

* Для получения Camps по дате используется метод ` _repository.GetAllCampsByEventDate()`.


Запросы:
```
http://localhost:6600/api/camps/search?theDate=2018-10-18
или
http://localhost:6600/api/camps/search?theDate=2018-10-18&includeTalks=true
```
Вернут нужные Camps за требуемую дату, иначе вернется Status Code 404 (Not Found).


## 04. Modifying Data

### 04-01. URI Design

Поведение verb зависит от ресурса, над которым совершается действие.

Пример. Действие, совершаемые над ресурсами customer:
```
Resource         /customers
GET    (read)    Get List
POST   (create)  Create Item
PUT    (update)  Update Batch
DELETE (delete)  Error

Resource         /customers/123
GET    (read)    Get Item
POST   (create)  Error
PUT    (update)  Update Item
DELETE (delete)  Delete Item
```

Пример. Результаты, получаемые после действий, проведенных над ресурсами customer:
```
Resource         /customers
GET    (read)    List
POST   (create)  New Item
PUT    (update)  Status Code Only
DELETE (delete)  Status Code Only (Error Status Code)
```

```
Resource         /customers/123
GET    (read)    Item
POST   (create)  Status Code Only (Error Status Code)
PUT    (update)  Updated Item
DELETE (delete)  Status Code Only
```


### 04-02. Model Binding

*Создание action для запроса POST (часть 1/3). Привязка модели к содержимому body запроса.*

Шаги:

1. Создание action'а для запроса POST в `CampsController`. Временный вариант.
```csharp
public async Task<ActionResult<CampModel>> Post(CampModel model)
{
    try
    {
        return Ok();
    }
    catch (Exception)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
    }
}
```

2. Запрос в `Postman`:
* Тип запроса POST.

* Тело запроса, Body -> raw -> JSON
```json
{
    "name": "San Diego Code Camp",
    "moniker": "SD2018",
    "father": "foo bar"
}
```

* Сам запрос
```
http://localhost:6600/api/camps
```

3. При выполнении запроса в метод приходит пустой `CampModel`. Необходимо выполнить Model Binding.
* Способ 1. Установка атрибута `[FromBody]` в самом action (устаревший способ):
```csharp
public async Task<ActionResult<CampModel>> Post([FromBody]CampModel model)
{
    ...
}
```

* Способ 2. Установка атрибута [ApiController] на весь контроллер (начиная с ASP.NET Core 2.1).
Рекомендуемый способ:
```csharp
[Route("api/[controller]")]
[ApiController]
public class CampsController : ControllerBase
{
    ...
}
```

4. Теперь, если выполнить запрос из шага 2, то в `CampModel` будут переданы данные из body запроса.
Из тела запроса:
```json
{
    "name": "San Diego Code Camp",
    "moniker": "SD2018",
    "father": "foo bar"
}
```
будут взяты данные только для свойств 'Name' и 'Moniker'.
 

### 04-03. Implementing POST

*Создание action для запроса POST (часть 2/3).*

Шаги:

1. Модификация (неокончательная) метода Post, созданного в предыдущем разделе:
```csharp
public async Task<ActionResult<CampModel>> Post(CampModel model)
{
    try
    {
        var camp = _mapper.Map<Camp>(model);
        _repository.Add(camp);

        if (await _repository.SaveChangesAsync())
        {
            return Ok();
        }

        return BadRequest();
    }
    catch (Exception)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
    }
}
```

2. Возвращать Status Code `Ok` в данном случае неверно. Для запросов POST
надо возвращать Status Code `Created`:
```csharp
public async Task<ActionResult<CampModel>> Post(CampModel model)
{
    try
    {
        var camp = _mapper.Map<Camp>(model);
        _repository.Add(camp);

        if (await _repository.SaveChangesAsync())
        {
            return Created($"/api/camps/{camp.Moniker}", _mapper.Map<CampModel>(camp));
        }

        return BadRequest();
    }
    catch (Exception)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
    }
}
```
Для `Created`:
* `$"/api/camps/{camp.Moniker}"` - URI для созданного ресурса. Не рекомендуется использовать,
т.к. путь захардкожен (правильный вариант в следующем пункте).

* `_mapper.Map<CampModel>(camp)` - преобразует `Camp` обратно в `CampModel`.
Рекомендуется так делать всегда, т.к. в `Camp` в процессе записи в БД могут записываться
какие-либо дополнительные данные.


3. Более грамотное задание URI (для ASP.NET Core версии 2.2 и выше).

3.1. Воспользоваться сервисом `LinkGenerator`. Добавление ссылки на него через конструктор
`CampsController`.

3.2. В методе `Post()` получить URI на новый ресурс:
```csharp
var location = _linkGenerator.GetPathByAction(
    "Get", "Camps", new {moniker = model.Moniker});

if (string.IsNullOrWhiteSpace(location))
{
    return BadRequest("Could not use current moniker");
}
...
return Created(location, _mapper.Map<CampModel>(camp));
```
`_linkGenerator.GetPathByAction("Get", "Camps", new {moniker = model.Moniker});`

* `Get` - наменование action. Используя его routing получаем нужный URI.

* `Camps` - наменование controller (пишется без окончания "Controller").

* `new {moniker = model.Moniker}` - анонимный объект для указания параметра метода
`Get(Moniker)`.

**Итого**. Сейчас метод `Post` выглядит так:
```csharp
public async Task<ActionResult<CampModel>> Post(CampModel model)
{
    try
    {
        // Get URI for created Camp
        var location = _linkGenerator.GetPathByAction(
            "Get", "Camps", new {moniker = model.Moniker});

        if (string.IsNullOrWhiteSpace(location))
        {
            return BadRequest("Could not use current moniker");
        }

        // Create a new Camp
        var camp = _mapper.Map<Camp>(model);
        _repository.Add(camp);

        if (await _repository.SaveChangesAsync())
        {
            return Created(location, _mapper.Map<CampModel>(camp));
        }
    }
    catch (Exception)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
    }

    return BadRequest();
}
```

Делаем запрос POST
```
http://localhost:6600/api/camps
```
С таким Body:
```json
{
    "name": "San Diego Code Camp",
    "moniker": "SD2018",
    "eventDate": "2018-05-05",
    "length": 1,
    "venue": "SD Community College",
    "locationPostalCode": "12345"
}
```

Создается новый Camp.

4. В видео не было показано, но у меня не заработало. Добавил обратный mapping:
```csharp
public CampProfile()
{
    CreateMap<Camp, CampModel>()
        .ForMember(c => c.Venue, o => o.MapFrom(m => m.Location.VenueName)).ReverseMap();
}
```


### 04-04. Adding Model Validation

*Добавление Validation данных для POST запроса. Использование встроенных средств для валидации
и написание своей валидации на уникальность Moniker.*

Шаги.

1. Добавление атрибутов в `CampModel` для некоторых свойств, для которых требуется проверка.
```csharp
public class CampModel
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    public string Moniker { get; set; }

    [Range(1, 100)]
    public int Length { get; set; } = 1;
    ...
}
```

2. При запросе POST
```
http://localhost:6600/api/camps
```

С Body без указания обязательного свойства "name" 
```json
{
    "moniker": "SD2018",
    "eventDate": "2018-05-05",
    "length": 1,
    "venue": "SD Community College",
    "locationPostalCode": "12345"
}
```

Возратится Status Code 400 (Bad Request) со следующим Body:
```json
{
    "errors": {
        "Name": [
            "The Name field is required."
        ]
    },
    "title": "One or more validation errors occurred.",
    "status": 400,
    "traceId": "8000000e-0001-fd00-b63f-84710c7967bb"
}
```

Данная валидация обеспечивается благодаря добавленному ранее атрибуту `[ApiController]`.

Если надо что-то дополнить/изменить в валидации, то можно дополнительно в метод `Post()`
добавить что-то типа:
```csharp
if (ModelState.IsValid)
...
```

#### Проверка moniker на уникальность

В нашем случае, перед добавлением нового Camp требуется проверка на уникальность его "Moniker".

Все требуемые изменения вносятся в метод `Post`. Следующее добавляется в начало метода:
```csharp
// Check for existing moniker
var existingCamp = await _repository.GetCampAsync(model.Moniker);
if (existingCamp != null)
{
    return BadRequest("Moniker in Use");
}

// Get URI for created Camp
...
```


### 04-05. Implementing PUT

*Обновление ресурса. Запрос PUT (обновление ресурса целиком).*

В `CampsController` создается новый метод:
```csharp
[HttpPut("{moniker}")]
public async Task<ActionResult<CampModel>> Put(string moniker, CampModel model)
{
    try
    {
        var oldCamp = await _repository.GetCampAsync(moniker);
        if (oldCamp == null)
        {
            return NotFound($"Could not find camp with moniker of {moniker}");
        }

        _mapper.Map(model, oldCamp);

        if (await _repository.SaveChangesAsync())
        {
            return _mapper.Map<CampModel>(oldCamp);
        }
    }
    catch (Exception)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
    }

    return BadRequest();
}
```
Особенности:

1. `_mapper.Map(model, oldCamp)` - переписывает данные из `model` в `oldCamp`.

2. Возврат `_mapper.Map<CampModel>(oldCamp)` и `Task<ActionResult<CampModel>>` автоматически
возвращают Status Code 200 (OK).

3. Как видно из кода, PUT переписывает объект целиком.

Запрос PUT (Camp с таким Moniker уже существует в БД):
```
http://localhost:6600/api/camps/SD2018
```

Тело запроса:
```json
{
    "name": "San Diego Code Camp",
    "moniker": "SD2018",
    "eventDate": "2018-05-05T00:00:00",
    "length": 3,
    "venue": "SD Community College",
    "locationAddress1": "123 Main Street",
    "locationAddress2": null,
    "locationAddress3": null,
    "locationCityTown": "San Diego",
    "locationStateProvince": "CA",
    "locationPostalCode": "98765",
    "locationCountry": "USA",
    "talks": []
}
```


### 04-06. Implementing DELETE

В `CampsController` создается новый метод:
```csharp
[HttpDelete("{moniker}")]
public async Task<IActionResult> Delete(string moniker)
{
    try
    {
        var oldCamp = await _repository.GetCampAsync(moniker);
        if (oldCamp == null)
        {
            return NotFound();
        }

        _repository.Delete(oldCamp);

        if (await _repository.SaveChangesAsync())
        {
            return Ok();
        }
    }
    catch (Exception)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
    }

    return BadRequest("Failed to delete the camp");
}
```
Особенности:

1. `Task<IActionResult>` - возвращется `IActionResult` т.к. возвращается только Status Code.

2. При удалении camp могут оставаться или удаляться все связанные с ним enitities: talks, locations
и т.д. Поведение объектов при удалении из БД должно определяться business rules.

Запрос DELETE (без Body):
```
http://localhost:6600/api/camps/MO2018
```

Удаление Camp с Moniker=MO2018.

При успешном удалении возвращается только Status Code 200 (OK).


## 05. Creating Association APIs

### 05-01. Introduction. Design Your API with Associations

CampsController:
```
/api/camps
/api/camps/atl2016
```

Контроллер имеющий дело с talks - TalksController:
```
/api/camps/atl2016/talks
/api/camps/atl2016/talks/1
```


### 05-02. Create an Association Controller. Create GET for all Talks

1. Создание контроллера для работы с Talk в `/Controllers/TalksController`.

Особенности (почти все такие же как и у `CampsController`):
* Базовый класс `ControllerBase`.

* В конструктор передаются три ссылки на: `ICampRepository`, `IMapper` и `LinkGenerator`.

* На контроллер добавляется атрибут `[ApiController]`.

* Для контроллера задается маршрут `[Route("api/camps/{moniker}/talks")]`. Секция `moniker`
может передаваться как соответствующий параметр в методы этого контроллера.


2. Создание метода GET для всех Talks в Camp:
```csharp
[HttpGet]
public async Task<ActionResult<TalkModel[]>> Get(string moniker)
{
    try
    {
        var talks = await _repository.GetTalksByMonikerAsync(moniker);
        return _mapper.Map<TalkModel[]>(talks);
    }
    catch (Exception)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get Talks");
    }
}
```
Значение `moniker` action получает из маршрута контроллера.

3. Запрос:
```
http://localhost:6600/api/camps/atl2018/talks
```

Вывод:
```json
[
    {
        "talkId": 2,
        "title": "Writing Sample Data Made Easy",
        "abstract": "Thinking of good sample data examples is tiring.",
        "level": 200,
        "speaker": null
    },
    {
        "talkId": 1,
        "title": "Entity Framework From Scratch",
        "abstract": "Entity Framework from scratch in an hour. Probably cover it all",
        "level": 100,
        "speaker": null
    }
]
```


### 05-03. GET an Individual Talk

1. Создание метода GET для требуемого Talk:
```csharp
[HttpGet("{id:int}")]
public async Task<ActionResult<TalkModel>> Get(string moniker, int id)
{
    try
    {
        var talk = await _repository.GetTalkByMonikerAsync(moniker, id);
        return _mapper.Map<TalkModel>(talk);
    }
    catch (Exception)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get Talks");
    }
}
```

2. Запрос:
```
http://localhost:6600/api/camps/atl2018/talks/2
```

Вывод:
* Выводит один Talk с определенным `TalkId` = 2.

* Для несуществующего `TalkId` сервер возвращает Status Code 204 (No Content).
(Ниже данное поведение будет исправлено).
