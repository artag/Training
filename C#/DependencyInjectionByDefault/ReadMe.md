# Стандартный DI-контейнер от Microsoft

*Источник: http://ts-soft.ru/blog/new-standard-net-libs-part-1*

Nuget пакет: **Microsoft.Extensions.DependencyInjection**

Смотреть файлы:
* `ProgramDisposePart.cs` - Работа Scopes. Задание различных времен существования объектов.

* `ProgramBindingInCollectionPart.cs` - биндинг в коллекции.
(При биндинге нескольких одинаковых дескрипторов можно получить `IEnumerable` таких дескрипторов).

* `ProgramBindingGeneric.cs` -  биндинг дженериков.

* `ProgramValidateScopesTests.cs` - запрет на доступ к сервисам,
добавленным в ServiceLifetime.Scoped, в корень ServiceProvider.

* Простая реализация биндинга по атрибутам - пока не стал изучать
