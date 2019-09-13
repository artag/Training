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

Два метода: `OnGet(int id) и `OnPost(int id)`.

В `OnGet()` вызывает страницу `Delete` для ресторана с заданным id. Если такового нет в БД,
то перенаправляет на страницу `NotFound`.

В `OnPost()` вызывается удаление ресторана - если он есть и перенаправляет на страницу `List`.
Если ресторана нет, то перенаправление на `NotFound`.


