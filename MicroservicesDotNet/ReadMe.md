# Microservices in .NET (SECOND EDITION)

- Part 1. Getting started with microservices
  - [Chapter 1. Microservices at a glance](Chapter01.md)
    - Understanding microservices and their core characteristics
    - Examining the benefits and drawbacks of microservices
    - An example of microservices working in concert to serve a user request
    - Using ASP.NET for a simple application
  - [Chapter 2. A basic shopping cart microservice](Chapter02.md)
    - A first iteration of an implementation of the shopping cart microservice
    - Creating HTTP endpoints with ASP.NET MVC
    - Implementing a request from one microservice to another
    - Implementing a simple event feed for a microservice
  - [Chapter 3. Deploying a microservice to Kubernetes](Chapter03.md)
    - Packaging a microservice in a Docker container
    - Deploying a microservice container to Kubernetes on localhost
    - ~~Creating a basic Kubernetes cluster on Azure's AKS (Azure Kubernetes Service)~~
    - ~~Deploying a microservice container to a Kubernetes cluster on AKS~~

- Part 2. Building microservices
  - [Chapter 4. Identifying and scoping microservices](Chapter04.md)
    - Scoping microservices for business capability.
    (Определение области применения микросервисов для бизнес-функций)

    - Scoping microservices to support technical capabilities.
    (Определение области применения микросервисов для поддержки технических функций)

    - Scoping microservices to support efficient development work.
    (Определение области применения микросервисов для эффективной разработки/развития)

    - Managing when scoping microservices is difficult.
    (Разрешение затруднений в определении области действия микросервисов)

    - Carving out new microservices from existing ones.
    (Создание новых микросервисов из существующих)

  - [Chapter 5. Microservice collaboration](Chapter05.md)
    - Understanding how microservices collaborate through commands, queries, and events.
    (Понимание того, как микросервисы взаимодействуют с помощью команд, запросов и событий)

    - Comparing event-based collaboration with collaboration based on commands and queries.
    (Сравнение совместной работы на основе событий с совместной работой, основанной на командах и запросах)

    - Implementing an event feed.
    (Реализация потока событий)

    - Implementing command-, query-, and event-based collaboration.
    (Реализация совместной работы на основе команд, запросов и событий)

    - Deploying collaborating microservices to Kubernetes.
    (Развертывание взаимодействующих микросервисов в Kubernetes)

  - [Chapter 6. Data ownership and data storage](Chapter06.md)
    - Which data microservices store.
    (Какие данные хранят микросервисы).

    - Understanding how data ownership follows business capabilities.
    (Понимание того, как владение данными соответствует бизнес-функциям).

    - Using data replication for speed and robustness.
    (Использование репликации данных для повышения скорости и надежности).

    - Building read models from event feeds with event subscribers.
    (Построение read models из каналов (лент) событий с помощью подписчиков на события).

    - Implementing data storage in microservices.
    (Реализация хранения данных в микросервисах).
