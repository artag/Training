# Building Your First ASP.NET Core 2 MVC Application


## 03. Setting up Your Site with Visual Studio 2017

*Как создать новый проект ASP.NET Core в VS 2017.*


### 03_02,03. Exploring the Project Structure

*Создание нового проекта (Empty) и его структура.*

* `File -> New Project`
* `Web -> .NET Core -> ASP.NET Core Web Application`
* .NET Core, ASP.NET Core 2.2, Configure for HTTPS. Тип проекта: **Empty Project**

В созданном проекте:

**Dependencies** - зависимости проекта

В ASP.NET Core используются пакеты NuGet. Основной пакет, который включает все необходимое
`Microsoft.AspNetCore.App (2.2.0)`.

Все зависимости описываются в файле `*.csproj`.

**appsettings.json** - здесь лежат настройки приложения.

**wwwroot** - директория в которой лежат статические файлы приложения: изображения, css, js, ...

Файл по url
```
www.bethanyspieshop.com/images/image1.jpg
```
Будет лежать в
```
wwwroot/images/image1.jpg
```

Все остальные файлы (сервисы и прочее), которые лежат в других директориях недоступны извне.


### 03_04,05. Site Configuration

*О файлах `Program.cs` и `Startup.cs`.*

По сути, ASP.NET Core консольное приложение.

`Program.cs`:
```csharp
public class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder
        CreateWebHostBuilder(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .UseStartup<Startup>();
}
```
Из `Program.Main()` производится запуск приложения. 

Здесь создается web-сервер Kestrel с настройками/конфигурацией по умолчанию
(`CreateDefaultBuilder(args)`).

Kestrel - внутренний web-сервер и все равно требует внешний сервер (например, IIS).

Метод `UseStartup<Startup>()` - устанавливает класс `Startup` в качестве "стартового" класса.
Здесь задается конфигурация и набор сервисов, используемых в приложении.

`Startup.cs`:
```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        ...
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        ...
    }
}
```
Содержит два метода, которые вызываются ASP.NET автоматически: 
* `ConfigureServices` конфигурирует сервисы, используемые в приложении.
```csharp
public void ConfigureServices(IServiceCollection services)
{
    // register framework services
    services.AddMvc();
    
    // register our own services (more later)
    ...
}
```
В `services` регистрируются необходимые сервисы, которые используются в Dependency Injection.

* `Configure()` конфигурирует pipeline.
```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseDeveloperExceptionPage();
    app.UseStatusCodePages();
    app.UseStaticFiles();
    app.UseMvcWithDefaultRoute();
}
```
Содержит компоненты Middleware, которые обрабатывают входящие http-запросы:
* Прерывают цепочку обработки запроса, обрабатывают запрос и возвращают ответ.
* Пропускают запрос далее по цепочке.
* Обрабатывают ответ, отправленный ранее более "дальним" Middleware в цепочке.

* `app.UseDeveloperExceptionPage()` - ловит исключения, кидаемые Middleware и показывает страницу
с подробной информацией.

* `app.UseStatusCodePages()` - показывает информацию о возвращаемом статусе request.

* `app.UseStaticFiles()` - выполняет поиск статичного содержимого в директории `wwwroot`
и возвращает его.

* `app.UseMvcWithDefaultRoute()` - более подробная информацию см. далее.

#### Процесс запуска приложения

1. Application starting (Program class)
2. Startup class
3. ConfigureServices method (Registering services)
4. Configure method (Pipeline is created)
5. Ready for requests


## 04. Creating the List Page

*Создание страницы со списком.*

### 04_02. MVC (Model-View-Controller)

*Про MVC.*

Особенности:
* Architectural pattern
* Separation of concerns
* Promotes testability and maintainability

Схема взаимодействия
```
Request                Update
--------> Controller ---------> Model
              |                  ^
              |                  |  Get data from
              |        Update    |
              |---------------> View
```

### 04_03,04. Creating the Model and the Repository

*Создание модели, интерфейса для доступа к репозиторию, реализация mock репозитория.
Регистрация репозитория в DI.*

Особенности The Model:
* Domain data + logic to manage data
* Simple API
* Hides details of managing the data

Пример класса, который будет использоваться в качестве модели (директория `Models`):
```csharp
public class Pie
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string LongDescription { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public string ImageThumbnailUrl { get; set; }
    public bool IsPieOfTheWeek { get; set; }
}
```

