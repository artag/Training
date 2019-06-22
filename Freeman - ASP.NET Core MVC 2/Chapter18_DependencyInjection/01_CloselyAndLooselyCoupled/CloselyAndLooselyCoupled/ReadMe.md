# Создание слабо связанных компонентов (введение, зачем они нужны)

## Исследование сильно связанных компонентов

Наиболее прямой путь - создание объекта хранилища, которые требуются контроллеру в нем самом.
Пример (из `Controllers/CloselyCoupledController`):
```cs
public ViewResult Index() =>
    View("List", new MemoryRepository().Products);
```

Недостаток: `CloselyCoupledController` и `MemoryRepository` сильно связаны
(нельзя заменить хранилище, не изменив класс контроллера).


## Развязывание компонентов для модульного тестирования

Следующий шаг - хранилище хранится в отдельном свойстве (`Controllers/DecoupledController`):
```cs
public IRepository Repository { get; set; } = new MemoryRepository();

public ViewResult Index() =>
    View("List", Repository.Products);
```

Достоинство: можно провести модульное тестирование.
Недостаток: это все еще частичное решение проблемы. Установить свойство `Repository`, когда
приложение функционирует невозможно (MVC не знает о свойстве `Repository` при создании контроллера).


### Модульное тестирование. Тестирование контроллера с хранилищем в public свойстве

Из `DecoupledTests`:
```cs
[Fact]
public void ControllerTest()
{
    // Arrange
    var data = new[] { new Product { Name = "Test", Price = 100M } };

    var mock = new Mock<IRepository>();
    mock.SetupGet(m => m.Products).Returns(data);

    var controller = new DecoupledController();
    controller.Repository = mock.Object;

    // Act
    var result = controller.Index();

    // Assert
    Assert.Equal(data, result.ViewData.Model);
}
```


## Использование брокера типов

Следующий шаг - вынесение кода, в котором решается какое приенять хранилище за пределы контроллера
(см. `Controllers/UsageTypeBrokerController`):
```cs
public IRepository Repository { get; } = TypeBroker.Repository;

public ViewResult Index() =>
    View("List", Repository.Products);
```

В проект добавлен брокер типов `Infrastructure/TypeBroker`, который позволяет еще сильнее
развязать контроллер и хранилище.


### Пример применения брокера типов

В проект был добавлен альтернативное хранилище `Models/AlternativeRepository`, реализующее
`IRepository`.

Установка альтернативного хранилища выполняется в `Startup.ConfigureServices()`:
```cs
...
TypeBroker.SetRepositoryType<AlternateRepository>();
...
```

Теперь `UsageTypeBrokerController` будет выводить данные из `AlternateRepository`.


### Модульное тестирование. Применение брокера типов

Из `TypeBrokerTests`:
```cs
[Fact]
public void ControllerTests()
{
    // Arrange
    var data = new[] { new Product { Name = "Test", Price = 100M } };

    var mock = new Mock<IRepository>();
    mock.SetupGet(m => m.Products).Returns(data);

    TypeBroker.SetTestObject(mock.Object);
    var controller = new UsageTypeBrokerController();

    // Act
    var result = controller.Index();

    // Assert
    Assert.Equal(data, result.ViewData.Model);
}
```
