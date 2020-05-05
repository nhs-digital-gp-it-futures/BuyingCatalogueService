CREATE TABLE dbo.SolutionStandardStatus
(
     Id int NOT NULL,
     [Name] varchar(16) NOT NULL,
     CONSTRAINT PK_SolutionStandardStatus PRIMARY KEY CLUSTERED (Id),
     CONSTRAINT IX_SolutionStandardStatusName UNIQUE NONCLUSTERED ([Name])
);
