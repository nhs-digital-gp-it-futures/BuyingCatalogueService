param (
    [string]$env = "development"
)
[string]$out_directory="docker/out"

function determine_environment() {
    if ("development" -match "$env"){
        return "development"
    } 
     if ("integration" -match "$env"){
        return "integration"
    } 
    return "$env"
}

function clean_out_directory() {
    if (Test-Path -Path "$out_directory"){
        rm -Force -Recurse "$out_directory"
    }
}

function launch_environment(){

    dotnet build .\NHSD.BuyingCatalogue.sln --configuration Release
    clean_out_directory
    dotnet publish "src\NHSD.BuyingCatalogue.API\NHSD.BuyingCatalogue.API.csproj" --configuration Release --output "$out_directory"
    cd docker
    docker-compose build --no-cache
    $dockercomposefile=(-join("docker-compose.","$env",".yml"))
    docker-compose -f "docker-compose.yml" -f "$dockercomposefile" up -d
    docker ps -a
    cd ..
  
}



$env=determine_environment
launch_environment

