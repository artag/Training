# Работа с механизмом Razor

## Прояснение представлений Razor
## Имя класса
## Визуализация представлений

Много текста, см. в книге

## Описание базового класса

Классы представлений наследуются от класса `RazorPage` или `RazorPage<T>`, если с помощью
директивы `@model` был указан тип модели.

Методы и свойства класса `RazorPage<T>`, полезные при разработке представлений:
* `Model` - Свойство. Возвращает данные модели, предоставляемые методом действия.

* `ViewData` - Свойство. Возвращает `ViewDataDictionary`, который обеспечивает доступ к другим данным
представления.

* `ViewContext` - Свойство. Возвращает `ViewContext`.

* `Layout` - Свойство. Применяется для указания компоновки (см. главу 5 и далее в этой главе).

* `ViewBag` - Свойство. Предоставляет доступ к `ViewBag` (см. главу 17).

* `TempData` - Свойство. Предоставляет доступ к `TempData` (см. главу 17).

* `Context` - Свойство. Возвращает `HttpContext`, который описывает текущий запрос и
подготавливаемый ответ.

* `User` - Свойство. Возвращает профиль пользователя, ассоциированного с текущим запросом
(см. главу 28).

* `RenderSection()` - Метод. Вставка раздела содержимого из представления в компоновку (см. далее).

* `RenderBody()` - Метод. Вставка в компоновку всего содержимого представления, которое не
находится внутри раздела (см. далее).

* `IsSectionDefined()` - Метод. Используется для выяснения, определен ли раздел в представлении.


Вспомогательные свойства Razor:
* `HtmlEncoder` - Свойство. Возвращает `HtmlEncoder`, который используется для безопасного
кодирования HTML-содержимого в представлении.

* `Component` - Свойство. Возвращает вспомогательный объект для компонента представления (глава 22).

* `Json` - Свойство. Возвращает вспомогательный объект JSON (см. далее в главе).

* `Url` - Свойство. Возвращает вспомогательный объект URL, который используется для генерации URL,
используя конфигурацию маршрутизации (см. главу 16).

* `Html` - Свойство. Возвращает вспомогательный объект HTML, который используется для генерации
динамического содержимого. Данное средство почти полностью замещено вспомогательными функциями
дескрипторов, но все еще используется для частичных представлений (см. далее в главе).


# Добавление динамического содержимого к представлению Razor

Динамическое содержимое генерируется во время выполнения и может быть разным для каждого запроса.

Способы добавления динамического содержимого:
* Встраиваемый код - Небольшие и самодостаточные порции логики (например, `if` и `foreach`).

* Вспомогательные функции дескрипторов - Генерация атрибутов в HTML-эелментах (см. главы 23-25).

* Разделы - Создание разделов содержимого, которые будут вставляться в специфические места внутри
компоновки (см. далее в главе).

* Частичные представления - Применяются для совместного использования подразделов компоновки
представления несколькими представлениями. Не вызывают какой-либо метод действия (не могут
применяться для выполнения бизнес-логики). См далее в главе.

* Компоненты представлений - Применяются для многократно используемых элементов управления или
виджетов пользовательского интерфейса, которые должны содержать бизнес-логику (см. главу 22).


## Использование разделов компоновки

Определение разделов (см. `Views/Section/Index.cshtml`):
```html
@model string[]
@{
    Layout = "_Layout";
}

@section Header
{
<div>
    --- HEADER ---
    @foreach (var str in new[] { "Home", "List", "Edit" })
    {
        <a asp-action="str">@str</a>
    }
</div>
}

--- BODY ---
This is a list of fruit names:
@foreach (var name in Model)
{
    <span><b>@name</b></span>
}

@section Footer
{
    <div>
        --- FOOTER ---
        This is the footer
    </div>
}
```

Здесь применяется файл компоновки `_Layout.cshtml` (в `Views/Section/_Layout.cshtml`):
```html
<!DOCTYPE html>

<html>

<head>
    <meta name="viewport" content="width=device-width" />
    <title>@ViewBag.Title</title>
</head>

<body>
    @RenderSection("Header")

    <div>
        This is part of the layout
    </div>

    @RenderBody()

    <div>
        This is part of the layout
    </div>

    @RenderSection("Footer")

    <div>
        This is part of the layout
    </div>
</body>
</html>
```

Вместо выражения `@RenderSection` Razor вставляет содержимое из раздела с указанным именем
из представления.

Части представления, которые не содержатся в каком-либо разделе, вставляются в компоновку с
использованием выражения `@RenderBody`.

