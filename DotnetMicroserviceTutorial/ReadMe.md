# Serivces

1. portainer.io

```text
localhost:9000
```

2. seq

```text
localhost:5341
```

# Commands

Portainer Server Deployment:

```text
docker volume create portainer_data
docker run -d -p 8000:8000 -p 9000:9000 --name=portainer --restart=always -v /var/run/docker.sock:/var/run/docker.sock -v portainer_data:/data portainer/portainer-ce
```

Docker-compose build containers:

```text
docker-compose up -d
```

Docker-compose build containers. Rebuild только одну из секций:

```text
docker-compose up -d --no-deps --build helloworld
```

Создание консольного приложения в новой директории:

```text
dotnet new console --name SecondConsoleApp
```

Создание worker service (по сути Background Service) в новой директории:

```text
dotnet new worker --name HelloWorldWorkerService
```

Удаление image:

```text
docker rmi <image_name>
```

Просмотр доступных images в системе:

```text
docker images
```

Добавление nuget package (Serilog) в проект:

```text
dotnet add package Serilog
dotnet add package Serilog.Sinks.Console        // Вывод в консоль
dotnet add package Serilog.Extensions.Hosting   // Для добавления в HostBuilder (ASP.NET)
dotnet add package Serilog.Sinks.Seq            // Вывод в сервис Seq
```

```text
docker-compose down --rmi all
```

Добавление nuget package (SpeedTest) в проект:

```text
dotnet add package SpeedTest.NetCore --version 2.1.0
```

В Seq можно выбирать с помощью выражений SQL:

1. Вывод сообщений, которые содержат "BandwidthTest".

```text
select *
from stream
where LogMessage = 'BandwidthTest'
```

2. Построение точек скорости download/upload через определенные промежутки времени (5 минут):

```text
select mean(download)
from stream
where LogMessage = 'BandwidthTest'
group by time(5m)
limit 1000
```

```text
select mean(upload)
from stream 
where LogMessage = 'BandwidthTest '
group by time(5m)
limit 10000
```

Далее в Seq можно выбрать "NEW DASHBOARD" и эти результаты этих запросов отобразятся на графике.
