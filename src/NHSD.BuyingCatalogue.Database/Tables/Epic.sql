CREATE TABLE dbo.Epic
(
     Id varchar(10) NOT NULL,
     [Name] varchar(150) NOT NULL,
     CapabilityId uniqueidentifier NOT NULL,
     SourceUrl varchar(max) NULL,
     CompliancyLevelId int NULL,
     Active bit NOT NULL,
     CONSTRAINT PK_Epic PRIMARY KEY NONCLUSTERED (Id),
     CONSTRAINT FK_Epic_Capability FOREIGN KEY (CapabilityId) REFERENCES dbo.Capability(Id),
     CONSTRAINT FK_Epic_CompliancyLevel FOREIGN KEY (CompliancyLevelId) REFERENCES dbo.CompliancyLevel(Id)
);
