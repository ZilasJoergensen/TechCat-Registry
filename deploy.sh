#!/bin/bash
set -e

cd ~/TechCat-Registry
git pull

docker build -f TechCatRegistry.Api/Dockerfile -t techcat-registry:1.0 .

docker stop techcat || true
docker rm techcat || true

docker run -d --name techcat \
  --network techcat-net \
  --restart unless-stopped \
  --env-file ~/TechCat-Registry/.env \
  -e ASPNETCORE_ENVIRONMENT=Development \
  techcat-registry:1.0

docker ps --filter name=techcat --format '{{.Status}}'
