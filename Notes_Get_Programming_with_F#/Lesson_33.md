# Lesson 33. Creating type provider-backed APIs

## Reasons for not exposing provided types over an API

* Данные, предоставляемые type provider'ом не всегда совпадают с бизнес-моделью (доменной моделью).

* Данные, создаваемые type provider'ом не являеются record или discriminated union,
что ограничивает их использование в коде.

* Данные, создаваемые type provider'ом не могут быть использованы "снаружи" F# (например, из
других проектов на C#).

* Данные, создаваемые type provider'ом чаще всего затираются во время runtime'а.
