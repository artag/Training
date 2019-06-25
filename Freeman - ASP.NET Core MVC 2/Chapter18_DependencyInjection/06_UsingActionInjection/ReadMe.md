# Использование внедрения в действия

Можно объявлять зависимости через параметры методов действий (см. `Controllers/HomeController`):
```cs
public IActionResult Index([FromServices]ProductTotalizer totalizer)
{
    ViewBag.Total = totalizer.Total;
    return View(_repository.Products);
}
```

Внедрение в действия выполняется с помощью атрибута `FromServices`, который применяется к параметру
действия.

Конфигурация служб обычна (из `Startup.ConfigureServices()`):
```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IRepository, MemoryRepository>();
    services.AddScoped<ProductTotalizer>();
    services.AddMvc();
}
```
