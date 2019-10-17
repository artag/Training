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
