# Жизненные циклы служб

Расширяющие методы, указывающие поставщику служб, каким образом распознавать зависимости:
* `AddTransient<service, implType>()`
Описание: Создание нового объекта реализации для службы.

* `AddTransient<service>`
Описание: Создание нового объекта одиночного типа.

* `AddTransient<service>(factoryFunc)`
Описание: Регистрация фабричной функции для создания объекта реализации службы.

* `AddScoped<service, implType>()`
* `AddScoped<service>()`
* `AddScoped<service>(factoryFunc)`
Описание: Созданные объекты должны использоваться повторно для всех объектов, ассоциированных
с общей областью действия, которой обычно является HTTP-запрос.

* `AddSingleton<service, implType>()`
* `AddSingleton<service>()`
* `AddSingleton<service>(factoryFunc)`
Описание: Созданные объекты создаются только один раз и используются на всем протяжении работы
приложения.

* `AddSingleton<service>(instance)`
Описание: Предоставляет объект, который должен применяться для обслуживания всех запросов к службе.
Новые объекты здесь не создаются.


Далее, в проекте происходит демонстрация времени жизни создаваемых объектов:

* `Transient`
Guid'ы объектов разные даже в рамках одного запроса.
При каждом новом запросе объекты создаются заново (Guid'ы обновляются).

* `Scoped`
Guid'ы объектов одинаковые в рамках одного запроса.
При каждом новом запросе объекты создаются заново (Guid'ы обновляются).

* `Singleton`
Guid'ы объектов одинаковые в рамках одного запроса.
При каждом новом запросе объекты остаются прежними (Guid'ы не меняются на всем протяжении работы приложения).

Вот как в текущем проекте выглядит `Startup.ConfigureServices()`:
```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddTransient<ITransientDI, TransientDIImpl>();
    services.AddTransient<TransientDI>();

    services.AddScoped<IScopedDI, ScopedDIImpl>();
    services.AddScoped<ScopedDI>();

    services.AddSingleton<ISingletonDI, SingletonDIImpl>();
    services.AddSingleton<SingletonDI>();

    services.AddTransient<ITransientFactoryDI>(provider => new TransientFactoryDIImpl());
...
}
```


## Использование фабричной функции

Отдельное внимание фабричной функции (из `Startup.ConfigureServices()`):
```cs
services.AddTransient<ITransientFactoryDI>(provider => new TransientFactoryDIImpl());
```

Здесь лямбда-выражение получает объект `IServiceProvider`.

В книге пример использования фабричной функции более мощный:
```cs
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddTransient<IRepository>(provider =>
    {
        if (env.IsDevelopment())
        {
            var x = provider.GetService<MemoryRepository>();
            return x;
        }
        else
        {
            return new AlternateRepository();
        }
    });

    services.AddTransient<MemoryRepository>()
    ...
}
```

В этом мощном примере `IServiceProvider` определяется одним из следующих методов:
* `GetService<service>()`
Используется поставщик служб для создания нового экземпляра типа службы.
Возвращается `null`, если для запрошенного типа отсутствует отображение.

* `GetRequiredService<service>()`
Используется поставщик служб для создания нового экземпляра типа службы.
Генерирует исключение, если для запрошенного типа отсутствует отображение.

Так как в книге `MemoryRepository` имеет свою собственную зависимость, ему тоже надо задать время
жизни:
```cs
services.AddTransient<MemoryRepository>()
```
