# Понятие компонентов представлений

Компонент представления - это класс C#, который предлагает частичное представление с необходимыми
ему данными, независимое от родительского представления и от визуализирующего действия.

Оно не может получать HTTP-зпросы, а выдаваемое им содержимое всегда будет включаться в
родительское представление.

Примеры компонентов представлений:
* Инструменты навигации по сайту
* Панели аутентификации

Данные, требуемые для отображения встроенного содержимого не являются частью данных модели,
которые передаются из действия в представление.


## Создание компонента представления

Три способа создания:
1. Определяя компонент представления POCO
2. Наследуя от базового класса `ViewComponent`
3. Применяя атрибут `ViewComponent`


### Создание компонентов представлений POCO

Компонент представления POCO - любой класс с именем, заканчивающийся на "ViewComponent", в котором
определен `Invoke()`.

Компоненты представлений могут располагаться где угодно в проекте, но, по соглашению они
располагаются в директории `Components`, в корне проекта.

Пример POCO компонента представления (см. в `Components/PocoViewComponent`):
```cs
public class PocoViewComponent
{
    private readonly ICityRepository _repository;

    public PocoViewComponent(ICityRepository repository)
    {
        _repository = repository;
    }

    public string Invoke()
    {
        return $"{_repository.Cities.Count()} cities, " +
               $"{_repository.Cities.Sum(c => c.Population)} people";
    }
}
```

Компоненты представлений могут использовать средство внедрения зависимостей.

Для использования компонента представления требуется Razor-выражение:
```
@await Component.InvokeAsync("Poco")
```

Чтобы применить компонент представления, методу `Invoke()` передается в качестве аргумента
его имя (в примере `Poco`). Результат вызова `Poco.Invoke()` вставляется в родительское
представление.

Пример вызова (из `Views/Shared/_Layout.cshtml`):
```html
...
<body class="m-1 p-1">
...
<div class="col col-lg-2">
    @await Component.InvokeAsync("Poco")
</div>
...
</body>
</html>
```

Особенности использования компонентов представлений:
1. Не зависит от родительского представления и HTTP-запроса.
2. Может быть подвергнуто модульному тестированию.
3. Не нарушает форму приложения (не вмешивается в модель родительского представления)


### Наследование от базового класса ViewComponent

Пример (`Components/CitySummary`):
```cs
public class CitySummary : ViewComponent
{
    private readonly ICityRepository _repository;

    public CitySummary(ICityRepository repository)
    {
        _repository = repository;
    }

    public string Invoke()
    {
        return $"{_repository.Cities.Count()} cities, " +
               $"{_repository.Cities.Sum(c => c.Population)} people";
    }
}
```

При наследовании от `ViewComponent` необязательно добавлять в конец слово "ViewComponent".

Пример вызова (из `Views/Shared/_Layout.cshtml`):
```html
...
<body class="m-1 p-1">
...
<div class="col col-lg-2">
    @await Component.InvokeAsync(nameof(CitySummary))
</div>
...
</body>
</html>
```


## Понятие результатов компонентов представлений

Компонент помимо `string` способен возвращать объекты, реализующие `IViewComponentResult`.

Встроенные классы реализации `IViewComponentResult`:
* `ViewViewComponentResult` - представление Razor. Экземпляры данного класса создаются
с применением метода `View()`.

* `ContentViewComponentResult` - текстовый результат, который безопасно кодируется с целью
включения в HTML-документ. Экземпляры данного класса создаются с применением метода `Content()`.

* `HtmlContentViewComponentResult` - фрагмент HTML-разметки, который будет включен в
HTML-документ без добавочного кодирования. Для создания такого типа результата методы в классе
`ViewComponent` не предусмотрены.

Два типа результатов обрабатываются специальным образом:
1. Если возвращается `string`, тогда он используется для создания объекта
`ContentViewComponentResult`.

2. Если возвращается `IHtmlContent`, то он применяется для создания
`HtmlContentViewComponentResult`.


### Возвращение частичного представления

Самый полезный ответ.

Возращаемый объект `View` может иметь четыре версии:
1. `View()` - выбирается стандартное представление для компонента представления,
модель представления не указывается.

2. `View(model)` - выбирается стандартное представление, указанный объект используется в качестве
модели представления.

3. `View(viewName)` - выбирается указанное представление, модель представления не задается.

4. `View(viewName, model)` - выбирается указанное представление, указанный объект используется в
качестве модели представления.

Данные частичного представления (см. `Models/CityViewModel`):
```cs
public class CityViewModel
{
    public int Cities { get; set; }
    public int Population { get; set; }
}
```

Частичное представление (из `Shared/Components/CitySummaryView/Default.cshtml`):
```html
@model CityViewModel

<table class="table table-sm table-bordered">
    <tr>
        <td>Cities:</td>
        <td class="text-right">
            @Model.Cities
        </td>
    </tr>
    <tr>
        <td>Population:</td>
        <td class="text-right">
            @Model.Population.ToString("#.###")
        </td>
    </tr>
</table>
```

Возврат частичного представления из компонента представления (`Components/CitySummaryView`):
```cs
public class CitySummaryView : ViewComponent
{
    ...
    public IViewComponentResult Invoke()
    {
        return View(new CityViewModel
        {
            Cities = _repository.Cities.Count(),
            Population = _repository.Cities.Sum(c => c.Population)
        });
    }
}
```

Использование (в `Views/Shared/_Layout.cshtml`):
```html
...
<body class="m-1 p-1">
...
<div class="col col-lg-2">
    @await Component.InvokeAsync(nameof(CitySummaryView))
</div>
...
</body>
</html>
```


### Возращение фрагментов HTML-разметки

