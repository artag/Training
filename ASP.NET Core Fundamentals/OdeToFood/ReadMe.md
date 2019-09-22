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


#### 04_10. Handling Create vs. Update Logic

*Модификация `OnPost()` метода для возможности подтверждения редактирования/создания страницы*.

Трюк, если Id ресторана <= 0, то это новый ресторан, и его надо добавить в репозиторий
(вызвать метод `Add()`), если Id > 0, то ресторан уже существует и его в репозитории необходимо
обновить (вызвать метод `Update()`).

Ну и последний штрих - желательно добавить сообщение об успешности сохранения/изменения данных.


#### 04_11. Confirming the Last Operation

*Добавление сообщения (подтверждение) сохранения, используя `TempData`*.

Как передать информацию об успешном соранении между страницами `Edit` и `Detail`?

Можно передавать параметр через url, например строку. Способ не подходит, так как
пользователь может внести такой адрес url в свои закладки.

Другой способ состоит в использовании `TempData` - структура данных, которая подобна словарю,
в котором можно передать что-либо между страницами. Но, после прочтения значения из `TempData`,
оно оттуда исчезает.

1. Создание `TempData["Message"]` в `Edit.OnPost()`:
```csharp
public IActionResult OnPost()
{
    ...
    TempData["Message"] = "Restaurant saved!";
    ...
}
```

2. Добавление свойства в `Detail.cshtml.cs`.

Можно сразу написать во View что-то типа:
```csharp
@if (TempData["Message"])
{
    // Что-то вывести на экран
}
```

Но, лучше, добавить свойство в Model (как показано в видео).

Можно сделать Bind для TempData. В `DetailModel` создать свойство со следующим атрибутом,
которое автоматически будет подхватываться во View:
```csharp
[TempData]
public string Message { get; set; }    // Имя свойства == имя ключа в TempData
```

3. Добавление показа сообщения в случае наличия этого сообщения в `Detail.cshtml`:

Изменения в `DetailView`:

```csharp
...
@if (Model.Message != null)
{
    <div class="alert alert-info">
        @Model.Message
    </div>
}
...
```


## 05. Working with SQL Server and the Entity Framework Core

*Работа с реальной БД (MS-SQL Server) с помощью Entity Framework*.


#### 05_02. Installing the Entity Framework

*Установка EF (NuGet пакеты).*

В проекте, созданным по умолчанию из template, в метапакете `Microsoft.AspNetCore.App` уже
содержатся ссылки на `EntityFramework`.

Но, в остальные проекты, которые созданы "вручную", требуется добавить пакеты самому. Для
текущего проекта требуется добавить (в видео проект `OdeToFood.Data`):
* `Microsoft.EntityFrameworkCore`
* `Microsoft.EntityFrameworkCore.Design`
* `Microsoft.EntityFrameworkCore.SqlServer` - для работы с MS-SQL Server.


#### 05_03. Implementing an Entity Framework DbContext

*Создание класса-наследника от `DbContext` для работы с данными в БД.*

Для работы с БД необходимо определить класс, унаследованный от `DbContext`.
Свойства в этом классе будут содержать информацию, которая будет находиться в БД и с которой
будет работать EF.

Создание в проекте `OdeToFood.Data` класса `OdeToFoodDbContext`:
```csharp
public class OdeToFoodDbContext : DbContext
{
    public DbSet<Restaurant> Restaurants { get; set; }
}
```

`DbSet` говорит EF, что данные будут использоваться не только для query, но и для insert,
update and delete.


#### 05_04. Using the Entity Framework Tools

*Инструменты командной сроки для работы с EF.*

Migration - меняет the schema of database.
Чтобы создать migration надо воспользоваться инструментами командной строки.

Из директории `OdeToFood.Data`:
```
dotnet ef dbcontext list
```
Вывести на экран все dbContext`ы. Сейчас выводит: `OdeToFood.Data.OdeToFoodDbContext`.

```
dotnet ef dbcontext info
```
При попытке вывести информацию о DbContext type выводится сообщение (ошибка) о том, что
не сконфигурирован database provider.


#### 05_05. Using Other Databases and Tools

*Немного о БД, инструментах для тех, что не на Windows и/или не использует VS.*

EF поддерживает несколько БД, такие как Sqlite, MySql, ...
Документацию о поддерживаемых БД можно найти тут (документация на Database Providers):
https://docs.microsoft.com/ru-ru/ef/core/providers/

Можно запустить на Linux MS-SQL Server через Docker Images(см. также на docs.microsoft.com)

Упоминается command-line query tool for SQL Server `mssql-cli`.
Это кроссплатформенная тулза, которая позволяет коннектиться к БД, выполнять query, ...

В данном курсе показывается использование `LocalDB` (ставится вместе с VS).

Меню View -> SQL Server Object Explore. Посмотреть на список доступных серверов.
Нас интересует наличие в этом списке сервера `(LocalDB)\MSSQLLocalDB`.

Название этого сервера понадобиться для задания `ConnectionString`.


#### 05_06. Adding Connection Strings and Registering Services

*Задание настроек подключения, регистрация настроек в сервисах, передача настроек в DbContext.*

##### Шаг 1
`ConnectionString` записывается в конфигурационный файл `appsettings.json`:
```json
...
"ConnectionStrings": {
    "OdeToFoodDb": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OdeToFood;Integrated Security=True;" 
  }
...
```

- `ConnectionStrings` - секция (может содержать информацию о нескольких возможных соединениях с БД).

- `OdeToFoodDb` - ключ, имя ConnectionString.

- Далее в кавычках - значение ConnectionString:
  - `Data Source=(localdb)\\MSSQLLocalDB` - название сервер
  - `Initial Catalog=OdeToFood` - Имя БД
  - `Integrated Security=True` - Использоваться Windows Identity для создания SQL Server'а
  (Особенность localdb).

##### Шаг 2
В `Startup.ConfigureServices()` добавляется:
```cs
services.AddDbContextPool<OdeToFoodDbContext>(options =>
{
    options.UseSqlServer(Configuration.GetConnectionString("OdeToFoodDb"));
});
```

* Есть `AddDbContext<T>` и `AddDbContextPool<T>`. В видео используется последний как более
быстрый (более ничего не объясняется).

* `UseSqlServer` используется для установки Database Provider.

##### Шаг 3
В классе `OdeToFoodDbContext` надо передать в базовый класс `DbContext` опции
`DbContextOptions<T>` (передача через конструктор):
```csharp
public class OdeToFoodDbContext : DbContext
{
    public OdeToFoodDbContext(DbContextOptions<OdeToFoodDbContext> options)
        : base(options)
    {
    }

