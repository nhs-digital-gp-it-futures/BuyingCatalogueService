IF NOT EXISTS (SELECT * FROM information_schema.schemata WHERE schema_name = 'test')
    EXEC sp_executesql N'CREATE SCHEMA test;'
GO

:r ./ClearData.sql
:r ./DropRole.sql
:r ./DropUserAndLogin.sql
:r ./RestoreUserAndLogin.sql
