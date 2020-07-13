﻿IF (UPPER('$(INSERT_TEST_DATA)') = 'TRUE')
BEGIN

    /*********************************************************************************************************************************************/
    /* AssociatedService */
    /*********************************************************************************************************************************************/

    CREATE TABLE #AssociatedService
    (
    [CatalogueItemId] varchar(14) NOT NULL,
    [Description] varchar(1000) NULL,
    OrderGuidance varchar(1000) NULL,
    LastUpdated datetime2(7) NULL,
    LastUpdatedBy uniqueidentifier NULL,
    );

    INSERT INTO #AssociatedService ([CatalogueItemId], [Description], OrderGuidance, [LastUpdated], [LastUpdatedBy]) 
    VALUES 
    (N'10000-S-002',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10000-S-005',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10000-S-006',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10000-002-01',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10000-001-02',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10000-001-03',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10000-001-04',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10000-024-01',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10000-001-05',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10000-001-06',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10000-S-007',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10000-S-003',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10000-S-008',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10047-001-01',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10047-001-02',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10004-001-01',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10004-002-02',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10004-001-03',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10004-S-001',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10073-009-03',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10073-009-01',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10073-009-02',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10029-003-02',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10029-003-04',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10029-003-01',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10029-003-03',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10029-003-05',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10052-002-08',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10052-S-001',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10052-S-002',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10052-002-05',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10052-002-06',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10052-002-07',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10052-S-003',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10052-002-11',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10052-002-01',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10052-S-004',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000'),
    (N'10052-S-005',N'DESCRIPTION',N'',GETUTCDATE(),N'00000000-0000-0000-0000-000000000000');


    MERGE INTO [dbo].[AssociatedService] AS TARGET
    USING #AssociatedService AS SOURCE
    ON TARGET.[AssociatedServiceId] = SOURCE.[CatalogueItemId] 
    WHEN MATCHED THEN  
    UPDATE SET TARGET.[Description] = SOURCE.[Description],
                TARGET.OrderGuidance = SOURCE.OrderGuidance,
                TARGET.[LastUpdated] = SOURCE.[LastUpdated],
                TARGET.[LastUpdatedBy] = SOURCE.[LastUpdatedBy]
    WHEN NOT MATCHED BY TARGET THEN  
    INSERT  ([AssociatedServiceId], [Description], [OrderGuidance], [LastUpdated], [LastUpdatedBy])
    VALUES  (SOURCE.[CatalogueItemId], SOURCE.[Description], SOURCE.[OrderGuidance], SOURCE.[LastUpdated], SOURCE.[LastUpdatedBy]);

END;
GO