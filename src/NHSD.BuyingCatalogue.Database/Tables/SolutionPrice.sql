CREATE TABLE dbo.SolutionPrice
(
     Id uniqueidentifier NOT NULL,
     PurchasingModelId uniqueidentifier NOT NULL,
     UnitId int NOT NULL,
     PriceTypeId int NOT NULL,
     ConsumptionPrice bit CONSTRAINT DF_Solution_ConsumptionPrice DEFAULT 0 NOT NULL,
     [Description] varchar(30) NULL,
     Price decimal(18, 4) NOT NULL,
     BandStart int NOT NULL,
     BandEnd int NULL,
     Created datetime2(7) CONSTRAINT DF_SolutionPrice_Created DEFAULT GETUTCDATE() NOT NULL,
     CONSTRAINT PK_SolutionPrice PRIMARY KEY NONCLUSTERED (Id),
     CONSTRAINT FK_SolutionPrice_PriceType FOREIGN KEY (PriceTypeId) REFERENCES dbo.PricingUnit(Id),
     CONSTRAINT FK_SolutionPrice_PricingUnit FOREIGN KEY (UnitId) REFERENCES dbo.PricingUnit(Id),
     CONSTRAINT FK_SolutionPrice_PurchasingModelId FOREIGN KEY (PurchasingModelId) REFERENCES dbo.PurchasingModel(Id) ON DELETE CASCADE,
     INDEX IX_SolutionPriceLastUpdated CLUSTERED (Created)
);
