# Форматирование содержимого

Формат содержимого, выбираемый MVC, зависит от 4-х факторов:
* форматов, которые клиент будет принимать
* форматов, которые MVC может выпускать
* политики содержимого, указанной действием
* типа, возвращаемого методом действия


## Стандартная политика содержимого

*Подходит для большинства приложений*

Ни клиент, ни метод действия не накладывает ограничения на форматы, которые можно использовать.
Результат прост и предсказуем:
* Если метод возвращает *string*, то он отправляется клиенту неизменным, а заголовок `Content-Type`
ответа устанавливается в `text/plain`.
* Для всех остальных типов данных, данные форматируются как JSON, а заголовок `Content-Type`
ответа устанавливается в `application/json`.

Пример (вызовы методов из API контроллера из `Controller/ContentController`):
Команда (в PowerShell):
```
Invoke-WebRequest http://localhost:5000/api/content/string |
select @{n='Content-Type'; e={ $_.Headers."Content-Type" }}, Content
```

Отвечает метод контроллера:
```cs
[Route("api/[controller]")]
public class ContentController : Controller
{
    [HttpGet("string")]
    public string GetString() => "This is a string response";
    ...
}
```

Отзыв:
```
Content-Type              Content
------------              -------
text/plain; charset=utf-8 This is a string response
```

Для отображения результата в формате JSON:
Команда (в PowerShell):
```
Invoke-WebRequest http://localhost:5000/api/content/object |
select @{n='Content-Type';e={$_.Headers."Content-Type"}}, Content
```

Отвечает метод контроллера:
```cs
[Route("api/[controller]")]
public class ContentController : Controller
{
    ...
    [HttpGet("object")]
    public Reservation GetObject() => new Reservation
    {
        ReservationId = 100,
        ClientName = "Joe",
        Location = "Board Room"
    };
}
```

Отзыв:
```
Content-Type                    Content
------------                    -------
application/json; charset=utf-8 {"reservationId":100,"clientName":"Joe","location":"Board Room"}
```


## Согласование содержимого

Большинство клиентов включает в запрос заголовок `Accept`, указывающий набор форматов, которые
он готов получить в ответе, выраженный в виде множества MIME.

Пример. Заголовок `Accept`, который посылает в запросах `Chrome`:
```
Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8
```

Что означает:
* `Chrome` предпочитает получать данные `HTML` или `XHTML`, либо изображения `WEBP`.
* Если эти форматы не доступны, то наиболее предпочтителен `XML` (`q=0.9`).
* Если ни один из вышеперечисленных форматов не доступен, то принимать любой формат.

`q` - относительные предпочтения: `q=1.0` - стандартное.

Если выполнить такую команду:
```
Invoke-WebRequest http://localhost:5000/api/content/object -Headers @{Accept="application/xml"} |
select @{n='Content-Type'; e={ $_.Headers."Content-Type"}}, Content
```

Ответ будет таким (все равно выдается JSON):
```
Content-Type                    Content
------------                    -------
application/json; charset=utf-8 {"reservationId":100,"clientName":"Joe","location":"Board Room"}
```

По умолчанию MVC поддерживает только формат `JSON`, поэтому он всегда посылается в ответе,
вне зависимости от запроса клиента.
Нужно включенить в запрос не только заголовок `Accept`, но и включить форматирование `XML`.


### Включение форматирования XML

В `Startup.ConfigureServices()` добавить:
```cs
// Новый класс сериализации Xml.
services.AddMvc().AddXmlDataContractSerializerFormatters();

// Старый класс сериализации Xml. Для совместимости со старыми клиентами .NET.
// services.AddMvc().AddXmlSerializerFormatters();
}
```

Теперь по этой команде:
```
Invoke-WebRequest http://localhost:5000/api/content/object -Headers @{Accept="application/xml"} |
select @{n='Content-Type'; e={ $_.Headers."Content-Type"}}, Content
```

К клиенту возвращаются данные xml:
```
Content-Type                   Content
------------                   -------
application/xml; charset=utf-8
                 <Reservation xmlns:i="http://www.w3.org/2001/XMLSchema-instance"
                 xmlns="http://schemas.datacontract.org/2004/07/ContentFormatting.Models">
                 <ClientName>Joe</ClientName><Location>Board Room</Location><Rese...
```


## Указание формата данных для действия

Систему согласования содержимого можно переопределить, указав формат данных прямо на методе
действия (см. `Controllers/ContentController`):
```cs
...
[HttpGet("jsonobject")]
[Produces("application/json")]
public Reservation GetJsonObject() => new Reservation
{
    ReservationId = 100,
    ClientName = "Joe",
    Location = "Board Room"
};
```

