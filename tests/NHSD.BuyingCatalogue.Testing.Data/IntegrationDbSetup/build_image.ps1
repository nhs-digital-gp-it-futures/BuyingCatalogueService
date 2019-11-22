$data_model_sql_resources="..\..\..\..\DataModel\SQL"
$integration_sql_resource="..\SqlResources"

if( -NOT (Test-Path $data_model_sql_resources)) 
{ 
    Write-Host "[ ! ] Couldn't find the folder containing sql files for the integration tests"
	Write-Host "[ ! ] Expected to find the DataModel/SQL directory to be at $data_model_sql_resources"
    Write-Host "[ x ] Please make sure this directory exists and try again."
    Write-Host "[ ! ] Stopping the execution of setup-script."
    exit 1
}

# copy over sql file from the DataModel/SQL directory
# copy over sql files from the SqlResources directory
# build the integration_db image
# clean up the copied sql files

if( -NOT (Test-Path ".\SQL")) 
{ 
    mkdir SQL > $null
}

Copy-Item "$data_model_sql_resources\Create Database.sql" -Destination ".\SQL"
Copy-Item -Recurse "$integration_sql_resource\*" -Destination ".\SQL"
docker build -f ".\Dockerfile.build.integration" . -t "integration_db:test"
rm -Force -Recurse ".\SQL"
