IF NOT EXISTS (SELECT * FROM dbo.PricingUnit)
    INSERT INTO dbo.PricingUnit(PricingUnitId, [Name], TierName, [Description])
    VALUES
    (1, 'patient', 'patients', 'per patient'),
    (2, 'bed', 'beds', 'per bed'),
    (3, 'consultation', 'consultations', 'per consultation'),
    (4, 'licence', 'licenses','per license'),
    (5, 'sms', 'SMS', 'per SMS');
GO
