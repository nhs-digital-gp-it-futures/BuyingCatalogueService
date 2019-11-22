#!/bin/bash

set -e

data_model_sql_resources="../../../../DataModel/SQL"
integration_sql_resource="../SqlResources"

if [ ! -d "$data_model_sql_resources" ]
then
    echo "[ ! ] Couldn't find the folder containing sql files for the integration tests"
	echo "[ ! ] Expected to find the DataModel/SQL directory to be at $data_model_sql_resources"
    echo "[ x ] Please make sure this directory exists and try again."
    echo "[ ! ] Stopping the execution of setup-script."
    exit 1
fi

# copy over sql file from the DataModel/SQL directory
# copy over sql files from the SqlResources directory
# build the integration_db image
# clean up the copied sql files
if [ ! -d "./SQL" ]
then
	mkdir SQL
fi

cp "$data_model_sql_resources"/"Create Database.sql" ./SQL/ && 
cp -r "$integration_sql_resource"/. ./SQL/ && 
docker build -f Dockerfile.build.integration . -t integration_db:test && 
rm -rf ./SQL
