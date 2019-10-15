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

