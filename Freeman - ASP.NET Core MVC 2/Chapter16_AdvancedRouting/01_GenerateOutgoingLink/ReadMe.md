# Генерирование исходящих URL в представлениях

## Генерирование исходящих ссылок

Нельзя взять и просто так добавить якорный элемент во `View`, содержащий жестко закодированный URL:
```html
`<a href="/Home/CustomVariable">This is an outgoing URL`</a>
```
Если что-то поменяется в структуре приложения, то надо будет руками менять все подобные места.


Строка в `View/Shared/Result.cshtml`, генерирующая якорный элемент:
```html
<a asp-action="CustomVariable">This is an outgoing URL</a>
```

Атрибут `asp-action` применяется для указания имени метода действия, на который должен быть
нацелен URL в атрибуте `href`.

В данном примере:
* при заходе в `HomeController.Index()` ссылка будет направлена на `HomeController.CustomVariable()`
* при заходе в `AdminController.Index() ссылка будет направлена на 'AdminController.CustomVariable()'

Даже с другим маршрутом (см. `Startup.Configure()`) href будет оставаться актуальным.


## Направление на другие контроллеры

Чтобы создать исходящий URL, который направляет на другой контроллер, используется атрибут
`asp-controller`.

Строка в `View/Shared/Result.cshtml`, создающая ссылку на другой контроллер:
```html
<a asp-controller="Admin" asp-action="Index">This targets another controller Admin</a>

<a asp-controller="Customer" asp-action="Index">
    This targets another controller Customer with attribute Route
</a>
```

Можно проверить, что ссылки создаются с учетом правил маршрутизации:

* Для `AdminController.Index()` ссылка выглядит как `/Admin`
(опущен сегмент `Index`, т.к. он задан по умолчанию).

* Для `CustomerController.Index()` ссылка выглядит как `app/Customer/actions/Index`
(из-за атрибута маршрутизации, определенным в `CustomerController`)


## Передача дополнительных значений

Маршрутизации можно передавать значения для переменных сегментов с атрибутами, начинающимися с
`asp-route-`.

Например, здесь (в `View/Shared/Result.cshtml`) используется атрибут `asp-route-id`, чтобы
передать в параметр метода строку:
```html
<a asp-controller="Home" asp-action="CustomVariable" asp-route-id="Hello">
    This target to controller=Home, action=Custom Variable with id=Hello
</a>
```

Из-за всяческих извращений в маршрутизации получается ссылка вида:
`/App/DoCustomVariable?id=Hello`


## Генерирование полностью заданных URL

Пример генерации полностью заданного URL (см. `View/Shared/Result.cshtml`):
```html
<a asp-controller="Home" asp-action="Index" asp-route-id="Hello"
   asp-protocol="https"
   asp-host="myserver.mydomain.com"
   asp-fragment="myFragment">
    This target to fully qualified URL
</a>
```

Из-за всяческих извращений в маршрутизации получается ссылка вида:
`https://myserver.mydomain.com/App/DoIndex?id=Hello#myFragment`

Атрибуты:
* `asp-protocol` - протокол
* `asp-host` - имя сервера
* `asp-fragment` - фрагмент URL

*Внимание!*: не рекомендуется использовать полностью заданные URL, т.к. какие-либо изменения
могут их сломать.


## Генерирование URL из специфического маршрута

Есть маршрут, определенный в (`Statup.Configure()`) под определенным именем (`name = "out"`):
```cs
routes.MapRoute(
    name: "out",
    template: "outbound/{controller=Home}/{action=Index}");
```

Пример генерации URL из специфического маршрута (используется маршрут с именем "out",
см. `View/Shared/Result.cshtml`):
```html
<a asp-route="out">
    This target to generated URL from a specific route
</a>
```

Атрибут `asp-route` указывает на необходимость использования маршрута `out` при генерации URL.

Атрибут `asp-route` может применяться только в ситуации, когда отсутствуют атрибуты `asp-controller`
и `asp-action`. Например, попытка создать ссылку такого вида приводит к исключению:
```html
<a asp-route="out" asp-controller="Home" asp-action="Index">
    This target to generated URL from a specific route
</a>
```


## Генерирование URL (без ссылок)

Пример можно найти в `View/Shared/Result.cshtml`:
```html
<p>URL: @Url.Action("CustomVariable", "Home", new { id = "HelloWorld" })</p>
```

Результат виден на экране как обычный текст ():
```
URL: /App/DoCustomVariable?id=HelloWorld
```

* `Url.Action()` используется для создания URL напрямую.
* 1-й аргумент - указывается метод действия
* 2-й аргумент - указывается контроллер
* 3-й аргумент - указывается значения для любых переменных сегментов


## Генерирование URL в методах действий

Пример использования `Url.Action()` (см. в `AdminController.GenerateUrl()`):
```cs
...
public ViewResult GenerateUrl(int id)
{
    ...
    result.Data["Url"] = Url.Action("GenerateUrl", "Admin", new { id = 100 });
    ...
}
...
```
