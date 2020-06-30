/*-----------------------------------------------------------------------
    Create migration schema
------------------------------------------------------------------------*/

IF NOT EXISTS (SELECT * FROM information_schema.schemata WHERE schema_name = 'migration')
    EXEC sp_executesql N'CREATE SCHEMA migration;'
GO

:r ./MigrateSolutionDetailToCatalogueItem.sql

IF OBJECT_ID(N'dbo.CatalogueItem', N'U') IS NULL AND OBJECT_ID(N'dbo.Solution', N'U') IS NOT NULL
    EXEC migration.PreDeployment;
GO
