# CoreHome

![build](https://github.com/lixinyang123/CoreHome/workflows/build/badge.svg?branch=main)

当前的个人网站，由 [ .NET 6.0 ](https://dotnet.microsoft.com/) 驱动

CoreHome 是一个基于 .NET 和 阿里云OSS 的博客系统，包含了个人信息管理，主页项目管理，主题管理（主页背景，亮暗主题，BGM），博客管理、分类、标签、归档，评论及反馈提醒，服务器网络状态检测等功能。

[![ReadMe Card](https://github-readme-stats.vercel.app/api/pin/?username=lixinyang123&repo=CoreHome)](https://github.com/lixinyang123/CoreHome) [![ReadMe Card](https://github-readme-stats.vercel.app/api/pin/?username=lixinyang123&repo=DockerHome)](https://github.com/lixinyang123/DockerHome)

### 从源码构建CoreHome

#### 准备

首先构建源码需要以下环境
- Visual Studio 2019 / Visual Studio Code
- .NET 6.0 SDK
- Entity Framework Core
- Libman
- Mysql
- Docker（推荐使用WSL2）

> 注意：使用 Visual Studio（非 Visual Studio Code）不需要 Libman CLI 和 Entity Framework CLI

安装 LibMan CLI：
```shell
dotnet tool install -g Microsoft.Web.LibraryManager.Cli
```

安装 Entity Framework Core CLI
```shell
dotnet tool install -g dotnet-ef
```

接下来clone源码
```shell
git clone https://github.com/lixinyang123/CoreHome.git
```

修改配置文件(如下5个配置文件)

不会修改的话，可以看上一篇博客[《介绍 DockerHome》](https://www.lllxy.net/Blog/Detail/ea8c626c-fac4-4a19-85e8-a46d41d938d5 "《介绍 DockerHome》")，里面详细介绍了怎样修改配置文件。

- CoreHome
	- CoreHome.HomePage
		- appsettings.json（项目配置）
		- wwwroot/SiteMap.txt（站点地图，SEO用）
		- wwwroot/favicon.ico（网站logo）
	- CoreHome.Admin
		- appsettings.json（项目配置）
		- wwwroot/favicon.ico（网站logo）


>注意：CoreHome.HomePage 和 CoreHome.Admin 中的 appsettings.json 内容完全一致，复制粘贴即可。

#### 修改数据连接配置（appsettings.json）:
CoreHome.HomePage和CoreHome.Admin都需要修改
```
  "CoreHome": "server=[数据库url];user id=[数据库用户名];password=[数据库密码];database=corehome"
```
	
#### 还原依赖

- 使用 Visual Studio
	- 后端：鼠标右键点击，解决方案资源管理器中的项目文件，弹出菜单中点击还原Nuget包。
		- CoreHome.Infrastructure
		- CoreHome.Data
		- CoreHome.HomePage
		- CoreHome.Admin
	- 前端：鼠标右键点击下面两个文件，弹出菜单中点击还原客户端库。
		- CoreHome.HomePage/libman.json
		- CoreHome.Admin/libman.json
- 使用 Visual Studio Code 或 CLI
	- 后端：切换到下方目录执行命令 **dotnet restore**
		- CoreHome.HomePage
		- CoreHome.Admin
	- 前端：切换到下方目录执行命令 **libman restore**
		- CoreHome.HomePage
		- CoreHome.Admin

#### 创建数据库

- 使用 Visual Studio
点击 工具-Nuget程序包管理器-程序包管理器控制台，执行

```shell
Update-Database
```

- 使用 Visual Studio Code 或 CLI
在 CoreHome.HomePage 和 CoreHome.Admin 目录下执行

```shell
dotnet-ef database update -p ..\CoreHome.Data
```

#### 启动项目

- Visual Studio 点击顶部运行即可
- Visual Studio Code 选择项目生成 launcher.json 并点击运行
- CLI 分别在 CoreHome.HomePage 和 CoreHome.Admin 执行 **dotnet run**

#### 构建Docker镜像

- Visual Studio：右键 CoreHome.HomePage/Dockerfile 和 CoreHome.Admin/Dockerfile 点击 **生成Docker映像**

- Visual Studio Code 或 CLI：在项目根目录下执行

```shell
docker build --file ./CoreHome.Admin/Dockerfile --tag corehomeadmin:latest .
docker build --file ./CoreHome.HomePage/Dockerfile --tag corehomehomepage:latest .
```

#### 在Docker中运行

- Visual Studio：启动项目更改为Docker点击启动即可。

- Visual Studio Code 或 CLI：在项目中 DockerCompose 目录中执行

```shell
docker-compose up
```

### 怎样部署CoreHome

在Linux上部署CoreHome可以使用[DockerHome](https://github.com/lixinyang123/DockerHome "DockerHome")进行快速部署

[![ReadMe Card](https://github-readme-stats.vercel.app/api/pin/?username=lixinyang123&repo=DockerHome)](https://github.com/lixinyang123/DockerHome)

详情可以见上一篇博客[《介绍 DockerHome》](https://www.lllxy.net/Blog/Detail/ea8c626c-fac4-4a19-85e8-a46d41d938d5 "《介绍 DockerHome》")，Windows上构建完成发布到IIS即可。

### 最后

如果你喜欢这个博客，可以去Github上给个Star
