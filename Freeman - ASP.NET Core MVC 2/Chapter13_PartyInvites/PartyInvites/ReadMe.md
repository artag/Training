# Работа над проектом ASP.NET в Visual Studio Code

## Подготовка
* Поставить `Node.js`
* Поставить `Git`
* Поставить `.NET Core` 
* Поставить `Visual Studio Code`

## Создание проекта 
Создать в папке `PartyInvites` Empty проект:
```
dotnet new web --language С# --framework netcoreapp2.0
```

## Установка bootstrap
Выполнить в папке `PartyInvites` команду:
```
npm install bootstrap
```
Node.js создаст директорию `node_modules` и скачает туда bootstrap.

Далее, сделать Middleware по рецепту Scott Allen, смотреть файлы:
`Middleware/ApplicationBuilderExtension.cs`
`Startup.cs`, в методе `Configure` добавить `app.UseNodeModules(env.ContentRootPath);`

## Конфигурирование приложения
Добавление Mvc в `Startup.cs`

## Запуск проекта
Из директории 'PartyInvites' выполнить в терминале:
```
dotnet run
```
Вручную открыть броузер и вбить адрес типа `http://localhost:5000`

## Воссоздание приложения PartyInvites
### Создание модели и хранилища
* `Models/GuestResponse.cs`
* `Models/IRepository.cs`
* `Models/EFRepository.cs` и добавить его в `Startup.cs`:
```
services.AddTransient<IRepository, EFRepository>();
```

### Создание базы данных
(В примере используется SQLite)

Добавить nuget-пакет `Microsoft.EntityFrameworkCore.Sqlite` (руками в `*.csproj`, например).

Создать файл: `Models/ApplicationDbContext.cs`

Создание начальной миграции базы данных (из директории 'PartyInvites'):
```
dotnet ef migrations add Initial
```
Создастся папка `Migrations` с файлами настройки схемы БД.

Применение миграции БД:
```
dotnet ef database update
```

### Создание контроллеров и представлений
* `Controllers/HomeController`

* `Views/_ViewImports.cshtml` - для импорта вспомогательных дескрипторов.
* `Views/Home/MyView.cshtml` - View для контроллера `Home`, для action `Index`.
* `Views/Home/RsvpForm.cshtml` - View для контроллера `Home` для action `RsvpForm` в `GET` и `POST` вариантах.
* `Views/Home/Thanks.cshtml` - View для контроллера `Home` возвращается из action `RsvpForm`, `POST`.
* `Views/Home/ListResponses.cshtml` - View для котроллера `Home` для action `ListResponses`.

## Модульное тестирование в Visual Studio Code
### Создание проекта для тестирования
Создать в папке `PartyInvites.Tests` (лежит рядом с `PartyInvites`) проект:
```
dotnet new xunit --language C# --framework netcoreapp2.0
```

Добавление ссылки на проект приложения
```
dotnet add reference ../PartyInvites/PartyInvites.csproj
```

### Создание модульного теста
Может понадобиться: у меня VSCode при написании тестов ругался на недостаток пакета.
Этот пакет я руками добавил в `PartyInvites.Tests.csproj` файл:
```
<PackageReference Include="Microsoft.AspNetCore.Mvc" Version="2.2.0" />
```
и сделал `dotnet restore`

Создание следующих файлов:
* `FakeRepository.cs` - "поддельный" репозиторий для тестирования
* `HomeControllerTests.cs` - тесты для контроллера `Home`

### Прогон тестов
Выполнить в терминале команду (из папки, где тестовый проект `PartyInvites.Tests`):
```
dotnet test
```