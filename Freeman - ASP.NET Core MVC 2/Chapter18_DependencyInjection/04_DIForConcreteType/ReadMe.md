# Использование внедрения зависимостей для конкретных типов

Здесь рассматривается использование средства внедрения зависимостей для конкретных типов,
доступ к которым не производится через интерфейсы.

В проект добавлен класс `ProductTotalizer`, который зависит от `IRepository`.

Конфигурация для поставщика служб выглядит так (из `Startup.ConfigureServices()`):
```cs
services.AddTransient<IRepository, MemoryRepository>();
services.AddTransient<IModelStorage, DictionaryStorage>();
services.AddTransient<ProductTotalizer>();
```

1. Здесь `InjectionController` зависит от `IRepository` и `ProductTotalizer`.
2. `ProductTotalizer` зависит от `IRepository`.
3. `MemoryRepository` (реализация `IRepository`) зависит от `IModelStorage`.

Также, в `InjectionController.Index()` используется `ViewBag` для передачи во View значения
общей суммы:
```cs
public ViewResult Index()
{
    ViewBag.Total = _totalizer.Total;
    return View("List", _repository.Products);
}
```

Во `View/Shared/List` значение из `ViewBag` "достается" так
(таблица отображается только при наличии во `ViewBag` каких-либо значений):
```cs
@if (ViewData.Count > 0)
{
    <table>
        @foreach (var kvp in ViewData)
        {
            <tr>
                <td>@kvp.Key</td><td>@kvp.Value</td>
            </tr>
        }
    </table>
}
```