Использование Repository позволяет использовать объекты, не зная никаких деталей об их хранении.

1. Для этого надо создать простой API (интерфейс), через который мы будем получать нужные объекты.

Pie Repository Interface (директория `Models`):
```csharp
public interface IPieRepository
{
    IEnumerable<Pie> GetAllPies();
    Pie GetPieById(int pieId);
}
```

2. Надо создать реализацию этого интерфейса - в данном примере создается `MockPieRepository`,
который содержит mock data (также лежит в директории `Models`).

3. Надо зарегистрировать Repository в DI Container (в `Startup.ConfigureServices()`):
```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddTransient<IPieRepository, MockPieRepository>();
}
```
Варианты регистрации:

* `AddTransient` - экземпляр MockPieRepository создается каждый раз, когда запрашивается.

* `AddSingleton` - экземпляр MockPieRepository создается только один раз.

* `AddScoped` - экземпляр MockPieRepository создается один раз для одного запроса, для другого
запроса создастся еще один экземпляр.


### 04_05,06. Creating the Controller

*Создание контроллера.*

Особенности контроллера:
* Respond to user interaction.
* Update model.
* No knowledge about data persistence.

Что из себя представляет контроллер:
* Это простой класс.
* Наследуется от базового класса `Controller`.
* Название контроллера по соглашению оканчивается строкой "Controller".
* Метод в контроллере, отвечающий за обработку запроса называется "Action".
* В качестве возвращаемого значения Action возвращает View.

Пример простого контроллера:
```csharp
public class PieController : Controller
{
    public ViewResult Index()        // <-- Action
    {
        return View();               // <-- View to show
    }
}
```

Все контроллеры располагаются в директории `Controllers`.

В директории `Controllers` создаем контроллер `HomeController`, использующий средства DI
для доступа к объекту `IPieRepository`.



### 04_07,08. Adding the View

*Добавление View. Про `_Layout.cshtml` и `_ViewStart.cshtml`.*

Особенности View:
* HTML template (файл *.cshml)
* "Plain" or strongly-typed
* Uses Razor

Все View по соглашению находятся в директории `/Views` и ее поддиректориях.

В Action контроллера, если не указано название View, то по умолчанию будет искаться View с
именем Action.
```csharp
public class PieController : Controller
{
    public ViewResult Index()        // <-- Action
    {
        return View();               // <-- View to show: Index.cshtml
    }
}
```
Здесь будет искаться View под именем `Index.cshtml` в `/Views/Pie` или `/Views/Shared` директориях.
Если View не найдено, то будет выброшено исключение.

Создание файла:
```
ПКМ -> Add -> New Item... -> Razor View
```


#### Способы передачи данных во View

Для передачи данных во View можно использовать:

1. **ViewBag** - объект, содержащий динамически задаваемые данные.
```csharp
public class PieController : Controller
{
    public ViewResult Index()
    {
        ViewBag.Message = "Welcome to Bethany’s Pie Shop";
        return View();
    }
}
```

Использование `ViewBag` во View:
```html
<div>
    @ViewBag.Message
</div>
```

2. Передача данных через **Model**
```csharp
public class PieController : Controller
{
    public ViewResult List()
    {
        return View(_pieRepository.Pies);
    }
}
```
Список из Pie передается во View в качестве параметра.


Использование Model во View:
```html
@model IEnumerable<Pie>
<html>
...
    <body>
        <div>
            @foreach (var pie in Model.Pies)
            {
                <div>
                    <h2>@pie.Name</h2>
                    <h3>@pie.Price.ToString("c")</h3>
                    <h4>@pie.Category.CategoryName</h4>
                </div>
            }
        </div>
    </body>
</html>
```

3. Создание и передача **ViewModel**.

Это по сути wrapper, содержащий в себе все необходимые данные для передачи во View.
В видео, в итоге был создан класс `HomeViewModel` (в директории `/ViewModels`), который содержит
в себе `Title` и `Pies`:
```csharp
public class HomeViewModel
{
    public string Title { get; set; }
    public IEnumerable<Pie> Pies { get; set; }
}
```
`HomeViewModel` создается и устанавливается в Action контроллера и передается во View.



#### Файл `_Layout.cshtml`

