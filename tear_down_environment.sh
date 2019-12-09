#!/bin/bash

env="${1:-development}"


determine_environment () {
	if [[ "integration" == $env* ]]; then
	  echo "integration"
	  return
	fi
	if [[ "development" == $env* ]]; then
	  echo "development"
	  return
	fi
	echo $env
}

 remove_integration () {

	docker rm integration_api -f
	docker rm integration_db -f
	docker image rm nhsd/buying-catalogue-api:test 
    docker image rm nhsd/buying-catalogue/api:latest
    docker ps -a
}

remove_development () {
    docker rm nhsd_bcapi -f
    docker rm nhsd_bcdb -f
    docker image rm nhsd/buying-catalogue-db:latest
    docker image rm nhsd/buying-catalogue/api:latest 
    docker ps -a
    }
	
env=$(determine_environment)
if [ $env = "development" ]; then
	remove_development
else
	remove_integration
fi
