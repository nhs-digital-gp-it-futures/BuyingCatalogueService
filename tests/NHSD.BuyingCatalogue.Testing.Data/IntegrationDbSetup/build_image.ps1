$integration_sql_resource="..\SqlResources"

# copy over sql files from the SqlResources directory
# build the integration_db image
# clean up the copied sql files

if( -NOT (Test-Path ".\SQL")) 
{ 
    mkdir SQL > $null
}

Copy-Item "$integration_sql_resource\Create Database.sql" -Destination ".\SQL"
Copy-Item -Recurse "$integration_sql_resource\*" -Destination ".\SQL"
docker build -f ".\Dockerfile.build.integration" . -t "integration_db:test"
rm -Force -Recurse ".\SQL"
