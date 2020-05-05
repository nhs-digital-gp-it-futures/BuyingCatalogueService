CREATE TABLE dbo.SolutionDefinedEpic
(
     Id uniqueidentifier NOT NULL,
     SolutionDefinedCapabilityId varchar(18) NOT NULL,
     EpicRef varchar(16) NOT NULL,
     [Name] varchar(100) NOT NULL,
     [Description] varchar(3000) NOT NULL,
     StatusId int NOT NULL,
     LastUpdated datetime2(7) NOT NULL,
     LastUpdatedBy uniqueidentifier NOT NULL,
     CONSTRAINT PK_SolutionDefinedEpic PRIMARY KEY NONCLUSTERED (Id),
     CONSTRAINT FK_SolutionDefinedEpic_SolutionDefinedCapability FOREIGN KEY (SolutionDefinedCapabilityId) REFERENCES dbo.SolutionDefinedCapability(Id),
     CONSTRAINT FK_SolutionDefinedEpic_SolutionEpicStatus FOREIGN KEY (StatusId) REFERENCES dbo.SolutionEpicStatus(Id),
     INDEX IX_SolutionDefinedEpicEpicRef CLUSTERED (EpicRef)
);
