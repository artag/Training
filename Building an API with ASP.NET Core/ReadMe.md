# Building an API with ASP.NET Core

О чем курс:
* Creating an API with ASP.NET Core
* Creating API Controllers
* Querying and Modifying Data
* Using Association Controllers
* Defining Operational APIs
* Versioning APIs with MVC 6


## 02-02. How Does HTTP Work?

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

### Основные виды запросов:

* `GET` - Retrieve a resource.

* `POST` - Add a new resource.

* `PUT` - Update an existing resource. Замещение ресурса целиком.

* `PATCH` - Update an existing resource with set of changes. Замещение части данных ресурса.
Используется более редко чем `PUT`.

* `DELETE` - Remove the existing resource

Есть еще другие запросы, но эти основные.


## 02-03. What Is REST

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


## 02-04. What Are Resources

Ресурсы это не только Enity:
* People
* Invoices
* Payments
* Products

Но и:
* Entity + окружающий Context
* Наборы (коллекции) из нескольких Entity - Entities
* Несколько взаимосвязанных сущностей (например, отчет)


## 02-05. What Are URIs

URI - **Uniform Resource Identifier** — унифицированный (единообразный) идентификатор ресурса.

URIs are just paths to Resources. Пример: `api.yourserver.com/people`.

Query Strings for non-data elements: например для format, sorting, searching, etc.


## 02-06. Designing the URI

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
