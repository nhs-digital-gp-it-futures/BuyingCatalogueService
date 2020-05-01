CREATE TABLE dbo.AssociatedService
(
     Id varchar(18) NOT NULL,
     SolutionId varchar(14) NOT NULL,
     [Name] varchar(100) NOT NULL,
     [Description] varchar(1000) NOT NULL,
     OrderGuidance varchar(1000) NULL,
     LastUpdated datetime2(7) NOT NULL,
     LastUpdatedBy uniqueidentifier NOT NULL,
     CONSTRAINT PK_AssociatedService PRIMARY KEY CLUSTERED (Id),
     CONSTRAINT FK_AssociatedService_Solution FOREIGN KEY (SolutionId) REFERENCES dbo.Solution (Id) ON DELETE CASCADE
);
