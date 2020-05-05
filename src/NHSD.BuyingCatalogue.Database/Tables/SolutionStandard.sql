CREATE TABLE dbo.SolutionStandard
(
     SolutionId varchar(14) NOT NULL,
     StandardId uniqueidentifier NOT NULL,
     StatusId int NOT NULL,
     LastUpdated datetime2(7) NOT NULL,
     LastUpdatedBy uniqueidentifier NOT NULL,
     CONSTRAINT PK_SolutionStandard_1 PRIMARY KEY CLUSTERED (SolutionId, StandardId),
     CONSTRAINT FK_SolutionStandard_Solution FOREIGN KEY (SolutionId) REFERENCES dbo.Solution(Id) ON DELETE CASCADE,
     CONSTRAINT FK_SolutionStandard_SolutionStandardStatus FOREIGN KEY (StatusId) REFERENCES dbo.SolutionStandardStatus(Id),
     CONSTRAINT FK_SolutionStandard_Standard FOREIGN KEY (StandardId) REFERENCES dbo.Standard(Id)
);
