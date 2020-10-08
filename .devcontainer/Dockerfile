FROM ubuntu

RUN apt update
RUN apt upgrade -y
RUN DEBIAN_FRONTEND=noninteractive apt install tzdata -y

# install dev tools
RUN apt install libgdiplus mysql-server nginx wget vim -y

# install dotnet
RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb

RUN apt-get update; \
	apt-get install -y apt-transport-https && \
	apt-get update && \
	apt-get install -y dotnet-sdk-3.1

RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
RUN ln -s ~/.dotnet/tools/libman /bin/libman
