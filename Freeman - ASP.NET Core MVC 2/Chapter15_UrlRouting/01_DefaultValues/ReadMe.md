# Введение в шаблоны URL

При обработке запроса система маршртузации заключается в:
1. Сопоставлении запрошенного URL с шаблоном.
2. Извлечении из URL значений для переменных сегментов, определенных в шаблоне.

Переменные сегментов выражаются внутри фигурных скобок ('{' и `}`).

Обычно приложение MVC имеет несколько маршрутов, и система маршрутизации сравнивает входящий URL,
с шаблоном каждого маршрута до тех пор, пока не найдет совпадение.

Простой URL: `http://mysite.com/Admin/Index`
Два сегмента: `Admin` и `Index`
Соответствующий шаблон: `{controller}/{action}`

* Входящий URL: `http://mysite.com/Admin/Index` - `controller` = Admin, `action` = Index
* Входящий URL: `http://mysite.com/Admin` - совпадения нет (сегментов мало)
* Входящий URL: `http://mysite.com/Admin/Index/Soccer` - совпадения нет (сегментов много)

## Создание и регистрация простого маршрута

Маршруты задаются в `Startup.Configure()` и передаются в виде аргумента `UseMvc()`.

Пример базового маршрута (см. `Startup.Configure()`):
```cs
...
app.UseMvc(routes.MapRoute(
               name: "base",
               template: "{controller}/{action}");
...
```

Лямбда-выражение получает объект `IRouteBuilder`, маршруты определяются с помощью `MapRoute()`.
Для облегчения читаемости, в `MapRoute()` указываются имена аргументов:
* `name` - имя маршрута (необязательно, именование даже может стать проблемой)
* `template` - шаблон


## Определение стандартных значений

Стандартное значение используется, когда URL не содержит сегмент, который можно было бы сопоставить
с шаблоном маршрута.

Пример предоставления стандартного значения (см. `Startup.Configure()`):
```cs
...
app.UseMvc(routes.MapRoute(
               name: "default_with_value",
               template: "{controller}/{action}",
               defaults: new { action = "Index" });
...
```

Стандартные значения задаются как свойства анонимного типа, передаваемому `MapRoute()` в аргументе
`defaults`.

Теперь, для URL вида `http://mydomain.com/Home` маршрут извлечет `/Home/Index`.

В этом примере эквивалентно определенному далее маршруту `default_with_embedded_value`.


## Определение встаиваемых стандартных значений

Пример предоставления встраиваемого стандартного значения (см. `Startup.Configure()`):
```cs
...
app.UseMvc(routes.MapRoute(
               name: "default_with_embedded_value",
               template: "{controller}/{action=Index}");
...
```

В этом примере эквивалентно определенному ранее маршруту `default_with_value`.


Этот маршрут (см. `Startup.Configure()`):
```cs
...
routes.MapRoute(
    name: "default",
    template: "{controller=Home}/{action=Index}");
...
```

Этот маршрут будет будет соответствовать URL с нулем, одним или двумя сегментами:
* Сегментов 0: `/` - `controller` = Home, `action` = Index
* Сегментов 1: `/Сustomer` - `controller` = Customer`, `action` = Index
* Сегментов 2: `/Сustomer/List` - `controller` = Customer`, `action` = List
* Сегментов 3: `/Сustomer/List/All` - совпадения нет (сегментов много)
