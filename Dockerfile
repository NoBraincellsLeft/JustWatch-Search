FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
COPY . ./
RUN dotnet publish JustWatchSearch/JustWatchSearch.csproj -c Release -o release --nologo
FROM nginx:alpine
WORKDIR /var/www/web
COPY --from=build-env /app/release/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80