Может являться шаблоном для других View (чтобы не делать copy-paste разметки).

Особенности:
* Template - не него могут ссылаться другие View
* Shared folder
* More than one can be created

Файл `_Layout.cshtml` по соглашению лежит в директории `Views/Shared`

Пример содержимого файла:
```html
!DOCTYPE html>
<html>
    <head>
        <title>Bethany's Pie Shop</title>
    </head>
    <body>
        <div>
            @RenderBody()  <!-- Replaced with view -->
        </div>
    </body>
</html>
```
Вместо `@RenderBody()` будет подставляться содержимое из View.

Создание файла:
```
ПКМ -> Add -> New Item... -> Razor Layout
```


#### Файл `_ViewStart.cshtml`

Файл `_ViewStart.cshtml` по соглашению лежит в директории `/Views`.
Содержимое из этого файла автоматом подставляется в любой View при создании последнего.

Пример содержимого файла:
```html
@{
    Layout = "_Layout";
}
```
В данном случае для всех View файлов перед их созданием устанавливается Layout.
Если надо, то можно переопределить значение Layout в самом View.

Создание файла:
```
ПКМ -> Add -> New Item... -> Razor View Start
```


### 04_09-13. Styling the View

*Стилизация View. Добавление в проект `bootstrap`. Рассматривается использование пакетных
менеджеров `Bower`, `Library Manager` и обычное добавление файлов "руками".
Добавление картинок, стилей. Прописывание путей на ссылки в `_Layout.cshtml`.*

Какие пакетные менеджеры используются в VS:
* Bower (до версии 15.5)
* Ручной процесс (версии 15.6 - 15.7)
* Library Manager (LibMan) (версия 15.8 и выше)

#### Bower

Как добавить конфигурационный файл:
```
ПКМ -> Add -> New Item... -> Bower Configuration File
```

В корне проекта появляется файл `bower.json`. В раздел `dependencies` надо добавить нужный пакет:
```json
{
  "name": "asp.net",
  "private": true,
  "dependencies": {
    "bootstrap": "v3.3.7"
  }
```

Нужные файлы автоматически появятся в `/wwwroot` после сохранения файла.


#### Ручной процесс

Берем библиотеку и разархивируем/копируем ее в `/wwwroot`.


#### Library Manager (LibMan)

