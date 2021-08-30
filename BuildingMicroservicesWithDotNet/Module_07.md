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

*CORS* cross-origin resource sharing.
