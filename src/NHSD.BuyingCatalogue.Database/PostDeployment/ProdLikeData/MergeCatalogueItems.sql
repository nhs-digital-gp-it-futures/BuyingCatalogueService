﻿IF (UPPER('$(INSERT_TEST_DATA)') = 'TRUE')
BEGIN
    /*********************************************************************************************************************************************/
    /* CatalogueItem */
    /*********************************************************************************************************************************************/

    CREATE TABLE #CatalogueItem
    (
        CatalogueItemId varchar(14) NOT NULL,
        [Name] varchar(255) NOT NULL,
        CatalogueItemTypeId int NOT NULL,
        SupplierId varchar(6) NOT NULL,
        PublishedStatusId int NOT NULL,
        Created datetime2(7) NOT NULL
    );

    --Catalogue Solutions
    INSERT #CatalogueItem ([CatalogueItemId], [Name], [CatalogueItemTypeId], [SupplierId], [PublishedStatusId], [Created]) 
    VALUES 
    (N'10000-001', N'Emis Web GP', 1, N'10000', 3, CAST(N'2020-03-25T07:30:18.1133333' AS DateTime2)),
    (N'10000-002', N'Anywhere Consult', 1, N'10000', 3, CAST(N'2020-04-06T10:50:03.2166667' AS DateTime2)),
    (N'10000-054', N'Online and Video Consult', 1, N'10000', 3, CAST(N'2020-04-03T12:25:59.0533333' AS DateTime2)),
    (N'10000-062', N'Video Consult', 1, N'10000', 3, CAST(N'2020-04-06T10:53:50.6266667' AS DateTime2)),
    (N'10004-001', N'Audit+', 1, N'10004', 3, CAST(N'2020-03-26T12:13:20.0833333' AS DateTime2)),
    (N'10004-002', N'FrontDesk', 1, N'10004', 3, CAST(N'2020-03-30T13:14:43.1666667' AS DateTime2)),
    (N'10007-002', N'Best Pathway', 1, N'10007', 3, CAST(N'2020-03-25T11:40:44.2900000' AS DateTime2)),
    (N'10011-003', N'Rapid VC', 1, N'10011', 1, CAST(N'2020-06-18T14:20:53.8233333' AS DateTime2)),
    (N'10020-001', N'Q doctor', 1, N'10020', 3, CAST(N'2020-04-06T12:50:27.8800000' AS DateTime2)),
    (N'10029-001', N'RIVIAM GP Portal', 1, N'10029', 1, CAST(N'2020-04-08T07:42:58.2633333' AS DateTime2)),
    (N'10029-003', N'RIVIAM Secure Video Services', 1, N'10029', 3, CAST(N'2020-04-08T08:59:03.8100000' AS DateTime2)),
    (N'10030-001', N'AccuRx', 1, N'10030', 3, CAST(N'2020-04-01T10:39:24.7066667' AS DateTime2)),
    (N'10031-001', N'GP&Me', 1, N'10031', 1, CAST(N'2020-04-01T10:37:59.3066667' AS DateTime2)),
    (N'10033-001', N'AlldayDr', 1, N'10033', 3, CAST(N'2020-04-01T10:40:33.7566667' AS DateTime2)),
    (N'10035-001', N'Evergreen Life', 1, N'10035', 3, CAST(N'2020-04-01T10:42:08.5066667' AS DateTime2)),
    (N'10046-001', N'Docman 10', 1, N'10046', 3, CAST(N'2020-03-30T13:02:24.5200000' AS DateTime2)),
    (N'10046-003', N'Docman 7', 1, N'10046', 3, CAST(N'2020-03-30T13:04:21.6500000' AS DateTime2)),
    (N'10046-006', N'PATCHS Online Consultation', 1, N'10046', 1, CAST(N'2020-06-25T14:31:07.2366667' AS DateTime2)),
    (N'10047-001', N'askmyGP', 1, N'10047', 3, CAST(N'2020-04-01T10:43:15.8533333' AS DateTime2)),
    (N'10052-002', N'SystmOne GP', 1, N'10052', 3, CAST(N'2020-03-30T13:19:48.8766667' AS DateTime2)),
    (N'10059-001', N'Advice & Guidance (Eclipse Live)', 1, N'10059', 3, CAST(N'2020-03-30T13:16:49.4100000' AS DateTime2)),
    (N'10062-001', N'FootFall', 1, N'10062', 3, CAST(N'2020-04-03T12:28:52.3800000' AS DateTime2)),
    (N'10063-002', N'Forms4Health', 1, N'10063', 1, CAST(N'2020-06-25T14:30:56.3300000' AS DateTime2)),
    (N'10064-003', N'Medloop Patient Management Optimiser', 1, N'10064', 1, CAST(N'2020-06-25T14:30:49.8600000' AS DateTime2)),
    (N'10072-003', N'Push Consult', 1, N'10072', 1, CAST(N'2020-06-25T14:30:33.5166667' AS DateTime2)),
    (N'10072-004', N'Digital Locum', 1, N'10072', 1, CAST(N'2020-06-25T14:31:34.0466667' AS DateTime2)),
    (N'10072-006', N'Push Access', 1, N'10072', 1, CAST(N'2020-06-25T14:31:15.0166667' AS DateTime2)),
    (N'10073-009', N'Remote Consultation', 1, N'10073', 3, CAST(N'2020-04-01T12:49:33.9433333' AS DateTime2)),
    --Additional Services
    (N'10030-001A001', N'AccuRx Video Consultation', 2, N'10030', 3, GETUTCDATE()),
    (N'10007-002A001', N'Localised Referral Forms', 2, N'10007', 3, GETUTCDATE()),
    (N'10007-002A002', N'Localised Supporting Content', 2, N'10007', 3, GETUTCDATE()),
    (N'10000-001A008', N'Enterprise Search and Reports', 2, N'10000', 3, GETUTCDATE()),
    (N'10000-001A007', N'Risk Stratification', 2, N'10000', 3, GETUTCDATE()),
    (N'10000-001A006', N'Document Management', 2, N'10000', 3, GETUTCDATE()),
    (N'10000-001A005', N'EMIS Web Dispensing', 2, N'10000', 3, GETUTCDATE()),
    (N'10000-001A003', N'Automated Arrivals', 2, N'10000', 3, GETUTCDATE()),
    (N'10000-001A004', N'Extract Services', 2, N'10000', 3, GETUTCDATE()),
    (N'10000-001A002', N'EMIS Mobile', 2, N'10000', 3, GETUTCDATE()),
    (N'10000-001A001', N'Long Term Conditions Manager', 2, N'10000', 3, GETUTCDATE()),
    (N'10035-001A001', N'Digital First Consultations', 2, N'10035', 3, GETUTCDATE()),
    (N'10052-002A001', N'SystmOne Enhanced', 2, N'10052', 3, GETUTCDATE()),
    (N'10052-002A002', N'SystmOne Mobile Working', 2, N'10052', 3, GETUTCDATE()),
    (N'10052-002A004', N'SystemOne Shared Admin', 2, N'10052', 3, GETUTCDATE()),
    (N'10052-002A003', N'SystmOne Auto Planner', 2, N'10052', 3, GETUTCDATE()),
    (N'10052-002A005', N'TPP Video Conferencing with Airmid', 2, N'10052', 3, GETUTCDATE()),
    --Associated Services
    (N'10046-S-001',  N'Consultancy', 3, N'10046', 3, GETUTCDATE()),
    (N'10046-002-05', N'Data Import Activity', 3, N'10046', 3, GETUTCDATE()),
    (N'10046-002-08', N'Database creation/software initialisation', 3, N'10046', 3, GETUTCDATE()),
    (N'10046-001-07', N'Docman 10 upgrade for current Docman 7 customer', 3, N'10046', 3, GETUTCDATE()),
    (N'10046-001-06', N'eLearning', 3, N'10046', 3, GETUTCDATE()),
    (N'10046-001-02', N'Post-Deployment/ Upgrade Day', 3, N'10046', 3, GETUTCDATE()),
    (N'10046-S-002',  N'Practice Merger or Split', 3, N'10046', 3, GETUTCDATE()),
    (N'10046-001-01', N'Pre-Deployment/ Upgrade Day', 3, N'10046', 3, GETUTCDATE()),
    (N'10046-S-003',  N'Remote Training', 3, N'10046', 3, GETUTCDATE()),
    (N'10046-S-004',  N'Training Services', 3, N'10046', 3, GETUTCDATE()),
    (N'10007-S-001',  N'Bespoke Documentation', 3, N'10007', 3, GETUTCDATE()),
    (N'10007-S-002',  N'Clinical System Migration/Data Consolidation', 3, N'10007', 3, GETUTCDATE()),
    (N'10007-S-003',  N'Deployment', 3, N'10007', 3, GETUTCDATE()),
    (N'10007-S-004',  N'Process Optimisation', 3, N'10007', 3, GETUTCDATE()),
    (N'10007-S-005',  N'Training', 3, N'10007', 3, GETUTCDATE()),
    (N'10000-S-001',  N'Video Consult, Online and Video Consult - Additional Minute Package (3000 minutes)', 3, N'10000', 3, GETUTCDATE()),
    (N'10000-001-01', N'Engineering', 3, N'10000', 3, GETUTCDATE()),
    (N'10000-S-004',  N'Automated Arrivals - Engineering Half Day', 3, N'10000', 3, GETUTCDATE()),
    (N'10000-054-03', N'Online and Video Consult - Implementation', 3, N'10000', 3, GETUTCDATE()),
    (N'10000-S-002',  N'Installation', 3, N'10000', 3, GETUTCDATE()),
    (N'10000-S-005',  N'Enterprise Search and Reports - Installation', 3, N'10000', 3, GETUTCDATE()),
    (N'10000-S-006',  N'Automated Arrivals - Installation of wall mounted kiosk, excludes cabling', 3, N'10000', 3, GETUTCDATE()),
    (N'10000-002-01', N'Anywhere Consult - Integrated Device', 3, N'10000', 3, GETUTCDATE()),
    (N'10000-001-02', N'Lloyd George digitisation', 3, N'10000', 3, GETUTCDATE()),
    (N'10000-001-03', N'Lloyd George Digitisation (upload only)', 3, N'10000', 3, GETUTCDATE()),
    (N'10000-001-04', N'Migration', 3, N'10000', 3, GETUTCDATE()),
    (N'10000-024-01', N'Doc Management - Migration', 3, N'10000', 3, GETUTCDATE()),
    (N'10000-001-05', N'Practice Reorganisation', 3, N'10000', 3, GETUTCDATE()),
    (N'10000-001-06', N'Project Management', 3, N'10000', 3, GETUTCDATE()),
    (N'10000-S-007',  N'Automated Arrivals - Specialist Cabling', 3, N'10000', 3, GETUTCDATE()),
    (N'10000-S-003',  N'Training Day at Practice', 3, N'10000', 3, GETUTCDATE()),
    (N'10000-S-008',  N'Automated Arrivals - Wall Mount Bracket', 3, N'10000', 3, GETUTCDATE()),
    (N'10047-001-01', N'Pathfinder', 3, N'10047', 3, GETUTCDATE()),
    (N'10047-001-02', N'Transform', 3, N'10047', 3, GETUTCDATE()),
    (N'10004-001-01', N'Customisation', 3, N'10004', 3, GETUTCDATE()),
    (N'10004-002-02', N'Data Migration from existing appointments solution', 3, N'10004', 3, GETUTCDATE()),
    (N'10004-001-03', N'End User Training Seminar', 3, N'10004', 3, GETUTCDATE()),
    (N'10004-S-001',  N'Training Day at Practice', 3, N'10004', 3, GETUTCDATE()),
    (N'10073-009-03', N'my GP Integration', 3, N'10073', 3, GETUTCDATE()),
    (N'10073-009-01', N'Setup/Provisioning/Launch', 3, N'10073', 3, GETUTCDATE()),
    (N'10073-009-02', N'Training', 3, N'10073', 3, GETUTCDATE()),
    (N'10029-003-02', N'Business Analysis', 3, N'10029', 3, GETUTCDATE()),
    (N'10029-003-04', N'Information Architecture', 3, N'10029', 3, GETUTCDATE()),
    (N'10029-003-01', N'Project Management', 3, N'10029', 3, GETUTCDATE()),
    (N'10029-003-03', N'Solution Model Development', 3, N'10029', 3, GETUTCDATE()),
    (N'10029-003-05', N'Staff Training', 3, N'10029', 3, GETUTCDATE()),
    (N'10052-002-08', N'Data Extraction Service', 3, N'10052', 3, GETUTCDATE()),
    (N'10052-S-001',  N'Full Training Day', 3, N'10052', 3, GETUTCDATE()),
    (N'10052-S-002',  N'Half Training Day', 3, N'10052', 3, GETUTCDATE()),
    (N'10052-002-05', N'Practice Merge', 3, N'10052', 3, GETUTCDATE()),
    (N'10052-002-06', N'Practice Split', 3, N'10052', 3, GETUTCDATE()),
    (N'10052-002-07', N'Provision of Legacy Data', 3, N'10052', 3, GETUTCDATE()),
    (N'10052-S-003',  N'Super User Course', 3, N'10052', 3, GETUTCDATE()),
    (N'10052-002-11', N'SystmOne GP Deployment (Data Migration, Full Training)', 3, N'10052', 3, GETUTCDATE()),
    (N'10052-002-01', N'SystmOne GP Deployment (Data Migration, Go-Live Support)', 3, N'10052', 3, GETUTCDATE()),
    (N'10052-S-004',  N'Train the Trainer Course', 3, N'10052', 3, GETUTCDATE()),
    (N'10052-S-005',  N'Training Environment', 3, N'10052', 3, GETUTCDATE());

    MERGE INTO [dbo].[CatalogueItem] AS TARGET
    USING #CatalogueItem AS SOURCE
    ON TARGET.CatalogueItemId = SOURCE.CatalogueItemId  
    WHEN MATCHED THEN  
        UPDATE SET TARGET.[Name] = SOURCE.[Name],
                   TARGET.CatalogueItemTypeId = SOURCE.CatalogueItemTypeId,
                   TARGET.SupplierId = SOURCE.SupplierId,
                   TARGET.PublishedStatusId = SOURCE.PublishedStatusId,
                   TARGET.Created = SOURCE.Created
    WHEN NOT MATCHED BY TARGET THEN  
        INSERT  (CatalogueItemId, [Name], CatalogueItemTypeId, SupplierId, PublishedStatusId, Created) 
        VALUES  (SOURCE.CatalogueItemId, SOURCE.[Name], SOURCE.CatalogueItemTypeId, SOURCE.SupplierId, SOURCE.PublishedStatusId, SOURCE.Created);
END;
GO
