CREATE TABLE #PricingUnit
(
    PricingUnitId uniqueidentifier NOT NULL,
    [Name] varchar(20) NOT NULL,
    TierName varchar(20) NOT NULL,
    [Description] varchar(35) NOT NULL    
);

INSERT INTO #PricingUnit(PricingUnitId, [Name], TierName, [Description])
VALUES
    ('F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 'patient', 'patients', 'per patient'),
    ('D43C661A-0587-45E1-B315-5E5091D6E9D0', 'bed', 'beds', 'per bed'),
    ('774E5A1D-D15C-4A37-9990-81861BEAE42B', 'consultation', 'consultations', 'per consultation'),
    ('8BF9C2F9-2FD7-4A29-8406-3C6B7B2E5D65', 'licence', 'licenses','per license'),
    ('90119522-D381-4296-82EE-8FE630593B56', 'sms', 'SMS', 'per SMS'),
    ('aad2820e-472d-4bac-864e-853f92e9b3bc', 'practice', 'practices', 'per practice'),
    ('cc6ee39d-41f1-4671-b31a-800485d05752', 'unit', 'units', 'per unit'),
    ('9d3bade6-f232-4b6e-9809-88a8fbb5c881', 'group', 'groups', 'per group'),
    ('599a1105-a16a-4861-b54b-d65da84366c9', 'day', 'days', 'per day'),
    ('121bd710-b73b-48f9-a429-f269a7bb0bf2', 'halfDay', 'half days', 'per half day'),
    ('823f814d-82c9-4994-94af-4c413ee418dc', 'hour', 'hours', 'per hour'),
    ('7e4dd1fd-c953-41a8-9e62-64dc306a6307', 'installation', 'installations', 'per installation'),
    ('701afb98-699e-4eda-9a66-e79a91769614', 'implementation', 'implementations', 'per implementation'),
    ('f2bb1b9d-b546-40b3-bfd9-d55221d96880', 'practiceMergerSplit', 'mergers/splits', 'per practice merge/split'),
    ('6f65c40f-e7cc-4140-85c5-592dcd216132', 'extract', 'extracts', 'per extract'),
    ('59fa7cad-87b8-4e78-92b7-5689f6b123dc', 'migration', 'migrations', 'per migration'),
    ('e17fbd0b-208f-453f-938a-9880bab1ec5e', 'course', 'courses', 'per course'),
    ('1d40c0d1-6bd5-40b3-ba2f-cf433f339787', 'environment', 'environments', 'per environment'),
    ('4b9a4640-a97a-4e30-8ed5-cccae9829616', 'user', 'users', 'per user'),
    ('66443acc-7e40-4f95-955b-47234016cff1', 'document', 'documents', 'per document'),
    ('626b43e6-c9a0-4ede-99ed-da3a1ad2d8d3', 'seminar', 'seminars', 'per seminar'),
    ('60523726-bbaf-4ec3-b29c-dee2f3d3eca8', 'item', 'items', 'per item'),
    ('8a5e119f-9b33-4017-8cc9-552e86e20898', 'activeUser', 'active users', 'per active user');


MERGE INTO [dbo].[PricingUnit] AS TARGET
USING #PricingUnit AS SOURCE
ON TARGET.[PricingUnitId] = SOURCE.[PricingUnitId] 
WHEN MATCHED THEN  
    UPDATE SET TARGET.[Name] = SOURCE.[Name],
               TARGET.TierName = SOURCE.TierName,
               TARGET.[Description] = SOURCE.[Description]
WHEN NOT MATCHED BY TARGET THEN  
    INSERT  ([PricingUnitId], [Name], TierName, [Description]) 
    VALUES  (SOURCE.[PricingUnitId], SOURCE.[Name], SOURCE.TierName, SOURCE.[Description]);    

DROP TABLE #PricingUnit;
GO