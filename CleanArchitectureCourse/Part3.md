# Анализ сэмпла чистой архитектуры от Steve Smith

Source Git: `https://github.com/ardalis/CleanArchitecture`

Fork Git: `https://github.com/denis-tsv/CleanArchitecture-ardalis`

## Обзор структуры проекта

*Проект: 17. FromSteveSmith*

<img src="images/30_solution_scheme.jpg" alt="Clean architecture from Steve Smith" style="width:400px">

### Слой 1. `CleanArchitecture.SharedKernel`

<img src="images/31_shared_kernel.jpg" alt="CleanArchitecture.SharedKernel project" style="width:320px">

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

### Слой 2. `CleanArchitecture.Core`

<img src="images/32_core.jpg" alt="CleanArchitecture.Core project" style="width:370px">

Зависит от `CleanArchitecture.SharedKernel`

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

### Слой 3. `CleanArchitecture.Infrastructure`

<img src="images/33_infrastructure.jpg" alt="CleanArchitecture.Infrastructure project" style="width:320px">

Зависит от `CleanArchitecture.Core` и `CleanArchitecture.SharedKernel`

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

### Слой 4. `CleanArchitecture.Web`

Composition Root.

<img src="images/34_web.jpg" alt="CleanArchitecture.Web project" style="width:320px">

Зависит от `CleanArchitecture.Infrastructure`

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