В аргументах атрибута `Produces` указывается формат, который будет применяться для результата,
возвращаемого действием. Допускается указание расширенных типов.

Атрибут `Produces` принудительно устанавливает формат ответа.

На команду:
```
(Invoke-WebRequest http://localhost:5000/api/content/jsonobject -Headers @{Accept="application/xml"})
.Headers."Content-Type"
```

Возвращается ответ, что в ответе возвращается данные в виде `JSON`:
```
application/json; charset=utf-8
```


## Получение формата данных из маршрута или строки запроса

Иногда удобно указать формат ответа через **маршрут**.

1. Определить сокращенные значения, которые применяются для ссылки на форматы в маршруте
или в строке запроса (см. `Startup.ConfigureServices()`):
```cs
...
services.AddMvc()
    .AddXmlDataContractSerializerFormatters()
    .AddMvcOptions(
        options =>
        {
            options.FormatterMappings.SetMediaTypeMappingForFormat(
                "xml", new MediaTypeHeaderValue("application/xml"));
        }
    );
...
```

Свойство `FormatterMappings` устанавливает и управляет отображениями.

`SetMediaTypeMappingForFormat()` создает отображение: сокращение `xml` будет ссылаться на формат
`application/xml`.

2. Применить атрибут `FormatFilter` к методу действия и применить маршрут (необязательный сегмент
`format`) (см. `Controllers/ContentController`):
```cs
...
[HttpGet("routeobject/{format?}")]
[FormatFilter]
[Produces("application/json", "application/xml")]
public Reservation GetRouteObject() => new Reservation
{
    ReservationId = 100,
    ClientName = "Joe",
    Location = "Board Room"
};
...
```

Команда:
```
(Invoke-WebRequest http://localhost:5000/api/content/routeobject/xml).Headers."Content-Type"
```

Ответ:
```
application/xml; charset=utf-8
```

Команда:
```
(Invoke-WebRequest http://localhost:5000/api/content/routeobject/json).Headers."Content-Type"
```

Ответ:
```
application/json; charset=utf-8
```

Формат, найденный атрибутом `FormatFilter`, переопределяет любые форматы, указанные в заголовке
`Accept`.


## Включение полного согласования содержимого

*В случае содержимого, которое не может быть предоставлено, возвращается ответ `406 - Not Acceptable`*.

Для включения согласования содержимого надо (см. `Startup.ConfigureServices()`):
```cs
services.AddMvc()
    .AddXmlDataContractSerializerFormatters()
    .AddMvcOptions(
        options =>
        {
            options.FormatterMappings.SetMediaTypeMappingForFormat(
                "xml", new MediaTypeHeaderValue("application/xml"));

            options.RespectBrowserAcceptHeader = true;
            options.ReturnHttpNotAcceptable = true;
        }
    );
```

`RespectBrowserAcceptHeader` - полностью ли соблюдается заголовок `Accept`.

`ReturnHttpNotAcceptable` - будет ли клиенту отправляться ответ `406 - Not Acceptable`
(406 - неприемлимо), если подходящий формат отсутствует.

Необходимо также удалить атрибут `Produces` из метода действия, чтобы процесс согласовния
содержимого не переопределялся.

Команда:
```
Invoke-WebRequest http://localhost:5000/api/content/object -Headers @{Accept="application/custom"}
```

Ответ:
```
Invoke-WebRequest : Status Code: 406; Not Acceptable
...
```


## Получение разных форматов данных

Когда клиент посылает данные контроллеру в запросе `POST`, с помощью атрибута `Consumes`
можно указывать разные методы действий для обработки специфических форматов данных.
Из `Controllers/ContentController`:
```cs
[HttpPost]
[Consumes("application/json")]
public Reservation ReceiveJson([FromBody] Reservation reservation)
{
    reservation.ClientName = "Json";
    return reservation;
}

[HttpPost]
[Consumes("application/xml")]
public Reservation ReceiveXml([FromBody] Reservation reservation)
{
    reservation.ClientName = "Xml";
    return reservation;
}
```

`ReceiveJson()` и `ReceiveXml()` принимают запросы `POST`. Отличие между ними связано с форматом
данных, указанным в атрибуте `Consumes`, который исследует заголовок `Content-Type`, чтобы
выяснить, способен ли метод действия обработать запрос.

Если заголовок `Content-Type` будет установлен в `application/json`, то будет применяться
`ReceiveJson()`, если `Content-Type` будет установлен в `application/xml`,
то будет использоваться `ReceiveXml()`.
