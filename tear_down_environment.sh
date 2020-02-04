#!/bin/bash

clearAll="false"
env="development"
quiet="false"

while test $# -gt 0; do
	case "$1" in
		-c|--clearAll)
			clearAll="true"
		  shift
		  ;;
		-d|--dev|--development)
			env="development"
		  shift
		  ;;
		-i|--int|--integration)
		  env="integration"
		  shift
		  ;;
		-q|--quiet)
		  quiet="true"
		  shift
		  ;;
		*)
		  env=$1
		  shift
		  ;;
	esac
done

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
    docker rm documents_api_wiremock -f

	docker image rm nhsd/buying-catalogue-api:test
    docker image rm nhsd/buying-catalogue/api:latest
    docker image rm nhsd/buying-catalogue/documents-api-wiremock:latest
}

remove_development () {
	docker_compose_down='docker-compose -f "docker/docker-compose.yml" -f "docker/docker-compose.development.yml" down'
	if [ "$clearAll" == "true" ]; then
		docker_args='-v --rmi "all"'
	fi
	eval $docker_compose_down $docker_args
    }

env=$(determine_environment)
if [ $env = "development" ]; then
	remove_development
else
	remove_integration
fi

if [ "$quiet" == "false" ]; then
	docker ps -a
fi
