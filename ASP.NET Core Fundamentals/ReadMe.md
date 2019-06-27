# ASP.NET Core Fundamentals

## 02. Drilling into Data

02_02. Создание проекта. Тип проекта `Web Application` (Configure for HTTPS, No Authentication)
Ну или создать с помощью командной строки `dotnet new ...`

02_03. Редактирование файла `Pages/Shared/_Layout.cshtml`:
Добавление меню Restaurants (ссылка на `/Restaurants/List`)

02_04. Добавление Razor Page (в `/Pages/Restaurants/List.cshtml`.
Добавление заголовка на page

02_05. Пример использования тулзы
`dotnet aspnet-codegenerator`:
```
dotnet aspnet-codegenerator razorpage List Empty -udl -outDir Pages\Restaurants\
```

02_06. Добавление свойства `Message` в `ListModel` и во view (`List.cshtml`).
Добавление строки `Message` в конфигурационный файл `appsettings.json`.
Добавление в `ListModel` чтения строки из конфигурационного файла.

02_07. Создание нового проекта `OdeToFood.Core` для Entities.
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
