#!/bin/bash

set -e

integration_sql_resource="../SqlResources"

# copy over sql file from the DataModel/SQL directory
# copy over sql files from the SqlResources directory
# build the integration_db image
# clean up the copied sql files
if [ ! -d "./SQL" ]
then
	mkdir SQL
fi

cp "$integration_sql_resource"/"Create Database.sql" ./SQL/ && 
cp -r "$integration_sql_resource"/. ./SQL/ && 
docker build -f Dockerfile.build.integration . -t integration_db:test && 
rm -rf ./SQL
