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
