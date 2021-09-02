# Module 7. Frontend Integration

## Lesson 47. Installing Node.js

[Node.js](https://nodejs.org)

### Проверка успешности установки node.js

Просмотр версии node.js и npm (пакетный менеджер).

```text
node -v
npm -v
```

## Lesson 49. Getting started with the frontend

*(Примечание: не искользовать frontend как шаблон для "боевых" проектов, только для обучения.)*

About the frontend project:

* A simple single-page application (SPA) that interacts with all the Play Economy microservices.
* Built using React, a JavaScript library for building interactive user interfaces.
* Uses Create React App to simplify local development, building and deployment.
* Hosted in a Node.js server.

**SPA** - web application that dynamically rewrites the current webpage with new data from a server
instead of constantly loading new pages.

**Links** для более подробной информации:

* [https://reactjs.org/](https://reactjs.org/)
* [https://create-react-app.dev/](https://create-react-app.dev/)
* [https://react-bootstrap.github.io/](https://react-bootstrap.github.io/)

### Запуск frontend

**ReadMe** от автора курса по запуску frontend'а

[Play.Frontend ReadMe.md](./Play.Frontend/README.md)

Согласно этому руководству:

1. Запуск из корня директории `Play.Frontend`. Закачивание и установка зависимостей:

```text
npm install
```

2. Запуск. Компилирует код и запускает Node.js сервер.

```text
npm start
```

Адрес для запуска в браузере на локальной машине: `http://localhost:3000`

### Описание структуры проекта Play.Frontend

* `package.json` - describes all dependencies of this project.

* `public/index.html` - page template. Страница для SPA. На ней постоянно загружаются и
обновляются компоненты.

  В элемент

  ```html
  <div id="root"></div>
  ```

  инжектируется содержимое JavaScript со всеми компонентами React.

* `public/config.js` - содержит адреса всех микросервисов.

* Директория `src` - содержит все исходные файлы компонентов React.

* `src/index.js` - the root where we start rendering React components.

  ```js
  <BrowserRouter basename={baseUrl}>
    <App />                             // App component (src/App.js)
  </BrowserRouter>,
  document.getElementById('root'));     // rendering in "root" div
  ```

* `src/App.js` - App component. Defines our app by using `Layout` component -
the basic layout (находится в `src/components/Layout.js`).

* `src/components/Catalog.js` - обеспечивает соединение с сервисом `Catalog`.

* `src/components/Inventory.js` - обеспечивает соединение с сервисом `Inventory`.

* `src/components/form/ItemModal.js` - render the modal dialogue (create or update item).

* `src/components/form/ItemForm.js` - сама форма, внутри `ItemModal`.

* `src/components/form/GrantItemModal.js` и `src/components/form/GrantItemForm.js` - modal dialogue. Передача предмета пользователю.

### Проверка работы. Неудачная

Запущено:

1. docker контейнеры.
2. `Play.Catalog` service
3. `Play.Frontend`

Пытаемся посмотреть каталоги через `Play.Frontend`, в браузере
`http://localhost:3000/`

Но каталог `Play.Catalog` недоступен для просмотра, пишет: "Could not load items".

Причину недоступности можно посмотреть в браузере: `F12 -> Console`

Пишет что-то типа:

```text
Access to fetch at 'https://localhost:5001/items' from origin 'http://localhost:3000' has been
blocked by CORS policy: No 'Access-Control-Allow-Origin' header is present on the requested
resource. If an opaque response serves your needs, set the request's mode to 'no-cors'
to fetch the resource with CORS disabled.
```

В следующем уроке будет рассмотрено решение данной проблемы - использование CORS.

## Lesson 50. Understanding CORS (Cross-Origin Resource Sharing)

Origin:

```text
protocol://host:port
```

Пример:

```text
http://localhost:3000
http      - protocol
localhost - host
3000      - port
```

Для browser и web server (node.js) их origin полностью совпадают - у обоих `http://localhost:3000`.

Web Server на Node.js хостит frontend и REST API.

В данном сценарии, браузер вначале обращается к Web Server для загрузки начальной страницы
frontend'а. Когда запрошена секция Catalog, браузер делает GET запрос к Catalog REST API *(1)*.
Web Server в ответ присылает JSON представление catalog data *(2)*.

```text
                       (1)  GET /items
        Browser       ----------------->      Web Server
                      <-----------------       Node.js
http://localhost:3000  (2)  JSON (answer)  http://localhost:3000
```

Сервис `Catalog` работает отдельно, на ASP.NET Core Kestrel webserver, который также
хостит REST API.
Когда browser делает запрос к `Catalog`, он делает cross origin request (разные origin)
напрямую к сервису *(3)*:

```text
GET https://localhost:5001/items
```

Сервис также возвращает JSON представление данных каталога *(4)*, но клиент теперь знает, что
response идет из другого origin (благодаря Request Header):

```text
                       (3)  GET https://localhost:5001/items
        Browser       --------------------------------------->       Catalog
                      <---------------------------------------
http://localhost:3000           (4)  JSON (answer)             https://localhost:5001

                                Request Headers
               http://localhost:3000  !=  https://localhost:5001
```

Поэтому клиент (броузер) отклоняет запрос с CORS Error: он следует Same-origin policy.

*Same-origin policy* - a web application can only request resources from the same origin
the application was loaded from.

Это сделано, чтобы избежать чтения конфиденциальной информации посторонними сайтами.

Решение - использование Cross-Origin Resource Sharing (CORS).

*CORS* - allows a server to indicate (указывать/задавать) any other origins than its own from
which a browser should permit (разрешать) loading of resources.

Браузер опять отсылает GET-запрос сервису Catalog. Но на этот раз микросервис сконфигурирован
с `Access-Control-Allow-Origin` header, который указывает, что разрешены доступ к API для
других origins:

```text
                       (5)  GET https://localhost:5001/items
                            Headers:
                            Origin: http://localhost:3000
        Browser       --------------------------------------->      Catalog
                      <---------------------------------------
http://localhost:3000  (4)  JSON (answer)                      https://localhost:5001
                            Headers:                           Access-Control-Allow-Origin:
                            Access-Control-Allow-Origin:       http://localhost:3000
                            http://localhost:3000
```

Теперь Request Headers совпадают и браузер принимает ответ.

Этот прием работает только для простых запросов GET. Для запросов POST, PUT и т.д. работает
немного более сложно.

Клиент (бразуер) отправляет серверу предварительный запрос *CORS Preflighted request*
с тремя Headers:

* `Origin: http://localhost:3000`
* `Access-Control-Request-Headers: content-type`
* `Access-Control-Request-Method: POST`

В ответ сервер должен отправить ответ также с тремя Headers:

* `Origin: http://localhost:3000`
* `Access-Control-Request-Headers: content-type`
* `Access-Control-Request-Method: POST`

```text
                       GET https://localhost:5001/items
                       Headers:
                       Origin: http://localhost:3000
                       Access-Control-Request-Headers: content-type
                       Access-Control-Request-Method: POST
        Browser       --------------------------------------------->        Catalog
                      <---------------------------------------------
http://localhost:3000  Headers:                                      https://localhost:5001
                       Origin: http://localhost:3000
                       Access-Control-Request-Headers: content-type
                       Access-Control-Request-Method: POST

                      POST https://localhost:5001/items
                      {"name":"a","description":"b","price":9}
                      Headers:
                      Origin: http://localhost:3000
                      --------------------------------------------->
                      <---------------------------------------------
                      201 Created
                      Headers:
                      Access-Control-Allow-Origin: http://localhost:3000
```

Только потом клиент (бразуер) отправляет серверу запрос POST с данными JSON. В ответ
сервер, если все закончилось удачно, должен прислать ответ 201 Created.

Т.о., чтобы все работало необходимо сконфигурировать CORS на стороне микросервисов
`Play.Catalog` и `Play.Inventory`.

## Lesson 51. Adding the CORS middleware

Добавление CORS на примере `Play.Catalog`. Аналогично добавляется и в `Play.Inventory`.

1. Правка файла конфигурации `appsettings.Development.json`.

Добавлена секция со строкой:

```json
"Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
"AllowedOrigin": "http://localhost:3000"
```

Добавлен адрес frontend'а.

Правка сделана в `appsettings.Development.json`, а не в `appsettings.json`, т.к. origin frontend'а
a development server at this point. Нам не требуется настройка CORS policy для origin in production.

2. Правка файла `Startup`.

2.1. Добавление константы

```csharp
string AllowedOriginSetting = "AllowedOrigin";
```

Для использования в качестве ключа при чтении значения из файла конфигурации.

2.2. Метод `void Configure(IApplicationBuilder, IWebHostEnvironment)`.

Добавление производится в секцию `if (env.IsDevelopment())` т.к. добавляем origin для режима
development.

```csharp
if (env.IsDevelopment())
{
    // ..

    app.UseCors(builder =>
    {
        builder.WithOrigins(Configuration[AllowedOriginSetting])
            .AllowAnyHeader()       // Allow any header that the client wants to send
            .AllowAnyMethod();      // Allow any method used from the client side
    });
}
```

*(Напоминание)*
`Configuration` является объектом:

```csharp
public IConfiguration Configuration { get; }
```

который автоматически инициализируется ASP.NET. Данное свойство включает в себя все содержимое
конфигурационных файлов `appsettings.Development.json` и `appsettings.json`.

### Проверка работы. Теперь удачная

Запущено:

1. docker контейнеры.
2. `Play.Catalog` service
3. `Play.Frontend`

Пытаемся посмотреть каталоги через `Play.Frontend`, в браузере
`http://localhost:3000/`

Теперь каталог `Play.Catalog` будет доступен для просмотра.

Можно посмотреть детали запроса: `F12 -> Network` -> request `items` В разделе Headers:

* в секции Request Headers можно увидеть значение *origin*, который отсылает клиент.

* в секции Response Headers можно увидеть значение *access-control-allow-origin* со стороны сервера.

#### Замечание

Может опять возникнуть ошибка при попытке загрузки Catalog. На этот раз такая:

```text
Failed to load resource: net::ERR_CERT_AUTHORITY_INVALID
```

На этот раз связано с установкой (точнее отсутствием) сертификатов безопасности.
Помогло разрешение в браузере unsafe соединения.

## Lesson 52. Exploring the frontend to microservices communication

Исследование поведения микросервисов и frontend'а. Для этого все они запускаются
из VS Code в debug режиме.

Итого запущены:

1. docker контейнеры.
2. `Play.Catalog` service
3. `Play.Inventory` service
4. `Play.Frontend`

* `Play.Catalog` и `Play.Inventory` зарускаются командой из VS Code "Start Debugging".

* `Play.Frontend` запускается командой из VS Code "Run and Debug", в режиме (Server/Client).

*Мое замечание*: может понадобиться поправить `Play.Frontend/.vscode/launch.json`, раздел
клиента для запуска нужного браузера:

```json
//"type": "pwa-msedge",   -- Изначально был такой параметр. Для Windows.
"type": "pwa-chrome",                           // Выставил для Linux, для отладки в chromium.
"runtimeExecutable": "/usr/bin/chromium",       // В Linux, добавил путь для запуска chromium.
"request": "launch",
"name": "Client",
"url": "http://localhost:3000",
"webRoot": "${workspaceFolder}/src"
```

Client fronend'а запускается в браузере не сразу - надо подождать, пока открывается `localhost:3000`.
Компиляция и запуск сервера видны в VS Code, в Debug Console.
