DECLARE @emptyGuid AS uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @now AS datetime = GETUTCDATE();

IF UPPER('$(INSERT_TEST_DATA)') = 'TRUE' AND NOT EXISTS (SELECT * FROM dbo.Supplier)
BEGIN
    INSERT INTO dbo.Supplier(Id, [Name], LegalName, Summary, [Address], LastUpdated, LastUpdatedBy)
    VALUES (
        '100000',
        'Really Kool Corporation',
        'Really Kool Corporation',
        'Really Kool Corporation is a fictious UK based IT company but that''s not going to stop us making Really Kool products!',
        '{"line1": "The Tower", "line2": "High Street", "city": "Leeds", "county": "West Yorkshire", "postcode": "LS1 1BB", "country": "UK"}',
        @now,
        @emptyGuid);

    INSERT INTO dbo.Supplier(Id, [Name], LegalName, Summary, LastUpdated, LastUpdatedBy)
    VALUES
    ('100001', 'Remedical Software', 'Remedical Limited', 'The Remedical Software', @now, @emptyGuid),
    ('100002', 'CareShare', 'CareShare Limited', 'The CareShare', @now, @emptyGuid),
    ('100003', 'Avatar Solutions', 'Avatar Solutions Plc', 'Avatar Solutions', @now, @emptyGuid),
    ('100004', 'Catterpillar Medworks', 'Catterpillar Medworks Ltd', 'Catterpillar Medworks', @now, @emptyGuid),
    ('100005', 'Curtis Systems', 'Curtis Systems Ltd', 'Curtis Systems', @now, @emptyGuid),
    ('100006', 'Clinical Raptor', 'Clinical Raptor Ltd', 'Clinical Raptor', @now, @emptyGuid),
    ('100007', 'Doc Lightning', 'Doc Lightning Ltd', 'Doc Lightning', @now, @emptyGuid),
    ('100008', 'Docability Software', 'Docability Ltd', 'Docability Software', @now, @emptyGuid),
    ('100009', 'Empire Softworks',  'Empire Softworks Plc', 'Empire Softworks', @now, @emptyGuid),
    ('100010', 'Cure Forward', 'Cure Forward Ltd', 'Cure Forward', @now, @emptyGuid),
    ('100011', 'Hansa Healthcare', 'Hansa Healthcare Plc', 'Hansa Healthcare', @now, @emptyGuid),
    ('100012', 'Moonlight Intercare', 'Moonlight Intercare', 'Moonlight Intercare', @now, @emptyGuid),
    ('100013', 'eHealth Development', 'eHealth Development', 'eHealth Development', @now, @emptyGuid),
    ('100014', 'Dr. Nick', 'Dr. Nick', 'Dr. Nick', @now, @emptyGuid),
    ('100015', 'Testproof Technology',  'Testproof Technology', 'Testproof Technology', @now, @emptyGuid),
    ('100016', 'Hojo Health', 'Hojo Health Ltd', 'Hojo Health', @now, @emptyGuid),
    ('100017', 'Jericho Healthcare', 'Jericho Ltd', 'Jericho Healthcare', @now, @emptyGuid),
    ('100018', 'Mana Systems', 'Mana Systems', 'Mana Systems', @now, @emptyGuid),
    ('100019', 'Sunhealth Nanosystems', 'Sunhealth Nanosystems', 'Sunhealth Nanosystems', @now, @emptyGuid),
    ('100020', 'Oakwood', 'Oakwood Ltd', 'Oakwood', @now, @emptyGuid);

    INSERT INTO dbo.Supplier(Id, [Name], LegalName, Summary, SupplierUrl, LastUpdated, LastUpdatedBy)
    VALUES
    (
        '99999',
        'NotEmis Health',
        'NotEgton Medical Information Systems',
        'We are the UK leader in connected healthcare software & services. Through innovative IT we help healthcare professionals access the information they need to provide better, faster and more cost effective patient care.

    Our clinical software is used in all major healthcare settings from GP surgeries to pharmacies, community, hospitals, and specialist services. By providing innovative, integrated solutions, we’re working to break the boundaries of system integration & interoperability.

    We also specialise in supplying IT infrastructure, software and engineering services, and through our technical support teams we have the skills and knowledge to enhance your IT systems.

    Patient (www.patient.info) is the UK’s leading health website. Designed to help patients play a key role in their own care, it provides access to clinically authored health information leaflets, videos, health check and assessment tools and patient forums.

    TRUNCATED FOR DEMO',
        'www.emishealth.com',
        @now,
        @emptyGuid),
    (
        '99998',
        'NotTPP',
        'NotThe Phoenix Partnership',
        'TPP is a digital health company, committed to delivering world-class healthcare software around the world. Its EHR product, SystmOne, is used by over 7,000 NHS organisations in over 25 different care settings. This includes significant deployments in Acute Hospitals, Emergency Departments, Mental Health services, Social Care services and General Practice. In recent years, TPP has increased its international presence, with live deployments in China and across the Middle East.',
        'https://www.tpp-uk.com/',
        @now,
        @emptyGuid);
END;
GO
