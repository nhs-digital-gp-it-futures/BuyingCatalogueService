IF NOT EXISTS 
    (SELECT name  
     FROM master.sys.server_principals
     WHERE name = 'NHSD-BAPI')
BEGIN
    CREATE LOGIN [NHSD-BAPI] WITH PASSWORD = '$(NHSD_PASSWORD)'
END
