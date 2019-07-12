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
/Views/Home/_Layout.cshtml
/Views/Common/_Layout.cshtml
/Pages/Common/_Layout.cshtml
/Views/Legacy/Home/_Layout/View.cshtml
```

Напомню, места по умолчанию, где производится поиск представления:
```
/Views/Expander/MyView.cshtml
/Views/Shared/MyView.cshtml
/Pages/Shared/MyView.cshtml
```


## Выбор специфических представлений для запросов

