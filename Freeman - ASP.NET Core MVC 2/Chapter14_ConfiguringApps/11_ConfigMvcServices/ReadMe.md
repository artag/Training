# Конфигурирование служб MVC
В `Startup.ConfigureServices()` вызывается метод `AddMvc()`.
Он настраивает все нужные службы.

Для дополнительной настройки.
`AddMvc()` возвращает объект `IMvcBuilder`, для которого есть следующие методы:
* `AddMvcOptions()` - конфигурирует службы, используемые MVC (см. ниже).
* `AddFormatterMappings()` - конфигурирует формат получаемых данных (глава 20).
* `AddJsonOptions()` - конфигурирует способ создания данных JSON (глава 20).
* `AddRazorOptions()` - конфигурирует механизм визуализации `Razor` (глава 21).
* `AddViewOptions()` - конфигурирует способ обработки представлений MVC, в том числе применяемые
механизмы визуализации.


## Про AddMvcOptions()
`AddMvcOptions()` конфигурирует самые важные службы MVC. Он принимает функцию, получающую
`MvcOptions`, представляющий набор конфигурационных свойств.

Наиболее полезные конфигурационные свойства:
* `Conventions` - возвращает список соглашений модели, которые используются для настройки способа
создания контроллеров и действий MVC (глава 31).
* `Filters` - возвращает список глобальных фильтров (глава 19).
* `FormatterMappings` - возвращает сопоставления, применяемые для того, чтобы позволить клиентам
указывать формат получаемых ими данных (глава 20).
* `InputFormatters` - возвращает список объектов, используемых для разбора запросов (глава 20).
* `ModelBinders` - возвращает список связывателей модели, которые применяются для разбора запросов
(глава 26).
* `ModelValidatorProviders` - возвращает список объектов, используемых для проверки достоверности
данных (глава 27).
* `OutputFormatters` - возвращает список классов, которые форматируют данные, отправляемые из
контроллеров API (глава 20).
* `RespectBrowserAcceptHeader` - указывает, должен ли учитываться заголовок `Accept`, когда
принимается решение о том, какой формат данных задействовать для ответа (глава 20).


## Пример изменения параметра конфигурации в файле Startup.cs
```cs
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddMvc().AddMvcOptions(options =>
    {
        options.RespectBrowserAcceptHeader = true;
    }
    ...
}
```

Лямбда-выражение принимает объект `MvcOptions`, который используется для установки свойства
`RespectBrowserAcceptHeader` в `True`. Это позволит клиентам оказывать большее влияние на формат
данных, выбираемый процессом согласования содержимого (будет в главе 20).