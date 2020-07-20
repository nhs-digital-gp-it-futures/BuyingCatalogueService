IF (UPPER('$(INSERT_TEST_DATA)') = 'TRUE')
BEGIN
    /*********************************************************************************************************************************************/
    /* CataloguePrice */
    /*********************************************************************************************************************************************/

    CREATE TABLE #CataloguePrice
    (
        CataloguePriceId int NOT NULL,
        CatalogueItemId varchar(14) NOT NULL,
        ProvisioningTypeId int NOT NULL,
        CataloguePriceTypeId int NOT NULL,
        PricingUnitId uniqueidentifier NOT NULL,
        TimeUnitId int NULL,
        CurrencyCode varchar(3) NOT NULL,
        LastUpdated datetime2(7) NOT NULL,
        Price decimal(18,3) NULL,
    );

    -- Solutions per patient
    INSERT INTO #CataloguePrice (CataloguePriceId, CatalogueItemId, ProvisioningTypeId, CataloguePriceTypeId, PricingUnitId, TimeUnitId, CurrencyCode, LastUpdated, Price) 
         VALUES (1001, N'10000-001', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP', GETUTCDATE(), 1.26),
                (1002, N'10000-054', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP', GETUTCDATE(), 0.15),
                (1003, N'10000-062', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP', GETUTCDATE(), 0.02),
                (1004, N'10004-001', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP', GETUTCDATE(), 0.22),
                (1005, N'10004-002', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP', GETUTCDATE(), 0.19),
                (1006, N'10007-002', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP', GETUTCDATE(), 0.14),
                (1007, N'10020-001', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP', GETUTCDATE(), 0.5),
                (1008, N'10029-003', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP', GETUTCDATE(), 0.84),
                (1009, N'10046-001', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP', GETUTCDATE(), 0.28),
                (1010, N'10046-003', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP', GETUTCDATE(), 0.28),
                (1011, N'10047-001', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP', GETUTCDATE(), 0.84),
                (1012, N'10052-002', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP', GETUTCDATE(), 1.26),
                (1013, N'10059-001', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP', GETUTCDATE(), 0.14),
                (1014, N'10030-001', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP', GETUTCDATE(), 0),
                (1015, N'10033-001', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP', GETUTCDATE(), 1.26),
                (1016, N'10062-001', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP', GETUTCDATE(), 0.3),
    -- Solutions Variable On Demand 
                (1017, N'10035-001', 3, 1,'8a5e119f-9b33-4017-8cc9-552e86e20898', NULL, 'GBP', GETUTCDATE(), 0),
    -- Solutions Declarative
                (1018, N'10000-002', 2, 1,'8BF9C2F9-2FD7-4A29-8406-3C6B7B2E5D65', 1, 'GBP', GETUTCDATE(), 37.92),
                (1019, N'10073-009', 2, 1,'AAD2820E-472D-4BAC-864E-853F92E9B3BC', 1, 'GBP', GETUTCDATE(),207.92),
    -- Additional Service Per Patient
                (1020, N'10000-001A001', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP',GETUTCDATE(), 0.25),
                (1021, N'10000-001A002', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP',GETUTCDATE(), 0.07),
                (1022, N'10000-001A004', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP',GETUTCDATE(), 0.06),
                (1023, N'10000-001A005', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP',GETUTCDATE(), 0.25),
                (1024, N'10000-001A006', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP',GETUTCDATE(), 0.12),
                (1025, N'10000-001A007', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP',GETUTCDATE(), 0.06),
                (1026, N'10000-001A008', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP',GETUTCDATE(), 0.06),
                (1027, N'10007-002A001', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP',GETUTCDATE(), 0.04),
                (1028, N'10007-002A002', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP',GETUTCDATE(), 0.05),
                (1029, N'10052-002A001', 1, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 2, 'GBP',GETUTCDATE(), 0.38),
    -- Additional Service Variable
                (1030, N'10030-001A001', 3, 1,'774E5A1D-D15C-4A37-9990-81861BEAE42B', NULL, 'GBP', GETUTCDATE(), 0.2),
                (1031, N'10035-001A001', 3, 1,'774E5A1D-D15C-4A37-9990-81861BEAE42B', NULL, 'GBP', GETUTCDATE(), 0),
                (1032, N'10052-002A005', 3, 1,'a92c1326-4826-48b3-b429-4a368adb9785', NULL, 'GBP', GETUTCDATE(), 0),
    -- Additional Service Declarative
                (1033, N'10000-001A003', 2, 1,'8BF9C2F9-2FD7-4A29-8406-3C6B7B2E5D65', 1, 'GBP', GETUTCDATE(),35.51),
                (1034, N'10052-002A002', 2, 1,'CC6EE39D-41F1-4671-B31A-800485D05752', 1, 'GBP', GETUTCDATE(),68.5),
                (1035, N'10052-002A003', 2, 1,'CC6EE39D-41F1-4671-B31A-800485D05752', 1, 'GBP', GETUTCDATE(),68.5),
                (1036, N'10052-002A004', 2, 1,'9D3BADE6-F232-4B6E-9809-88A8FBB5C881', 1, 'GBP', GETUTCDATE(),291.67),
    -- Associated Service Variable
                (1037, N'10000-001-02', 3, 1,'774E5A1D-D15C-4A37-9990-81861BEAE42B', NULL, 'GBP', GETUTCDATE(), 0.2),
                (1038, N'10000-001-03', 3, 1,'774E5A1D-D15C-4A37-9990-81861BEAE42B', NULL, 'GBP', GETUTCDATE(), 0.2),
                (1039, N'10047-001-02', 3, 1,'F8D06518-1A20-4FBA-B369-AB583F9FA8C0', NULL, 'GBP', GETUTCDATE(), 0.2),
    -- Associated Service Declarative
                (1040, N'10073-009-02', 2, 1,'823f814d-82c9-4994-94af-4c413ee418dc', NULL, 'GBP', GETUTCDATE(), 75),
                (1041, N'10052-S-003',  2, 1,'e17fbd0b-208f-453f-938a-9880bab1ec5e', NULL, 'GBP', GETUTCDATE(), 3188.72),
                (1042, N'10052-S-004',  2, 1,'e17fbd0b-208f-453f-938a-9880bab1ec5e', NULL, 'GBP', GETUTCDATE(), 6377.44),
                (1043, N'10052-002-07', 2, 1,'6f65c40f-e7cc-4140-85c5-592dcd216132', NULL, 'GBP', GETUTCDATE(), 1415.13),
                (1044, N'10000-001-01', 2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 595),
                (1045, N'10000-001-06', 2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 510),
                (1046, N'10000-S-003',  2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 650),
                (1047, N'10004-001-01', 2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 500),
                (1048, N'10004-S-001',  2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 500),
                (1049, N'10007-S-002',  2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 300),
                (1050, N'10007-S-004',  2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 405),
                (1051, N'10007-S-005',  2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 495),
                (1052, N'10029-003-01', 2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 750),
                (1053, N'10029-003-02', 2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 750),
                (1054, N'10029-003-03', 2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 750),
                (1055, N'10029-003-04', 2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 750),
                (1056, N'10029-003-05', 2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 750),
                (1057, N'10046-001-01', 2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 750),
                (1058, N'10046-001-02', 2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 750),
                (1059, N'10046-001-07', 2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 750),
                (1060, N'10046-002-05', 2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 750),
                (1061, N'10046-002-08', 2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 750),
                (1062, N'10046-S-001',  2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 925),
                (1063, N'10046-S-002',  2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 750),
                (1064, N'10046-S-003',  2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 700),
                (1065, N'10046-S-004',  2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 750),
                (1066, N'10052-S-001',  2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 637.74),
                (1067, N'10073-009-01', 2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 600),
                (1068, N'10073-009-03', 2, 1,'599a1105-a16a-4861-b54b-d65da84366c9', NULL, 'GBP', GETUTCDATE(), 600),
                (1069, N'10007-S-001',  2, 1,'66443acc-7e40-4f95-955b-47234016cff1', NULL, 'GBP', GETUTCDATE(), 20),
                (1070, N'10052-002-08', 2, 1,'6f65c40f-e7cc-4140-85c5-592dcd216132', NULL, 'GBP', GETUTCDATE(), 1979.21),
                (1071, N'10000-S-004',  2, 1,'121bd710-b73b-48f9-a429-f269a7bb0bf2', NULL, 'GBP', GETUTCDATE(), 252.5),
                (1072, N'10052-S-002',  2, 1,'121bd710-b73b-48f9-a429-f269a7bb0bf2', NULL, 'GBP', GETUTCDATE(), 637.74),
                (1073, N'10000-001-04', 2, 1,'701afb98-699e-4eda-9a66-e79a91769614', NULL, 'GBP', GETUTCDATE(), 3000),
                (1074, N'10000-054-03', 2, 1,'7e4dd1fd-c953-41a8-9e62-64dc306a6307', NULL, 'GBP', GETUTCDATE(), 800),
                (1075, N'10000-S-002',  2, 1,'7e4dd1fd-c953-41a8-9e62-64dc306a6307', NULL, 'GBP', GETUTCDATE(), 1000),
                (1076, N'10000-S-005',  2, 1,'7e4dd1fd-c953-41a8-9e62-64dc306a6307', NULL, 'GBP', GETUTCDATE(), 2500),
                (1077, N'10000-002-01', 2, 1,'60523726-bbaf-4ec3-b29c-dee2f3d3eca8', NULL, 'GBP', GETUTCDATE(), 1680),
                (1078, N'10000-S-001',  2, 1,'60523726-bbaf-4ec3-b29c-dee2f3d3eca8', NULL, 'GBP', GETUTCDATE(), 90),
                (1079, N'10000-S-006',  2, 1,'60523726-bbaf-4ec3-b29c-dee2f3d3eca8', NULL, 'GBP', GETUTCDATE(), 2048.88),
                (1080, N'10000-S-007',  2, 1,'60523726-bbaf-4ec3-b29c-dee2f3d3eca8', NULL, 'GBP', GETUTCDATE(), 435),
                (1081, N'10000-S-008',  2, 1,'60523726-bbaf-4ec3-b29c-dee2f3d3eca8', NULL, 'GBP', GETUTCDATE(), 75),
                (1082, N'10000-024-01', 2, 1,'59fa7cad-87b8-4e78-92b7-5689f6b123dc', NULL, 'GBP', GETUTCDATE(), 1500),
                (1083, N'10052-002-01', 2, 1,'59fa7cad-87b8-4e78-92b7-5689f6b123dc', NULL, 'GBP', GETUTCDATE(), 2927.93),
                (1084, N'10052-002-11', 2, 1,'59fa7cad-87b8-4e78-92b7-5689f6b123dc', NULL, 'GBP', GETUTCDATE(), 4826.88),
                (1085, N'10004-002-02', 2, 1,'aad2820e-472d-4bac-864e-853f92e9b3bc', NULL, 'GBP', GETUTCDATE(), 1300),
                (1086, N'10007-S-003',  2, 1,'aad2820e-472d-4bac-864e-853f92e9b3bc', NULL, 'GBP', GETUTCDATE(), 400),
                (1087, N'10047-001-01', 2, 1,'aad2820e-472d-4bac-864e-853f92e9b3bc', NULL, 'GBP', GETUTCDATE(), 2300),
                (1088, N'10000-001-05', 2, 1,'f2bb1b9d-b546-40b3-bfd9-d55221d96880', NULL, 'GBP', GETUTCDATE(), 1500),
                (1089, N'10004-001-03', 2, 1,'626b43e6-c9a0-4ede-99ed-da3a1ad2d8d3', NULL, 'GBP', GETUTCDATE(), 850),
                (1090, N'10052-S-005',  2, 1,'1d40c0d1-6bd5-40b3-ba2f-cf433f339787', NULL, 'GBP', GETUTCDATE(), 953.03),
                (1091, N'10052-002-05', 2, 1,'f2bb1b9d-b546-40b3-bfd9-d55221d96880', NULL, 'GBP', GETUTCDATE(), 2089.16),
                (1092, N'10052-002-06', 2, 1,'f2bb1b9d-b546-40b3-bfd9-d55221d96880', NULL, 'GBP', GETUTCDATE(), 2089.16),
                (1093, N'10046-001-06', 2, 1,'4b9a4640-a97a-4e30-8ed5-cccae9829616', NULL, 'GBP', GETUTCDATE(), 40);

    SET IDENTITY_INSERT dbo.CataloguePrice ON; 

    MERGE INTO dbo.CataloguePrice AS TARGET
    USING #CataloguePrice AS SOURCE
    ON TARGET.CataloguePriceId = SOURCE.CataloguePriceId
    WHEN MATCHED THEN
           UPDATE SET TARGET.CatalogueItemId = SOURCE.CatalogueItemId,
                      TARGET.ProvisioningTypeId = SOURCE.ProvisioningTypeId,
                      TARGET.CataloguePriceTypeId = SOURCE.CataloguePriceTypeId,
                      TARGET.PricingUnitId = SOURCE.PricingUnitId,
                      TARGET.CurrencyCode = SOURCE.CurrencyCode,
                      TARGET.LastUpdated = SOURCE.LastUpdated
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (CataloguePriceId, CatalogueItemId, ProvisioningTypeId, CataloguePriceTypeId, PricingUnitId, TimeUnitId, CurrencyCode, LastUpdated, Price)
        VALUES (SOURCE.CataloguePriceId, SOURCE.CatalogueItemId, SOURCE.ProvisioningTypeId, SOURCE.CataloguePriceTypeId, SOURCE.PricingUnitId, SOURCE.TimeUnitId, SOURCE.CurrencyCode, SOURCE.LastUpdated, SOURCE.Price);

    SET IDENTITY_INSERT dbo.CataloguePrice OFF;
END;
GO
