docker rm integration_api -f
docker rm integration_db -f
docker image rm nhsd/buying-catalogue-api:latest -f
docker ps -a