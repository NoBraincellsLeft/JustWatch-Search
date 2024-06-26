#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DownloadApi/DownloadApi.csproj", "DownloadApi/"]
RUN dotnet restore "./DownloadApi/./DownloadApi.csproj"
COPY . .
WORKDIR "/src/DownloadApi"
RUN dotnet build "./DownloadApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DownloadApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
USER root
ENV POETRY_BIN_PATH=/root/.local/bin
ENV VT_INSTALL_PATH=/vt
ENV ASPNETCORE_HTTP_PORTS=8080
RUN apt-get -y update; apt-get -y install curl
RUN apt-get -y install wget
RUN apt -y install python3
RUN apt -y install pipx
RUN apt -y install ffmpeg
RUN apt-get -y install aria2
RUN apt -y install mkvtoolnix
RUN apt-get install -y libmediainfo-dev
RUN curl -sSL https://install.python-poetry.org | python3 -
RUN wget https://github.com/shaka-project/shaka-packager/releases/download/v2.6.1/packager-linux-x64 
RUN mv packager-linux-x64 /usr/bin/packager
RUN chmod +x /usr/bin/packager
RUN ln -s /usr/bin/python3 /usr/bin/python
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DownloadApi.dll"]