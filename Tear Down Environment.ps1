﻿param (
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

function remove_integration(){

    docker rm integration_api -f
    docker rm integration_db -f
    docker image rm nhsd/buying-catalogue-api:test 
    docker image rm nhsd/buying-catalogue/api:latest
    docker ps -a
}

function remove_development() {
    docker rm nhsd_bcapi -f
    docker rm nhsd_bcdb -f
    docker image rm nhsd/buying-catalogue-db:latest
    docker image rm nhsd/buying-catalogue/api:latest 
    docker ps -a
    }

$env=determine_environment
if ($env -eq "development") {
    remove_development
} else {
    remove_integration
}