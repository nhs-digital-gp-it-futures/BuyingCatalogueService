CREATE TABLE dbo.PricingUnit
(
    PricingUnitId INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(20) NOT NULL,
    TierName NVARCHAR(20) NOT NULL,
    [Description] NVARCHAR(35) NOT NULL
    CONSTRAINT PK_PricingUnit PRIMARY KEY (PricingUnitId)
)
