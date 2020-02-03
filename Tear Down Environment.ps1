param (
    [string]$env = "development",
    [switch]$c,
    [switch]$q
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

function remove_integration(){

    docker rm integration_api -f
    docker rm integration_db -f
    docker rm documents_api_wiremock -f
    
    docker image rm nhsd/buying-catalogue/api:latest
    docker image rm nhsd/buying-catalogue/documents-api-wiremock:latest
    
}

function remove_development() {
    $DockerComposeDown = 'docker-compose -f "docker\docker-compose.yml" -f "docker\docker-compose.development.yml" down'
    $Args=''
    if ($c) {
        $Args='-v --rmi "all"'
    }

    Invoke-Expression "$DockerComposeDown $Args"
    }

$env=determine_environment

if ($env -eq "development") {
    remove_development
} else {
    remove_integration
}

if (!$q) {
    docker ps -a
}