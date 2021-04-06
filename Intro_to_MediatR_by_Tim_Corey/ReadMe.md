# Intro to MediatR - Implementing CQRS and Mediator Patterns

*Источник: https://youtu.be/yozD5Tnd8nw*

* Command
* Query
* Responsibility
* Segregation

При CQRS подходе операции CRUD делятся на:
* R
* CUD

Имеет смысл использовать CQRS для больших приложений.

Типичная структура приложения.

```text
Frontend (FE) -> Business Logic (BL) -> DataAccess (DA) -> DataBase (DB)
```

Сложные зависимости сервисов (классов) приложения:

```text
A (FE) зависит от B (BL), C (BL), D (BL), E(FE)
B (BL) зависит от F (DA)
E (FE) зависит от C (BL), D (BL), F (DA)
```

При использовании паттерна mediator. Mediator используется как связующее звено между классами
`A` и `B`:

```text
    Caller             Handler
A --------> Mediator --------> B

Caller : Handler
     1 : 1
```

Используются:

* Query - не изменяет данные (только чтение)
* Command
* Handler

Для одного Query или Command используется один Handler.

Более подробно - см. прилагаемый код.
