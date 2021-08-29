# Module 4. Preparing for the next microservice

## Lesson 22. Using Postman

Скачивается клиент Postman. Отсюда: <https://www.postman.com/downloads>

### GET запрос

1. Ставится `GET` запрос.
2. Адрес `https://localhost:5001/items`.
3. Send.

При запуске ругнется: "SSL Error: Unable to verify the first certificate". Это нормально, т.к.
сертификаты у нас самоподписанные и для разработки. Жмем на "Disable SSL Verification".

### POST запрос

1. Ставится `POST` запрос.
2. Адрес `https://localhost:5001/items`.
3. Переключение на Body.
4. Выбор флага `raw`.
5. В выпадающем списке `Text` выбирается `JSON`.
6. Сам текст (поле ниже):

```json
{
    "name": "Potion",
    "description": "Restores a small amount of HP",
    "price": 5
}
```

7. Send.

### Postman. Import from Swagger

Это слишком сложно, долго и нудно вот так вот вручную вводить запросы в Postman. Их можно
импортировать из Swagger.

1. Открываем стартовую страницу микросервиса в браузере: `https://localhost:5001/swagger/index.html`.
2. Копируем адрес ссылки с открывшейся страницы `https://localhost:5001/swagger/v1/swagger.json`
в буфер обмена.
3. В Postman нажимаем "Import" -> "Link" -> вставляется линк -> "Import".

3 пункт через Link не получилось импортировать. Postman пишет: "error while fetching data from link".

Получилось импортировать через:
"Import" -> "Raw text" -> Вставить в виде текста содержимое `swagger.json` -> "Import".

4. В Postman, во вкладке "Collections" появляются импортированные запросы.
5. Адреса в импортированных запросах выглядят подобным образом: `{{baseUrl}}/items`.
6. Задание значения для `{{baseUrl}}`:
   1. Перейти на верхнюю папку коллекции запросов и выбрать `...` (View more actions).
   2. Edit -> Variables
   3. В столбцах "Initial value" и "Current value" для переменной "baseUrl" задать адрес запущенного
   микросервиса: `https://localhost:5001`.
   4. Update

Теперь можно пробовать посылать запросы к API через Postman.

### Postman. Export collection, History, Environment, 

**Export collection**:

1. Перейти на верхнюю папку коллекции запросов и выбрать `...` (View more actions).
2. Export

**History** (Закладка слева). Содержит список запросов, которые были выполнены ранее.

**Environment** (комбобокс справа вверху). Позволяет переключать конфигурации запросов.

**Authorization** (Закладка). Позволяет сгенерировать запрос для авторизации (разные виды) через API.

### Отключение автоматического открытия броузера при запуске .NET Core приложения

1. Файл `.vscode/launch.json` -> секция `serverReadyAction`
2. Если удалить эту секцию, то браузер прекратит автоматически открываться при запуске приложения.
Тем не менее, микросервис все равно будет запускаться.

## Lesson 23. Reusing common code via NuGet

* Don't Repeat Yourself (DRY). Надо вынести общий код для всех микросервисов в отдельное, доступное
для них место.
* Microservices should be independent of each other. Нельзя оставить общий код в одном микросервисе
и сослаться на него из другого микросервиса.
* Each microservice should live in its own source control repository. И положить библиотеку
с общим кодом в виде проекта рядом с микросервисами тоже не получится.
* Решение - NuGet.

Немного про NuGet:

* NuGet is the package manager for .NET.
* A NuGet package is a single ZIP file (.nupkg) that contains files to share with others.
* Microservice projects don't need to know where NuGet packages are hosted.
* The common code is now maintained in a single place.
* The time to build new microservices is significantly reduced.

## Lesson 26. Moving generic code into a reusable NuGet package

Используется отдельная директория `Play.Common`. Лежит по соседству с другими директориями
микросервисов.

Создание библиотеки:

```text
dotnet new classlib -n Play.Common
```

