# ASP.NET Core Fundamentals

## 02. Drilling into Data

#### 02_02. Creating the New Project
Создание проекта. Тип проекта `Web Application` (Configure for HTTPS, No Authentication)

Ну или создать с помощью командной строки `dotnet new ...`


#### 02_03. Editing Razor Pages
Редактирование файла `Pages/Shared/_Layout.cshtml`:

Добавление меню Restaurants (ссылка на `/Restaurants/List`)


#### 02_04. Adding a Razor Page
Добавление Razor Page (в `/Pages/Restaurants/List.cshtml`).

Добавление заголовка на page:
```html
<h1>Restaurants</h1>
```


#### 02_05. Using the Scaffolding Tools
Пример использования тулзы (Scaffolding Tools) из командной строки.

`dotnet aspnet-codegenerator`

Пример. Создание Razor Page в директории `\Pages\Restaurants` с именем `List`, пустой (`Empty`),
с useDefaultLayout (`-udl`).
```
dotnet aspnet-codegenerator razorpage List Empty -udl -outDir Pages\Restaurants\
```


#### 02_06. Injecting and Using Configuration
Добавление свойства `Message` в `ListModel` и во view (`List.cshtml`).

Во View значение свойства отображается так:
```html
@Model.Message
```

Добавление строки `Message` в конфигурационный файл `appsettings.json`.

Создание конструктора `ListModel` с инжектированным `IConfiguration`.
Добавление в `ListModel`, в метод `OnGet()` чтения строки из конфигурационного файла.


#### 02_07. Creating an Entity
Создание нового проекта `OdeToFood.Core` для Entities.

Создание класса `Restaurant` и перечисления `CuisineType`.


#### 02_08. Building a Data Access Service
Создание нового проекта `OdeToFood.Data` для доступа к данным.

Создание интерфейса `IRestaurantData` и его реализации `InMemoryRestaurantData`.

Было показано, как "руками", без VS, можно добавить ссылку в `*.csproj` на другой проект в солюшене.


#### 02_09. Registering a Data Service
Регистрация `IRestaurantData` и `InMemoryRestaurantData` в `Startup.ConfigureServices()`
```csharp
services.AddSingleton<IRestaurantData, InMemoryRestaurantData>();
```
Время жизни - один объект на всем протяжении работы приложения (`AddSingleton`).

Предупреждение, что данная коллекция только для целей разработки и не годится для работы в
многопоточном режиме (т.к. внутренний `List` не поддерживает).

Добавлена передача `IRestaurantData` через конструктор `ListModel`
(в `Pages/Restaurants/List.cshtml`).


#### 02_10. Building a Page Model
Добавление свойства `Restaurants` в класс `ListModel` и инициализация этого свойства.


#### 02_11. Displaying a Table of Restaurants
Добавление таблицы в `Pages/Restaurants/List.cshtml`, используя данные из
`Restaurants` класса `ListModel`.


## 03. Working with Models and Model Binding

#### 03_02. Working with HTML Forms

*Рассказывается про `form`, `get` и `post`. Почему надо использовать для чтения данных (или для
поиска на странице) `get`, и только для изменения данных `post`.*

Примеры форм (на слайдах).

`Get`
```html
<form action="/update" method="get">
    <label for="fname">First Name:</label>
    <input type="text" name="fname" />
    <button type="submit">Save</button>
</form>

GET /update?fname=Scott
```

`Post`
```html
<form action="/update" method="post">
    <label for="fname">First Name:</label>
    <input type="text" name="fname" />
    <button type="submit">Save</button>
</form>

POST /update
name=Scott
```


#### 03_03. Building a Search Form

*Добавление поля для задания поиска списка ресторанов, начинающихся с определенных букв.
Перехода на другую страницу нет.*

Добавление в `List.cshtml` form с `get`.

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


##### Добавление пакетов для web через `LibMan`

