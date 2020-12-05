sudo apt update

sudo apt install libgdiplus mysql-server -y

sudo service mysql start

wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

sudo apt-get update; \
	sudo apt-get install -y apt-transport-https && \
	sudo apt-get update && \
	sudo apt-get install -y dotnet-sdk-5.0

dotnet tool install -g Microsoft.Web.LibraryManager.Cli

cd ../CoreHome.HomePage
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
