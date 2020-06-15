CREATE TABLE dbo.ProvisioningType
(
    ProvisioningTypeId INT NOT NULL,
    [Name] NVARCHAR(35) NOT NULL
    CONSTRAINT PK_ProvisioningType PRIMARY KEY CLUSTERED (ProvisioningTypeId)
)
