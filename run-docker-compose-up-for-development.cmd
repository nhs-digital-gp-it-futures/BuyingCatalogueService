xcopy "..\DataModel\SQL\*.sql" "scripts\data\" /F /S /Y

call run-docker-compose-up "development"

pause
