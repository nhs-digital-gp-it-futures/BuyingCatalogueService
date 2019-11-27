
dotnet publish "src\NHSD.BuyingCatalogue.API\NHSD.BuyingCatalogue.API.csproj" --configuration Release --output "out"
docker-compose -f "docker-compose.yml" -f "docker-compose.integration.yml" up -d
docker ps -a