После создания нового проекта проверяем запущен ли OmniSharp сервер в VS Code: для этого отрываем
любой `*.cs` файл в проекте.

После, в Command Palette выбираем: ".NET Generate Assets for Build and Debug". Это сгенерирует
директорию `.vscode` с `tasks.json` (сгенерится в директории, откуда запущен VS Code).

В `tasks.json` добавляем:

```json
"tasks": [
{
    //...
    "args": [ //...
    ],
    "problemMatcher": "$msCompile",    // Добавить под эту строку
    "group": {
        "kind": "build",
        "isDefault": true
    }
},
```

Добавление nuget-пакетов:

```text
dotnet add package MongoDB.Driver
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Binder
dotnet add package Microsoft.Extensions.DependencyInjection
```

### Создание nuget-пакета

```text
dotnet pack -o ../../../packages
```

где `-o` - выходная директория.

### Добавление в проект источника nuget-пакетов (локальная директория)

```text
dotnet nuget add source /полный_путь/packages -n PlayEconomy
```

где `n` - имя источника nuget-пакетов.

В Linux источник nuget-пакетов добавляется сюда: `/home/USER/.nuget/NuGet/NuGet.Config`.
`

## Lesson 27. Introduction to Docker Compose

Why need yet another Docker tool?

1. Multiple docker container to run.
2. Too many steps to setup infrastructure services.
3. Too many arguments to remember.
4. Some containers may need to talk to each other.
5. What if container depends on another container?

### What is Docker Compose?

A tool for defining and running multi-container Docker applications.

1. Для конфигурации docker compose используется файл `docker-compose.yml`.
Этот файл содержит информацию о:

* Запускаемых контейнерах
* Переменных окружения
* Порты
* Зависимости между контейнерами

2. Одна команда для запуска: `docker-compose up`.

3. Также docker compose предоставляет *Compose network*, благодаря которой контейнеры могут
общаться друг с другом.

## Lesson 28. Moving MongoDB to docker compose

Включение показа пробелов в VS Code:

```text
File -> Preferences -> Settings ->
-> Editor: Render Whitespace (поиск по "render whitespace") -> all
```

Установка extension для VS Code `Docker` (by Microsoft).

В отдельной (соседней) директории (Play.Infra) создается файл `docker-compose.yml`.

### В docker-compose.yml

(Пробельные отступы для задания свойств внутри секций обязательны - рекомендуется включить
в IDE показ пробелов).

1. Задание *version*. Version определяет какие features будут доступны для docker compose engine.

```yml
version: "3.8"      # На linux у меня запустилась только версия "3.3"
```

2. Секция *services*. Задает docker container. На примере container для mongodb.

Напоминание. Для запуска под docker-контейнером использовалась следующая команда:

```text
docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo:4.4.7
```

```yml
services:                             # секция
    mongo:                            # имя сервиса 
        image: mongo:4.4.7            # имя image контейнера с версией (которое в конце команды)
        container_name: mongo         # отображаемое имя контейнера (которое --name)
        ports:
            - 27017:27017             # массив портов, каждый порт на отдельной строке
        volumes:
            - mongodbdata:/data/db    # массив volumes (один элемент)

volumes:
    mongodbdata:
```

**Примечания:**

1. Если для `image` версия не нужна, то она просто не указывается.

2. docker-compose создает новый экземпляр `volumes` при переходе с обычного запуска docker.
Поэтому все `volumes` при таком переходе становятся пустыми.

### Остановка контейнера docker

1. Смотрим запущен ли контейнер:

```text
docker ps
```

2. Остановка контейнера:

```text
docker stop mongo
```

где `mongo` - имя контейнера, которое устанавливается при помощи атрибута `--name`.

### Запуск docker compose

Из директории, где лежит файл `docker-compose.yml`.

```text
docker-compose up -d
```

* `-d` (detach) - запуск docker-compose в "backround" режиме (в консоль не выводятся логи работы).
