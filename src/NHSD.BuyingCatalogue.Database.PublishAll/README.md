# Data Migration Package

This is a SSIS package that migrates the following tables from one buying catalogue database to another.

* CatalogueItem
* Solution
* AssociatedService
* AdditionalService
* FrameworkSolutions
* MarketingContact
* SolutionCapability
* SolutionEpic
* SupplierServiceAssociation

## Working with the package in Visual Studio

### Pre-requisites

* Visual Studio 2019
* Visual Studio SQL Server Data Tools

You will need to install SQL Server Data Tools as per the instructions [here](https://docs.microsoft.com/en-us/sql/ssdt/download-sql-server-data-tools-ssdt?view=sql-server-ver15).

Note that you may need to modify your Visual Studio installation (use the Visual Studio Installer) to include the **Data storage and processing** workload.

*Integration Services* is then enabled by installing the Visual Studio extension **SQL Server Integration Services Projects**.

## Running the package in Visual Studio

### Preparation

1. Obtain credentials for both source and destination databases and ensure you have access.

2. Open the package in Visual Studio and amend the parameters of the two connection managers as appropriate.
    1. Do this by editing the relevant parameters in `Project.params`.
    2. Temporarily set `Sensitive` to `False` for `Password`.
    3. Test each connection.

3. If running the package against a production environment, seek a peer to review: specifically to validate that the **correct environments** are being accessed and that **source and destination are the right way round**.

#### Source and Destination

The source and destination databases are currently set as follows.

* Source: localhost, buyingcatalogue_source
* Destination: localhost, buyingcatalogue_dest

**This is deliberate. Please reset the source and destination connection information (in `Project.params`) before running the package**.

### Execution

1. If running against production make sure the connection details have been reviewed.
2. Right-click on the package in solution explorer and select *Execute Package*.
3. Verify that data transfer has taken place and the destination database has been updated.
4. Use a git client to revert your local changes.

**DO NOT COMMIT PRODUCTION CREDENTIALS TO GIT.**

## Deploying the package to Azure

### Prerequisites

The Azure-SSIS Integration Runtime has been provisioned  in Azure Data Factory as described [here](https://docs.microsoft.com/en-us/sql/integration-services/lift-shift/ssis-azure-lift-shift-ssis-packages-overview?view=sql-server-ver15#provision-ssis-on-azure).

### Steps

1. Revert any changes to `Project.params`.
2. Right-click the SSIS project in Solution Explorer and select *Deploy*.
3. Follow the wizard, selecting *SSIS in Azure Data Factory* when prompted for the deployment target.
4. Enter the server name, and suitable SQL Server Authentication credentials.
5. Select *Connect*.
6. Click *Browse* to browse the integration services catalogue and select the folder that contains the previous version of the package, if pre-existing. Otherwise, create a new folder.
7. Review the settings and deploy.
8. If this is the first deployment, open SSMS to create a SSIS environment and configure the package.

When overwriting an older version of the package the environment variables will be preserved. The only time an existing environment will need to be edited is when deleting, renaming or adding a project or package parameter, or when one of the existing values needs updating.

## Running the package in Azure

1. In SSMS, connect to the relevant instance.
2. Expand *Integration Service Catalogs*, *SSISDB*, the folder that contains the project, and *Projects*.
3. Right-click on the project and select *Execute Package*.
4. In the *Execute Package* window, check the `Environment` check box and select the appropriate environment.
5. Click *OK*.
6. Open the execution report, if desired. The report will need to be refreshed manually using the refresh icon or *F5*.
