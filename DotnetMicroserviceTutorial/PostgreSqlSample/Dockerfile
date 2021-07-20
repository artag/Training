FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["PostgreSqlSample/PostgreSqlSample.csproj", "PostgreSqlSample/"]
RUN dotnet restore "PostgreSqlSample/PostgreSqlSample.csproj"
COPY . .
WORKDIR "/src/PostgreSqlSample"
RUN dotnet build "PostgreSqlSample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PostgreSqlSample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PostgreSqlSample.dll"]
