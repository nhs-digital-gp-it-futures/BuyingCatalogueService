:r ./InsertCapabilityStatuses.sql
:r ./InsertCapabilityCategories.sql
:r ./InsertCompliancyLevels.sql
:r ./InsertPublicationStatuses.sql
:r ./InsertSolutionCapabilityStatuses.sql
:r ./InsertSolutionEpicStatuses.sql
:r ./InsertCatalogueItemTypes.sql
:r ./InsertFrameworks.sql
:r ./InsertCapabilities.sql
:r ./InsertCataloguePriceTypes.sql
:r ./InsertEpics.sql
:r ./InsertPricingUnits.sql
:r ./InsertProvisioningTypes.sql
:r ./InsertSuppliers.sql

:r ./MigrateSolutionDetailToCatalogueItem.sql

IF NOT EXISTS (SELECT * FROM dbo.CatalogueItem)
    EXEC migration.PostDeployment;
GO

DROP PROCEDURE IF EXISTS migration.PreDeployment;
DROP PROCEDURE IF EXISTS migration.PostDeployment;
DROP SCHEMA IF EXISTS migration;
GO

:r ./InsertTimeUnits.sql
:r ./InsertSolutions.sql
:r ./InsertAdditionalServices.sql
:r ./DropImport.sql
:r ./DropPublish.sql
