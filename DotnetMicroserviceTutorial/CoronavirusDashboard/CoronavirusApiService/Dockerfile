FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CoronavirusApiService/CoronavirusApiService.csproj", "CoronavirusApiService/"]
RUN dotnet restore "CoronavirusApiService/CoronavirusApiService.csproj"
COPY . .
WORKDIR "/src/CoronavirusApiService"
RUN dotnet build "CoronavirusApiService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoronavirusApiService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoronavirusApiService.dll"]