[Use LibMan with ASP.NET Core in Visual Studio](https://docs.microsoft.com/en-us/aspnet/core/client-side/libman/libman-vs?view=aspnetcore-2.1)

In Solution Explorer, right-click the project folder in which the files should be added.
Choose Add > Client-Side Library. The Add Client-Side Library dialog appears.
Choose `cdnjs` and set the name of library.

В файл `Pages\Shared\_Layout.cshtml` добавить строку:
```html
<environment include="Development">
    ...
    <link rel="stylesheet" href="~/font-awesome/css/all.min.css" />
</environment>
```


##### Добавление пакетов через `package.json`

*Примечание: из одноименного старого курса Scott Allen*. font-awesome у меня завелся.
1. Создается `package.json` в корне проекта.
2. Создается `Middleware` `ApplicationBuilderExtension`.
3. В `Startup.Configure` добавляется вызов этого middleware.


#### 03_04. Finding Restaurants by Name

*Изменение интерфейса в части Data для получения списка ресторанов по имени.*

Меняется `OdeToFood.Data/IRestaurantData`:
вместо `IEnumerable<Restaurant> GetAll();` будет:
```csharp
`IEnumerable<Restaurant> GetRestaurantsByName(string name);`
```

Меняется соответственно `OdeToFood.Data/InMemoryRestaurantData`:
```csharp
public IEnumerable<Restaurant> GetRestaurantsByName(string name = null)
{
    ...
}
```
Если ничего не передается, то `name == null` и выводятся все рестораны.


#### 03_05. Binding to a Query String

*Связывание данных формы и модели. Способ передачи данных из формы в модель через параметр дествия.*

Меняется `Pages/Restaurants/List.cshtml` - в form для input добавился `name`
```html
<input type="search" class="form-control" value="" name="searchTerm" />
```

Меняется `Pages/Restaurants/List.cshtml.cs`. Показывается что можно передать значение из View в Model.
"Model Binding" автоматом ищет в:
* Свойствах `HttpContext.Request.Headers`, `HttpContext.Request.Query`,
`HttpContext.Request.Query` `

* В параметре метода (в примере OnGet):
```csharp
public void OnGet(string searchTerm)
{
    Restaurants = _restaurantData.GetRestaurantsByName(searchTerm);
}
```
Все нормально ищется. Но, проблема: строка поиска скидывается после вызова поиска.


#### 03_06. Using Model Binding and Tag Helpers

*Другой способ привязки модели. Рассматривается два способа передачи данных из формы в модель и
назад.*


##### 1 способ задания Model Binding (в итоге, не применяется)

*Из View (form) в Model передается через параметр метода `OnGet`, обратно, через свойство
Model `SearchTerm`. Во View (form) при задании value ставится `@Model.SearchTerm`.*

Файл `ListModel`:
```csharp
...
// Используется как output Model, для установки значения value (см. ниже)
public string SearchTerm { get; set; }
...
public void OnGet(string searchTerm)
{
    SearchTerm = searchTerm;
    Restaurants = _restaurantData.GetRestaurantsByName(searchTerm);
}
```

В файле `ListView` надо задать value через `@Model.SearchTerm`:
```html
<form method="get">
    <div class="form-group">
        <div class="input-group">
            <input type="search" class="form-control"
                   value=@Model.SearchTerm name="searchTerm"/>
                <button class="btn btn-light">
                    <i class="fas fa-search"></i>
                </button>
        </div>
    </div>
</form>
```


##### 2 способ задания Model Binding (более легкий)

*Во View (form) используется tag-helper `asp-for`, в Model используется свойство со специальным
атрибутом*.

В `OdeToFood/List.cshtml.cs` (Model):
1. Вводится свойство `SearchTerm`:
```csharp
[BindProperty(SupportsGet = true)]
public string SearchTerm { get; set; }
```

`BindProperty` - означает, что свойство может устанавливаться через Http Request до вызова метода
`OnGet()`. По умолчанию, `BindProperty` используется для post-операций, чтобы "включить" свойство
для операций get надо добавить `SupportsGet = true`.

2. И метод `OnGet()` опять становится без параметра:
```csharp
public void OnGet()
{
    Restaurants = _restaurantData.GetRestaurantsByName(SearchTerm);

    Message = "Hello World!";
    MessageFromConfig = _configuration["Message"];
}
```

В `Pages/Restaurants/List.cshtml` (View) - в form для input добавился tag-helper (`asp-for`):
```html
...
<input type="search" class="form-control" asp-for="SearchTerm" />
...
```

Tag-helper `asp-for` означает, что ввод предназначен для определенного свойства в Model.
`asp-for` неявно задает атрибуты `value` и `name` элемента `input`.

*Примечание*: `SearchForm` пишется без "приставки" `Model` (она автоматом подразумевается).


#### 03_07. Building a Detail Page

*Добавление страницы для показа более подробной информации о ресторане.*

Добавление Razor Page для detail view: `Pages/Restaurants/Detail.cshtml`
Модификация Model и View. В Model добавляется свойство `Restaurant`, во View через
это свойство выводится информация о выбранном ресторане.

Во View tag-helper `asp-page` (применяется в anchor-tag `<a>`) содержит линк на другую Razor Page.
Пример (линк в виде кнопки на страницу `List`):
```html
<a asp-page="./List" class="btn btn-outline-dark">All Restaurants</a>
``` 


#### 03_08. Linking to the Details

*Подготовка. Добавление параметра id (в `DetailModel.OnGet()`) и кнопки для показа деталей
(в `List.cshtml`).*

1. Меняется `Pages/Restaurants/Detail.cshtml.cs`, в `OnGet` добавляется параметр (пока только
в сигнатуру):
```csharp
public void OnGet(int restaurantId)
{
    Restaurant = new Restaurant();
    Restaurant.Id = restaurantId;
}
```

2. Меняется `Pages/Restaurants/List.cshtml`. Добавление в крайний правый столбец большой кнопки
для показа деталей для соотв. ресторана:
```html
<td>
    <a class="btn btn-outline-secondary btn-lg">
        <i class="fas fa-search-plus"></i>
    </a>
</td>
```

В видео показывается, что в anchor-tag `<a>` можно добавить жестко-заданный линк (`href`):
```html
<a class="btn btn-outline-secondary btn-lg"
   href="/Restaurants/Detail?restaurantId=@restaurant.Id">
    ...
</a>
```

Но рекомендуется использовать tag-helper `asp-route-имя_передаваемого_параметра`
(вместе с `asp-page`). Пример:
```html
<a class="btn btn-outline-secondary btn-lg"
   asp-page="./Detail"
   asp-route-restaurantId="@restaurant.Id">
    <i class="fas fa-search-plus"></i>
</a>
```

`asp-route-restaurantId` - "динамический" tag-helper, частица `restaurantId` служит для передачи
одноименного параметра в Model.

Линк на страницу `Detail` передается примерно в таком виде:
```
https://localhost:44364/Restaurants/Detail?restaurantId=2
```
В следующем разделе будет объяснено, как сделать обращение к Detail по такому линку:
```
https://localhost:44364/Restaurants/Detail/2
```


#### 03_09. Specifying Page Routes

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


#### 03_10. Fetching Restaurants by ID

*Добавление ссылки в `DetailModel` на `IRestaurantData` для получения ресторана по его id.*

1. Модификация (добавление метода `GetById()`) в `IRestaurantData` и его реализацию.
`GetById` может возвратить null, если ресторан с таким Id не найден.

2. Добавление зависимости от `IRestaurantData` через конструктор в `DetailModel`.

3. Добавление вызова `IRestaurantData.GetById()` в `DetailModel.OnGet()` и присвоение
полученного ресторана свойству `Restaurant`, которое используется для показа во View.

Проблема: при попытке вызова Detail для несуществующего Id приложение крашится с NRE.
Исправление будет далее.


#### 03_11. Handling Bad Requests

*Добавление обработки ошибки для ресторана с несуществующим id.
Добавление перенаправления на страницу с информацией о "страница не существует".
Или возвращается текущий View, если ресторан найден.*

Не рекомендуется добавлять проверки на null-refence во View:
```csharp
@if(...)
{
    ...
}
```

Проверку на null надо делать на месте, в Model.

1. Модификация `DetailModel.OnGet()`.
  * Замена возвращаемого типа `void` на `IActionResult`.
  * Добавление проверки на `null` и добавление `RedirectToPage`.
```csharp
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

2. Создание `NotFound` Page в `Pages/Restaurants/NotFound.cshtml`. Просто View, без Model.


## 04. Editing Data with Razor Pages

*Добавление функциональности для создания и редактирования ресторана*.


#### 04_02. Creating the Restaurant Edit Page

*Добавление кнопки на `List.cshtml` для редактирования ресторана и добавление новой
страницы `Edit.cshtml`*.

1. Добавление кнопки на View `List.cshtml` для редактирования ресторана. (Рядом с кнопкой Details).

2. Добавление новой страницы Razor Page в `/Pages/Restaurants/Edit.cshtml`.

Для `Edit.cshtml.cs`: здесь в `OnGet()` будет в виде параметра передаваться `id` редактируемого
ресторана:
```csharp
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

Для `Edit.cshtml.cs` добавление (добавление "красивого" роутинга - см. пред. раздел):
```html
@page "{restaurantId:int}"
...
```


#### 04_03. Building an Edit Form with Tag Helpers

*Добавление полей ввода в `Edit.cshtml` (для редактирования деталей ресторана)*.

Особенности:
* Скрытое поле для id:
```html
<input type="hidden" asp-for="Restaurant.Id"/>`
```
Скрыто, чтобы пользователь не видел и не редактировал Id, но Id нужен для поиска ресторана в БД.

* Для редактирования `Name` и `Location`:
```html
<div class="form-group">
    <label asp-for="Restaurant.Name"></label>
    <input asp-for="Restaurant.Name" class="form-control" />
</div>
```
Tag-helper `asp-for` позволяет не указывать аттрибуты: `type` (видит, что редактируется string),
`name` и `value`.

* Для выбора `Cuisine` - выпадающий список.
```html
<div class="form-group">
    <label asp-for="Restaurant.Cuisine"></label>
    <select class="form-control"
            asp-for="Restaurant.Cuisine" 
            asp-items="Model.Cuisines"></select>
</div>
```

`select` - выпадающий список, `asp-items` - элементы в выпадающем списке `IEnumerable<SelectListItem>`
(все элементы перечисления `CuisineType`).
*Примечание*: `asp-for` не требует префикса `Model`, `asp-items` - требует.


**Не рекомендуется** реализовывать логику выбора списка во View следующими двумя способами.
1. Не рекомендуется использование конструкции `option` (из-за hard coding)
```html
<select class="form-control asp-for="Restaurant.Cuisine">
    <option ...>
    <option ...>
    ...
</select>
```

2. Не рекомендуется использование `asp-items` следующим способом (сложно тестировать, View не должен этим заниматься):
```html
<select class="form-control
        asp-for="Restaurant.Cuisine"
        asp-items="Html.GetEnumSelectList<CuisineType>()">
    ...
</select>
```
`Html` - свойство типа `IHtmlHelper`, генерирует html.
`Html.GetEnumSelectList<CuisineType>()` - генерирует список для выпадающего списка.


Для `Edit.cshtml.cs`:
1. Добавление зависимости класса от `IHtmlHelper`.
2. Ввод свойства `public IEnumerable<SelectListItem> Cuisines { get; set; }` - для выбора `CuisineType`.
3. Инициализация поля в OnGet(), используя `IHtmlHelper`:
```csharp
public IActionResult OnGet(int restaurantId)
{
    Cuisines = _htmlHelper.GetEnumSelectList<CuisineType>();
    ...
}
```
Достоинства данного подхода получения списка enum для выпадайки во View:
* тестируемость
* гибкость


#### 04_04. Model Binding an HTTP POST Operation

*Добавление операции POST нажатии на кнопку `Save` в `Edit.cshtml`*.

1. Изменение `IRestaurantData`. Добавление нового метода для обновления ресторана:
* `Restaurant Update(Restaurant updatedRestaurant);`

* `int Commit();`
Метод для сохранения изменений в БД (в `InMemoryRestaurantData` не используется).
Похожие методы есть во многих системах Data Sources, например в EF.

2. Реализация добавленных методов в `InMemoryRestaurantData`.
```csharp
public Restaurant Update(Restaurant updatedRestaurant)
{
    var restaurant = GetById(updatedRestaurant.Id);
    if (restaurant != null)
    {
        restaurant.Name = updatedRestaurant.Name;
        restaurant.Location = updatedRestaurant.Location;
        restaurant.Cuisine = updatedRestaurant.Cuisine;
    }

    return restaurant;
}

public int Commit()
{
    return 0;        // Dummy реализация
}
```

3. Добавление `OnPost` в `Edit.cshtml.cs`:
```csharp
public IActionResult OnPost()
{
    Restaurant = _restaurantData.Update(Restaurant);
    _restaurantData.Commit();
    return Page();
}
```

И установка атрибута для свойства `Restaurant` (вкл. поддержки для передачи в POST):
```csharp
[BindProperty]
public Restaurant Restaurant { get; set; }
```
Это свойство будет заполнено данными из form (из View).

Есть несколько недочетов после нажатия на кнопку `Save` в `Edit.cshtml`:
1. Пропадание типа кухни в выпадающем списке.
2. Нет валидации вводимых данных (проверка и отображение предупреждений на форме).


#### 04_05. Adding Validation Checks

*Исправление недочетов, оставшихся из предыдущего пункта*.

Пропадание типа кухни в выпадающем списке лечится добавлением установки значения для свойства
`Cuisines` в `Edit.OnPost()`:
```csharp
public IActionResult OnPost()
{
    Cuisines = _htmlHelper.GetEnumSelectList<CuisineType>();
    ...
}
```


##### Добавление валидации (не до конца, окончание в следующем пункте)
Валидацию данных можно добавить так:
1. Либо добавив руками кучу проверок в `Edit.OnPost()` (не рекомендуется!).

2.1 Добавить в класс `Restaurant` в некоторые свойства нужные атрибуты, которые задают
ограничения на вводимые данные. Примеры:
  * `Required` - обязательное поле
  * `StringLength(80)` - ограничение по длине в 80 символов

(*Примечание: для более мощных проверок в классе `Restaurant` можно реализовать
`IValidatableObject` (в видео не показано)*).


#### 04_06. Using Model State and Showing Validation Errors

*Продолжение реализации валидации, оставшейся из предыдущего пункта*.

2.2 В `Edit.OnPost()` добавляется проверка статуса `ModelState`.
`ModelState` - это словарь, содержащий данные о валидности данных модели.
Можно обратиться напрямую, например:
```csharp
ModelState["Location"] - информация об элементе "Location".
ModelState["Location"].Errors - сюда записываются ошибки при валидации.
ModelState["Location"].AttemptedValue - введенное значение.
```
Все вышеперечисленное редко используется, на практике используется следующее
(как показано в видео):
```csharp
if (ModelState.IsValid)
{
    ...
}
```
Проблема: теперь при вводе хотя бы одного невалидного значения, введенные данные
на форме редактирования не сохряняются и не видно никаких информационных сообщений
об этом.

Это решается путем добавления полей, содержащих информацию о валидации. 


##### Добавление валидации на форму

Добавляется во View (`Edit.cshtml`).
Для проверямых полей ввода добавляется следущее (элемент `span` - подпись снизу):
```html
<div class="form-group">
    <label asp-for="Restaurant.Name"></label>
    <input asp-for="Restaurant.Name" class="form-control" />
    <span asp-validation-for="Restaurant.Name" class="text-danger"></span>
</div>
```
Tag-helper `asp-validation-for` проверяет свойство на правильность. Если свойство не проходит
валидацию, на форме выводится текст ошибки.

Проблема: возможность повторной отправки POST при обновлении страницы.


#### 04_07. Following the POST-GET-REDIRECT Pattern

*Решение повторной отправки POST - использование паттерна POST-GET-REDIRECT*.

В веб-дизайне всегда надо переправлять со страницы с POST на страницу с GET, чтобы не было
непреднамеренной модификации данных при обновлении страницы.

При успешном сохранении данных в `Edit.OnPost()` добавляется RedirectToPage на `Detail.cshtml`:
```csharp
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

`new { restaurantId = Restaurant.Id }` - это дополнительные routing detail, которые требуются
для перенаправления на страницу Detail.


#### 04_08. Building a Create Restaurant Page

*Построение страницы для создания новой страницы (на самом деле используя `Edit.cshtml`)*.

1. Добавление в `List.cshtml` ссылки в виде кнопки на `Edit.cshtml`:
```html
<a asp-page="./Edit" class="btn btn-primary">Add New</a>
```

При нажатии на добавленную кнопку никакой реакции не будет, т.к. ссылка на страницу `Edit` не
содержит `restaurantId`. Исправим это. 

2. Добавление в `Edit` возможность приема Nullable int id.
2.1. В `Edit.cshtml` - сверху:
```html
@page "{restaurantId:int?}"
...
```

Если урл заканчивается на `/Edit` (`restaurantId` будет null), то будет редактирование для новой
страницы, если на `/Edit/1`, то будет редактирование уже существующего ресторана.

2.2. Модификация `Edit.OnGet()`:
```csharp
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

Теперь необходимо реализовать добавление нового ресторана в репозитории данных и методе
`Edit.OnPost()`.


#### 04_09. Adding Create to the Data Access Service

*Изменение интерфейса `IRestaurantData`. Добавление нового метода `Add`*.

1. Добавление нового метода в `IRestaurantData`: `Restaurant Add(Restaurant newRestaurant);`.

2. Добавление нового метода в `InMemoryRestaurantData`:
```csharp
public Restaurant Add(Restaurant newRestaurant)
{
    _restaurants.Add(newRestaurant);
    // Эмуляция автоматического присвоения Id новому ресторану базой данных.
    newRestaurant.Id = _restaurants.Max(r => r.Id) + 1;

    return new Restaurant();
}
```

Осталось реализовать добавление нового ресторана в методе `Edit.OnPost()`.


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



