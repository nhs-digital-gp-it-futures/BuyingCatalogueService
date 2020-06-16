/*-----------------------------------------------------------------------
    Create migration schema
------------------------------------------------------------------------*/

IF NOT EXISTS (SELECT * FROM information_schema.schemata WHERE schema_name = 'migration')
    EXEC sp_executesql N'CREATE SCHEMA migration;'
GO

:r ./MigrateSolutionDetailToCatalogueItem.sql

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    EXEC migration.PreDeployment;
GO
