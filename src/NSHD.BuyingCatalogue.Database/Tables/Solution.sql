CREATE TABLE dbo.Solution
(
     Id varchar(14) NOT NULL,
     ParentId varchar(14) NULL,
     SupplierId varchar(6) NOT NULL,
     SolutionDetailId uniqueidentifier NULL,
     [Name] varchar(255) NOT NULL,
     [Version] varchar(10) NULL,
     PublishedStatusId int CONSTRAINT DF_Solution_PublishedStatus DEFAULT 1 NOT NULL,
     AuthorityStatusId int CONSTRAINT DF_Solution_AuthorityStatus DEFAULT 1 NOT NULL,
     SupplierStatusId int CONSTRAINT DF_Solution_SupplierStatus DEFAULT 1 NOT NULL,
     OnCatalogueVersion int CONSTRAINT DF_Solution_OnCatalogueVersion DEFAULT 0 NOT NULL,
     ServiceLevelAgreement nvarchar(1000) NULL,
     WorkOfPlan nvarchar(max) NULL,
     LastUpdated datetime2(7) NOT NULL,
     LastUpdatedBy uniqueidentifier NOT NULL,
     CONSTRAINT PK_Solution PRIMARY KEY CLUSTERED (Id),
     CONSTRAINT FK_Solution_AuthorityStatus FOREIGN KEY (AuthorityStatusId) REFERENCES dbo.SolutionAuthorityStatus(Id),
     CONSTRAINT FK_Solution_Parent FOREIGN KEY (ParentId) REFERENCES dbo.Solution(Id),
     CONSTRAINT FK_Solution_PublicationStatus FOREIGN KEY (PublishedStatusId) REFERENCES dbo.PublicationStatus(Id),
     CONSTRAINT FK_Solution_SolutionDetail FOREIGN KEY (SolutionDetailId) REFERENCES dbo.SolutionDetail(Id),
     CONSTRAINT FK_Solution_Supplier FOREIGN KEY (SupplierId) REFERENCES dbo.Supplier(Id),
     CONSTRAINT FK_Solution_SupplierStatus FOREIGN KEY (SupplierStatusId) REFERENCES dbo.SolutionSupplierStatus(Id)
);
