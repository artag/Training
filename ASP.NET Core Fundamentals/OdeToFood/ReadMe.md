# ASP.NET Core Fundamentals

## 02. Drilling into Data

**02_02**. Создание проекта. Тип проекта `Web Application` (Configure for HTTPS, No Authentication)
Ну или создать с помощью командной строки `dotnet new ...`

**02_03**. Редактирование файла `Pages/Shared/_Layout.cshtml`:
Добавление меню Restaurants (ссылка на `/Restaurants/List`)

**02_04**. Добавление Razor Page (в `/Pages/Restaurants/List.cshtml`).
Добавление заголовка на page:
```html
<h1>Restaurants</h1>
```

**02_05**. Пример использования тулзы (Scaffolding Tools) из командной строки.

`dotnet aspnet-codegenerator`

Пример. Создание Razor Page в директории `\Pages\Restaurants` с именем `List`, пустой (`Empty`),
с useDefaultLayout (`-udl`).
```
dotnet aspnet-codegenerator razorpage List Empty -udl -outDir Pages\Restaurants\
```

**02_06**. Добавление свойства `Message` в `ListModel` и во view (`List.cshtml`).
Во View значение свойства отображается так:
```html
@Model.Message
```

Добавление строки `Message` в конфигурационный файл `appsettings.json`.

Создание конструктора `ListModel` с инжектированным `IConfiguration`.
Добавление в `ListModel`, в метод `OnGet()` чтения строки из конфигурационного файла.

**02_07**. Создание нового проекта `OdeToFood.Core` для Entities.
Создание класса `Restaurant` и перечисления `CuisineType`.

02_08. Создание нового проекта `OdeToFood.Data` для доступа к данным.
Создание интерфейса `IRestaurantData` и его реализации `InMemoryRestaurantData`.

02_09. Регистрация `IRestaurantData` и `InMemoryRestaurantData` в `Startup.ConfigureServices()`
Добавлена передача `IRestaurantData` через конструктор `ListModel` (в `Pages/Restaurants/List.cshtml`).

02_10. Добавление свойства `Restaurants` в класс `ListModel` и инициализация этого свойства.

02_11. Добавление таблицы в `Pages/Restaurants/List.cshtml`, используя данные из `Restaurants`
класса `ListModel`.


## 03. Working with Models and Model Binding

### 03_02. Working with HTML Forms

*Рассказывается про `form`, `get` и `post`. Почему надо использовать для чтения данных `get`,
и только для изменения данных `post`.*

Примеры форм (на слайдах).

`Get`
```cs
<form action="/update" method="get">
    <label for="fname">First Name:</label>
    <input type="text" name="fname" />
    <button type="submit">Save</button>
</form>

GET /update?fname=Scott
```

`Post`
```cs
<form action="/update" method="post">
    <label for="fname">First Name:</label>
    <input type="text" name="fname" />
    <button type="submit">Save</button>
</form>

POST /update
name=Scott
```


### 03_03. Building a Search Form

*Добавление поля для поиска и вывода списка ресторанов, начинающихся с определенных букв.
Перехода на другую страницу нет.*

Добавление в `List.cshtml` form с get.

По сравнению с видео - в bootstrap 4 нет таких иконок.
В bootstrap 3 можно украсить кнопку таким образом:
```html
<span class="input-group-btn">
    <button class="btn btn-primary">
        <i class="glyphicon glyphicon-search"></i>
    </button>
</span>
```

В случае bootstrap 4 для добавления иконок рекомендуют поставить `Font Awesome` или
`Github Octicons`.

#### Добавление пакетов для web через `LibMan`

*Примечание: установилось, но у меня не получилось включить font-awesome в коде.*