    public DbSet<Restaurant> Restaurants { get; set; }
}
```
`DbContextOptions<T>` используется для передачи информации о ConnectionString 
и других опций в `DbContext`.


#### 05_07. Adding Database Migrations

*Создание (добавление) новой миграции.*

Из директории `OdeToFood.Data` опять попробовать запустить:
```
dotnet ef dbcontext info
```
Ругается: `Unable to create an object of type 'OdeToFoodDbContext'...`

Это из-за того, что `OdeToFood.Data` не включает `appsettings.json` (не стартовый проект).

Для решения этого вопроса можно:

* Имплементировать `IDesignTimeDbContextFactory<OdeToFoodDbContext>`. Эта реализация должна
будет предоставить всю необходимую информацию (Database Provider, ConnectionString, ...)
для dbContext.

* Или можно просто указать путь к стартовому проекту прямо в команде:
```
dotnet ef dbcontext info -s ..\OdeToFood\OdeToFood.csproj
```
Команда должна вывести что-то типа:
```
info: Microsoft.EntityFrameworkCore.Infrastructure[10403]
      Entity Framework Core 2.2.6-servicing-10079 initialized 'OdeToFoodDbContext'
      using provider 'Microsoft.EntityFrameworkCore.SqlServer'
      with options: MaxPoolSize=128
Provider name: Microsoft.EntityFrameworkCore.SqlServer
Database name: OdeToFood
Data source: (localdb)\MSSQLLocalDB
Options: MaxPoolSize=128
```

Теперь можно создать первый migration:
```
dotnet ef migrations add initialcreate -s ..\OdeToFood\OdeToFood.csproj
```
`initialcreate` - имя migration.

Данная команда создаст в проекте `OdeToFood.Data` папку `Migrations` с файлы миграции.
(Самый "интересный" файл будет называться *_initialcreate.cs").


#### 05_08. Running Database Migrations

*Запуск миграции. (Автоматическое обновление БД согласно миграции).*

Из директории `OdeToFood.Data` запустить:
```
dotnet ef database update -s ..\OdeToFood\OdeToFood.csproj
```
Данная команда обновляет базу данных используя миграцию.

После первого запуска EF создал БД и таблицу в ней.


#### 05_09. Implementing a Data Access Service

*Добавление операции Delete в `IRestaurantData` и в `InMemoryRestaurantData`.*

Добавленный метод в `InMemoryRestaurantData`:
```csharp
public Restaurant Delete(int id)
{
    var restaurant = GetById(id);
    if (restaurant != null)
    {
        _restaurants.Remove(restaurant);
    }

    return restaurant;
}
```


#### 05_10. Saving and Commiting Data

*Создание класса `SqlRestaurantData` для работы с реальной БД.*

В проекте `OdeToFood.Data`, создается класс `SqlRestaurantData`, который является имплементацией
`IRestaurantData`.

Через конструктор классу передается ссылка на `OdeToFoodDbContext`.

*Напоминание:* после изменений в БД (одного или нескольких), EF надо давать команду `SaveChanges`.
В нашем случае это метод `Commit()`:
```csharp
public int Commit()
{
    return _db.SaveChanges();
}
```

Все методы в принципе реализуются довольно просто (см. `SqlRestaurantData`). Исключением
является метод `Update()`:
```csharp
public Restaurant Update(Restaurant updatedRestaurant)
{
    var entity = _db.Restaurants.Attach(updatedRestaurant);
    entity.State = EntityState.Modified;
    return updatedRestaurant;
}
```
* Метод `Attach` - подключиться к объекту (сущности) в БД и отслеживать его изменения.
* `entity.State` - установить состояние отслеживаемой сущности.


#### 05_11. Modifying the Service Registration

*Модификация конфигурации сервиса в `Startup.Configuration()`, который создает объект
`IRestaurantData`. Тестирование работы приложения.*

```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddScoped<IRestaurantData, SqlRestaurantData>();
    ...
}
```

* Интерфейс `IRestaurantData` теперь реализует "реальный" класс `SqlRestaurantData`.

* Время жизни объекта исправлено с `AddSingleton` на `AddScoped`.
Использование `AddSingleton` для `SqlRestaurantData` - это очень плохо для реального приложения.
`AddScoped` - объект создается один раз для каждого запроса Http Request.

Теперь можно запустить проект.

Посмотреть данные в таблице Restaurants в БД можно так (в VS):
```
1. View -> SQL Server Object Explorer
2. SQL Server -> (localdb)\MSSQLLocalDB -> Databases -> OdeToFood -> Tables -> dbo.Restaurants
3. dbo.Restaurants -> ПКМ -> View Data
```


## 06. Building the User Interface

*Tips and Tricks (более детальное рассмотрение) для Razor Pages и специальных Pages.*

#### 06_02. Using Razor Layout Pages and Sections

*Про специальный файл _Layout.cshtml. `@RenderBody()` и `@RenderSection()`.*

* Компоненты (файлы) в директории `Shared` могут использоваться в нескольких местах.

* Razor Page имеет наверху "метку" (directive) `@page`.

* `_Layout` - технически является Razor View.

* `_` в начале файла, не требуется, но является частью соглашения. Файлы с таким подчеркиванием
не являются самостоятельными и используются совместно с другими.

Задача `_Layout` - специальный файл, задача которого:
1. Задание структуры пользовательского интерфейса.
2. Какие скрипты и css файлы будут использоваться на странице.
3. Описание общих элементов.

`@RenderBody()` - сюда помещается содержимое Razor Pages и Razor Views, которые используют
данный `_Layout`.

`@RenderSection("footer", required: false)` - сюда помещается код из Razor Pages и Razor Views
из секции с названием "footer" (может быть и другое).

Параметр `required: false` говорит, что наличие этой секции в Razor Page/Razor View необязательно.
(Если этот параметр в true и такой секции нет в Razor Page/Razor View, то будет выбрасываться
исключение).

**Пример**. Как выглядит RenderSection в `_Layout.cshtml`:
```html
<footer class="border-top footer text-muted">
    <div class="container">
        @RenderSection("footer", required: false)
        &copy; 2019 - OdeToFood - <a asp-area="" asp-page="/Privacy">Privacy</a>
    </div>
