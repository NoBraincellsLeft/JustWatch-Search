# Define the base image based on architecture
ARG TARGETARCH
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env-${TARGETARCH}
WORKDIR /app
COPY . ./
RUN dotnet publish JustWatchSearch/JustWatchSearch.csproj -c Release -o /app/release --nologo

# Nginx stage
FROM nginx:alpine
WORKDIR /var/www/web
COPY --from=build-env-${TARGETARCH} /app/release/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
