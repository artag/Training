FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BandwidthTester/BandwidthTester.csproj", "BandwidthTester/"]
RUN dotnet restore "BandwidthTester/BandwidthTester.csproj"
COPY . .
WORKDIR "/src/BandwidthTester"
RUN dotnet build "BandwidthTester.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BandwidthTester.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BandwidthTester.dll"]
