FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CoreHome.ReverseProxy/CoreHome.ReverseProxy.csproj", "CoreHome.ReverseProxy/"]
RUN dotnet restore "CoreHome.ReverseProxy/CoreHome.ReverseProxy.csproj"
COPY . .
WORKDIR "/src/CoreHome.ReverseProxy"
RUN dotnet build "CoreHome.ReverseProxy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoreHome.ReverseProxy.csproj" -c Release -o /app/publish -r linux-x64 -p:PublishReadyToRun=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoreHome.ReverseProxy.dll","urls=http://*"]
