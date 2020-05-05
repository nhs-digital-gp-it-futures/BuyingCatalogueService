CREATE TABLE dbo.PurchasingModel
(
     Id uniqueidentifier NOT NULL,
     FrameworkId varchar(10) NOT NULL,
     SolutionId varchar(14) NOT NULL,
     StatusId int CONSTRAINT DF_PurchasingModel_Status DEFAULT 1 NOT NULL,
     AuthorityStatusId int CONSTRAINT DF_PurchasingModel_AuthorityStatus DEFAULT 1 NOT NULL,
     Deleted bit CONSTRAINT DF_PurchaseModel_Deleted DEFAULT 0 NOT NULL,
     LastUpdated datetime2(7) NOT NULL,
     LastUpdatedBy uniqueidentifier NOT NULL,
     CONSTRAINT PK_PurchasingModel PRIMARY KEY NONCLUSTERED (Id),
     CONSTRAINT FK_PurchasingModel_AuthorityStatus FOREIGN KEY (AuthorityStatusId) REFERENCES dbo.SolutionAuthorityStatus(Id),
     CONSTRAINT FK_PurchasingModel_FrameworkId FOREIGN KEY (FrameworkId) REFERENCES dbo.Framework(Id),
     CONSTRAINT FK_PurchasingModel_PublicationStatus FOREIGN KEY (StatusId) REFERENCES dbo.PublicationStatus(Id),
     CONSTRAINT FK_PurchasingModel_SolutionId FOREIGN KEY (SolutionId) REFERENCES dbo.Solution(Id),
     INDEX IX_PurchasingModelFrameworkId CLUSTERED (FrameworkId)
);