</footer>
```

Пример, как выглядит задание секции "footer" в `List.cshtml`, которая будет выводиться
в соотв. месте `_Layout.cshtml`:
```html
@section footer
{
    @Model.Message
    <br />
    @Model.MessageFromConfig
    <br />
}
```


#### 06_03. Implementing a Delete Restaurant Page Model

*Добавление страницы `Delete` для подтверждения удаления. Работа над `DeleteModel`.*

Два метода: `OnGet(int id)` и `OnPost(int id)`.

В `OnGet()` вызывает страницу `Delete` для ресторана с заданным id. Если такового нет в БД,
то перенаправляет на страницу `NotFound`.

В `OnPost()` вызывается удаление ресторана - если он есть и перенаправляет на страницу `List`.
Если ресторана нет, то перенаправление на `NotFound`.


#### 06_04. Implementing the Delete Markup

*Работа над `DeleteView`. Добавление кнопки Delete на страницу `List`.*

Так выглядит `DeleteView`:
```html
...
<h2>Delete</h2>
<div class="alert alert-danger">
    Are you sure you want to delete @Model.Restaurant.Name?
</div>
<form method="post">
    <button type="submit" class="btn btn-danger">Yes!</button>
    <a asp-page="List" class="btn btn-outline-secondary">Cancel</a>
</form>
```

Кнопка Delete добавлена на страницу `List` по аналогии с кнопками Detail и Edit.


#### 06_05. Using _ViewImports and _ViewStart Files

*Демонстрация установки ссылки на страницу `Layout` в null. Установка `Layout` на несуществующий
`_Layout2`. Назначение файла `_ViewStart.cshtml`. Назначение файла `_ViewImports.cshtml`.*


##### Про `/Shared/_Layout.cshtml`

Демонстрация установки ссылки в `DeleteView` на `Layout` в `null`:
```html
@page "{restaurantId:int}"
@model OdeToFood.Pages.Restaurants.DeleteModel
@{
    Layout = null;
}
...
```

При установке на несуществующий View `_Layout2`:
```html
...
@{
    Layout = "_Layout2";
}
...
```
будет выброшено исключение InvalidOperationException.

Рассказано, что:
* Общие View обычно лежат в папке `Shared` - они там всегда ищутся инфраструктурой.
* В одном проекте могут быть несколько View типа `_Layout`.


##### Про `/_ViewStart.cshtml`

Содержимое файла по умолчанию:
```html
@{
    Layout = "_Layout";
}
```
Содержимое в этом файле всегда выполняется **до** создания RazorPage (которые лежат в данной и/или
в дочерних директориях). 

Если задать "ручками" значение Layout на самой RazorPage, то новое значение перепишет значение
Layout из файла `_ViewStart.cshtml`.


##### Про `/_ViewImports.cshtml`

Содержимое файла:
```html
@using OdeToFood
@namespace OdeToFood.Pages
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```
Содержимое в этом файле включает определения `using`, `namespace` и `tag-helper`'ов для
всех RazorPage, которые лежат в данной и/или в дочерних директориях.

Поэтому в каждую из этих страниц не надо копировать эти строки.

Строка:
```
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```
задает использование всех (*) tag-helper'ов из сборки `Microsoft.AspNetCore.Mvc.TagHelpers`.


#### 06_06. Reusing Markup with Partial Views

*Что такое Partial View. Создание Partial View `_Summary.cshtml`.*

Partial View используется для:
1. Рендера какой-либо части от большого View и может использовать его какую-то более малую часть
Model.

2. Для создания переиспользуемого куска html.

Создание Page View (будет использоваться как Partial View) `/Pages/Restaurants/_Summary.cshtml`.
Данный Partial View не содержит Model.

В качестве примера создается Partial View для ресторана из списка `List`.

Содержимое `_Summary.cshtml`:
```html
@using OdeToFood.Core
@model Restaurant

<div class="card">
    <div class="card-header bg-secondary text-white">
        <h3>@Model.Name</h3>
    </div>
    <div class="card-body">
        <span>Location: @Model.Location</span>
        <span>Cuisine: @Model.Cuisine</span>
    </div>
    <div class="card-footer">
        ...
        <!-- Скопированные кнопки из List. -->
        ...
    </div>
</div>
```

Это обычный Page View без своей Model.


#### 06_07. Rendering Partial Views

*Использование Partial View, созданного в предыдущем разделе на странице `List`.*

В `List` старая таблица с ресторанами была заменена на:
```csharp
@foreach (var restaurant in Model.Restaurants)
{
    <partial name="_Summary" model="restaurant"/>
}
```

* `partial` - это tag-helper.
* `name` - параметр для tag-helper. Имя Partial View, который будет использован для подстановки.
* `model` - параметр для tag-helper. Модель, передаваемая в Partial View для его рендеринга.


#### 06_08. Implementing a ViewComponent

*Использование `ViewData` для передачи данных между View. Создание ViewComponent для отображения
количества ресторанов.*


##### Про ViewData

`ViewData` - динамический словарь, в который можно записать объект лбого типа. Через `ViewData`
можно передавать данные между Razor Pages (между несколькими View).

Пример, передача Title в `_Layout.cshtml` из различных Razor Pages (из различных Views).

В `Layout.cshtml`:
```html
...
<title>@ViewData["Title"] - OdeToFood</title>
...
```

В `Detail.cshtml`:
```html
...
@{
    ViewData["Title"] = "Detail";
}
...
```
И так далее для `List.cshtml`, `Delete.cshtml`, `Edit.cshtml`.


##### Про ViewComponent

`ViewComponent` - встраиваемый компонент в другие View (как Partial View). Особенности:
* Более самостоятельный компонент чем PartialView.
* Наследуется от класса `ViewComponent`.
* Лежит в отдельной директории `ViewComponent`.
* Не содержит методов `OnGet(), OnPost()` (не обрабатыват Get и Post запросы).
* Когда выполняется рендер ViewComponent, то вызывается метод `Invoke()`:
```csharp
public IViewComponentResult Invoke() { ... }
```

1. Создание нового ViewComponent в `/ViewComponent.RestaurauntCountViewComponent.cs`.
Это класс, наследуемый от `ViewComponent`. Содержит метод `Invoke()`, который вызывается при
рендеринге компонента.

2. В `IRestaurantData` и его имплементации добавляется новый метод
`int GetCountOfRestaurants()`.

Уточнение для `SqlRestaurantData.GetCountOfRestaurants()`:
```csharp
public int GetCountOfRestaurants()
{
    return _db.Restaurants.Count();
}
```
Если данный метод будет использоваться в `_Layout.cshtml`, то он будет часто вызываться.
Поэтому, для performance-ориентированных приложений необходимо выполнять кеширование данных.


3. Редактиуем метод `RestaurauntCountViewComponent.Invoke()`:
```csharp
public IViewComponentResult Invoke()
{
    var count = _restaurantData.GetCountOfRestaurants();
    return View(count);
}
```
Здесь `count` будет работать как model для View.


#### 06_09. Rendering a ViewComponent

*Создание Razor View для View Component. Регистрация tag-helper из своей сборки.
Вставка Razor View в виде tag-helper внутрь `_Layout.cshtml`.*


##### Создание View для ViewComponent

Для ViewComponent `RestaurauntCountViewComponent`, создается Razor View (вложенность и
наименования важны) в `/Shared/Components/RestaurauntCount/Default.cshtml`:
```html
@model int

