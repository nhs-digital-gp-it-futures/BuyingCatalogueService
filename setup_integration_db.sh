#!/bin/bash
set -e

image="nhsd/buying-catalogue-integration-db:test"
cwd=$(pwd)

echo "[ x ] Checking whether the $image image exists..."
# assert whether the 'nhsd/buying-catalogue-integration-db' image exists or not
docker image inspect "$image" >/dev/null 2>&1 && image_exists=1 || image_exists=0

if [ $image_exists -eq 1 ]
then
   echo "[ ! ] The $image image already exists, discarding it and creating a new one ..."
   id=$(docker image inspect "$image" --format='{{.Id}}') #get the Id field of inspect log
   IFS=':' # colon is set as the delimiter
   read -ra ADDR <<< "$id" # id is read into an array as tokens separated by IFS
   id_hash=${ADDR[1]} #get the actual id hash (id is in format of 'shaxxx:hash')
   docker image rm "$id_hash"
   echo "[ x ] Getting rid of any dangling images..."
   docker rmi -f $(docker images -f "dangling=true" -q) || true
fi

# copy over sql files from the BuyingCatalog integration test directory
# build the integration_db image
# clean up the copied sql files
echo "[ x ] Creating new image ..."
cd tests/NHSD.BuyingCatalogue.Testing.Data/IntegrationDbSetup
bash build_image.sh
echo "[ x ] Created new image."
cd "$cwd"

