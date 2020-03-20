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

function remove_environment() {
    $DockerComposeDown = "docker-compose -f `"docker\docker-compose.yml`" -f `"docker\docker-compose.$($env).yml`" down"
    $Args=''
    if ($c) {
        $Args='-v --rmi "all"'
    }

    Invoke-Expression "$DockerComposeDown $Args"
    }

$env=determine_environment

remove_environment

if (!$q) {
    docker ps -a
}