<div class="card card-body bg-light">
    There are @Model restaurants here.
    <a asp-page="/Restaurants/List">See them all!</a>
</div>
```

Во ViewComponent для возвращаемого значения `IViewComponentResult` можно указать
имя для View вместе с передаваемой моделью:
```csharp
public IViewComponentResult Invoke()
{
    ...
    return View("count", count);
}
```
Если имя не указано, то название View будет `Default`.


##### Регистрация своего tag-helper в своем namespace

В файл `_ViewImports.cshtml` добавляется следующая строка:
```csharp
@addTagHelper *, OdeToFood
```
Регистрация всех tag-helper (*) из сборки `OdeToFood`.


##### Вставка ViewComponent внутрь _Layout.cshtml

В footer вставил:
```html
<footer class="border-top footer text-muted">
    ...
        <vc:restaurant-count></vc:restaurant-count>
    ...
</footer>
```
Вызов `RestaurantCountViewComponent` из razor в виде `vc:restaurant-count`.

Если бы требовалось передать во ViewComponent из View какой-либо параметр, то надо было бы сделать
следующее:

1. Во ViewComponent:
```csharp
public IViewComponentResult Invoke(string zipcode)
{
    ...
}
```

2. На месте вызова:
```html
<vc:restaurant-count zipcode="123abc"></vc:restaurant-count>
```


#### 06_10. Scaffolding a Complete Set of CRUD Pages

*Использование New Scaffolded Items для создания CRUD структуры страниц "одним махом".*

1. Создается директория `/Pages/R2`.

2. В Solution Explorer:
```
ПКМ -> Add -> New Scaffolded Item...
```

  2.1 Выбор `Razor Pages using Entity Framework (CRUD)`

  2.2 Установки

    * Model class: `Restaurant (OdeToFood.Core)`
    * Data context class: `OdeToFoodDbContext (OdeToFood.Data)`
    * Create as a partial view: `False`
    * Reference script libraries: `True`
    * Use a layout page: `True`

3. Готово. Создадутся 5 Razor Page:
  * Create
  * Delete
  * Details
  * Edit
  * Index


## 07. Integrating Client-side JavaScript and CSS

#### 07_02. Serving Static Files and Content from wwwroot

*Место хранения по умолчанию для статического содержимого в `wwwroot`.*

По умолчанию ASP.NET хранит статические файлы (картинки, js, css, ...) в `/wwwroot`.

В других частях приложения ресурсы "снаружи" не доступны (и это хорошо).

`jquery-validation` - набор js файлов для валидации данных на стороне клиента.

Пример добавления ссылки на картинку, которая будет лежать в `/wwwroot` и будет доступна
"снаружи". Файл `_Layout.cshtml`:
```html
<footer class="border-top footer text-muted">
    ...
    <img src="/OdeToFood.png"/>
    ...
</footer>
```
Картинка `OdeToFood.png` лежит в `/wwwroot`.


#### 07_03. Using ASP.NET Core Environments

*Как происходит загрузка js и css файлов. Про `environment` tag-helper.
Немного про `launchSettings.json`.*

В директории `/wwwroot/lib` лежат библиотеки bootstrap, jquery.

В примере все эти файлы загружаются в `_Layout.cshtml`:
1. Сверху, внутри тега `head` лежат линки на:
  * bootstrap.css
  * `site.css`

2. В нижней части лежат ссылки на загружаемые скрипты:
  * bootstrap.js
  * jquery.js
  * site.js

Tag-helper `environment` выполняется на стороне сервера. Он смотрит на переменную окружения.
```
<environment include="Development">
    // Скрипты/линки загружаются, если environment == Development
</environment>

<environment exclude="Development">
    // Скрипты/линки загружаются, если environment != Development
</environment>
```

Можно устанавливать любые наименования environment, но, как правило, используется
"Development" и "Production".

По умолчанию разработка приложения ASP.NET начинается в Development.

Все настройки для environment задаются в `/Properties/launchSettings.json`.

Эта переменная определяет environment:
```
"ASPNETCORE_ENVIRONMENT": "Development"
```
Если эта переменная не задана/отсутствует, то считается, что environment "Production".


#### 07_04. Enforcing Validation on the Client

*Включение скриптов на странице для валидации вводимых данных на стороне клиента.
Про `_ValidationScriptsPartial` и RenderSection "Scripts".*

Сейчас вся проверка идет только на стороне сервера, когда выполняется POST-операция.

На стороне клиента проверка выполняется при помощи:
* `jquery-validation`
* `jquery-validation-unobtrusive` - от Microsoft, связывает вместе ASP.NET и jquery-validation.
(например, обеспечивает поддержку tag-helper`ов).

Если посмотреть на генерируемый html-код полей, где производится валидация, то можно увидеть
атрибуты, которые явно "заточены под проверку jquery":
```html
<input class="form-control"
       ...
       data-val="true"
       data-val-length="The field Name must be a string with a maximum length of 80."
       data-val-length-max="80"
       data-val-required="The Name field is required."
       ... />
```

Таким образом, надо прописать загрузку скриптов на нужной странице.

Можно включить их для всех страниц в `_Layout.cshtml` или (предпочтительнее) загружать
для "избранных" страниц.

##### Загрузка jquery-validation для "избранных" страниц

1. Есть файл `/Pages/Shared/_ValidationScriptsPartial.cshtml`, который уже включает скрипты для
валидации на стороне клиента.

2. В файле `_Layout.cshtml` есть необязательная секция:
```html
@RenderSection("Scripts", required: false)
```

