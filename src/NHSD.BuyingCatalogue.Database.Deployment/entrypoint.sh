#!/bin/bash

# wait for MSSQL server to start
export STATUS=1
i=0

while [[ $STATUS -ne 0 ]] && [[ $i -lt 30 ]]; do
    i=$i+1
    sleep 1
    /opt/mssql-tools/bin/sqlcmd -S $DB_SERVER,1433 -t 1 -U sa -P $SA_PASSWORD -Q "SELECT 1;" &>/dev/null
    STATUS=$?
done

if [ $STATUS -ne 0 ]; then 
    echo "Error: MSSQL SERVER took more than thirty seconds to start up."
    exit 1
fi

/sqlpackage/sqlpackage /Action:publish /SourceFile:NHSD.BuyingCatalogue.Database.Deployment.dacpac /TargetServerName:$DB_SERVER /TargetDatabaseName:$DB_NAME /TargetUser:sa /TargetPassword:$SA_PASSWORD
/opt/mssql-tools/bin/sqlcmd -S $DB_SERVER,1433 -U sa -P $SA_PASSWORD -d $DB_NAME -I -i "PostDeployment.sql"
