CREATE TABLE dbo.StandardStatus
(
     Id int NOT NULL,
     [Name] varchar(16) NOT NULL,
     CONSTRAINT PK_StandardStatus PRIMARY KEY CLUSTERED (Id),
     CONSTRAINT IX_StandardStatusName UNIQUE NONCLUSTERED ([Name])
);
