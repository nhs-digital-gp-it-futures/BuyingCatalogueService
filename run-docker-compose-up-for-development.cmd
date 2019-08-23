copy "..\DataModel\SQL\Tables.sql" "scripts\data\Tables.sql" /Y 
copy "..\DataModel\SQL\Capabilities and Standards.sql" "scripts\data\Capabilities and Standards.sql" /Y
copy "..\DataModel\SQL\Common Values.sql" "scripts\data\Common Values.sql" /Y 
copy "..\DataModel\SQL\Sample Data.sql" "scripts\data\Sample Data.sql" /Y 

call run-docker-compose-up "development"

pause
