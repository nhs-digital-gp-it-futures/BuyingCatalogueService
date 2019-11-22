#!/bin/bash

# wait for MSSQL server to start
export STATUS=1
i=0

while [[ $STATUS -ne 0 ]] && [[ $i -lt 30 ]]; do
	i=$i+1
	/opt/mssql-tools/bin/sqlcmd -t 1 -U sa -P $SA_PASSWORD -Q "select 1" >> /dev/null
	STATUS=$?
done

if [ $STATUS -ne 0 ]; then 
	echo "Error: MSSQL SERVER took more than thirty seconds to start up."
	exit 1
fi

echo "======= MSSQL SERVER STARTED ========" | tee -a ./config.log
# Run the setup script to create the DB using the passed in env. variables and the schema as per the needs of integration tests
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d master -i "sql/Create Database.sql"
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d $DB_NAME -i "sql/Create.sql"
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d $DB_NAME -i "sql/ReferenceData.sql"

echo "======= MSSQL CONFIG COMPLETE =======" | tee -a ./config.log