3. На нужной странице, на которой необходимо включить jquery-validation надо определить
секцию "Scripts". Пример определения секции из `Edit.cshtml`:
```html
@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```
* `partial` - это tag-helper.

Этим простым движением включается валидация на стороне клиента.
```
Edit.cshtml        -> _Layout.cshtml     -> _ValidationScriptsPartial
Определение секции    Определение секции    Загрузка скриптов
```


#### 07_05. Loading Restaurants from the Client

*Создание Razor Page (View) для получения списка ресторанов через api.
На примере использования jQuery.*

Для демонстрации получения списка ресторанов из api создается обычная Razor Page
`ClientRestaurants.cshtml`.

Работать с api можно с помощью различных js framework'ов: Angular, React, Vue.js, ...
В примере будет использован jQuery.

В ClientRestaurantsModel ничего нет, во View же переопределяется section `Scripts`:
```js
@section Scripts {
    <script>

        $(function() {
            $.ajax("/api/restaurants/",
                    { method: "get" })
                .then(function(response) {
                    console.dir(response);
                });
        });

    </script>
}
```

*Замечание:* обычно js скрипт кладется в отдельный файл - для тестируемости, переиспользования и
для читаемости кода во View представлении. Но для этого примера сделано исключение.

По скрипту:
1. `ajax` - perform asyncronous HTTP (ajax) request

2. `/api/restaurants/` - по этому адресу будут запрашиваться все рестораны.
Если запрашиваемый адрес такой: `/api/restaurants/3`, то будет возвращен ресторан с id = 3.

3. `method: "get"` - запрос для чтения (GET request).

4. `then` - handler для обработки возвращаемых данных.

5. `.then(function(response) { console.dir(response); })` - вывод полученных данных (в виде
JSON) на консоль разработчика в браузере.

При запросе страницы вида `https://localhost:44364/Restaurants/ClientRestaurants`
в консоли отладки браузера вылетает сообщение:
```
Failed to load resource: the server responded with a status of 404 ()
https://localhost:44364/api/restaurants/
```
Это потому, что еще не определен соответствующий api.


#### 07_06. Implementing an API Controller

*Краткое введение в api. Создание контроллера api (используя "wizard"). Проверка работы api
(get-запросы).*

Для рендеринга html на сервере и/или для POST requests рекомендуется использовать Razor Pages.
Для реализации api, для отдачи данных в виде text, json рекомендуется использовать Controllers из
MVC.

##### Создание контроллера для реализации api

Для создания api будет использоваться scaffolding tool.

1. Шаг. Создание директории `/Api`, где будет контроллер, отвечающий за api.

2. Шаг. ПКМ -> Add -> Controller...

  * Выбрать "API Controller with actions, using Entity Framework". Это создаст полный
    набор в api.

  * Model class: `Restaurant(OdeToFood.Core)`
  * Data context class: `OdeToFoodDbContext(OdeToFood.Data)`
  * Controller name: `RestaurantsController`

##### Особенности созданного `RestaurantsController`.

* Наследуется от `ControllerBase`.

* Сгенерированные методы api: Get (получить весь список или один ресторан), Put (обновить),
Post (создать новый), Delete (удалить).

##### Проверка работы api (только get запросы)

По примерно такому адресу `https://localhost:44364/api/Restaurants` должен вернуться набор
JSON данных, описывающий все рестораны.

По такому адресу `https://localhost:44364/api/Restaurants/2` возвращается только один ресторан
в виде JSON (если он конечно есть).

При запросе страницы вида `https://localhost:44364/Restaurants/ClientRestaurants`
(см. пред. раздел) в консоли отладки браузера появится массив с ресторанами.


#### 07_07. Using DataTables

*Установка DataTables (таблица, плагин для jQuery). Различные виды установки. Установка
используя CDN (для "Development" environment).*

Для отображения данных, полученных из api можно использовать `Angular`, `React` или `Vue.js`.

