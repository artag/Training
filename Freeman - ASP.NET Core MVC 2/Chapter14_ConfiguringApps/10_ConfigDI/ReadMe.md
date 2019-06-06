# Конфигурирование внедрения зависимостей
См. пример в `Program.BuildWebHost()`
```cs
...
.UseDefaultServiceProvider(
    (context, options) =>
    {
        options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
    })
...
```

`UseDefaultServiceProvider()` - применяет встроенный поставщик служб ASP.NET Core.
Он принимает лямбда-функцию, которая получает объекты `WebHostBuilderContext` и
`ServiceProviderOptions`, применяемые для конфигурирования.

`ValidateScopes` - единственное свойство, его установка в `false` является обязательной при
работе с Entity Framework Core.

Про установку `ValidateScopes` в `false` было написано в главе 8:
> ..без такого изменения попытка создать схему базы данных в следvющем разделе приведет к генерации
> исключения.

```cs
public static IWebHost BuildWebHost(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .UseDefaultServiceProvider(options =>
            options.ValidateScopes = false)
        .Build();
```
