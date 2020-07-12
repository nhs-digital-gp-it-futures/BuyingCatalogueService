IF NOT EXISTS (SELECT * FROM dbo.PricingUnit)
    INSERT INTO dbo.PricingUnit(PricingUnitId, [Name], TierName, [Description])
    VALUES
    ('F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 'patient', 'patients', 'per patient'),
    ('D43C661A-0587-45E1-B315-5E5091D6E9D0', 'bed', 'beds', 'per bed'),
    ('774E5A1D-D15C-4A37-9990-81861BEAE42B', 'consultation', 'consultations', 'per consultation'),
    ('8BF9C2F9-2FD7-4A29-8406-3C6B7B2E5D65', 'licence', 'licenses','per license'),
    ('90119522-D381-4296-82EE-8FE630593B56', 'sms', 'SMS', 'per SMS'),
    ('aad2820e-472d-4bac-864e-853f92e9b3bc', 'practice', 'practices', 'per practice'),
    ('cc6ee39d-41f1-4671-b31a-800485d05752', 'unit', 'units', 'per unit'),
    ('9d3bade6-f232-4b6e-9809-88a8fbb5c881', 'group', 'groups', 'per group'),
    ('8a5e119f-9b33-4017-8cc9-552e86e20898', 'activeUser', 'active users', 'per active user');
GO