Но, в данном курсе используется плагин к jQuery - `DataTables`. (https://datatables.net)
Это тулза для отображения таблицы.

##### Как установить DataTables

1. DataTables можно скачать с сайта различными способами и в разных конфигурациях.

2. Можно использовать стандартный установщик в VS Client-Side Library:
```
wwwroot -> ПКМ -> Add -> Cient-Side Library...
```
Но, мы не ищем легких путей.

##### Установка DataTables используя CDN

1. С сайта скопировать линки на CDN.

2. Скопированные линки можно вставить в `_Layout.cshtml`, но лучше вставлять только в те места,
где эти скрипты будут использоваться - в `ClientRestaurants.cshtml`.

3. Вставка в `ClientRestaurants.cshtml`:
```html

@section Scripts {
    <environment include="Development">
    </environment>
    <environment exclude="Development">
        <link <!-- Какой-то путь -->/datatables.min.css" />
        <script <!-- Какой-то путь -->/datatables.min.js"></script>
    </environment>

    <script>
        <!-- Скрипт получения данных из api -->
    </script>
}
```
Установка скриптов из CDN подходит для Release сборки приложения.
Установку скриптов для Development сборки см. далее.


#### 07_08. Managing Client Libraries Using npm and NodeJS

*Использование npm. Создание `package.json`. Установка DataTables локально.*

Как установить DataTables локально:

1. Вручную скачать zip архив и распаковать его
2. Использовать Bower. Не рекомендуется, т.к. он устарел.
3. Использовать Yarn.
4. Использовать NPM. **Автор рекомендует**. Надо поставить на машину `Node.js`.


##### Успользование npm для установки DataTables

1. Шаг. Создание файла `package.json`. Выполняется из командной строки, из директории, где лежит главный проект
(где находится `OdeToFood.csproj`):
```
npm init
```
Создается файл `package.json`. Здесь содержится информация о всех пакетах и модулях, установленных
с помощью npm.

2. Шаг. Установка DataTables в директорию `/node_modules`.
```
npm install --save datatables.net-bs4
```

3. Шаг. Редактирование `package.json` до такого состояния:
```json
{
  "name": "odetofood",
  "version": "1.0.0",
  "dependencies": {
    "datatables.net-bs4": "^1.10.19"
  }
}
```

Важен файл `package.json`. Именно его надо помещать в систему контроля версий.
Файлы в директории `/node_modules` помещать под контроль **не надо**.

Для скачивания пакетов и модулей в `/node_modules` пользователю надо будет выполнить команду:
```
npm install
```


#### 07_09. Managing Production Scripts and Development Scripts

*Задание путей к установленным пакетам в `node_modules` на нужной странице.*

1. Показать скрытые директории:
```
Solution Explorer -> Action Bar -> Show All Files
```

2. "Добавить" скрытую директорию `/node_modules` в проект (чтобы эта директория не была скрыта):
```
ПКМ на /node_modules -> Include In Project
```

3. Задание путей к установленным пакетам в `ClientRestaurants.cshtml`. Простой Drag'n'drop файлов:
```html
@section Scripts {
    <environment include="Development">
        <script <!-- Какой-то путь -->/jquery.dataTables.js"></script>
        <script <!-- Какой-то путь -->/dataTables.bootstrap4.js"></script>
        <link <!-- Какой-то путь -->/dataTables.bootstrap4.css" rel="stylesheet" />
    </environment>
    <environment exclude="Development">
        <!-- Линки и скрипты, полуаемые из CDN -->
    </environment>

    <script>
        <!-- Скрипт получения данных из api -->
    </script>
}
```

4. **Дополнительный шаг**. Clean up. Удаление всех скриптов и css файлов из `wwwroot` и их
установка при помощи npm в директорию `/node_modules`.

4-ый шаг здесь (и видео) не показан.


#### 07_10. Serving Files from the node_modules Directory

*Конфигурирование директории `node_modules` в качетсве еще одного места для хранения и обеспечения
доступа к статическим файлам. Установка для этих целей NuGet пакета `OdeToCode.UseNodeModules`.
Добавление в pipeline нужного Middleware.*

Чтобы использовать директорию `node_modules` как и `wwwroot` для хранения статических файлов и
чтобы можно было обращаться к ней "снаружи", необходимо использовать специальный NuGet package
`OdeToCode.UseNodeModules` (автор Scott Allen).

*Мое примечание:* в старом варианте этого курса было показано, как сделать самому этот
Middleware, регистрирующий директорию `node_modules` как директорию для хранения статических
данных.

После добавления этого NuGet пакета, в `Startup.Configure()` необходимо добавить Middleware
из этого пакета:
```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    ...
    app.UseStaticFiles();
    app.UseNodeModules(env);
    ...
}
```


#### 07_11. Creating Sortable, Searchable Data Grids with DataTables

*Создание таблицы DataTables (плагин к jQuery).*

Последовательность действий при редактировании `ClientRestaurants.cshtml`.

1. Создание пустой таблицы
```html
<table class="table" id="restaurants">
</table>
```

2. Добавить вместо вывода JSON данных на девелоперскую консоль в браузере заполнение
таблицы:
```js
$(function() {
    $.ajax("/api/restaurants/",
            { method: "get" })
        .then(function(response) {
            $("#restaurants").dataTable({
                data: response,
                columns: [
                    { "data": "name" },
                    { "data": "location" },
                    { "data": "cuisine" }
                ]
            });
        });
});
```
* `$("#restaurants").dataTable` - jQuery selector, выбирает элемент по id (`restaurants`).
`dataTable` - расширение(плагин) для jQuery.

* `data: response` - первый передаваемый параметр. Данные для заполнения таблицы берутся из
объекта `response`.

* `columns: [ { "data": "name" }, ...]` - второй передаваемый параметр. Определение колонок,
которые будут отображаться.

Эта таблица разбивается по страницам, ее можно сортировать по различным стобцам,
можно выполнять поиск. Единственно, тип кухни ("cuisine") отображается как число.

Чтобы это исправить надо по хорошему сериализовать (как?) все типы кухонь в формат JSON и
подгрузить их в таблицу. В примере же показан самый простой и тупой подход:

1. Захардкодить наименования кухонь в скрипте 
```js
$(function () {
    var cuisines = ["Unknown", "Mexican", "Italian", "Indian"];
    ...
```

2. Для колонки "cuisine" использовать кастомный рендер:
```js
columns: [
        { "data": "name" },
        { "data": "location" },
        {
            "data": "cuisine", "render": function(data) {
                return cuisines[data];
            }
        }
    ]
```

* `function(data)` - функция которая принимает текущее значение в строке `data` (0, 1, 2 или 3)
и возвращает захардкоженное значение.


## 08. Working with the Internals of ASP.NET Core

#### 08_02. Exploring the Application Entry Point

*Изучение точки входа в приложение (`Main()`, `launchSettings.json`, Startup.cs).*

##### Как запускается приложение?

Как обычное консольное .NET приложение из метода `Program.Main()`.

Чтобы запустить приложение из консоли (из директории проекта, где находится `Main()`) ввести:
```
dotnet run
```

Во время выполнения приложения, при его запуске из консоли, можно наблюдать логируемые события.

В файле `/Properties/launchSettings.json` определяется конфигурация запуска приложения.

##### Про `Main()`

В `Main()` вызывается `CreateWebHostBuilder(args)` в котором происходит конфигурирование 
билдера `CreateDefaultBuilder`, который будет создавать сервер с сервисами, заданными
по умолчанию: (например, консольный логгер).

`UseStartup<Startup>` - Задает класс `Startup` для конфигурации сервера.

`Build()` - создание WebHost'а, который будет слушать определенные порты и
обрабатывать HTTP запросы.

`Run()` - запуск WebHost'а.

##### Класс `Startup`

Здесь при инициализации выполняются два метода: `ConfigureServices()` и `Configure()`.

* `ConfigureServices()` - конфигурируется сервисы, используемые в приложении.

* `Configure()` - конфигурирует Middleware pipeline, использующийся для обработки входящих
(и исходящих) запросов.


#### 08_03. Processing Summer Corn with the Allen Family

*Рассказ про то, что такое Middleware (на абстрактном примере).*


#### 08_04. Exploring the Application Middleware

*Немного более подробно о Middleware, используемых в текущем приложении. И еще немного о некоторых.*

Рассматривается метод `Startup.Configure()`.

##### О pipeline

* Порядок размещения Middleware имеет значение.

* Middleware может обрабатывать как входящие так и исходящие request.

* Одни Middleware обрабатывают только входящие запросы, другие - только исходящие, третьи - и те и
другие.

* Любой из Middleware в pipeline может прервать цепочку обработки запроса и вернуть ответ.

##### О Middleware, используемых в текущем приложении

```csharp
if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseNodeModules(env);
app.UseCookiePolicy();

app.UseMvc();
```
* `app.UseDeveloperExceptionPage()` - перехватывает исключения, порождаемые последующими
Middleware и отображает страницу с подбробным описанием исключения. Как видно, для данного
примера работает только в `Development`.

* `app.UseExceptionHandler("/Error")` - перехватывает исключения, порождаемые последующими
Middleware и отображает страницу `Error`.

* `app.UseHsts()` - использовать https secure connection.

* `app.UseHttpsRedirection()` - перенаправление браузера, который пытается использовать
простой http.

* `app.UseStaticFiles()` - работа со статическим содержимым в `wwwroot`.
В опциях `StaticFilesOptions` можно задать `FileProvider`, который может поставлять файлы
откуда угодно: из облака, из БД, ...

* `app.UseNodeModules(env)` - работа со статическим содержимым в `node_modules`.

* `app.UseCookiePolicy()` - использование Cookies.

* `app.UseMvc()` - роутер для MVC.

##### Какие еще есть Middleware.

* `app.UseAuthentication()` (впереди `app.UseMvc()`) - иденфтификация пользователя.

* `app.UseSignalR()` (впереди `app.UseMvc()`) - realtime socket communication.

* `app.UseSpa()` (после `app.UseMvc()`) - возвращает страницу по умолчанию для
Single Page Application (SPA).


#### 08_05. Building Custom Middleware

*Пример создания своего Middleware на примере отображения надписи `Hello World`
при вводе определенного адреса в url.*

Обычно пишется полноценный класс Middleware (см. старый вариант данного курса), но в данном
курсе Middleware пишется прямо в методе `Configure()`.

В `Configure()` поместили впереди всех свой самодельный Middleware:
```csharp
...
app.Use(SayHelloMiddleware);

app.UseHttpsRedirection();
...
app.UseMvc();
```
`app.Use()` примнимает `Func<RequestDelegate, RequestDelegate>` - middleware delegate.

`RequestDelegate()` - A function that can process an HTTP request.
```csharp
public delegate Task RequestDelegate(HttpContext context);
```

Сам Middleware delegate:
```csharp
private RequestDelegate SayHelloMiddleware(RequestDelegate next)
{
    return async ctx =>
    {
        if (ctx.Request.Path.StartsWithSegments("/hello"))
        {
            await ctx.Response.WriteAsync("Hello, World!");
        }
        else
        {
            await next(ctx);
        }
    };
}
```
Если HttpContext `context` в пути запроса содержит сегмент `/hello`, то цепочка в pipeline
прервется и будет послан ответ, содержащий простую строку.

Иначе, будет вызван следущий в pipeline RequestDelegate `next` - следующий Middleware.

Если надо вставить обработку ответа (Response) от последующих Middleware, то это делается здесь:
```csharp
...
else
{
    await next(ctx);

    // Здесь будет обработка ответа
    if (ctx.Response.StatusCode == ...)
    {
        ...
    }
}
...
```


#### 08_06. Logging Application Messages

*Еще раз про логирование. Как записать что-либо в логи самому.*

Вывод логов можно увидеть в консоли, откуда запускается приложение (см. ранее).

Если запуск из VS, то можно увидеть логи в окне Output,
output from ASP.NET Core Web Server (у меня Debug).

Настройки уровней логирования можно задать в файле `appsettings.json`.
Про уровни логирования можно почитать на сайте MS.

Пример добавления записи в логи можно посмотреть в ListModel, файл `List.cshtml.cs`:
```csharp
// Конструктор
public ListModel(
    IConfiguration config,
    IRestaurantData restaurantData,
    ILogger<ListModel> logger)
{
    ...
    _logger = logger;
}

public void OnGet()
{
    _logger.LogError("Executing ListModel");

    ...
}
```
В этом примере создается запись в логи с уровнем "Error" при каждом get request.


#### 08_07. Configuring the App Using the Default Web Host Builder

*Исследование конфигурации приложения при создании сервера с настройками по умолчанию.
Рассматриваются источники для задания конфигурации и настройки логирования.*

Исходники для `CreateDefaultBuilder` можно найти на github:
```
https://github.com/aspnet/AspNetCore, файл src/DefaultBuilder/src/WebHost.cs
```

##### Источники для задания конфигурации

Здесь можно увидеть, что конфигурация в настройках по умолчанию задается из:
* `appsettings.json`

* `appsettings.{env.EnvironmentName}.json`
(перекрывает одноименные настройки из `appsettings.json`)

* `EnvironmentVariables` - из переменных среды

* `CommandLine` - из командной строки

Плюс, еще есть `UserSecrets`, которые доступны для Development.

Перекрытие параметра:
```
("самый слабый") json -> environment variable -> command line parameter ("самый сильный")
```

##### Конфигурация для логов

Из файла на github `WebHost.cs`, метод `CreateDefaultBuilder()`:
```csharp
...
.ConfigureLogging((hostingContext, logging) =>
{
    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
    logging.AddConsole();
    logging.AddDebug();
    logging.AddEventSourceLogger();
}).
```
Видно, что для задания конфигурации используется настройка "Logging" в конфигурации.

Также здесь добавляются различные инструменты для вывода логов.


## 09. Deploying ASP.NET Core Web Applications

#### 09_02. Publishing Apps vs. Deploying Apps

*Краткий обзор как сделать publish приложения из VS.*

**Publish** - все нужные для работы приложения файлы складываются в какую-то директорию
перед Deploy.

В VS Publish делается так:

1. `ПКМ на проекте -> Publish...`

2. Выбираем `Folder`. 

Настройки:

  * Configuration: Debug / Release
  * Target Framework: netcoreapp2.1 (версия framework)
  * Deployment Mode:
    * Framework-Dependent (помимо приложения нужен установленный framework)
    * Self-Contained (все включено)
  * Target Runtime:
    * Portable
    * win-x86
    * win-x64
    * osx-x64
    * linux-x64

Но, в примере будет показано как сделать publish из командной строки.


#### 09_03. Using dotnet publish

*publish из командной строки.*

Команда запускается из директории, где находится основной проект (есть файл `*.csproj`).

Publish в определенную директорию
```
dotnet publish -o C:\Temp\OdeToFood
```
* В publish в `wwwroot` будет скопировано все содержимое.

* В publish не копируется `node_modules`. Это будет исправлено далее.
(Для создания node_modules можно запустить `npm install` руками, но это не выход).

* Приложение компилируется в `*.dll` файл.

Запуск такого приложения:
```
dotnet OdeToFood.dll
```


#### 09_04. Using MSBuld to Execute npm install

*Правка файла `*.csproj` для публикации с использованием npm.*

Чтобы `node_modules` устанавливались при выполнении publish надо внести изменения
в главный `*.csproj` файл.

##### Что добавить

Добавить следующие строки
```xml
<Target Name="PostBuild" AfterTargets="ComputeFilesToPublish">
  <Exec Command="npm install" />
</Target>
```
и
```xml
<ItemGroup>
  <Content Include="node_modules/**" CopyToPublishDirectory="PreserveNewest" />
</ItemGroup>
```
Первый блок отвечает за запуск `npm install` после окончания публикации.
Дополнительно, можно проверить наличие npm на машине, проверить какая конфигурация
(Debug или Publish).

Второй блок отвечает за копирование содержимого в `node_modules` в директорию, куда
выполняется publish.

##### Что удалить

Удалить строки вида:
```xml
<ItemGroup>
  <Content Include="node_modules\datatables.net-bs4\css\dataTables.bootstrap4.css" />
  <Content Include="node_modules\datatables.net\License.txt" />
  <Content Include="node_modules\jquery\AUTHORS.txt" />
  ...
</ItemGroup>
```
Эти строки появляются в `*.csproj`, когда папка `node_modules` добавляется в проект
и становится видимой. Плюс, могут быть глюки: не все файлы из этой директории 
могут быть сюда включены.

Теперь по команде `dotnet publish` все устанавливается как надо.


#### 09_05. Building Self-contained Applications

*Публикация приложения в режиме self-contained.*

```
dotnet publish -o C:\Temp\OdeToFood --self-contained -r win-x64
```
Параметр `--self-contained` позволяет опубликовать приложение со всем нужным содержимым
(скопирует приложение и framework).
Никаких дополниьельных framework`ов на target машине устанавливать не придется.

Параметр `-r win-x64` задает runtime identifier для target system.
На сайте https://docs.microsoft.com/ru-ru/dotnet/core/rid-catalog можно найти
.NET Core RID Catalog - список кратких значений RID платформ, где будет работать приложение.

В publish-директории при такой публикации будет файл для запуска `*.exe`.


#### 09_06. Deploying to a Web Server (IIS)

*Deploy на IIS сервер.*

Можно deploy'ить приложение на Nginx, Apache, IIS. Всю документацию можно найти на сайте MS.
Здесь рассматривается deploy на IIS сервер.

##### Предварительные шаги

1. Поставить на target-машину (см. документацию):
* Install the .NET Core Hosting Bundle
* (возможно не потребуется ставить) Microsoft Visual C++ 2015 Redistributable 

2. Из `Startup.ConfigureServices()`
Временно отключить обращение к SQL-серверу (зачем?). Потом, после установки на IIS,
опять включить:
```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    //services.AddScoped<IRestaurantData, SqlRestaurantData>();
    services.AddScoped<IRestaurantData, InMemoryRestaurantData>();
    ...
}
```

3. Заново выполнить publish, с учетом сделанных изменений.

##### Краткие шаги по Deploy на IIS.

1. Запуск Internet Information Services (IIS) Manager.

2. Зайти в Modules

Видно, что присутствуют модуль(и) `AspNetCoreModule` и/или `AspNetCoreModuleV2`.

3. Слева ПКМ на папку `Sites -> Add Website...`
* Site name: OdeToFood
* Physical path: *куда выполнялся publish*
* Type: https
* IP address: All Unassigned
* Port: 443
* SSL certificate: IIS Express Development Certificate
(т.к. будет использоваться на локальной машине)

Новый сайт должен появиться в папке `Sites`.

4. В браузере по линку `https://localhost` (без указания порта) должно открыться приложение.


