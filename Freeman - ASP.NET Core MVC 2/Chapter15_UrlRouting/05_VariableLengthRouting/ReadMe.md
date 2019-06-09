# Определение маршрутов переменной длины

Можно маршрутизировать URL произвольной длины в единственном маршруте.
Для этого надо определить переменную *общего захвата* для чего она предваряется символом `*`
(см. `Startup.Configure()`):
```cs
...
routes.MapRoute(
    name: "MyRoute",
    template: "{controller=Home}/{action=Index}/{id?}/{*catchall}"
...
```

* Сегментов 0: - "/" - controller = `Home`, action = `Index`
* Сегментов 1: - "/Customer" - controller = `Customer`, action = `Index`
* Сегментов 2: - "/Customer/List" - controller = `Customer`, action = `List`
* Сегментов 3: - "/Customer/List/All" - controller = `Customer`, action = `List`, id = `All`
* Сегментов 4: - "/Customer/List/All/Delete" -
controller = `Customer`, action = `List`, id = `All`, catchall = `Delete`
* Сегментов 5: - "/Customer/List/All/Delete/Perm" -
controller = `Customer`, action = `List`, id = `All`, catchall = `Delete/Perm`

Первые три сегмента применяются для установки `controller`, `action` и `id`. Если URL содержит
дополнительные сегменты, то все они присваиваются переменной `catchall`.

*Замечание*: название переменной общего захвата может быть любым,
главное - звездочка `*` перед переменной.

Пример использования переменных `id` и `catchall` см. в `CustomerController.List()`.
