#!/bin/sh

# Create the JSON content, substituting the environment variable into the JSON structure
json_content=$(jq -n --arg url "$DOWNLOAD_API_URL" '{ConnectionStrings: {DOWNLOAD_API_URL: $url}}')

# Write the JSON content to a file
echo "$json_content" > /var/www/web/appsettings.json
nginx -g 'daemon off;'