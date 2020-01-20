#!/bin/bash

port=${1:-1430}
username=${2:-NHSD}
password=${3:-Str0nk3RP@55W0rD}

validate() {
	message="The password does not meet SQL Server password policy requirements because it is not complex enough.\nThe password must be at least 8 characters long and contain characters from three of the following four sets: Uppercase letters, Lowercase letters, Base 10 digits, and Symbols"
	declare -i count=0
	
	if [[ $port -gt 65535 ]]; then
		echo -e "You can't have your database on port $port chief, that's outside the range.\nPlease choose a port between 0 and 65535 and try again."
		exit 1
	fi
	
	if [[ ${#password} -lt 8 ]]; then
		echo -e $message
		exit 1
	fi
	
	(echo "$password" | grep -Eq  [[:digit:]]+) && count=$count+1 
	(echo "$password" | grep -Eq  [[:lower:]]+) && count=$count+1 
	(echo "$password" | grep -Eq  [[:upper:]]+) && count=$count+1 
	(echo "$password" | grep -Eq  '[]!@#$^&*()-+={}\|/:;<>?,.[]') && count=$count+1 
	
	if [[ $count -lt 3 ]]; then
		echo -e $message
		exit 1
	fi
}

set_env_variables() {
	export NHSD_LOCAL_DB_PORT="$port"
	export NHSD_LOCAL_DB_USERNAME="$username"
	export NHSD_LOCAL_DB_PASSWORD="$password"
}

docker-compose -f "docker/docker-compose.debug.yml" down -v --rmi "all"

validate

set_env_variables

docker-compose -f "docker/docker-compose.debug.yml" up -d

connection_string="Data Source=127.0.0.1,$port;Initial Catalog=buyingcataloguegpit;MultipleActiveResultSets=True;User Id=$username;Password=$password"

echo -e "\nYour Connection string for BuyingCatalogue is:\n"

echo $connection_string

echo -e "\nPlease make sure to update your 'secrets.json' file before running the API"