# Конфигурирование проекта
## Конфигурационный файл
Самый важный конфигурационный файл - файл вида `*.csproj` (в более ранних версиях был `project.json`).

* `Project` - корневой элемент, который обозначает конфигурационный файл MSBuild.
* `PropertyGroup` - группирует связанные свойства для придания структуированности.
* `TargetFramework` - указывает целевую платформу .NET Framework для процесса построения. Должен определяться
внутри `PropertyGroup`. Для .NET Core 2.0 стартное значение `netcoreapp2.0`.
* `ItemGroup` - группирует связанные конфигурационные элементы для придания структуированности.
* `Folder` - сообщает MSBuild о том, как иметь дело с той или иной папкой в проекте. В текущем файле `*csproj` указывается необходимость включения папки `wwwroot`, когда приложение публикуется.
* `PackageReference` - определяет зависимость от пакета `NuGet`, который индентифицируется через `Include` и 
`Version`.

## Добавление пакетов в проект
3 способа:
1. В `Visual Studio`:
Tools -> NuGet Package Manager -> Manage NuGet Packages for Solution

2. Из командной строки:
```
dotnet add package System.Net.Http --version 4.3.2
```

3. Отредактировать файл `*.csproj`
Добавить:
```xml
<ItemGroup>
    ...
    <PackageReference Include="System.Net.Http" Version="4.3.2" />
    ...
</ItemGroup>
```

## Добавление пакетов инструментов в проект
Пакеты инструментов можно добавлять только путем правки файла `*.csproj`:
```xml
<ItemGroup>
    ...
    <PackageReference Include="System.Net.Http" Version="4.3.2" />
    <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.0" />
    ...
</ItemGroup>
```
