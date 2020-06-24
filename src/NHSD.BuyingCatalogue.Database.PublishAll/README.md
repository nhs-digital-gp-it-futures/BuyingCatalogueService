# Data Migration Package

This is a SSIS package migrates the following tables from one buying catalogue database to another.

* CatalogueItem
* Solution
* AssociatedService
* AdditionalService
* FrameworkSolutions
* MarketingContact
* SolutionCapability
* SolutionEpic
* SupplierServiceAssociation

## Pre-requisites

* Visual Studio 2019
* Visual Studio SQL Server Data Tools

You will need to install SQL Server Data Tools as per the instructions [here](https://docs.microsoft.com/en-us/sql/ssdt/download-sql-server-data-tools-ssdt?view=sql-server-ver15).

Note that you may need to modify your Visual Studio installation (use Visual Studio Installer) to include the **Data storage and processing** workload.

*Integration Services* is then enabled by installing the Visual Studio extension **SQL Server Integration Services Projects**.

## Source and Destination

Source and Destination are currently set as follows.

* Source: localhost, buyingcatalogue_source
* Destination: localhost, buyingcatalogue_dest

**This is deliberate, please reset the source and destination connection information (in `Project.params`) before running the script**.

## Running the script

### Preparation

1. Obtain credentials for both source and destination databases and ensure you have access.

1. Open the package in Visual Studio and amend the connection details of the two connection managers as appropriate.
    1. Do this by editing the relevant parameters in `Project.params`.
    2. Temporarily set `Sensitive` to `False` for `Password`.
    3. Test each connection.

1. Seek a peer to review: specifically to validate that the **correct environments** are being accessed and that **source and destination are the right way round**.

### Execution

Once your reviewer grants approval:

1. Run the package in Visual Studio 2019.
2. Verify that data transfer has taken place and the destination database has been updated.
3. Use a git client to revert your local changes.

**DO NOT COMMIT PRODUCTION CREDENTIALS TO GIT.**
