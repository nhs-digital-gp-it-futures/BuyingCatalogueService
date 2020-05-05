CREATE TABLE dbo.SolutionDefinedEpicAcceptanceCriteria
(
     Id uniqueidentifier NOT NULL,
     EpicId uniqueidentifier NOT NULL,
     EpicRef varchar(16) NOT NULL,
     [Name] varchar(100) NOT NULL,
     [Description] nvarchar(max) NOT NULL,
     LastUpdated datetime2(7) NOT NULL,
     LastUpdatedBy uniqueidentifier NOT NULL,
     CONSTRAINT PK_SolutionDefinedEpicAcceptanceCriteria PRIMARY KEY NONCLUSTERED (Id),
     CONSTRAINT FK_SolutionDefinedEpicAcceptanceCriteria_SolutionDefinedEpic FOREIGN KEY (EpicId) REFERENCES dbo.SolutionDefinedEpic(Id),
     INDEX IX_SolutionDefinedEpicAcceptanceCriteriaEpicRef CLUSTERED (EpicRef)
);
