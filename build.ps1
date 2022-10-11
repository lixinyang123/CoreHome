dotnet tool install -g Microsoft.Web.LibraryManager.Cli
dotnet tool install -g dotnet-ef

cd ./CoreHome.HomePage
dotnet restore
~/.dotnet/tools/libman restore
dotnet build
dotnet publish -o ../bin/CoreHome/HomePage

cd ../CoreHome.Admin
dotnet restore
~/.dotnet/tools/libman restore
dotnet build
dotnet publish -o ../bin/CoreHome/Admin

cd ../CoreHome.ReverseProxy
dotnet restore
dotnet build
dotnet publish -o ../bin/CoreHome/ReverseProxy
