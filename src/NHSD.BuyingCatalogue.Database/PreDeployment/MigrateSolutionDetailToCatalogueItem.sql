IF OBJECT_ID('migration.CatalogueItem', 'U') IS NOT NULL AND '$(MIGRATE_TO_CATALOGUEITEM)' = 'TRUE'
    CREATE TABLE migration.CatalogueItem
    (
        CatalogueItemId varchar(14) NOT NULL PRIMARY KEY,
        [Name] varchar(255) NULL,
        Created datetime2(7) NULL,
        CatalogueItemTypeId int NULL,
        SupplierId varchar(6) NULL,
        PublishedStatusId int NULL
    );
GO

IF '$(MIGRATE_TO_CATALOGUEITEM)' = 'TRUE' AND EXISTS(SELECT * FROM migration.CatalogueItem)
    TRUNCATE TABLE migration.CatalogueItem;
GO

/*-----------------------------------------------------------------------
    Copy Solution ID and Names to CatatlogueItem Table	
------------------------------------------------------------------------*/
DECLARE @solutionCatalougeItemType int = 1;

IF '$(MIGRATE_TO_CATALOGUEITEM)' = 'TRUE'
    INSERT INTO migration.CatalogueItem(CatalogueItemId, [Name], Created, CatalogueItemTypeId, SupplierId, PublishedStatusId)
         SELECT CatalogueItemId, [Name], LastUpdated, @solutionCatalougeItemType, SupplierId, PublishedStatusId
           FROM dbo.Solution;
GO

/*-----------------------------------------------------------------------
    Copy Solution Detail information to Solution Table	
------------------------------------------------------------------------*/

IF OBJECT_ID('migration.SolutionDetail', 'U') IS NOT NULL AND '$(MIGRATE_TO_CATALOGUEITEM)' = 'TRUE'
    CREATE TABLE migration.SolutionDetail
    (
         SolutionId varchar(14) NOT NULL PRIMARY KEY,
         Features nvarchar(max) NULL,
         ClientApplication nvarchar(max) NULL,
         Hosting nvarchar(max) NULL,
         ImplementationDetail varchar(1000) NULL,
         RoadMap varchar(1000) NULL,
         IntegrationsUrl varchar(1000) NULL,
         AboutUrl varchar(1000) NULL,
         Summary varchar(300) NULL,
         FullDescription varchar(3000) NULL
    );
GO

IF '$(MIGRATE_TO_CATALOGUEITEM)' = 'TRUE' AND EXISTS(SELECT * FROM migration.SolutionDetail)
    TRUNCATE TABLE migration.SolutionDetail;
GO

IF '$(MIGRATE_TO_CATALOGUEITEM)' = 'TRUE'
    INSERT INTO migration.SolutionDetail(SolutionId, Features, ClientApplication, Hosting, ImplementationDetail, RoadMap, IntegrationsUrl, AboutUrl, Summary, FullDescription)
         SELECT SolutionId, Features, ClientApplication, Hosting, ImplementationDetail, RoadMap, IntegrationsUrl, AboutUrl, Summary, FullDescription
         FROM dbo.SolutionDetail;
GO
