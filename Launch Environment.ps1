param (
    [string]$env = "development"
)

function determine_environment() {
    if ("development" -match "$env"){
        return "development"
    } 
     if ("integration" -match "$env"){
        return "integration"
    } 
    return "$env"
}

function launch_environment(){

    dotnet build .\NHSD.BuyingCatalogue.sln --configuration Release
    dotnet publish "src\NHSD.BuyingCatalogue.API\NHSD.BuyingCatalogue.API.csproj" --configuration Release --output "out"
    docker-compose build --no-cache
    $dockercomposefile=(-join("docker-compose.","$env",".yml"))
    docker-compose -f "docker-compose.yml" -f "$dockercomposefile" up -d
    docker ps -a
  
}


$env=determine_environment
launch_environment

