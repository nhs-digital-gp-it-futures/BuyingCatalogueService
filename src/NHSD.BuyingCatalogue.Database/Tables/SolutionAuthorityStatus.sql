CREATE TABLE dbo.SolutionAuthorityStatus
(
     Id int NOT NULL,
     [Name] varchar(16) NOT NULL,
     CONSTRAINT PK_SolutionAuthorityStatus PRIMARY KEY CLUSTERED (Id),
     CONSTRAINT IX_SolutionAuthorityStatusName UNIQUE NONCLUSTERED ([Name])
);
