# BuyingCatalogueService – `NHSD.BuyingCatalogue.Database.Deployment` Project

## Background

The SQL Server Database Project format (`*.sqlproj`) has not been ported to .NET Core yet, so it is only possible to build the [`NHSD.BuyingCatalogue.Database`](../NHSD.BuyingCatalogue.Database) project on a Windows machine.

The `NHSD.BuyingCatalogue.Database.Deployment` project exists to generate the `.dacpac` file used to deploy the database to the containerized SQL Server instance and as a container for database deployment artefacts, currently the Dockerfile and the [entrypoint](#entrypoint.sh) script for the deployment container.

The deployment project contains links to the SQL files defined in the [`NHSD.BuyingCatalogue.Database`](../NHSD.BuyingCatalogue.Database) project. These links ensure that the `.dacpac` file is built correctly in the DevOps pipelines. Please see the section below for any notes related to [making changes](#making-changes).

## Limitations

The SDK used to generate the `.dacpac` does not currently support include files (`:r` command) in post-deployment scripts, so post-deployment actions are currently initiated by the [entrypoint](#entrypoint.sh) script.

## Making Changes

Changes to the contents of this project will be required when adding new database artefacts (anything other than pre- or post-deployment scripts) or when making changes to the database deployment process for the development and test environments.

No changes to the deployment project should be required when amending existing scripts.

All database schema changes need to be made in the `NHSD.BuyingCatalogue.Database` project.

### Adding new database artefacts

When adding a new database artefact (other than a pre- or post-deployment script) to the database, you will need to add a link to the deployment project to ensure that the `.dacpac` generated in the DevOps pipelines remains correct.

#### Adding a linked file to the deployment project

There are two ways to add a linked file to the database deployment project.

1. Edit the project file directly. An existing link can be used as a template.
2. Using the UI in Visual Studio.
    1. Right-click on the deployment project and select `Add->Existing Item...` or hit Shift+Alt+A.
    2. Select the file(s) you've just added to the database project.
    3. Click the drop-down arrow next to `Add` and select `Add As Link`.

##### Build Action

The build action for the linked files **must** be set to `Content.`

##### Logical Directories

It is possible to reflect the directory structure of the database project in the deployment project by editing the value of the `Link` attribute of the relevant file element(s) (in the project file).

## Deployment Artefacts

### Dockerfile

The Dockerfile defines two images:

- `dacpacbuild` used to build the `.dacpac` file
- `dacfx` used to deploy the `.dacpac` to SQL Server

### Entrypoint.sh

The entrypoint script performs the following functions:

1. Checks that the SQL Server instance is available.
2. Initiates the DACPAC deployment using `sqlpackage`.
3. Executes the post-deployment script using `sqlcmd`.
4. For the integration test setup, executes an additional integration test-specific post-deployment script.

## Notes

When running the database container or viewing its logs you may notice the following output that can be ignored.

### Login Failure

One or more occurences of:

```powershell
2020-05-07 11:26:47.55 Logon       Error: 18456, Severity: 14, State: 7.
2020-05-07 11:26:47.55 Logon       Login failed for user 'sa'. Reason: An error occurred while evaluating the password. [CLIENT: 172.24.0.4]
```

This is caused by a deployment attempt occurring before SQL Server has finished initalizing.

### 'Unnamed' Database Artefacts

```powershell
nhsd_bc_integration_db_deploy                  | Creating <unnamed>...
nhsd_bc_integration_db_deploy                  | Creating <unnamed>...
nhsd_bc_integration_db_deploy                  | Creating <unnamed>...
```

This is the assignation of database roles/permissions.
