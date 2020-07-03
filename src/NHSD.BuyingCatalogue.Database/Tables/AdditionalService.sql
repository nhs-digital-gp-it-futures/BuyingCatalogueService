CREATE TABLE dbo.AdditionalService
(
    CatalogueItemId varchar(14) NOT NULL,
    Summary varchar(300) NULL,
    FullDescription varchar(3000) NULL,
    LastUpdated datetime2(7) NULL,
    LastUpdatedBy uniqueidentifier NULL,
    SolutionId varchar(14) NULL,
    CONSTRAINT FK_AdditionalService_CatalogueItem FOREIGN KEY (CatalogueItemId) REFERENCES dbo.CatalogueItem(CatalogueItemId) ON DELETE CASCADE,
    CONSTRAINT FK_AdditionalService_Solution FOREIGN KEY (SolutionId) REFERENCES dbo.Solution(Id)
);
