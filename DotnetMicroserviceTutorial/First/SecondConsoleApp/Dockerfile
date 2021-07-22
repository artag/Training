FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SecondConsoleApp/SecondConsoleApp.csproj", "SecondConsoleApp/"]
RUN dotnet restore "SecondConsoleApp/SecondConsoleApp.csproj"
COPY . .
WORKDIR "/src/SecondConsoleApp"
RUN dotnet build "SecondConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SecondConsoleApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SecondConsoleApp.dll"]
