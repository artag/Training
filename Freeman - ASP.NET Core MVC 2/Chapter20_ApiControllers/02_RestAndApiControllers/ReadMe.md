# Введение в REST и контроллеры API

*Контроллер API* - это контроллер MVC, который отвечает за предоставление доступа к данным
в приложении, не инкапсулируя их в HTML-разметку.

Самый популярный подход к доставке данных из приложения - использование паттерна **REST**
(Representational State Transfer - передача состояния представления).

Запросы REST указывают серверу операцию, подлежащую выполнению, а URL запроса определяет один или
большее число объектов данных, к которым операция будет применена.

Пример URL:
```
/api/reservation/1
```

* `api` - используется для отделения части данных приложения от стандартных контроллеров,
которые генерируют HTML-разметку.

* `reservations` - коллекция объектов, с которой будет производиться работа.

* `1` - задает индивидуальный объект внутри коллекции `reservations`.

Методы HTTP и URL для указания веб-службы REST
* Команда: `GET`
  * URL: `/api/reservations`
  * Описание: Извлекает все объекты.
  * Полезная нагрузка: Ответ содержит полную коллекцию объектов `Reservation`.

* Команда: `GET`
  * URL: `/api/reservations/1`
  * Описание: Извлекает объект `Reservation`, свойство `ReservationId` которого имеет значение 1.
  * Полезная нагрузка: Ответ содержит указанный объект `Reservation`.

* Команда: `POST`
  * URL: `/api/reservation`
  * Описание: Создает новый объект `Reservation`.
  * Полезная нагрузка: Запрос содержит значения для других свойств, которые требуются при создании
объекта `Reservation`. Ответ содержит объект, который был сохранен, гарантируя получение клиентом
сохраненных данных.

* Команда: `PUT`
  * URL: `/api/reservation`
  * Описание: Заменяет существующий объект `Reservation`.
  * Полезная нагрузка: Запрос содержит значения, требуемые для изменения свойств указанного объекта
`Reservation`. Ответ содержит объект, который был сохранен, гарантируя получение клиентом
сохраненных данных.

* Команда: `PATCH`
  * URL: `/api/reservation/1`
  * Описание: Эта комбинация модифицирует существующий объект `Reservation`, свойство `ReservationId`
которого имеет значение 1.
  * Полезная нагрузка: Запрос содержит набор модификаций, которые должны быть применены к
указанному объекту `Reservation`. Ответом является подтверждение о том, что изменения были применены.

* Команда: `DELETE`
  * URL: `/api/reservation/1`
  * Описание: Эта комбинация удаляет объект `Reservation`, свойство `ReservationId` которого имеет
значение 1.
  * Полезная нагрузка: Полезная нагрузка в запросе и ответе отсутствует.

Следование соглашению REST не является требованием, но содействует упрощению работы с приложением.


## Создание контроллера API

Пример контроллера, реализующего всю указанную выше REST функциональность
см. в `Controllers/ReservationController`.

Контроллеры API работают также как и обычные контроллеры, т.е.:
* Можно создать контроллер POCO.
* Унаследовать контроллер от базового класса `Controller`.
* Можно определять контроллер API где угодно в проекте.

Далее будет рассмотрен контроллер API `ReservationController` более детально.


### Определение маршрута

```cs
[Route("api/[controller]")]
public class ReservationController : Controller
{
    ...
}
```

Маршрут, посредством которого достигаются контроллеры API, может определяться **только** с
использованием атрибута `Route`, но не конфигурацией приложения в `Startup`.

По соглашению, для контроллеров API маршрут предваряется префиксом `api`, за которым следует
имя контроллера.

В примере контроллер API достигается через URL `/api/reservation`.


### Объявление зависимостей

```cs
private readonly IRepository _repository;

public ReservationController(IRepository repository)
{
    _repository = repository;
}
```


### Определение методов действий

