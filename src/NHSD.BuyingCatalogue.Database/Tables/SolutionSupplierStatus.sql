CREATE TABLE dbo.SolutionSupplierStatus
(
     Id int NOT NULL,
     [Name] varchar(16) NOT NULL,
     CONSTRAINT PK_SolutionSupplierStatus PRIMARY KEY CLUSTERED (Id),
     CONSTRAINT IX_SolutionSupplierStatusName UNIQUE NONCLUSTERED ([Name])
);
