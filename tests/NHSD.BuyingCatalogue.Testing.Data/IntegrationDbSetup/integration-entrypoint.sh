#!/bin/bash

echo "===== STARTED entrypoint script ======"

# Start SQL Server
/opt/mssql/bin/sqlservr &

# Start the script to create the DB and user
/usr/config/configure-integration-db.sh

# Call extra command
eval $1

echo "===== FINISHED entrypoint script ======"