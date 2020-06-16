﻿:r ./InsertCapabilityStatuses.sql
:r ./InsertCapabilityCategories.sql
:r ./InsertCompliancyLevels.sql
:r ./InsertPublicationStatuses.sql
:r ./InsertSolutionCapabilityStatuses.sql
:r ./InsertSolutionEpicStatuses.sql
:r ./InsertCatalogueItemTypes.sql
:r ./InsertFrameworks.sql
:r ./InsertCapabilities.sql
:r ./InsertEpics.sql
:r ./InsertSuppliers.sql

:r ./MigrateSolutionDetailToCatalogueItem.sql

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    EXEC migration.PostDeployment;
GO

DROP PROCEDURE IF EXISTS migration.PreDeployment;
DROP PROCEDURE IF EXISTS migration.PostDeployment;
DROP SCHEMA IF EXISTS migration;

:r ./InsertSolutions.sql
:r ./DropImport.sql
:r ./DropPublish.sql
