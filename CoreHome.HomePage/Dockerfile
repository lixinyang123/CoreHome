#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime

COPY ["./sources.list","/etc/apt/sources.list"]
RUN apt update
RUN apt install libgdiplus -y

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
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
RUN dotnet publish "CoreHome.HomePage.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoreHome.HomePage.dll","urls=http://*"]
