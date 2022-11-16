# Анализ сэмпла чистой архитектуры от Steve Smith

Source Git: `https://github.com/ardalis/CleanArchitecture`

Fork Git: `https://github.com/denis-tsv/CleanArchitecture-ardalis`

## Обзор структуры проекта

*Проект: 17. FromSteveSmith*

<img src="images/30_solution_scheme.jpg" alt="Clean architecture from Steve Smith" style="width:400px">

### Слой 1. `Clean.Architecture.SharedKernel`

<img src="images/31_shared_kernel.jpg" alt="Clean.Architecture.SharedKernel project" style="width:320px">

Содержит набор базовых классов и интерфейсов.

- Папка `Interfaces`
  - `IAggregateRoot` - интерфейс-маркер для Aggregate Root Entities
  - `IDomainEventDispatcher`
  - `IReadRepository`
  - `IRepository` - интерфейс репозитория

- Корень проекта
  - `DomainEventBase` - абстрактный класс DomainEvent
  - `DomainEventDispatcher`
  - `EntityBase` - абстрактный класс Entity
  - `IgnoreMemberAttribute` - атрибут
  - `ValueObject` - абстрактный класс ValueObject

### Слой 2. `Clean.Architecture.Core`

<img src="images/32_core.jpg" alt="Clean.Architecture.Core project" style="width:370px">

Зависит от `Clean.Architecture.SharedKernel`

- Папка `Interfaces`
  - `IMailSender` - интерфейс отправки email.
  - `ITodoItemSearchService` - сервис для поиска.

- Папка `ProjectAggregate`

  - Папка `Events`. Доменные события.
    - `NewItemAddedEvent`
    - `ToDoItemCompletedEvent`

  - Папка `Handlers`
    - `ItemCompletedEmailNotificationHandler`. Handler для отправки email.

  - Папка `Specification`
    - `IncompleteItemsSearchSpec`
    - `IncompleteItemsSpec` - все нерешенные задачи.
    - `ProjectByIdWithItemsSpec`

  - `PriorityStatus`. Какой-то класс
  - `Project`. Какое-то Entity.
  - `ProjectStatus`. Enum.
  - `ToDoItem`. Entity. Используется Rich модель.

- Папка `Services`
  - `ToDoItemSearchService` - реализация сервис для поиска.

### Слой 3. `Clean.Architecture.Infrastructure`

<img src="images/33_infrastructure.jpg" alt="Clean.Architecture.Infrastructure project" style="width:320px">

Зависит от `Clean.Architecture.Core` и `Clean.Architecture.SharedKernel`

- Папка `Data`
  - Папка `Config`. Файлы конфигурации сущностей в EntityFramework.
    - `ProjectConfiguration`
    - `Configure`

  - `AppDbContext`. Внутри него mediator, при успешном сохранении дынных в БД, все events,
  которые есть в сохраненном mediator, публикуются.
  - `EfRepository` - реализация `IRepository`. Реализация репозитория является вырожденной - все,
  что делают методы репозитория это делегируют вызовы методов в `AppDbContext`.

- Корень проекта
  - `DefaultInfrastructureModule` - модуль, регистрирующий зависимости (`Autofac`).
  - `FakeEmailSender`
  - `SmtpEmailSender` - реализация сервиса для отправки email.
  - `StartupSetup` - вспомогательный класс для регистрации `AppDbContext`.

### Слой 4. `Clean.Architecture.Web`

Composition Root.

<img src="images/34_web.jpg" alt="Clean.Architecture.Web project" style="width:320px">

Зависит от `Clean.Architecture.Infrastructure`

- Папка `Api`. В этой папке находятся API контроллеры:
  - `BaseApiController`
  - `MetaController`
  - `ToDoItemsController`

- Папка `ApiModels`. В этой папке находятся DTO для контроллеров:
  - `ProjectDTO`
  - `ToDoItemDTO`

- Папка `Controllers`. В этой папке находятся "обычные" (не API) контроллеры:
  - `HomeController`
  - `ProjectController`

Прямо в методах контроллеров API реализована Application логика. Пример:

- Получение данных
- Выполнение бизнес-операции
- Сохранение данных в репозитории
- Маппинг результата в DTO

## Проблемы проекта

### 1. Непонятное содержимое проекта `Clean.Architecture.SharedKernel`

<img src="images/31_shared_kernel.jpg" alt="Clean.Architecture.SharedKernel project" style="width:320px">

Что такое "SharedKernel"? Какое его назначение? Это неудачное наименование.

С точки зрения чистой архитектуры:

#### Переместить в `Entities`

- Папка `Interfaces`
  - `IAggregateRoot` - интерфейс-маркер для Aggregate Root Entities
  - `IDomainEventDispatcher`

- Корень проекта
  - `DomainEventBase` - абстрактный класс DomainEvent
  - `DomainEventDispatcher`
  - `EntityBase` - абстрактный класс Entity
  - `IgnoreMemberAttribute` - атрибут
  - `ValueObject` - абстрактный класс ValueObject

#### Переместить в `Infrastructure.Interfaces` (или `Database.Infrastructure.Interfaces`)

- Папка `Interfaces`
  - `IReadRepository`
  - `IRepository` - интерфейс репозитория

т.к. данные интерфейсы относятся к инфраструктуре (доступ к БД) и согласно чистой архитектуре
они не должны входить в состав Entities.