Каждый метод действия декорируется атрибутом, указывающим метод HTTP, который он принимает.
Например:
```cs
[HttpGet]
public IEnumerable<Reservation> Get() => _repository.Reservations;
```

Атрибуты методов HTTP:
* `HttpGet` - Действие вызывается только HTTP-запросами, использующими команду `GET`.
* `HttpPost` - Действие вызывается только HTTP-запросами, применяющими команду `POST`.
* `HttpDelete` - Действие вызывается только HTTP-запросами, использующими команду `DELETE`.
* `HttpPut` - Действие вызывается только HTTP-запросами, применяющими команду `PUT`.
* `HttpPatch` - Действие вызывается только HTTP-запросами, использующими команду `PATCH`.
* `HttpHead` - Действие вызывается только HTTP-запросами, применяющими команду `HEAD`.
* `AcceptVerbs` - Используется для указания множества команд HTTP.

Маршруты могут дополнительно конкретизироваться за счет включения фрагмента маршрутизации
в качестве аргумента для атрибута метода HTTP:
```cs
[HttpGet("{id}")]
public Reservation Get(int id) => _repository[id];
```

Здесь фрагмент маршрутизации `{id}` объединяется с маршрутом, задаваемым атрибутом `Route`,
который применен к контроллеру.

Т.о. запрос GET к API отправляется через URL вида `/api/reservation/{id}`.


### Определение результатов действий

Методы действий контроллера API возвращают типы данных:
```cs
[HttpGet]
public IEnumerable<Reservation> Get() => _repository.Reservations;
```

В данном блоке кода возвращается последовательность объектов `Reservation`, ифраструктура MVC
выполняет сериализацию в формат, который может быть обработан клиентом.

Иногда можно просто возвращать объекты C#, а MVC будет само решать, что с ними делать.
Например: При возврате `null`, клиенту отправляется ответ `204 - No Content`
(204 - содержимое отсутствует).

Можно возвращать `IActionResult`, который указывает вид результата для отправки.
Пример (в коде этого нет):
```cs
[HttpGet("{id}")]
public IActionResult Get(int id)
{
    var result = _repository(id);
    if (result == null)
    {
        return NotFound();
    }

    return Ok(result);
}
```

Здесь отправляется ответ `404 - Not Found` для запросов, которые не соответствуют каким-либо
объектам в модели. При `result == null` вызывается метод `NotFound()`, который создает объект
`NotFoundResult`, который приводит к отправке клиенту ответа `404 - Not Found`.


## Тестирование контроллера API

Инструменты для тестирования API-интерфейсов веб-приложения:
* Fiddler
* Swashbuckle
* PowerShell (описано в книге)

Для тестирования API:
1. Запустить приложение без отладки
2. Открыть PowerShell (Start -> Run -> powershell)
3. Вбивать команды


### Тестирование операций GET

Команда (получить все объекты):
```
Invoke-RestMethod http://localhost:5000/api/reservation -Method GET
```

Вывод (представление JSON объектов `Reservation` в табличном формате):
```
reservationId clientName location
------------- ---------- --------
            0 Alice      Board Room
            1 Bob        Lecture Hall
            2 Joe        Meeting Room 1
```

Команда (извлечь одиночный объект):
```
Invoke-RestMethod http://localhost:5000/api/reservation/1 -Method GET
```

Вывод:
```
reservationId clientName location
------------- ---------- --------
            1 Bob        Lecture Hall
```


### Тестирование операции POST

Команда отправляет контроллеру API запрос `POST` для создания нового объекта `Reservation` и
записывает полученные данные в ответ.

Команда:
```
Invoke-RestMethod http://localhost:5000/api/reservation -Method POST
-Body (@{clientName="Anne"; location="Meeting Room 4"} | ConvertTo-Json)
-ContentType "application/json"
```

С помощью аргумента `-Body` указывается тело запроса, кодируемое как JSON.
Аргумент `-ContentType` используется для установки заголовка `Content-Type` в запросе.

