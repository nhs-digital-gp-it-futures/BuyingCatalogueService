CREATE TABLE dbo.[Standard]
(
     Id uniqueidentifier NOT NULL,
     StandardRef varchar(10) NOT NULL,
     [Version] varchar(10) NOT NULL,
     PreviousId int NULL,
     [Name] varchar(100) NOT NULL,
     [Description] varchar(500) NOT NULL,
     CategoryId int NOT NULL,
     SourceUrl varchar(1000) NULL,
     StatusId int NOT NULL,
     EffectiveDate date NULL,
     CONSTRAINT PK_Standard PRIMARY KEY NONCLUSTERED (Id),
     CONSTRAINT FK_Standard_StandardCategory FOREIGN KEY (CategoryId) REFERENCES dbo.StandardCategory(Id),
     CONSTRAINT FK_Standard_StandardStatus FOREIGN KEY (StatusId) REFERENCES dbo.StandardStatus(Id),
     INDEX IX_StandardStandardRef CLUSTERED (StandardRef)
);
