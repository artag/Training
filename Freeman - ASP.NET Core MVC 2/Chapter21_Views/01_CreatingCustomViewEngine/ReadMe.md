# Создание специального механизма визуализации

*В большинстве проектов так поступать не придется. Данный раздел для изучения "внутренностей"*.

Механизмы визуализации - классы, реализующие `IViewEngine`.
Определение `IViewEngine`:
```cs
public interface IViewEngine
{
    ViewEngineResult GetView(string executingFilePath, string viewPath, bool isMainPage);
    ViewEngineResult FindView(ActionContext context, string viewName, bool isMainPage);
}
```

Роль механизма визуализации - трансляция запросов к представлениям в объекты `ViewEngineResult`.

Когда инфраструктура MVC нуждается в представлении, она:
* Вызовает `GetView()` - предоставление представления, просто используя его имя.
* Если методу `GetView()` не удалось предоставить представление, то вызывается `FindView()`.

Для создания `ViewEngineResult` используются следующие статические методы:
* `Found(name, view)` - Создает объект `ViewEngineResult`, предоставляет MVC запрошенное
представление, которое указывается в параметре `view`. Представления реализуют `IView`.

* `NotFound(name, locations)` - Создает объект `ViewEngineResult`, который сообщает MVC, что
запрошенное представление не удалось найти. Параметр `locations` - перечисление значений `string`,
описывающих местоположения, в которых механизм визуализации искал представление.

`IView` описывает функциональность, обеспечиваемую представлениями вне зависимости от того,
какой механизм физуализации их создал.
```cs
public interface IView
{
    string Path { get; }
    Task RenderAsync(ViewContext context);
}
```

`Path` - путь к представлению.
`RenderAsync()` - вызывается MVC, генерация ответа клиенту.

Данные контекса `ViewContext` (производный от `ActionContext`) передаются представлению.

Полезные свойства класса `ViewContext`:
* `ViewData` - Возвращает `ViewDataDictionary`, который содержит данные представления,
предоставленные контроллером.

* `TempData` - Возвращает словарь, содержащий временные данные (см. главу 17).

* `Writer` - Возвращает `TextWrtiter`, который применяется для записи вывода из представления.

Полезные свойства `ViewDataDictionary`:

* `Model` - свойство, тип `object`. Данные модели контрллера.

* `ModelMetadata` - свойство, тип `ModelMetadata`. Рефлексия типа данных модели.

* `ModelState` - свойство. Возвращает состояние модели (см. главу 27).

* `Keys` - свойство. Возвращает перечисление значений ключей, которые могут применяться для
доступа к данным *ViewBag*.


## Создание специальной реализации интерфейса `IView`

Из `Infrastructure/DebugDataView`:
```cs
public class DebugDataView : IView
{
    public string Path => string.Empty;

    public async Task RenderAsync(ViewContext context)
    {
        context.HttpContext.Response.ContentType = "text/plain";

        var sb = new StringBuilder();
        sb.AppendLine("---Routing Data---");        // Данные маршрутизации
        foreach (var kvp in context.RouteData.Values)
        {
            sb.AppendLine($"Key: {kvp.Key}, Value: {kvp.Value}");
        }

        sb.AppendLine("---View Data---");           // Данные представления
        foreach (var kvp in context.ViewData)
        {
            sb.AppendLine($"Key: {kvp.Key}, Value: {kvp.Value}");
        }

        await context.Writer.WriteAsync(sb.ToString());
    }
}
```

Когда представление `DebugDataView` визуализируется, оно записывает детали данных маршрутизации и
представления в виде простого текста `text/plain`.

Без указания `Content-Type` по умолчанию применяется `text/html`.


## Создание реализации интерфейса `IViewEngine`

Цель механизма визуализации - выпуск объекта `ViewEngineResult`, который содержит либо
экземпляр реализации `IView`, либо список мест где производился поиск подходящего представления.

Из `Infrastructure/DebugDataViewEngine`:
```cs
public class DebugDataViewEngine : IViewEngine
{
    public ViewEngineResult GetView(string executingFilePath, string viewPath, bool isMainPage)
    {
        var searchedLocations = new string[] { "(Debug View Engine - GetView)" };
        return ViewEngineResult.NotFound(viewPath, searchedLocations);
    }

    public ViewEngineResult FindView(ActionContext context, string viewName, bool isMainPage)
    {
        if (viewName == "DebugData")
        {
            return ViewEngineResult.Found(viewName, new DebugDataView());
        }

        var searchedLocations = new string[] { "(Debug View Engine - FindView)" };
        return ViewEngineResult.NotFound(viewName, searchedLocations);
    }
}
```

`GetView()` здесь всегда возвращает `NotFound`. `FindView()` здесь поддерживает только одно
представление `DebugData` - возращает объект `DebugDataView`.

Метод `ViewEngineResult.NotFound()` предполагает, что механизм визуализации располагает
информацией о местах, в которых необходимо искать представления.


## Регистрация специального механизма визуализации

Производится в `Startup/ConfigureServices()`:
```cs
...
services.Configure<MvcViewOptions>(
    options =>
    {
        options.ViewEngines.Insert(index: 0, item: new DebugDataViewEngine());
    });
...
```

Механизм Razor добавляет в коллекцию `ViewEngine` с помощью `AddMvc()` и стандартный механизм
визуализации дополняется самодельным классом.

Когда MVC получает `ViewResult` от метода действия, она вызывает `FindView()` каждого
механизма визуализации из коллекции `ViewEngines`, до тех пор, пока не получит объект
`ViewEngineResult`.

Порядок вставки механизмов в коллекцию `ViewEngines` важен (в примере, он вставляется вперед,
в индекс 0).


## Удаление других механизмов визуализации

Производится в `Startup/ConfigureServices()` (добавление `options.ViewEngines.Clear();`):
```cs
...
services.Configure<MvcViewOptions>(
    options =>
    {
        options.ViewEngines.Clear();
        options.ViewEngines.Insert(index: 0, item: new DebugDataViewEngine());
    });
...
```


## Тестирование механизма визуализации

Последовательность вызовов:
1. Корневой URL проекта

2. Контроллер `Home`, метод действия `Index()`

3. `Index()` использует `View("DebugData")` для возвращения `ViewResult`

4. MVC обращается к механизмам визуализации и вызывает их методы `FindView()`

5. Механизм визуализации `DebugDataViewEngine` снабжает MVC представлением `DebugDataView`

При запросе `https://localhost:44332/Home/List` вылетит exception, что требуемое представление
не найдено ни DebugDataViewEngine`, ни стандартным механизмом `Razor`.
