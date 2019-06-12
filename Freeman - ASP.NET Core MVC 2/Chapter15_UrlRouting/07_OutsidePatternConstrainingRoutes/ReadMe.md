# Ограничение маршрутов. Ограничение указанное за пределами шаблона URL

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


## Пример ограничения на тип `int`

Пример такого ограничения (см. `Startup.Configure()`):
```cs
...
routes.MapRoute(
    name: "IntConstraint",
    template: "{controller}/{action}/{id?}",
    defaults: new { controller="Home", action = "Index" },
    constraints: new { id = new IntRouteConstraint() });
...
```

Ограничения здесь указаны за пределами шаблона с использованием аргумента `constraints` метода
`MapRoute()`.
Объект `IntRouteConstraint` применяется для сегмента `id`, что эквивалентно встраиваемому
ограничению `{id:int?}`.

При использовании такого формата стандартные значения также выражаются внешне (аргумент `defaults`).


## Объединение ограничений

Пример такого ограничения (см. `Startup.Configure()`):
```cs
...
routes.MapRoute(
    name: "UnionConstraint",
    template: "{controller}/{action}/{id?}",
    defaults: new { controller = "Union", action = "Index" },
    constraints: new
    {
        id = new CompositeRouteConstraint(
            new IRouteConstraint[]
            {
                new AlphaRouteConstraint(),
                new MinLengthRouteConstraint(6)
            })
    });
...
```

Здесь конструктор класса `CompositeRouteConstraint` принимает перечисление
объектов `IRouteConstraint`, ограничивающее маршруты.

Маршрут соответствует URL, если он содержит два сегмента и не содержит 'id'.
К сегменту 'id' применены ограничения на алфавитные символы (`AlphaRouteConstraint()`) и
на минимальную длину строки (`MinLengthRouteConstraint()`).
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

Включение специального ограничения см. в `Startup.Configure()`:
```cs
...
routes.MapRoute(
    name: "WeekDayConstraint",
    template: "{controller}/{action}/{id?}",
    defaults: new { controller = "WeekDay", action = "Index" },
    constraints: new { id = new WeekDayConstraint() });
...
```

Маршрут соответствует URL, если он содержит два сегмента и не содержит 'id'.
Если сегмент `id` есть, то маршрут будет соответствовать URL, когда значение `id` совпадает
со днем недели, хранимым внутри `WeekDayConstraint`.
