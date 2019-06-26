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

07. Создание нового проекта `OdeToFood.Core` для Enitities.
Создание класса `Restaurant` и перечисления `CuisineType`.

08. Создание нового проекта `OdeToFood.Data` для доступа к данным.