# Определение необязательных сегментов URL

Необязательным является такой сегмент URL, который пользователь может не указывать и для которого
не предусмотрено стандартного значения.

Пример задания необязательного сегмента (из `Startup.Configure()`):
```cs
...
routes.MapRoute(
    name: "MyRoute",
    template: "{controller=Home}/{action=Index}/{id?}");
...
```

Данный марщрут будет соответствовать URL имеющим или не имеющим сегмент `id`:
* Сегментов 0: `/` - controller = `Home`, action = `Index`
* Сегментов 1: `/Customer` - controller = `Cusomer`, action = `Index`
* Сегментов 2: `/Customer/List` - controller = `Customer`, action = `List`
* Сегментов 3: `/Customer/List/All` - controller = `Customer`, action = `List`, id = `All`
* Сегментов 4: `/Customer/List/All/Delete` - Соответствия нет. Сегментов слишком много.

Переменная `id` добавляется к набору переменных, только когда во входящем URL присутствует
соответствующий сегмент.

Можно глянуть `HomeContoller.CustomVariable()` на предмет обработки id == null.


## Стандартная конфигурация маршрутизации

Метод `UseMvcWithDefaultRoute()`, который используется для настройки маршрутизации по умолчанию
в MVC, эквивалентен следующему коду:
```cs
...
routes.MapRoute(
    name: "default",
    template: "{controller=Home}/{action=Index}/{id?}");
...
```
