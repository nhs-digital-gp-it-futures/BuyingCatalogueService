CREATE TABLE dbo.StandardCategory
(
     Id int NOT NULL,
     [Name] varchar(16) NOT NULL,
     CONSTRAINT PK_StandardCategory PRIMARY KEY CLUSTERED (Id),
     CONSTRAINT IX_StandardCategoryName UNIQUE NONCLUSTERED ([Name])
);
