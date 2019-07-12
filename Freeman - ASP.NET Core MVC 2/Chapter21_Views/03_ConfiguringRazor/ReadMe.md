# Конфигурирование механизма Razor

Механизм Razor можно конфигурировать с применением класса *RazorViewEngineOptions*.
Класс определяет два свойства:
* `FileProvider` - Установка объекта, который предоставляет механизму Razor содержимое файлов и
каталогов. Функциональность определяется `IFileProvider`, его стандартной реализацией является
класс `PhysicalFileProvider`, который обеспечивает чтение файлов из диска.

* `ViewLocationExpanders` - применяется при конфигурировании расширителей местоположений
представлений, которые используются для изменения способа поиска представления механизмом Razor.

`FileProvider` используется только для загрузки представлений, чтобы их можно было скомпилировать,
когда приложение запускается в первый раз. Чтение файлов представлений из диска является в точности
тем, что требуется в большинстве проектов. Поэтому вероятность изменений в `FileProvider` очень мала.

Более полезно `ViewLocationExpanders`.


## Расширители местоположений представлений

Реализуют `IViewLocationExpander`:
```cs
public interface IViewLocationExpander
{
    void PopulateValues(ViewLocationExpanderContext context);

    IEnumerable<string> ExpandViewLocations(
        ViewLocationExpanderContext context, IEnumerable<string> viewLocations);
}
```


### Создание простого расширителя местоположений представлений

Простейший расширитель местоположений представлений всего лишь изменяет набор мест, где Razor
проводит поиск представлений. Для этого надо реализовать `ExpandViewLocations()` и возвратить
список местоположений, которые должны поддерживаться (из `Infrastructure/SimpleExpander.cs`):
```cs
public class SimpleExpander : IViewLocationExpander
{
    public void PopulateValues(ViewLocationExpanderContext context)
    {
    }

    public IEnumerable<string> ExpandViewLocations(
        ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        foreach (var location in viewLocations)
        {
            yield return location.Replace("Shared", "Common");
        }

        yield return "/Views/Legacy/{1}/{0}/View.cshtml";
    }
}
```

Механизм Razor вызывает `ExpandViewLocations()`, когда ему требуется список местоположений поиска,
и предоставляет стандартные местоположения как последовательность строк в параметре `viewLocations`.

Местополжения выражаются в виде шаблонов с заполнителями для метода действия и контроллера.

Шаблоны местоположений по умолчанию:
```
/Views/{1}/{0}.cshtml
/Views/Shared/{0}.cshtml
```

`{0}` - заполнитель для метода действия, `{1}` - заполнитель для контроллера


### Применение расширителя местоположений представлений

Механизм Razor конфигурируется в `Startup.ConfigureServices()`:
```cs
services.Configure<RazorViewEngineOptions>(
    options =>
    {
        options.ViewLocationExpanders.Add(new SimpleExpander());
    });
```

Свойство `ViewLocationExpanders` возвращает `List<IViewLocationExpander`, на котором вызывается
метод `Add()`.

После запуска появится exception с перечнем мест, где производился поиск представления:
```
/Views/Expander/MyView.cshtml
/Views/Common/MyView.cshtml
/Pages/Common/MyView.cshtml
/Views/Legacy/Expander/MyView/View.cshtml
```

Напомню, места по умолчанию, где производится поиск представления:
```
/Views/Expander/MyView.cshtml
/Views/Shared/MyView.cshtml
/Pages/Shared/MyView.cshtml
```


## Выбор специфических представлений для запросов

Каждый раз, когда Razor требуется представление, он вызывает метод `PopulateValues()`,
передавая `ViewLocationExpanderContext` для данных контекста.

Свойства `ViewLocationExpanderContext`:
* `ActionContext` - возвращает `ActionContext`. Описывает метод действия, запросивший представление,
включает детали запроса и ответа.

* `ViewName` - возвращает имя представления, которое запросил метод действия.

* `ControllerName` - возвращает имя контроллера, который содержит метод действия.

* `AreaName` - возвращает имя области, содержащей контроллер, если области были определены.

* `IsMainPage` - возвращает `false`, если Razor ищет частичное представление,
`true` - в противном случае.

* `Values` - возвращает `IDictionary<string, string>`, к которому расширитель местоположений
представлений добавляет пары "ключ-значение", уникально идентифицирующие категорию запроса
(см. далее).

Пример (из `Infrastructure/ColorExpander.cs`):
```cs
public class ColorExpander : IViewLocationExpander
{
    private static Dictionary<string, string> Colors = new Dictionary<string, string>
    {
        ["red"] = "Red", ["green"] = "Green", ["blue"] = "Blue",
    };

    public void PopulateValues(ViewLocationExpanderContext context)
    {
        var routeValues = context.ActionContext.RouteData.Values;

        if (routeValues.ContainsKey("id") &&
            Colors.TryGetValue(routeValues["id"] as string, out string color) &&
            !string.IsNullOrEmpty(color))
        {
            context.Values["color"] = color;
        }
    }

    public IEnumerable<string> ExpandViewLocations(
        ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        context.Values.TryGetValue("color", out var color);
        foreach (var location in viewLocations)
        {
            if (!string.IsNullOrEmpty(color))
            {
                yield return location.Replace("{0}", color);
            }
            else
            {
                yield return location;
            }
        }
    }
}
```

`PopulateValues()` использует `ActionContext` для получения данных маршрутизации и ищет значение
сегмента `id` в URL. Если есть сегмент `id` и его значением является `red`, `green` или `blue`,
тогда расширитель местоположений представлений добавляет в словарь `Values` ключ `color`.

`ExpandViewLocations()` генерирует набор мест с учетом наличия ключа `color` в словаре `Values`.

Включение в использование класса `ColorExpander` (в `Startup.ConfigureServices()`):
```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc();
    services.Configure<RazorViewEngineOptions>(
        options =>
        {
            options.ViewLocationExpanders.Add(new SimpleExpander());
            options.ViewLocationExpanders.Add(new ColorExpander());
        });
}
```

Порядок регистрации расширителей местоположений представлений важен, т.к. каждый расширитель передает
следующему набор маршрутов.

Теперь, запустив с URL вида `https://localhost:44343/Expander/Index/blue` можно получить такие
маршруты поиска:
```
/Views/Expander/Blue.cshtml
/Views/Common/Blue.cshtml
/Pages/Common/Blue.cshtml
/Views/Legacy/Expander/Blue/View.cshtml
```
