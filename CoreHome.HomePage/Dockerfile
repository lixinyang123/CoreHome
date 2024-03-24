FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS base
WORKDIR /app
EXPOSE 8080

RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime

RUN apt update
RUN apt install libgdiplus -y

FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /src
COPY ["CoreHome.HomePage/CoreHome.HomePage.csproj", "CoreHome.HomePage/"]
COPY ["CoreHome.Infrastructure/CoreHome.Infrastructure.csproj", "CoreHome.Infrastructure/"]
COPY ["CoreHome.Data/CoreHome.Data.csproj", "CoreHome.Data/"]
RUN dotnet restore "CoreHome.HomePage/CoreHome.HomePage.csproj"
COPY . .

WORKDIR "/src/CoreHome.HomePage"
RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
RUN ~/.dotnet/tools/libman restore
RUN dotnet build "CoreHome.HomePage.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoreHome.HomePage.csproj" -c Release -o /app/publish -p:PublishReadyToRun=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoreHome.HomePage.dll"]
