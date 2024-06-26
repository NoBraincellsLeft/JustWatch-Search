# Stage 1: Build the .NET Application
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim-amd64 AS build-env
WORKDIR /app
COPY . ./
RUN dotnet publish JustWatchSearch/JustWatchSearch.csproj -c Release -o /app/release --nologo

# Stage 2: Setup Nginx and Deploy Application
FROM nginx:alpine
WORKDIR /var/www/web
COPY --from=build-env /app/release/wwwroot .
RUN apk add --no-cache jq
ENV DOWNLOAD_API_URL="http://localhost:2000/"

# Inline script to generate the JSON file
COPY startup.sh /startup.sh
RUN chmod 777 /startup.sh
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD /startup.sh