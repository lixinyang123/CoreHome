#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CoreHome.Admin/CoreHome.Admin.csproj", "CoreHome.Admin/"]
COPY ["CoreHome.Infrastructure/CoreHome.Infrastructure.csproj", "CoreHome.Infrastructure/"]
COPY ["CoreHome.Data/CoreHome.Data.csproj", "CoreHome.Data/"]
RUN dotnet restore "CoreHome.Admin/CoreHome.Admin.csproj"
COPY . .

WORKDIR "/src/CoreHome.Admin"
RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
RUN ~/.dotnet/tools/libman restore
RUN dotnet build "CoreHome.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoreHome.Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoreHome.Admin.dll","urls=http://*"]