Но, DDD допускает наличие подобных интерфейсов в слое Entities.

### 2. Проблемы `Clean.Architecture.Core`

<img src="images/32_core.jpg" alt="Clean.Architecture.Core project" style="width:370px">

Что такое "Core"? Какое его назначение? Это Entities, Application Service, Domain Service?
Короче, неудачное наименование.

С точки зрения чистой архитектуры:

#### Переместить в `Entities`

- Папка `ProjectAggregate`

  - Папка `Events`. Доменные события.
    - `NewItemAddedEvent`
    - `ToDoItemCompletedEvent`

  - Папка `Specification`. (Относится к бизнес логике).
    - `IncompleteItemsSearchSpec`
    - `IncompleteItemsSpec` - все нерешенные задачи.
    - `ProjectByIdWithItemsSpec`

  - `PriorityStatus`. Какой-то класс
  - `Project`. Какое-то Entity.
  - `ProjectStatus`. Enum.
  - `ToDoItem`. Entity. Используется Rich модель.

#### Переместить в `Infrastructure.Interfaces`

- Папка `Interfaces`
  - `IMailSender` - интерфейс отправки email.

#### Переместить в `ApplicationServices.Interfaces`

- Папка `Interfaces`
  - `ITodoItemSearchService` - сервис для поиска.

#### Переместить в `ApplicationService` (или `UseCases`)

- Папка `ProjectAggregate`

  - Папка `Handlers`
    - `ItemCompletedEmailNotificationHandler`. Handler для отправки email.

- Папка `Services`
  - `ToDoItemSearchService` - реализация сервиса для поиска.

### 3. Проблемы `Clean.Architecture.Infrastructure`

<img src="images/33_infrastructure.jpg" alt="Clean.Architecture.Infrastructure project" style="width:320px">

В принципе, здесь проблем нет: здесь находится инфраструктура. Единственное, возможно,
придется разделить этот проект на несколько, если будет слишком много инфраструктуры.

## Перепроектирование (рефакторинг) проекта

*Проект: 18. FromSteveSmithRefactor1*

### Изменения в `Clean.Architecture.SharedKernel`

1. Создание нового проекта `Clean.Architecture.Infrastructure.Interfaces` и перенос туда:

- `IReadRepository`
- `IRepository` - интерфейс репозитория

Ссылка на `Clean.Architecture.Entity.Abstractions`

2. Переименование `Clean.Architecture.SharedKernel` в `Clean.Architecture.Entity.Abstractions`.

Т.к. в данном классе остались абстрактные сущности для их использования в Entities.

Этот проект нельзя назвать `*.Utils`, т.к. здесь нет:

- Extensions methods
- Helper'ов
- Cross cutting concerns (пронизывающая функциональность)
- Логирования
- Обработки ошибок

### Изменения в  `Clean.Architecture.Core`

1. Переименование `Clean.Architecture.Core` в `Clean.Architecture.ApplicationServices`.

2. Создание нового проекта `Clean.Architecture.Entities` и перенос туда:

- Папка `ProjectAggregate`

  - Папка `Events`. Доменные события.
    - `NewItemAddedEvent`
    - `ToDoItemCompletedEvent`

  - Папка `Specification`
    - `IncompleteItemsSearchSpec`
    - `IncompleteItemsSpec` - все нерешенные задачи.
    - `ProjectByIdWithItemsSpec`

  - Папка `Specification`. (Относится к бизнес логике).
    - `IncompleteItemsSearchSpec`
    - `IncompleteItemsSpec` - все нерешенные задачи.
    - `ProjectByIdWithItemsSpec`

  - `PriorityStatus`. Какой-то класс
  - `Project`. Какое-то Entity.
  - `ProjectStatus`. Enum.
  - `ToDoItem`. Entity. Используется Rich модель.

`Clean.Architecture.Entities` будет ссылаться на `Clean.Architecture.Entity.Abstractions`.

3. Перенос в `Clean.Architecture.Infrastructure.Interfaces`

Из папки `Interfaces`, `IMailSender` - интерфейс отправки email.

4. Создание нового проекта `Clean.Architecture.ApplicationServices.Interfaces` и перенос туда:

Из папки `Interfaces`, `ITodoItemSearchService` - сервис для поиска.

5. Это остается в `ApplicationServices`:

- Папка `ProjectAggregate`
  - Папка `Handlers`
    - `ItemCompletedEmailNotificationHandler`. Handler для отправки email.

- Папка `Services`
  - `ToDoItemSearchService` - реализация сервиса для поиска.

6. Проект `Clean.Architecture.ApplicationServices` будет ссылаться на

- `Clean.Architecture.Entities`
- `Clean.Architecture.Infrastructure.Interfaces`
- `Clean.Architecture.ApplicationServices.Interfaces`

### Ссылки в проектах `Clean.Architecture.Infrastructure` и `Clean.Architecture.Web`

- `Clean.Architecture.Infrastructure` ссылается на:

  - `Clean.Architecture.Entities`
  - `Clean.Architecture.Infrastructure.Interfaces`

- `Clean.Architecture.Web` ссылается на:

  - `Clean.Architecture.ApplicationServices`
  - `Clean.Architecture.Infrastructure`

### Итоговая промежуточная архитектура после рефакторинга


<img src="images/35_refactor_arch1.jpg" alt="Architecture after the first refactoring" style="width:750px">
