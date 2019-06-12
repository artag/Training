# Маршрутизация с помощью атрибутов

Маршруты определяются через атрибуты C#, которые применяются напрямую через классы контроллеров.


## Определение марщрута к методу действия

Атрибут `Route` определяет маршрут к методу действия или к контроллеру

В `Controllers/CustomerController` атрибут определен к `Index()` с указанием `"myroute"` в качестве
маршрута:

```cs
[Route("myroute")]
public ViewResult Index()
{
    ...
}
```

Результат:
* Маршрут: `/Customer/List` - используется стандартный маршрут из `Startup`.
* Маршрут: `/myroute` - используется маршрут из атрибута.
* Маршрут: `/Customer/Index` - теперь не будет достижим (*!*).


## Изменение имени метода действия

Из `Controllers/CustomerController`:
```cs
[Route("[controller]/MyAction")]
public ViewResult Index2()
{
    ...
}
```

Результат:
* Маршрут: `/Customer/List` - URL нацелен на `List()`.
* Маршрут: `/Customer/MyAction` - URL нацелен на `Index()`


## Создание более сложного маршрута

Атрибут `Route` можно применять к классу контроллера. См. класс `Controllers/AdminController`:
```cs
[Route("app/[controller]/actions/[action]/{id?}")]
public class AdminController : Controller
{
    ...
}
```

Здесь:
* Смесь из статических и переменных сегментов
* Маркеры `[controller]` и `[action]`

Результат:
* Маршрут: `app/admin/actions/index` - URL нацелен на `AdminController.Index()`.
* Маршрут: `app/admin/actions/index/myid`  - URL нацелен на `AdminController.Index()`
с необязательным сегментом `id`.
* Маршрут: `app/admin/actions/list` - URL нацелен на `AdminController.List()`
* Маршрут: `app/admin/actions/list/myid` - URL нацелен на `AdminController.List()`
с необязательным сегментом `id`.


## Применение ограничений к маршрутам

(Примечание: здесь будет показан пример использования специального ("самодельного") ограничения
`WeekDayConstraint`).

Атрибут `Route` можно применять с ограничениями, как было описано ранее в этой главе (15).
См. пример задания маршрута для класса `Controllers/HomeController`:
```cs
[Route("app/[controller]/actions/[action]/{id:weekday?}")]
public class HomeController : Controller
{
    ...
}
```

Все работает как обычно: можно использовать все доступные ограничения, специальные ограничения,
использовать несколько ограничений (цепочка ограничений, отделяются друг от друга `:`).
