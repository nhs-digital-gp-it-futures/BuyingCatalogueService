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
        Remove-Item -Force -Recurse "$out_directory"
    }
}

function build_api_locally() {
    dotnet build src\NHSD.BuyingCatalogue.API\NHSD.BuyingCatalogue.API.csproj --configuration Release
    clean_out_directory
    dotnet publish "src\NHSD.BuyingCatalogue.API\NHSD.BuyingCatalogue.API.csproj" --configuration Release --output "$out_directory"
}

function spin_containers_up {
    $DockerComposeUp = "docker-compose -f `"docker/docker-compose.$($env).yml`" up --build"
    $Args="-d"
    if ($a) {
        $Args=""
    }
    
    Set-Location docker
    Set-Location ..
    Invoke-Expression "$DockerComposeUp $Args"

    if (!$q) {
        docker ps -a
    }
}

function launch_environment(){
    build_api_locally
    spin_containers_up
}

$env:MSBUILDSINGLELOADCONTEXT = '1'
$env=determine_environment
launch_environment
