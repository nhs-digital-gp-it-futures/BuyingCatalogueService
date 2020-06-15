IF NOT EXISTS (SELECT * FROM dbo.PricingUnit)
    INSERT INTO dbo.PricingUnit(PricingUnitId, [Name], TierName, [Description])
    VALUES
    ('F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 'patient', 'patients', 'per patient'),
    ('D43C661A-0587-45E1-B315-5E5091D6E9D0', 'bed', 'beds', 'per bed'),
    ('774E5A1D-D15C-4A37-9990-81861BEAE42B', 'consultation', 'consultations', 'per consultation'),
    ('8BF9C2F9-2FD7-4A29-8406-3C6B7B2E5D65', 'licence', 'licenses','per license'),
    ('90119522-D381-4296-82EE-8FE630593B56', 'sms', 'SMS', 'per SMS');
GO