#### 09_07. Exploring web.config and How IIS Hosting Works

*Где хранятся настройки в IIS для запуска приложения (в файле `web.config`).*

IIS смотрит в файл `web.config`. Этот файл уже не используется в ASP.NET Core, но все еще
требуется в работе с IIS.

`web.config` появляется в publish директории. Его примерное содержимое:
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="dotnet" arguments=".\OdeToFood.dll" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout" hostingModel="InProcess" />
    </system.webServer>
  </location>
</configuration>
<!--ProjectGuid: FEB3CB78-F614-440B-B2BB-7446403503A5-->
```

Рассматривается секция `handlers`.
* `name` - имя секции
* `path="*"` - для любого запроса
* `verb="*"` - для любого verb: add, get, post, delete, ...

Все запросы переадресовывать на `modules="AspNetCoreModuleV2"`.

В секции `aspNetCore` прописывается как запускать приложение.

Перед запуском приложения внутри IIS, его можно запустить напрямую, из командной строки.
Если запустится отсюда, то скорее всего запустится и из IIS.

При запуске напрямую используется встроенный в ASP.NET сервер Kestrel.
Не рекомендуется использовать такой вид запуска в Production, т.к. могут быть проблемы
с безопасностью приложения. Рекомендуется запускать приложение под управлением Nginx, Apache, IIS.


#### 09_08. Setting up Automatic Entity Framework Migrations

*Создание автоматического Migration для БД при запуске приложения.*

**Шаг 1**. Возвращение подключения к реальной БД в `Startup.ConfigureServices()`:
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IRestaurantData, SqlRestaurantData>();
    //services.AddScoped<IRestaurantData, InMemoryRestaurantData>();
}
```

**Шаг 2.0** (мы не будем его использовать). Создание скрипта для Migrations

Из директории проекта, где находится OdeToFoodDbContext (OdeToFood.Data) запустить команду:
```
dotnet ef migrations script -s ..\OdeToFood\OdeToFood.csproj
```

Но это надо будет каждый раз делать руками, для каждого развертывания приложения

**Шаг 2.1**. Запуск Migrations (если надо) при запуске приложения.
Редактирование `Program.cs`:
```csharp
public static void Main(string[] args)
{
    var host = CreateWebHostBuilder(args).Build();
    MigrateDatabase(host);
    host.Run();
}

private static void MigrateDatabase(IWebHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<OdeToFoodDbContext>();
        dbContext.Database.Migrate();
    }
}
```
Здесь вставили запуск Migration после создания webhost`а, но до его запуска.

Запрашивается сервис `OdeToFoodDbContext`, для него запускается метод `Migrate()`.
Здесь БД будет автоматически создаваться при ее отсутствии.

Конечно, до запуска `Migrate()` в реальном Production приложении могут быть дополнительные
проверки, backup'ы БД и прочее.
