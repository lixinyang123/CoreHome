FROM alpine AS base
WORKDIR /app
EXPOSE 8080
RUN apk add libc6-compat

FROM mcr.microsoft.com/dotnet/nightly/sdk:8.0-jammy-aot AS build
WORKDIR /src
COPY ["CoreHome.ReverseProxy/CoreHome.ReverseProxy.csproj", "CoreHome.ReverseProxy/"]
RUN dotnet restore "CoreHome.ReverseProxy/CoreHome.ReverseProxy.csproj"
COPY . .
WORKDIR "/src/CoreHome.ReverseProxy"
RUN dotnet build "CoreHome.ReverseProxy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoreHome.ReverseProxy.csproj" -c Release -o /app/publish
RUN rm /app/publish/CoreHome.ReverseProxy.dbg

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./CoreHome.ReverseProxy", "urls=http://*:8080"]
