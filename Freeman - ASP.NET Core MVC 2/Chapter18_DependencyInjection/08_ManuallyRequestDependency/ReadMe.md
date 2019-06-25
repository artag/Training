# Запрашивание объекта реализации вручную

Можно работать с поставщиком служб напрямую (см. `Controllers/HomeController`):
```cs
public IActionResult Index()
{
    var repository = HttpContext.RequestServices.GetService<IRepository>();
    return View(repository.Products);
}
```

Свойство `HttpContext` определяет свойство `RequestServices`, которое возвращает `IServiceProvider`.
На `IServiceProvider` можно использовать следующие методы:

* `GetService<service>()`
Используется поставщик служб для создания нового экземпляра типа службы.
Возвращается `null`, если для запрошенного типа отсутствует отображение.

* `GetRequiredService<service>()`
Используется поставщик служб для создания нового экземпляра типа службы.
Генерирует исключение, если для запрошенного типа отсутствует отображение.

Этот прием получения зависимости известен как *Локатор служб* и его рекомендуется избегать.
