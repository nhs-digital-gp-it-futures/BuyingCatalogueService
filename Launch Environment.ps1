param (
    [string]$env = "development",
    [switch]$a,
    [switch]$q
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

function build_api_locally() {
    dotnet build .\NHSD.BuyingCatalogue.sln --configuration Release
    clean_out_directory
    dotnet publish "src\NHSD.BuyingCatalogue.API\NHSD.BuyingCatalogue.API.csproj" --configuration Release --output "$out_directory"
}

function spin_containers_up {
    $DockerComposeUp = "docker-compose -f `"docker-compose.yml`" -f `"docker-compose.$($env).yml`" up"
    $Args="-d"
    if ($a) {
        $Args=""
    }
    cd docker
    docker-compose build --no-cache
    Invoke-Expression "$DockerComposeUp $Args"
    if (!$q) {
        docker ps -a
    }
    cd ..
}

function launch_environment(){
    build_api_locally
    spin_containers_up
}



$env=determine_environment
launch_environment

