CREATE TABLE dbo.PricingUnit
(
    PricingUnitId uniqueidentifier NOT NULL,
    [Name] nvarchar(20) NOT NULL,
    TierName nvarchar(20) NOT NULL,
    [Description] nvarchar(35) NOT NULL,
    CONSTRAINT PK_PricingUnit PRIMARY KEY NONClUSTERED (PricingUnitId)
);
