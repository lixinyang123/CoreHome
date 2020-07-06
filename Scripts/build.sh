wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb

sudo apt-get update; \
	sudo apt-get install -y apt-transport-https && \
	sudo apt-get update && \
	sudo apt-get install -y dotnet-sdk-3.1

rm packages-microsoft-prod.deb

dotnet tool install -g Microsoft.Web.LibraryManager.Cli

echo ====================================
echo | Start building CoreHome.HomePage |
echo ====================================
cd ../CoreHome.HomePage
libman restore
dotnet build

echo =================================
echo | Start building CoreHome.Admin |
echo =================================
cd ../CoreHome.Admin
libman restore
dotnet build
