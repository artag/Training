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

*Создание модели, интерфейса для доступа к репозиторию. Регистрация репозитория в DI.*

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