[Use LibMan with ASP.NET Core in Visual Studio](https://docs.microsoft.com/en-us/aspnet/core/client-side/libman/libman-vs?view=aspnetcore-2.1)

In Solution Explorer, right-click the project folder in which the files should be added.
Choose Add > Client-Side Library. The Add Client-Side Library dialog appears.
Choose `cdnjs` and set the name of library.

#### Добавление пакетов через `package.json`

*Примечание: из одноименного старого курса Scott Allen*. font-awesome у меня завелся.
1. Создается `package.json` в корне проекта.
2. Создается `Middleware` `ApplicationBuilderExtension`.
3. В `Startup.Configure` добвляется вызов этого middleware.


### 03_04. Finding Restaurants by Name

*Изменение интерфейса в части Data для получения списка ресторанов по имени.*

Меняется `OdeToFood.Data/IRestaurantData`:
вместо `IEnumerable<Restaurant> GetAll();` будет:
```cs
`IEnumerable<Restaurant> GetRestaurantsByName(string name);`
```

Меняется соответственно `OdeToFood.Data/InMemoryRestaurantData`.


### 03_05. Binding to a Query String

*Связывание данных формы и модели. Способ передачи данных из формы через параметр дествия.*

Меняется `Pages/Restaurants/List.cshtml` - в form для input добавился `name`
```html
<input type="search" class="form-control" value="" name="searchTerm" />
```

Меняется `Pages/Restaurants/List.cshtml.cs`. Показывается что можно передать значение из View в Model:
* Через свойство `HttpContext.Request`
* Через параметр метода:
```cs
public void OnGet(string searchTerm)
{
    Restaurants = _restaurantData.GetRestaurantsByName(searchTerm);

    Message = "Hello World!";
    MessageFromConfig = _configuration["Message"];
}
```

Проблема: строка поиска скидывается после вызова поиска.


### 03_06. Using Model Binding and Tag Helpers

*Другой способ привязки модели. Через свойство с атрибутоми tag-helper.*

В `OdeToFood/List.cshtml.cs`:
1. Вводится свойство `SearchTerm`:
```cs
[BindProperty(SupportsGet = true)]
public string SearchTerm { get; set; }
```

`BindProperty` - означает, что свойство может устанавливаться через Http Request до вызова метода
`OnGet()`. По умолчанию, `BindProperty` используется для post-операций, чтобы "включить" свойство
для операций get надо добавить `SupportsGet = true`.

2. И метод `OnGet()` опять становится без параметра:
```cs
public void OnGet()
{
    Restaurants = _restaurantData.GetRestaurantsByName(SearchTerm);

    Message = "Hello World!";
    MessageFromConfig = _configuration["Message"];
}
```

В `Pages/Restaurants/List.cshtml` - в form для input добавился tag-helper (`asp-for`):
```html
<input type="search" class="form-control" asp-for="SearchTerm" />
```

Это автоматом ставит атрибут `value` для input'а.


### 03_07. Building a Detail Page

*Добавление страницы для показа более подробной информации о ресторане.*

Добавление Razor Page для detail view: `Pages/Restaurants/Detail.cshtml`
Модификация model и view.


### 03_08. Linking to the Details

*Подготовка. Добавление параметра id (в `DetailModel.OnGet()`) и кнопки для показа деталей
(в `List.cshtml`).*

1. Меняется `Pages/Restaurants/Detail.cshtml.cs`, в `OnGet` добавляется параметр (пока только
в сигнатуру):
```cs
public void OnGet(int restaurantId)
{
    Restaurant = new Restaurant();
}
```

2. Меняется `Pages/Restaurants/List.cshtml`. Добавление в крайний правый столбец кнопки для
показа деталей для соотв. ресторана:
```html
<td>
    <a class="btn btn-lg" asp-page="./Detail" asp-route-restaurantId="@restaurant.Id">
        <i class="fa fa-search-plus"></i>
    </a>
</td>
```

`asp-route-restaurantId` - "динамический" tag-helper, частица `restaurantId` служит для передачи
одноименного параметра в model.


### 03_09. Specifying Page Routes

*Настройка маршрутизации для более красивого адреса для показа деталей ресторана.*

Делается чтобы получать доступ к деталям ресторана через строку вида `/Restaurants/Detail/2`

Показываются различные способы установки route для Detail view.
В `Pages/Restaurants/Detail.cshtml` меняется `@page` следующим образом:

* по умолчанию, если ничего не укзано, путь к этой странице такой: `/Restaurants/Detail`

* если `@page "/food/place"`, путь к этой странице: `/food/place`

* если `@page "{id}"`, то добавляется дополнительный сегмент: `/Restaurants/Detail/42` или
`/Restaurants/Detail/xyz`

* если `@page "{id:int}"`, то добавляется дополнительный сегмент int типа: `/Restaurants/Detail/42`

* если `@page "{id?:int}"`, то дополнительный сегмент int типа будет опциональным

В нашем случае - `@page "{restaurantId:int}"`


### 03_10. Fetching Restaurants by ID

*Добавление ссылки в `DetailModel` на `IRestaurantData` для получения ресторана по его id.*

1. Модификация (добавление метода `GetById()`) в `IRestaurantData` и его реализацию.
2. Добавление зависимости от `IRestaurantData` через конструктор в `DetailModel`.
3. Добавление вызова `IRestaurantData.GetById()` в `DetailModel.OnGet()`.


### 03_11. Handling Bad Requests

*Добавление обработки ошибки для ресторана с несуществующим id. Добавление перенаправления
на страницу с информацией о "страница не существует".*

1. Модификация `DetailModel.OnGet()`. Добавление проверки на `null` и добавление `RedirectToPage`.
2. Создание `NotFound` Page.


## 04. Editing Data with Razor Pages

*Добавление функциональности для создания и редактирования ресторана*.


### 04_02. Creating the Restaurant Edit Page

*Добавление кнопки на `List.cshtml` для редактирования ресторана и добавление новой
страницы `Edit.cshtml`*.

1. Добавление кнопки на `List.cshtml` для редактирования ресторана. (Рядом с кнопкой Details).

2. Добавление новой страницы `/Pages/Restaurants/Edit.cshtml*.
Для `Edit.cshtml.cs`:
Здесь в `OnGet()` будет в виде параметра передаваться `id` редактируемого ресторана:
```cs
public IActionResult OnGet(int restaurantId)
{
    Restaurant = _restaurantData.GetById(restaurantId);
    if (Restaurant == null)
    {
        return RedirectToPage("./NotFound");
    }

    return Page();
}
```

Для `Edit.cshtml.cs` добавление:
```html
@page "{restaurantId:int}"
...
```


### 04_03. Building an Edit Form with Tag Helpers

*Добавление полей ввода в `Edit.cshtml` (для редактирования деталей ресторана)*.

Особенности:
* Скрытое поле для id:
```html
<input type="hidden" asp-for="Restaurant.Id"/>`
```

* Для редактирования `Name` и `Location`:
```html
<div class="form-group">
    <label asp-for="Restaurant.Name"></label>
    <input asp-for="Restaurant.Name" class="form-control" />
</div>
```

* Для выбора `Cuisine` - выпадающий список.
```html
<div class="form-group">
    <label asp-for="Restaurant.Cuisine"></label>
    <select asp-for="Restaurant.Cuisine" class="form-control"
            asp-items="Model.Cuisines"></select>
</div>
```

`select` - выпадающий список, `asp-items` - элементы в выпадающем списке
(все элементы перечисления `CuisineType`.

Для `Edit.cshtml.cs`:
1. Добавление зависимости класса от `IHtmlHelper`.
2. Ввод свойства `public IEnumerable<SelectListItem> Cuisines { get; set; }` - для выбора
`CuisineType`.
3. Инициализация поля в OnGet(), используя `IHtmlHelper`:
```cs
public IActionResult OnGet(int restaurantId)
{
    Cuisines = _htmlHelper.GetEnumSelectList<CuisineType>();
    ...
}
```


### 04_04. Model Binding an HTTP POST Operation

*Добавление операции POST нажатии на кнопку `Save` в `Edit.cshtml`*.

1. Изменение `IRestaurantData`. Добавление:
* `Restaurant Update(Restaurant updatedRestaurant);`
* `int Commit();` - метод для сохранения изменений в БД (в `InMemoryRestaurantData` не исаользуется)

2. Реализация добавленных методов в `InMemoryRestaurantData`.

3. Добавление `OnPost` в `Edit.cshtml.cs`:
```cs
public IActionResult OnPost()
{
    _restaurantData.Update(Restaurant);
    _restaurantData.Commit();
    return Page();
}
```

И установка атрибута для свойства `Restaurant` (вкл. поддержки для передачи в POST):
```cs
[BindProperty]
public Restaurant Restaurant { get; set; }
```

Есть несколько недочетов после нажатия на кнопку `Save` в `Edit.cshtml`:
1. Пропадание типа кухни в выпадающем списке.
2. Нет валидации вводимых данных (проверка и отображение предупреждений на форме).


### 04_05. Adding Validation Checks

*Исправление недочетов, оставшихся из предыдущего пункта*.

Пропадание типа кухни в выпадающем списке лечится добавлением установки значения для свойства
`Cuisines` в `Edit.OnPost()`:
```cs
public IActionResult OnPost()
{
    Cuisines = _htmlHelper.GetEnumSelectList<CuisineType>();
    ...
}
```


#### Добавление валидации
Валидацию данных можно добавить так:
1. Либо добавив руками кучу проверок в `Edit.OnPost()` (не рекомендуется!).

2.1 Добавить в класс `Restaurant` нужные атрибуты, которые задают ограничения на вводимые данные.
(*Примечание: можно также в `Restaurant` реализовать `IValidatableObject` (в видео не показано)*).


### 04_06. Using Model State and Showing Validation Errors

*Продолжение реализации валидации, оставшейся из предыдущего пункта*.

2.2 В `Edit.OnPost()` добавляется проверка статуса `ModelState` - это словарь, содержащий данные
о валидности данных модели.

Можно обратиться напрямую: `ModelState["Location"]...` или как в видео:
```cs
if (ModelState.IsValid)
{
    ...
}
```

#### Добавление валидации на форму

Для проверямых полей ввода добавляется следущее (элемент `span` - подпись снизу):
```html
<input asp-for="Restaurant.Name" class="form-control" />
<span asp-validation-for="Restaurant.Name" class="text-danger" ></span>
```

Проблема: возможность повторной отправки POST при обновлении страницы.


### 04_07. Following the POST-GET-REDIRECT Pattern

*Решение повторной отправки POST - использование паттерна POST-GET-REDIRECT*.

При успешном сохранении данных в `Edit.OnPost()` добавляется Redirect на `Detail.cshtml`:
```cs
public IActionResult OnPost()
{
    if (ModelState.IsValid)
    {
        _restaurantData.Update(Restaurant);
        _restaurantData.Commit();
        return RedirectToPage("./Detail", new { restaurantId = Restaurant.Id });
    }

    Cuisines = _htmlHelper.GetEnumSelectList<CuisineType>();
    return Page();
}
```


### 04_08. Building a Create Restaurant Page

*Построение страницы для создания новой страницы (на самом деле используя `Edit.cshtml`)*.

1. Добавление в `List.cshtml` ссылки в виде кнопки на `Edit.cshtml`.

2. Добавление в `Edit` возможность приема Nullable int id.
2.1. В `Edit.cshtml` - сверху:
```html
@page "{restaurantId:int?}"
...
```

Если урл заканчивается на `/Edit`, то будет редактирование для новой страницы, если на `/Edit/1`, 
то будет редактирование уже существующего ресторана.

2.2. Модификация `Edit.OnGet()`:
```cs
public IActionResult OnGet(int? restaurantId)
{
    Cuisines = _htmlHelper.GetEnumSelectList<CuisineType>();

    Restaurant = restaurantId.HasValue
        ? _restaurantData.GetById(restaurantId.Value)   // Редактирование существующего ресторана
        : new Restaurant();                             // Создание нового ресторана

    if (Restaurant == null)
    {
        return RedirectToPage("./NotFound");
    }

    return Page();
}
```


### 04_09. Adding Create to the Data Access Service

*Изменение интерфейса `IRestaurantData`. Добавление нового метода `Add`*.

1. Добавление нового метода в `IRestaurantData`: `Restaurant Add(Restaurant newRestaurant);`.
2. Добавление нового метода в `InMemoryRestaurantData`.


### 04_10. Handling Create vs. Update Logic

*Модификация `OnPost()` метода для возможности подтверждения редактирования/создания страницы*.


### 04_11. Confirming the Last Operation

*Добавление подтверждения сохранения, используя `TempData`*.

1. Добавление создания `TempData["Message"]` в `Edit.OnPost()`.

2. Добавление свойства в `Detail.cshtml.cs`:
```cs
[TempData]
public string Message { get; set; }
```

3. Добавление показа сообщения в случае наличия этого сообщения в `Detail.cshtml`:
```html
...
@if (Model.Message != null)
{
    <div class="alert alert-info">
        @Model.Message
    </div>
}
...
```



