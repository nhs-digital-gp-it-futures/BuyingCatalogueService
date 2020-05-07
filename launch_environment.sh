#!/bin/bash

set -e
env="development"
attached="false"
quiet="false"
out_directory="docker/out"

while test $# -gt 0; do
	case "$1" in
		-a|--attached)
			attached="true"
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

clean_out_directory () {
	if [ -d $out_directory ]; then
		rm -rf $out_directory
	fi
}

build_api_locally () {
	dotnet build src/NHSD.BuyingCatalogue.API/NHSD.BuyingCatalogue.API.csproj --configuration Release
    clean_out_directory
    dotnet publish "src/NHSD.BuyingCatalogue.API/NHSD.BuyingCatalogue.API.csproj" --configuration Release --output "$out_directory"
}

spin_containers_up () {
	docker_compose_up="docker-compose -f \"docker-compose.yml\" -f \"docker-compose.$environment.yml\" up"
	if [ "$attached" == "false" ]; then
		docker_args="-d"
	fi

	cd docker
    docker-compose build --no-cache
    eval $docker_compose_up $docker_args

	if [ "$quiet" == "false" ]; then
    	docker ps -a
	fi
    cd ..
}

launch_environment () {
	build_api_locally
	spin_containers_up
}

environment=$(determine_environment)
launch_environment
