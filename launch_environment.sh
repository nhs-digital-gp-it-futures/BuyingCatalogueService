#!/bin/bash

set -e
env="${1:-development}"
out_directory="docker/out"

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

launch_environment () {

    dotnet build ./NHSD.BuyingCatalogue.sln --configuration Release
    clean_out_directory 
    dotnet publish "src/NHSD.BuyingCatalogue.API/NHSD.BuyingCatalogue.API.csproj" --configuration Release --output "$out_directory"
    cd docker
    docker-compose build --no-cache 
    docker-compose -f "docker-compose.yml" -f "docker-compose.$environment.yml" up -d
    docker ps -a
    cd ..
  
}
environment=$(determine_environment)
launch_environment
#echo $environment
