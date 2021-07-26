## Lesson 8. Creating a microservice via the .NET CLI

### Новый Web API проект

```text
dotnet new webapi -n Play.Catalog.Service
```

### Рассказано про:

* `Program.cs`
* `Startup.cs`
* `WeatherForecast.cs`
* Директория `Controllers` и `Controllers/WeatherForecastController.cs`
* `.vscode/launch.json` определяет how VS Code launches the app
* `.vscode/tasks.json` определяет how VS Code starts a build

В `Properties/launchSettings.json` microservice addresses "applicationUrl":

```text
"applicationUrl": "https://localhost:5001;http://localhost:5000",
```

### Build проект

Из директории, где лежит файл `*.csproj`:

```text
dotnet build
```

Ну или задать файл `.vscode/tasks.json` и нажать `Ctrl+Shift+B` (я поставил `F6`).

### Run проект

Из директории, где лежит файл `*.csproj`:

```text
dotnet run
```

Ну или задать файл `.vscode/launch.json` и нажать `Ctrl+Shift+D` - запуск, `F5` - запуск в режиме
отладки.

### Задать сертификат для HTTPS development

Из терминала, из директории, где лежит файл `*.csproj`:

```text
dotnet dev-certs https --trust
```

Для Linux появляется следующее сообщение:

"Trusting the certificate on Linux distributions automatically is not supported. For instructions
on how to manually trust the certificate on your Linux distribution, go to
https://aka.ms/dev-certs-trust"

### Entry address for web api project

`https://localhost:5001/swagger/index.html`
