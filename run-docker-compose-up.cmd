docker-compose -f ".\docker-compose.yml" -f ".\docker-compose.%1.yml" build
docker-compose -f ".\docker-compose.yml" -f ".\docker-compose.%1.yml" up -d