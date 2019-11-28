dotnet build .\NHSD.BuyingCatalogue.sln --configuration Release
dotnet publish "src\NHSD.BuyingCatalogue.API\NHSD.BuyingCatalogue.API.csproj" --configuration Release --output "out"
docker-compose build --no-cache
docker-compose -f "docker-compose.yml" -f "docker-compose.development.yml" up -d
docker ps -a
