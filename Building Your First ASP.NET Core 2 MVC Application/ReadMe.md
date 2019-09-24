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

### 04_03,04 Creating the Model and the Repository

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


### 04_05,06 Creating the Controller

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



### 04_07,08 Adding the View

*Добавление View. Про `_Layout.cshtml` и `_Layout.cshtml`.*

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

Может являться шаблоном для других View (что-бы не делать copy-paste разметки).

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
ПКМ -> Add -> New Item... -> Razor View Imports
```