Обычно разделы не смешиваются с остальной частью представления - они определяются либо в начале,
либо в конце представления.

Ну или определить представление **полностью в терминах разделов** (см. `Views/AllSection/Index.cshtml`):
```html
@model string[]
@{
    Layout = "_Layout";
}

@section Header
{
    ...
}

@section Body
{
    ...
}

@section Footer
{
    ...
}
```

Компоновка (см. `Views/AllSection/_Layout.cshtml`):
```html
...
<body>
    @RenderSection("Header")
    @RenderSection("Body")
    @RenderSection("Footer")
</body>
...
```


## Проверка существования разделов

Можно выполнить проверку, определен ли в представлении специфический раздел из компоновки
(см. `Views/TestingForSection/_Layout.cshtml`):
```html
...
<body>
    @RenderSection("Header")
    @RenderSection("Body")

    @if (IsSectionDefined("Footer"))
    {
        @RenderSection("Footer")
    }
    else
    {
        <h2>No footer section</h2>
    }

</body>
...
```

Вспомогательный метод `IsSectionDefined()` принимает имя проверяемого раздела и возвращает
`true`, если он определен в визуализированном представлении.


## Визуализация необязательных разделов

По умолчанию, представление должно содержать все разделы, для которых в компоновке имеются
выражения `@RenderSection`. Если разделы отсутствуют, тогда MVC сгенерирует исключение.

Чтобы этого избежать, можно задействовать метод `IsSectionDefined()`, либо указать в
`@RenderSection` дополнительный аргумент `false`.

Пример (из `Views/RenderingOptionalSection/_Layout.cshtml`):
```html
...
<body>
    @RenderSection("Header", false)
    @RenderBody()
    @RenderSection("Footer", false)
</body>
...
```


## Использование частичных представлений

**Частичные представления** - отдельные файлы представлений, содержащие фрагменты дескрипторов и
разметку, которые могут быть включены в другие представления.


### Создание частичного представления

Пример страницы частичного представления (`Views/Partial/MyPartial.cshtml`):
```html
<div>
    <div>
        This is the message from the partial view.
    </div>
    <div>
        <a asp-action="Index">This is a link to the Index action</a>
    </div>
</div>
```


### Применение частичного представления

Пример применения частичного представления (`Views/Partial/Index.cshtml`):
```html
...
<body>
    <div>
        This is the Index View of Partial Controller.
    </div>

    @Html.Partial("MyPartial")
    ...
</body>
...
```

`Partial()` - расширяющий метод, применяемый к свойству `Html`. Методу `Partial()` передается
аргумент, указывающий имя частичного представления, содержимое которого вставляется в вывод,
отправляемый клиенту.


## Использование строго типизированных частичных представлений

Пример строго типизированного частичного представления (из `Views/Partial/MyStronglyTypedPartial.cshtml`):
```html
@model IEnumerable<string>

<div>
    This is message from the partial view.
    <ul>
        @foreach (var str in Model)
        {
            <li>@str</li>
        }
    </ul>
</div>
```

Его применение (из `Views/Partial/Index.cshtml`):
```html
...
<body>
    ...
    @Html.Partial("MyStronglyTypedPartial", new string[] { "Apple", "Orange", "Pear"})
    ...
</body>
...
```


## Добавление содержимого JSON в представления

Содержимое JSON часто включается в представления с целью снабжения кода JavaScript клиентской
стороны данными, которые могут применяться при динамической генерации содержимого.

Пример (из `Views/Json/Index.cshtml`):
```html
...
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Razor</title>

    <script id="jsonData" type="application/json">
        @Json.Serialize(new string[] { "Apple", "Orange", "Pear" })
    </script>
    
    <script asp-src-include="lib/jquery/*.min.js"></script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            var list = $("#list")
            JSON.parse($("#jsonData").text()).forEach(function(val) {
                console.log("Val: " + val);
                list.append($("<li>").text(val));
            });
        });
    </script>
</head>

<body>
    <div>
        This is the List View
    </div>

    <div>
        <ul id="list"></ul>
    </div>
</body>
</html>
```

Выражение `@Json.Serialize` принимает объект и сериализирует его в JSON:
```html
<script id="jsonData" type="application/json">
    @Json.Serialize(new string[] { "Apple", "Orange", "Pear" })
</script>
```

Чтобы работать с данными JSON, добавляется библиотека `jQuery` и встраиваемый код JavaScript,
который применяет ее для разбора данных JSON и динамически создает HTML-элементы.
