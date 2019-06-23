# Использование цепочек зависимостей

В данном проекте демонстрируется как работает внедрение зависимостей с цепочками зависимостей.

1. MVC надо создать `InjectionController`.
2. `InjectionController` зависит от `IRepository`.
3. Класс `MemoryRepository`, который реализует `IRepository`, сам зависит от `IModelStorage`.
4. Класс `DictionaryStorage` реализует `IModelStorage` и ни от чего более не зависит.

Особенность: в классе `Startup.ConfigureServices()` должны быть определены все требуемые интерфейсы
и их реализации для всей цепочки зависимостей, чтобы создать объект класса `InjectionController`:
```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddTransient<IRepository, MemoryRepository>();
    services.AddTransient<IModelStorage, DictionaryStorage>();
    ...
}
```

И это всё!
