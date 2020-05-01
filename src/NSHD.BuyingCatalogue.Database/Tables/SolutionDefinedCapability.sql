CREATE TABLE dbo.SolutionDefinedCapability
(
     Id varchar(18) NOT NULL,
     SolutionId varchar(14) NOT NULL,
     [Name] varchar(100) NOT NULL,
     StatusId int NOT NULL,
     [Description] varchar(1000) NOT NULL,
     Tag varchar(100) NULL,
     LastUpdated datetime2(7) NOT NULL,
     LastUpdatedBy uniqueidentifier NOT NULL,
     CONSTRAINT PK_SolutionDefinedCapability PRIMARY KEY CLUSTERED (Id),
     CONSTRAINT FK_SolutionDefinedCapability_CapabilityStatus FOREIGN KEY (StatusId) REFERENCES dbo.SolutionCapabilityStatus(Id),
     CONSTRAINT FK_SolutionDefinedCapability_Solution FOREIGN KEY (SolutionId) REFERENCES dbo.Solution(Id) ON DELETE CASCADE
);
