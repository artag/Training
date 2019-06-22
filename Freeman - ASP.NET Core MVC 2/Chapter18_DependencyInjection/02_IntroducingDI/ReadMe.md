# Введение в средство внедрения зависимостей ASP.NET

Процесс внедрения зависимости:
1. MVC получает запрос к методу действия в контроллере (для примера `Home`).

2. MVC требует у компонента поставщика служб новый экземпляр `HomeController`.

3. Поставщик служб инспектирует конструктор `HomeController` на предмет зависимостей
(например от `IRepository`).

4. Поставщик служб обращается к своим отображениям, чтобы найти класс реализации, который
подлежит применению для зависимостей от интерфейса `IRepository`.

5. Поставщик служб создает новый экземпляр найденного класса реализации `IRepository`.

6. Поставщик служб создает новый экземпляр `HomeController`, используя найденный класс реализации
в качестве аргумента конструктора.

7. Поставщик служб возвращает созданный объект `HomeController` MVC, который она применяет для
обработки входящего HTTP-запроса.

В ASP.NET в качестве средства внедрения зависимостей могут использоваться сторонние пакеты.


## Контроллер для внедрения зависимостей

`Controllers/InjectionController`:
```cs
public class InjectionController : Controller
{
    private readonly IRepository _repository;

    public InjectionController(IRepository repository)
    {
        _repository = repository;
    }

    public ViewResult Index() =>
        View("List", _repository.Products);
}
```

Особенности контроллера, готового для внедрения зависимостей:
он объявляет все свои зависимости через конструктор.


## Конфигурирование поставщика служб

Конфигурация для поставщика служб определяется в `Startup.ConfigureServices()` способом
наподобие этого:
```cs
...
services.AddTransient<IRepository, MemoryRepository>();
...
```

Внедрение зависимостей конфигурируется с использованием методов, которые вызываются на
`IServiceCollection`.

`AddTransient()` сообщает поставщику как обрабатывать зависимость (будет более подробно описан в
этой главе).

`<IRepository, MemoryRepository>` указывает, что зависимости от `IRepository` решаются путем
создания объекта `MemoryRepository`.


## Модульное тестирование. Контроллер с зависимостью

Из `DIControllerTests`:
```cs
[Fact]
public void ControllerTest()
{
    // Arrange
    var data = new[] { new Product { Name = "Test", Price = 100M } };

    var mock = new Mock<IRepository>();
    mock.SetupGet(m => m.Products).Returns(data);

    var controller = new InjectionController(mock.Object);

    // Act
    var result = controller.Index();

    // Assert
    Assert.Equal(data, result.ViewData.Model);
}
```