Как добавить:
`ПКМ -> Add -> Clien-Side Library...`
* Provider: cdnjs
* Library: *нужная библиотека*
* Target Location: wwwroot/lib/*путь к библиотеке*

После нажатия на `Install` все нужные файлы появятся в `/wwwroot`.

В корне проекта появится файл `libman.json`, содержащий список установленных пакетов:
```json
{
  "version": "1.0",
  "defaultProvider": "cdnjs",
  "libraries": [
    {
      "library": "twitter-bootstrap@3.3.7",
      "destination": "wwwroot/lib/bootstrap/"
    }
  ]
}
```

#### Копирование (создание) картинок и стилей для сайта

В директории:
* `/wwwroot/images` - сюда копируются картинки для сайта
* `/wwwroot/content` - сюда копируются стили (*.css) для сайта

Создание стилевой страницы:
```
ПКМ -> Add -> New Item... -> Style Sheet
```
site.css
```css
body {
    padding-top: 50px;
    padding-bottom: 20px;
    background-image: url('/images/pattern.png');
    background-repeat: repeat;
}

.body-content {
    padding-left: 15px;
    padding-right: 15px;
}
```


#### Вставка ссылок на css в _Layout.cshtml

```html
<head>
    ...
    <link href="~/lib/bootstrap/css/bootstrap.css" rel="stylesheet" />
    <link href="~/content/site.css" rel="stylesheet" />
    ...
</head>
```


## 05. Adding Data with Entity Framework Core (EF Core)

*Работа с данными в Entity Framework Core.*

### 05_02. EF Core

Особенности:
* ORM (Object-Relational Mapping, рус. объектно-реляционное отображение, или преобразование)
* LINQ support
* Lightweight & Cross-platform
* Open-source
* SQL Server and other, non-relational DB support
* Code-First only

Взято из просторов Интернета:
```
Поход, называемый Code First (сначала код) предполагает минимальное участие в проектировании
сущностей базы данных программистом. Он просто пишет код, остальное делает Entity и Visual Studio.

Он подходит в случаях если главное в проекте – бизнес логика, а база данных – это способ хранения
данных. Или в случаях если проект уже написан, но в качестве источников данных использованы списки,
массивы, коллекции.

Code First позволяет с минимальными усилиями изменить проект с использованием баз данных в качестве
источников данных вместо стандартных коллекций .NET.
```

EF Core заменяет работу с БД с использованием SQL-выражений на работу с БД, используя обычные классы.


### 05_03,04. Adding EF Core to the Application

*Создание DbContext, "реального" репозитория, application configuration (appsettings.json).*

Для включения EF нужно:
1. Domain classes
2. Database context
3. Connection string (как коннектиться к БД)
4. Application configuration (изменения в Startup классе)


#### Database context

Пример:
```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Pie> Pies { get; set; }
}
```
* Наследуется от `DbContext`.

* Передача в базовый класс `DbContextOptions<T>` (можно через overriding метод `OnConfiguring()`
или через конструктор (так делается чаще всего)).

* Для таблицы в БД создается свойство `DbSet<T>`.

Создаем класс `/Models/AppDbContext`.

#### "Реальный" репозиторий

Создание класса `/Models/PieRepository`.

Особенности:
* Реализует `IPieRepository`.
* Работа с данными выполняется через объект `AppDbContext` (передается через конструктор).


#### Добавление информации о подключении к БД

1. Добавление `ConnectionString`.

Один из способов задания `ConnectionString` - чтение настроек из файла `appsettings.json`.
В примере создается файл `appsettings.json` (в корне проекта):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=BethanyDemo123;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```
`localdb` - специальная "разработческая" версия БД, которая поставляется вместе с VS.


2. В `Startup` следующие изменения:
```csharp
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddTransient<IPieRepository, PieRepository>();
        ...
    }
    ...
}
```
* В конструктор добавляется ссылка на объект `IConfiguration`.

* В `ConfigureServices()`

  * Добавляется информация о `ConnectionString` для `AppDbContext`.
  * Реализация интерфейса `IPieRepository` меняется на "реальную" реализацию `PieRepository`.

Теперь необходимо создать и инициализировать БД.


### 05-05,06. Creating and Initializing the Database

*Создание БД. Инициализация БД первоначальными данными.*

#### Создание БД

Используется Package Manager Console.
```
View -> Other Windows -> Package Manager Console
```

Команды

1. Добавление начальной Migration
```
add-migration InitialMigration
```
InitialMigration - наименование миграции.

Это команда автоматом создает Migration в `/Migrations`.
В файле вида `дата_InitialMigration.cs` содержатся методы `Up()` и `Down()`.
Первый метод создает БД и таблицу в ней, второй drop'ает таблицу.

2. Обновление БД
```
update-database
```
Производится синхронизация состояния БД и Data Context в коде с помощью миграций
(сейчас пока только одна).

Т.о. на данном шаге создана БД с пустой таблицей `Pies`.

#### Заполнение БД первоначальными данными

После создания, БД инициализируется данными.

1. Создание класса-заполнителя `/Model/DbInitializer`.

Класс `DbInitializer` статический, содержит статический метод `Seed()`.
Здесь производится заполнение пустой таблицы в БД начальными данными.

2. Запуск первоначальной инициализации БД данными при загрузке приложения
(если надо).

Запуск `DbInitializer.Seed()` производится из класса `Program`.
Так рекомендует MS. Раньше было в `Startup`.

Кусок из `Program.Main()`:
```csharp
CreateWebHostBuilder(args).Build().Run();
```
Превратился в:
```csharp
var host = CreateWebHostBuilder(args).Build();

using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        DbInitializer.Seed(context);
    }
    catch (Exception)
    {
        // We could log this in a real-world situation
    }
}

host.Run();
```
Здесь получаем доступ к `DbContext` и запускаем инициализацию данных (если надо).


### 05_07,08. Modifying the Model

*Внесение изменения в класс-модель `Pie`. Создание новой Migration. Обновление БД.*

Действия при изменении Model:
* Создание новой migration.
* Запуск обновления ("синхронизации" с моделью) БД.

Пример.

1. Внесли изменение в `/Model/Pie` - добавили новое свойство.

2. В Package Manager Console

Создание новой миграции под именем PieModelChanged
```
add-migration PieModelChanged
```

3. В Package Manager Console

Запуск обновления БД
```
update-database
```

В SQL Server Object Explorer можно увидеть, что в БД, в таблице `Pies` появился новый столбец.


## 06. Adding Navigation to the Site

*Изучение Routing. Создание Detail View для выбранного элемента из списка.*

### 06_02. Understanding Navigation in MVC

*Объяснение Routing.*

Есть ссылка:
```
http://www.bethanyspieshop.com/Pie/List
```
Поиск выполняется в соответствии с заданным шаблоном.

Шаблон: `{Controller}/{Action}`
* `Pie` - Controller
* `List` - Action
```csharp
public class PieController : Controller
{
    public ViewResult List()
    {
        return View();
    }
}
```

Для ссылки:
```
http://www.bethanyspieshop.com/Pie/Details/2
```
Шаблон: `{Controller}/{Action}/{id}`
* `Pie` - Controller
* `Details` - Action
* `2` - Value
```csharp
public class PieController : Controller
{
    public ViewResult Details(int id)
    {
        //Do something
    }
}
```

Шаблон для Routing устанавливается в `Startup.Configure()`:
```csharp
app.UseMvcWithDefaultRoute();
```
Это шаблон по умолчанию, который подходит для множества сайтов.
Это шаблон вида:
```
{controller=Home}/{action=Index}/{id?}
```
- `Home` - Controller по умолчанию
- `Index` - Action по умолчанию
- `id` - Value. `?` означает что параметр необязательный

Шаблон по умолчанию для Routing можно записать в таком виде в `Startup.Configure()`:
```csharp
app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");
});
```

Можно описать несколько Routing, порядок их описания важен.
```csharp
app.UseMvc(routes =>
{
    routes.MapRoute(
        ...
        );
    routes.MapRoute(
        ...
        );
});
```

Такой шаблон подходит для следующих адресов:
* `www.bethanyspieshop.com`
* `www.bethanyspieshop.com/Pie/List`
* `www.bethanyspieshop.com/Pie/Details/1`


### 06_03,04. Adding Navigation

*Добавление навигации: создание action в controller (Details), создание view Details,
модификация `Index.cshtml` для создания линка на страницу Details. Добавление
верхнего меню со ссылкой на главную страницу.*

Во View используются Tag Helpers. Их особенности:
* Server-side
* Trigger code execution (можно "повесить" код при запуске на исполнение)
* Built-in or custom (есть встроенные, можно сделать свои)
* Replace HTML Helpers (замена устаревшим HTML Helpers)

Пример, link в html:
```html
<a asp-controller="Pie" asp-action="List">
    View Pie List
</a>
```
Будет преобразован с учетом заданного Routing в:
```html
<a href="/Pie/List">View Pie List</a>
```

Здесь рассматриваются следующие теги:
* `asp-controller` - указывает на Controller, которому предназначен запрос.

* `asp-action` - указывает на Action.

* `asp-route-*` - определяет значение для определенного параметра.

* `asp-route` - указывает на название маршрута.
Толком не объяснено что это, но говорится, что это редко используется.

#### Добавление функционала по выводу деталей для выбранного пирога

**1.** Создание нового action `Details`.

В /Controllers/HomeController создается новый action:
```csharp
public IActionResult Details(int id)
{
    var pie = _pieRepository.GetPieById(id);
    if (pie == null)
    {
        return NotFound();
    }

    return View(pie);
}
```

**2.** Создание нового view `Details`.

Во `/Views/Home` создается `Details.cshtml`.

**3.** Создание файла `/Views/_ViewImports.cshtml`

Файл `_ViewImports.cshtml` лежит во `/Views` и предназначен для определения всех
используемых usage и tag helpers в остальных Views.
```
@using BethanysPieShop.Models;
@using BethanysPieShop.ViewModels;
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```
Последняя строка добавляет все taghelper'ы из указанного assembly.

Теперь можно почистить все View от namespace'ов.

**4.** Создание линка в списке пирогов (`Index.cshtml`) на Details для определенного пирога.

Вместо простого наименования пирога ставится ссылка на Details:
```html
<h4>
    <a asp-controller="Home"
       asp-action="Details"
       asp-route-id="@pie.Id">@pie.Name</a>
</h4>
```

#### Добавление верхнего меню и ссылки на главную страницу в `_Layout.cshtml`.

Добавлен следующий код (в начало body):
```html
<nav class="navbar navbar-inverse navbar-fixed-top">
    <div class="container">
        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li>
                    <a asp-controller="Home"
                       asp-action="Index"
                       class="navbar-brand">
                        Bethany's Pie Shop
                    </a>
                </li>
            </ul>
        </div>
    </div>
</nav>
```


## 07. Creating a Simple Form

*Создание страницы для feedback. Валидация вводимых данных.*

### 07_02,03. Using Tag Helpers

*Cоздание страницы для feedback.*

Tag helper'ы для выполнения операции post.
* Form tag helper
* Input tag helper
* Label tag helper
* Textarea tag helper
* Select tag helper
* Validation tag helpers

Эти tag helper'ы, по сути, являются расширениями стандартных html тегов.

Пример использования Form Tag Helper:
```html
<form asp-action="Index" method="post"
      class="form-horizontal" role="form">
    ...
</form>
```

Пример использования Tag Helper:
В разметке Razor:
```html
<label asp-for="Name">
</label>
```
Получившийся Html:
```html
<label for="Name">
    Name
</label>
```

### Этапы работы

1. Создание Model `/Models/Feedback.cs`.
2. Добавление новой таблицы в `AppDbContext`:
```csharp
public DbSet<Feedback> Feedbacks { get; set; }
```
3. Создание FeedbackRepository в `/Models` (`IFeedbackRepository` и `FeedbackRepository`).
4. Регистрация FeedbackRepository в `Startup.ConfigureServices()`. 
5. Создание Migration и обновление БД.
```
add-migration Feedback
update-database
```
6. Создание Controller `/Controllers/FeedbackController.cs`.
Через конструктор передается ссылка на `IFeedbackRepository`.
7. Создание View `Views/Feedback/Index.cshtml`.
(Элементы ввода: input, textarea, checkbox, submit button).

Продолжение см. в следующем разделе.


### 07_04,05. Model Binding and Validation

*Объяснение работы Model Binding. Добавление валидации для вводимых данных.
Завершение работы над формой feedback.*

Связывание для **простых** типов данных
```
Запрос                             Action в Controller
/Pie/Details/1 -> Model binders -> public ViewResult Details(int id)
```

В Model binders поиск ведется в (порядок важен):
1. Form data
2. Route variables
3. Query string

Связывание для **сложных** типов данных.

Передаваемые данные верны
```
Запрос                             Action в Controller
Add "Feedback" -> Model binders -> public ViewResult Index(Feedback feedback)

Feedback
---
Name = "Some User"
ContactMe = true
```

В передаваемых данных ошибка
```
Запрос                             Action в Controller
Add "Feedback" -> Model binders -> public ViewResult Index(Feedback feedback)
                                                                        ^
Feedback                                                             ОШИБКА
---
Name = "Some User"
ContactMe = ABC
```

Для **валидации данных** используется:
1. В Controller, Action. Используется `ModelState`:
```csharp
if (ModelState.IsValid)
{
    _appDbContext.Feedbacks.Add(feedback);
    _appDbContext.SaveChanges();
}
else
{
    return View();
}
```

2. В Model для соответствующих полей (свойств) ставятся атрибуты:
```csharp
public class Feedback
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(5000)]
    public string Message { get; set; }
}
```

3. Во View вставляются tag helper'ы для отображения сообщений об ошибках.

#### Атрибуты валидации
* Required
* StringLength
* Range
* RegularExpression
* DataType
  * Phone
  * Email
  * Url

#### Продолжение работы над формой feedback

8. В `FeedbackController` добавляются два Action'а:
```csharp
[HttpPost]
public IActionResult Index(Feedback feedback)
{
    _feedbackRepository.AddFeedback(feedback);
    return RedirectToAction(nameof(FeedbackComplete));
}

public IActionResult FeedbackComplete()
{
    return View();
}
```
Post-action помечается атрибутом `HttpPost`.

9. Создание View `/Views/Feedback/FeedbackComplete.cshtml`.
10. Добавление ссылки на страницу с feedback в верхнее меню (редактирование
`/Views/Shared/_Layout.cshtml`):
```html
<li>
    <a asp-controller="Feedback" asp-action="Index">
        Feedback
    </a>
</li>
```
