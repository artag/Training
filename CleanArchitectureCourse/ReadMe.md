# Чистая архитектура на практике

Курс: `https://www.udemy.com/course/clean-architecture-csharp-ru/`

Git: `https://github.com/denis-tsv/CleanArchitectureCourse`

Проект `01. Initial` - начальный проект по "стандартной" архитектуре.

## Создание Migration

### Установка (обновление) Entity Framework Core Tool

Надо сделать только раз. Либо шаг 1, либо шаг 2.

Шаг 1. Если `dotnet-ef` еще не установлен

```text
dotnet tool install --global dotnet-ef
```

Шаг 2. Если `dotnet-ef` еще не установлен, но требует обновления:

```text
dotnet tool update --global dotnet-ef
```

### Создание миграции

```text
dotnet ef migrations add Initial -s WebApp.csproj -p ../DataAccess/DataAccess.csproj
```

- `Initial` - имя миграции

- `-s WebApp.csproj` - The startup project to use.
  Defaults to the current working directory.

- `-p ../DataAccess/DataAccess.csproj` - The project to use. Defaults to the current
  working directory. (Именно здесь и будет создана migration).

### Применение миграции

```text
dotnet ef database update -v
```

- `-v` или `--verbose` - необязательный параметр, позволяет увидеть более подробную информацию
  о том, что происходит при выполнении команды.

Файл БД `WebApp.db` появляется прямо в директории `01. Initial/WebApp/`.
