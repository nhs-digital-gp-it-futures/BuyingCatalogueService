CREATE TABLE dbo.Capability
(
     Id uniqueidentifier NOT NULL,
     CapabilityRef varchar(10) NOT NULL,
     [Version] varchar(10) NOT NULL,
     PreviousVersion varchar(10) NULL,
     StatusId int NOT NULL,
     [Name] varchar(255) NOT NULL,
     [Description] varchar(500) NOT NULL,
     SourceUrl varchar(1000) NULL,
     EffectiveDate date NOT NULL,
     CategoryId int NOT NULL,
     CONSTRAINT PK_Capability PRIMARY KEY NONCLUSTERED (Id),
     CONSTRAINT FK_Capability_CapabilityCategory FOREIGN KEY (CategoryId) REFERENCES dbo.CapabilityCategory(Id),
     CONSTRAINT FK_Capability_CapabilityStatus FOREIGN KEY (StatusId) REFERENCES dbo.CapabilityStatus(Id),
     INDEX IX_CapabilityCapabilityRef CLUSTERED (CapabilityRef)
);
