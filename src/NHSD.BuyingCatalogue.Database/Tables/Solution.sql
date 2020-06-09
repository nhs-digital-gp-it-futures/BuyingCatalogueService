CREATE TABLE dbo.Solution
(
     Id varchar(14) NOT NULL,
     [Version] varchar(10) NULL,
     Summary varchar(300) NULL,
     FullDescription varchar(3000) NULL,
     Features nvarchar(max) NULL,
     ClientApplication nvarchar(max) NULL,
     Hosting nvarchar(max) NULL,
     ImplementationDetail varchar(1000) NULL,
     RoadMap varchar(1000) NULL,
     IntegrationsUrl varchar(1000) NULL,
     AboutUrl varchar(1000) NULL,
     ServiceLevelAgreement nvarchar(1000) NULL,
     WorkOfPlan nvarchar(max) NULL,
     LastUpdated datetime2(7) NOT NULL,
     LastUpdatedBy uniqueidentifier NOT NULL,
     CONSTRAINT PK_Solution PRIMARY KEY CLUSTERED (Id),
     CONSTRAINT FK_Solution_CatalougeItem FOREIGN KEY (Id) REFERENCES CatalogueItem(CatalogueItemId) ON DELETE CASCADE
);
