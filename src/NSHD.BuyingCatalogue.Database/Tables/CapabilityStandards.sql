CREATE TABLE dbo.CapabilityStandards
(
     CapabilityId uniqueidentifier NOT NULL,
     StandardId uniqueidentifier NOT NULL,
     IsOptional bit CONSTRAINT DF_CapabilityStandards_IsOptional DEFAULT 0 NOT NULL,
     CONSTRAINT PK_CapabilityStandards PRIMARY KEY CLUSTERED (CapabilityId, StandardId),
     CONSTRAINT FK_CapabilityStandards_Capability FOREIGN KEY (CapabilityId) REFERENCES dbo.Capability(Id),
     CONSTRAINT FK_CapabilityStandards_Standard FOREIGN KEY (StandardId) REFERENCES dbo.[Standard](Id)
);
