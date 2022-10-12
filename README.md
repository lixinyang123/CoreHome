# CoreHome

![build](https://github.com/lixinyang123/CoreHome/workflows/build/badge.svg?branch=main)

Current personal [website](https://www.lllxy.net)，Powered by [.NET](https://dotnet.microsoft.com/) .

CoreHome is a blog system based on. NET and Alibaba Cloud OSS. It includes functions such as personal information management, home page project management, theme management (home page background, light and dark themes, BGM), blog management, classification, tagging, archiving, comments and feedback alerts, and server network status detection.

<div style="display:flex; width: 100%;">
    <image style="width: 49%;" src="https://user-images.githubusercontent.com/32838371/168005656-1e49ccb1-737c-464c-bd25-5a7e2f89541b.png" />
    <image style="width: 49%;" src="https://user-images.githubusercontent.com/32838371/195136555-73ce167e-114b-45c5-a6f2-11d65724174f.png" />
    <image style="width: 49%;" src="https://user-images.githubusercontent.com/32838371/195137729-485080ad-5f69-4a8b-859d-ac8bf0245c06.jpeg" />
    <image style="width: 49%;" src="https://user-images.githubusercontent.com/32838371/141163201-b37de67f-91f2-4333-840e-b888ae1d505e.jpeg" />
    <image style="width: 49%;" src="https://user-images.githubusercontent.com/32838371/195309268-a655c813-1432-49ff-b24c-0fdafe52620a.jpeg" />
    <image style="width: 49%;" src="https://user-images.githubusercontent.com/32838371/195309417-02554c9e-4c8d-4514-96fd-94fbda9e6ff4.jpg" />
</div>
	
### Build from source

#### Install pre-requisites

- Visual Studio 2022 / Visual Studio Code
- .NET 7.0 SDK
- Entity Framework Core
- Libman
- Mysql
- Docker（WSL2 is recommended）

> If you use Visual Studio (Not Visual Studio Code), you don't need to install Libman CLI and Entity Framework CLI.

Install Libman CLI：

```shell
dotnet tool install -g Microsoft.Web.LibraryManager.Cli
```

Install Entity Framework Core CLI

```shell
dotnet tool install -g dotnet-ef
```

Clone source code

```shell
git clone https://github.com/lixinyang123/CoreHome.git
```

Configuration 

You can refer to [this](https://www.lllxy.net/Blog/Detail/ea8c626c-fac4-4a19-85e8-a46d41d938d5) blog to configure.

- CoreHome
	- CoreHome.HomePage
		- appsettings.json
		- wwwroot/SiteMap.txt
		- wwwroot/favicon.ico
	- CoreHome.Admin
		- appsettings.json
		- wwwroot/favicon.ico
	- CoreHome.ReverseProxy
		- appsettings.json

> The content of appsettings.json in CoreHome.HomePage and CoreHome.Admin is exactly the same, just copy and paste.

#### Configure database（appsettings.json）:

Both CoreHome.HomePage and CoreHome.Admin

```
  "CoreHome": "server=[host];user id=[user];password=[password];database=corehome"
```
	
#### Dependencies

- Visual Studio
	- Backend：Click Restore Nuget Packages in Solution Explorer.
		- CoreHome.Infrastructure
		- CoreHome.Data
		- CoreHome.HomePage
		- CoreHome.Admin
		- CoreHome.ReverseProxy
	- Frontend：Click Restore Client Libraries in Solution Explorer.
		- CoreHome.HomePage/libman.json
		- CoreHome.Admin/libman.json

- Visual Studio Code or CLI
	- Backend：Execute `dotnet restore` in the following directory.
		- CoreHome.HomePage
		- CoreHome.Admin
	- Frontend：Execute `libman restore` in the following directory.
		- CoreHome.HomePage
		- CoreHome.Admin

#### Migrate Database

- Visual Studio

Tools \> Nuget Package Manager \> Package Manager Console

```shell
Update-Database
```

- Visual Studio Code or CLI

Execute the following commands in the CoreHome.HomePage and CoreHome.Admin directory.

```shell
dotnet-ef database update -p ..\CoreHome.Data
```

#### Startup

- Visual Studio or Visual Studio Code

Click `Startup` in Solution Explorer or `Ctrl+F5`.

- CLI 

Execute the following commands in the CoreHome.HomePage and CoreHome.Admin directory.

```shell
dotnet run
```

#### Build Dockerfile

- Visual Studio

Click `Build Dockerfile` in Solution Explorer.

- Visual Studio Code or CLI

Execute the following command in the project root directory.

```shell
docker build --file ./CoreHome.Admin/Dockerfile --tag lixinyang/corehome-admin:latest .
docker build --file ./CoreHome.HomePage/Dockerfile --tag lixinyang/corehome-homepage:latest .
docker build --file ./CoreHome.ReverseProxy/Dockerfile --tag lixinyang/corehome-reverseproxy:latest .
```

### Deploy

You can use [DockerHome](https://github.com/lixinyang123/DockerHome "DockerHome") to deploy CoreHome.

[![ReadMe Card](https://github-readme-stats.vercel.app/api/pin/?username=lixinyang123&repo=DockerHome)](https://github.com/lixinyang123/DockerHome)

You can use DockerHome to deploy CoreHome, or you can deploy it manually.

> You can refer to [this](https://www.lllxy.net/Blog/Detail/b73acc42-ec42-4151-b108-a680bd1e0c87) blog to use DockerHome.

