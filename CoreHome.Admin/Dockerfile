FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 8080

RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
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
RUN dotnet publish "CoreHome.Admin.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoreHome.Admin.dll"]
