# 8. Writing tests for microservices

    - Writing good automated tests.
    (Написание хороших автоматизированных тестов).

    - Understanding the test pyramid and how it applies to microservices.
    (Понимание тестовой пирамиды и ее применимость к микросервисам).

    - Testing microservices from the outside.
    (Тестирование микросервисов извне).

    - Writing fast, in-process tests for endpoints
    (Написание быстрых in-process (внутрипроцесных) тестов для endpoints).

    - Using `Microsoft.AspNetCore.TestHost` for integration and unit tests
    (Использование `Microsoft.AspNetCore.TestHost` для интеграционых и юнит тестов).

## 8.1 What and how to test

### 8.1.1 The test pyramid: What to test in a microservices system

<img src="images/50_test_pyramid.jpg" alt="The test pyramid" width=600/>

В этой книге показана test pyramid из трех уровней:

- System tests (верхний уровень). Тесты, которые охватывают всю систему микросервисов и
обычно реализуются через GUI.

- Service tests (средний уровень). Тесты, которые работают с одним микросервисом.
Тестируют его целиком.

- Unit tests (нижний уровень). Тесты, которые проверяют одну небольшую часть функциональности
в микросервисе - модульные (unit) тесты.

На тестовой пирамиде видно, что чем ближе к нижнему уровню, тем больше должно быть тестов.
На самом деле, количество тестов на каждом из уровней зависит от: размера системы, ее
сложности и стоимости отказов.

### 8.1.2 System-level tests: Testing a complete microservice system end to end

Характеристики system-level тестов:

- Широкий охват. Тестируют сразу наибольший объем кода.
- Если тест завершается ошибкой, не понятно где ее искать и в чем ее причина.
- Самые медленные.

Рекомендуется писать system-level тесты для тестирования success paths в наиболее важных
частях системы. Если необходимо, можно писать system-level тесты и для наиболее
важных failure сценариев.

### 8.1.3 Service-level tests: Testing a microservice from outside its process

Характеристики service-level тестов:

- Тесты взаимодействуйте с одним целым изолированным микросервисом
- Взаимодействующие микросервисы  заменяются mock'ами
- Service-level тестам взаимодействуют с тестируемым микросервисом извне.
- Данные тесты напрямую взаимодействуют с API микросервиса.
- Тесты service-level более точны, чем тесты system-level, т.к. они охватывают
только один микросервис.

Microservice mock'и:

- Содержат те же endpoints, что и реальный микросервис.
- Ответы (responses) hardcoded.

Тесты service-level тестируют сценарии, а не отдельные запросы.
То есть они делают последовательность запросов, которые вместе формируют сценарий.
Запросы, отправляемые тестируемым микросервисом в сервисы-mock'и, являются
реальными HTTP-запросами, а ответы - реальными HTTP-ответами.

Для теста loyalty program микросервиса:

<img src="images/51_loyalty_program.jpg" alt="The loyalty program microservice collaboration" width=600/>

можно создать mock версии микросервисов, с которыми он взаимодействует:

<img src="images/52_mocked_service.jpg" alt="Mocked service" width=600/>

Тест service-level для микросервиса loyalty program может делать следующее:

1. Отправлять команду для создания пользователя.

2. Сделать запрос от loyalty program к mock версии микросервиса special offers. Затем
получить от последнего жестко запрограммированые события о новом специальном предложении.

3. Записывать любые команды, отправленные в микросервис notifications.
Проверять, что команда уведомления пользователя о новом специальном предложении была
отправлена.

Для тестовых вызовов endpoints, реализованных в при помощи MVC контроллеров,
используется библиотека `Microsoft.AspNetCore.TestHost`.
Эта библиотека позволяет вам писать тесты, которые вызывают
ASP.NET endpoints в памяти. Вызовы к тестируемому коду делаются через `TestServer` из
`Microsoft.AspNetCore.Mvc.Testing`.

Рекомендации:

- Писать service-level тесты для всех успешных сценариев (success paths) микросервиса.
Желательно покрыть все endpoint'ы микросервиса.

- По желанию и исходя из требований. Писать service-level тесты только для самых важных
failure сценариев.

#### Contract tests

Контрактный тест - это тест для определения, реализует ли
вызываемый микросервис контракт, ожидаемый вызывающей стороной (другим микросервисом).

Статья о contract tests: https://martinfowler.com/bliki/ContractTest.html

Контрактные тесты:

- написаны с точки зрения вызывающей стороны
- не должны знать о том, как реализованы тестируемые ими микросервисы
- не должны знать о других микросервисах, с которыми взаимодействут тестируемый микросервис

Неплохой идеей является автоматический запуск подобных тестов при каждом развертывании
тестируемого микросервиса.

<img src="images/53_contract_test.jpg" alt="A contract test" width=600/>

### 8.1.4 Unit-level tests: Testing endpoints from within the process

Характеристики unit-level тестов:

- Тестируют только один микросервис
- Тестируют только часть микросервиса
- Самые быстрые среди всех тестов
- Идеальны для тестирования failure сценариев
- Легко обнаруживают ошибки из-за малого тестового покрытия

Два типа unit-level тестов:

- Используют реальную БД (*мое примечание: я бы не назвал это полноценными unit-тестами*)
- Используют in-memory БД

## 8.2 Testing libraries: `Microsoft.AspNetCore.TestHost` and `xUnit`

### 8.2.3 xUnit and Microsoft.AspNetCore.TestHost working together

<img src="images/54_service-level_test.jpg" alt="A service-level test" width=600/>
