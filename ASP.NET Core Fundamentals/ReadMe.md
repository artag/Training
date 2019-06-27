# ASP.NET Core Fundamentals

## 02. Drilling into Data

02. Создание проекта. Тип проекта `Web Application` (Configure for HTTPS, No Authentication)
Ну или создать с помощью командной строки `dotnet new ...`

03. Редактирование файла `Pages/Shared/_Layout.cshtml`:
Добавление меню Restaurants (ссылка на `/Restaurants/List`)

04. Добавление Razor Page (в `/Pages/Restaurants/List.cshtml`.
Добавление заголовка на page

05. Пример использования тулзы
`dotnet aspnet-codegenerator`:
```
dotnet aspnet-codegenerator razorpage List Empty -udl -outDir Pages\Restaurants\
```

06. Добавление свойства `Message` в `ListModel` и во view (`List.cshtml`).
Добавление строки `Message` в конфигурационный файл `appsettings.json`.
Добавление в `ListModel` чтения строки из конфигурационного файла.

07. Создание нового проекта `OdeToFood.Core` для Entities.
Создание класса `Restaurant` и перечисления `CuisineType`.

08. Создание нового проекта `OdeToFood.Data` для доступа к данным.
Создание интерфейса `IRestaurantData` и его реализации `InMemoryRestaurantData`.

09. Регистрация `IRestaurantData` и `InMemoryRestaurantData` в `Startup.ConfigureServices()`
Добавлена передача `IRestaurantData` через конструктор `ListModel` (в `Pages/Restaurants/List.cshtml`).

10. Добавление свойства `Restaurants` в класс `ListModel` и инициализация этого свойства.

11. Добавление таблицы в `Pages/Restaurants/List.cshtml`, используя данные из `Restaurants`
класса `ListModel`.


## 03. Working with Models and Model Binding

### 02. Working with HTML Forms

Рассказывается про `form`, `get` и `post`. Почему надо использовать для чтения данных `get`,
и только для изменения данных `post`.

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


### 03. Building a Search Form

Добавление в `List.cshtml` form с get. Это будет поле для вывода списка ресторанов, начинающихся
с определенных букв. Перехода на другую страницу нет.

По сравнению с видео - в bootstrap 4 нет таких иконок.
В bootstrap 3 можно украсить кнопку таким образом:
```html
<span class="input-group-btn">
    <button class="btn btn-primary">
        <i class="glyphicon glyphicon-search"></i>
    </button>
</span>
```

Для добавления иконок рекомендуют поставить `Font Awesome` или `Github Octicons`.



#### Добавление пакетов для web через `LibMan`.

[Use LibMan with ASP.NET Core in Visual Studio](https://docs.microsoft.com/en-us/aspnet/core/client-side/libman/libman-vs?view=aspnetcore-2.1)

In Solution Explorer, right-click the project folder in which the files should be added.
Choose Add > Client-Side Library. The Add Client-Side Library dialog appears.
Choose `cdnjs` and set the name of library.


