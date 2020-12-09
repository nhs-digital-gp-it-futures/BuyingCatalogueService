CREATE TABLE dbo.PricingUnit
(
    PricingUnitId uniqueidentifier NOT NULL,
    [Name] varchar(20) NOT NULL,
    TierName varchar(30) NOT NULL,
    [Description] varchar(40) NOT NULL,
    CONSTRAINT PK_PricingUnit PRIMARY KEY NONClUSTERED (PricingUnitId)
);