Вывод (возврат клиенту JSON представление нового объекта):
```
reservationId clientName location
------------- ---------- --------
            3 Anne       Meeting Room 4
```


### Тестирование операции PUT

Метод `PUT` применяется для замены существующих объектов в модели. Значение `reservationId`
указывается как часть URL запроса, а значения `clientName` и `location` предоставляются в теле
запроса.

Команда:
```
Invoke-RestMethod http://localhost:5000/api/reservation -Method PUT
-Body (@{reservationId="1"; clientName="Bob"; location="Media Room"} | ConvertTo-Json)
-ContentType "application/
json"
```

Вывод (ответ, отражающий внесенное изменение):
```
reservationId clientName location
------------- ---------- --------
            1 Bob        Media Room
```


### Тестирование операции PATCH

Метод `PATCH` используется для модификации существующего объекта в модели. Запрос `PATCH`
(в отличие от `PUT`) позволяет указать набор детализированных изменений, которые необходимо внести
в объект.

ASP.NET Core работает со стандартом JSON Path, который делает возможным указание изменений в
унифицированной манере. В примере клиент будет отправлять контроллеру API в HTTP-запросе PATCH
данные JSON следующего вида:
```json
[
{ "op": "replace", "path": "clientName", "value": "Bob"},
{ "op": "replace", "path": "location", "value": "Lecture Hall"}
]
```

JSON Path выражается как массив операций. Каждая операция имеет свойство `op`, указывающее тип
операции и свойство `path`, которое определяет, где операция будет применена.

В примере приложения (и практически в большинстве приложений) требуется только операция `replace`.
Здесь устанавливаются новые значения для свойств `clientName` и `location`.

Команда:
```
Invoke-RestMethod http://localhost:5000/api/reservation/2 -Method PATCH
-Body (@{op="replace"; path="clientName"; value="Bob"},
       @{op="replace"; path="location"; value="Lecture Hall"} | ConvertTo-Json)
-ContentType "application/json"
```

Здесь модифицируются свойства у объекта `Reservation` с `id`=2.


### Тестирование операции DELETE

Запрос `DELETE` удаляет объект `Reservation` из хранилища.

Команда:
```
Invoke-RestMethod http://localhost:5000/api/reservation/2 -Method DELETE
```

Объект `Reservation` с `id`=2 был удален из хранилища.


## Использование контроллера API в браузере

В браузере для работы с API обычно используются запросы `Ajax` (Asynchronous JavaScript and XML).
Делать запросы Ajax проще всего с использованием библиотеки `jQuery`.

Что надо дополнительно сделать:
1. Добавить в проект пакет `jQuery`.
В данном примере, в `package.json`, добавлена соответствующая строка.

2. В `wwwroot/js` добален `client.js`:
```js
$(document).ready(function () {

    $("form").submit(function (e) {
        e.preventDefault();
        $.ajax({
            url: "api/reservation",
            contentType: "application/json",
            method: "POST",
            data: JSON.stringify({
                clientName: this.elements["ClientName"].value,
                location: this.elements["Location"].value
            }),
            success: function (data) {
                addTableRow(data);
            }
        })
    });
});

var addTableRow = function (reservation) {
    $("table tbody").append(
        "<tr>" +
        "<td>" + reservation.reservationId + "</td>" +
        "<td>" + reservation.ClientName + "</td>" +
        "<td>" + reservation.Location +"</td>" +
        "</tr>");
}
```

Когда пользователь отправляет форму в браузере, файл JavaScript создает ответ, кодирует данные формы
как JSON и отправляет их серверу с применением HTTP-запроса POST.

Данные, возвращаемые сервером, автоматически разбираются `jQuery` и затем используются для
добавления строки в HTML-таблицу.

3. В файл компоновки `_Layout.cshtml` добавлено включение элементов `script` для `jQuery` и `client.js`:
```html
<head>
    ...
    <script src="~/node_modules/jquery/dist/jquery.jquery.min.js"></script>
    <script src="js/client.js"></script>
</head>
```
