# Ограничение маршрутов. Встраиваемое ограничение

(Примечание: все маршруты вместе не работают, запускать маршруты лучше по отдельности друг от друга)


## Полный набор встраиваемых ограничений и их эквивалентных классов

| Встраиваемое ограничение  | Описание                                        | Конструктор класса                |
| ------------------------- | ----------------------------------------------- | --------------------------------- |
| alpha                     | Алфавитные символы (A-Z, a-z)                   | `AlphaRouteConstraint()`          |
| bool                      | Тип `bool`                                      | `BoolRouteConstraint()`           |
| datetime                  | Тип `DateTime`                                  | `DateTimeRouteConstraint()`       |
| decimal                   | Тип `decimal`                                   | `DecimalRouteConstraint()`        |
| double                    | Тип `double`                                    | `DoubleRouteConstraint()`         |
| float                     | Тип `float`                                     | `FloatRouteConstraint()`          |
| guid                      | Тип `GUID` (Globally Unique Identifier)         | `GuidRouteConstraint()`           |
| int                       | Тип `int`                                       | `IntRouteConstraint()`            |
| length(len)               | Строковое значение, длиной `len`                | `LengthRouteConstraint(len)`      |
| length(min, max)          | Строковое значение, длиной между `min` и `max`  | `LengthRouteConstraint(min, max)` |
| long                      | Тип `long`                                      | `LongRouteConstraint()`           |
| maxlength(len)            | Строковое значение, длиной не более `len`       | `MaxLengthRouteConstraint()`      |
| max(val)                  | Значение `int`, если оно меньше `val`           | `MaxRouteConstraint()`            |
| minlength(len)            | Строковое значение, длиной равной и более `len` | `MinLengthRouteConstraint(len)`   |
| min(val)                  | Значение `int`, если оно больше `val`           | `MinRouteConstraint(val)`         |
| range(min,max)            | Значение `int`, если оно между `min` и `max`    | `RangeRouteConstraint(min,max)`   |
| regex(expr)               | Регулярное выражение                            | `RegexRouteConstraint(expr)`      |


## Пример встраиваемого ограничения на тип `int`

Из `Startup.Configure()`:
```cs
...
routes.MapRoute(
    name: "IntConstraint",
    template: "{controller=Home}/{action=Index}/{id:int?}");
...
```

Ограничения отделяются от имени переменной двоеточием `:`.
Ограничением является `int`, оно применяется к сегменту `id`.

Это ограничение разрешает сопоставление URL
* либо без сегмента `id` (он необязателен).
* либо только с сегментами `id`, значение которых может быть преобразовано в тип `int`.

* URL: `/` - `controller` = Home, `action` = Index, `id` = null
* URL: `/Home/CustomVariable/Hello` - соответствия нет (сегмент `id` не может быть преобразован в `int`)
* URL: `/Home/CustomVariable/1` - `controller` = Home, `action = `CustomVariable`, `id` = 1
* URL: `/Home/CustomVariable/1/2` - соответствия нет (слишком много сегментов)


## Ограничение с использованием регулярного выражения

Из `Startup.Configure()`:
```cs
...
routes.MapRoute(
    name: "RegexConstraint2",
    template: "{controller:regex(^H.*)=Home}/{action=Index}/{id?}");
...
```

Ограничение - controller начинается с буквы `H`.

Еще один пример из `Startup.Configure()`:
```cs
...
routes.MapRoute(
    name: "RegexConstraint1",
    template: "{controller:regex(^H.*)=Home}/{action:regex(^Index$|^About$)=Index}/{id?}");
...
```

Такое ограничение разрешает соответствовать URL:
* controller начинается с буквы `H`.
* action имеют значения `Index` или `About`.


## Ограничение с использованием ограничений на основе типов и значений

Пример из `Startup.Configure()`:
```cs
...
routes.MapRoute(
    name: "IntRangeConstraint",
    template: "{controller=Admin}/{action=CustomVariable}/{id:range(10,20)?}");
...
```

Маршрут соответствует URL, если он содержит два сегмента и не содержит 'id'.
Если сегмент `id` есть, то маршрут будет соответствовать URL, когда значение `id` может быть
преобразовано в `int` и находится в диапазоне от 10 по 20.


## Объединение ограничений

Надо соединить ограничения в цепочку, отделяя каждое ограничение двоеточием `:`.
```cs
...
routes.MapRoute(
    name: "UnionConstraint",
    template: "{controller=Union}/{action=Index}/{id:alpha:minlength(6)?}");
...
```

Маршрут соответствует URL, если он содержит два сегмента и не содержит 'id'.
К сегменту 'id' применены ограничения `alpha` и `minlength`
Если сегмент `id` есть, то маршрут будет соответствовать URL, когда значение `id`
содержит по крайней мере 6 букв (только букв).


## Определение специального ограничения

В классе `/Infrastructure/WeekDayConstraint` определяется собственное ограничение.
Интерфейс `IRouteConstraint` содержит один метод `Match()`, который решает, должен ли
соответствовать запрос маршруту.

Параметры `Match()`:
* запрос, поступивший от клиента - (`HttpContext httpContext`)
* маршрут - (`IRouter route`)
* имя ограничиваемого сегмента - (`string routeKey`)
* переменные сегментов, извлеченные из URL - (`RouteValueDictionary values`)
* тип URL: входящий или исходящий - (`RouteDirection routeDirection`)

В этом примере с помощью `routeKey` из `values` извлекается значение переменной сегмента,
к которому применяется ограничение, преобразуется в строку нижнего регистра, и проверяется на
соответствие одному из дней недели из статического массива `Days`.

Включение специального ограничения. 2 шага:
1. В `Startup.ConfigureServices()`:
```cs
...
services.Configure<RouteOptions>(
             options => options.ConstraintMap.Add("weekday", typeof(WeekDayConstraint)));
...
```

Здесь конфигурируется объект `RouteOptions`, который управляет рядом линий поведения системы
маршрутизации.
В словарь `ConstraintMap` добавляется новое отображение, поэтому на новое специальное ограничение
`WeekDayConstraint` можно ссылаться встраиваемым образом через `weekday`. (см. след. шаг 2).

2. В `Startup.Configure()`:
```cs
...
routes.MapRoute(
    name: "WeekDayConstraint",
    template: "{controller=WeekDay}/{action=Index}/{id:weekday?}");
...
```
