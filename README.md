

# BuyingCatalogueService - Service architecture for the NHS Digital Buying Catalogue
.Net Core application, based on a service architecture.

## IMPORTANT NOTES!
**You can use either the latest version of Visual Studio or .NET CLI for Windows, Mac and Linux**.

### Architecture overview
This application uses <b>.NET core</b> to provide an API capable of running on Linux or Windows.

> For the frontend web application see <a>https://github.com/nhs-digital-gp-it-futures/public-browse</a>.
>
> For the data model see <a>https://github.com/nhs-digital-gp-it-futures/DataModel</a>

### Overview of the application code
This repo consists of one service to provide multiple resource endpoints for the NHS Digitial Buying Catalogue application using <b>.NET Core</b> and <b>Docker</a>.

The application is broken down into the following project libraries:

- API - Defines and exposes the available Buying Catalogue resources to the frontend
- Application - Provides the different use cases and functionality for the Buying Catalogue
- Domain - Defines the entities and business logic for the application
- Persistence Layer - Provides access and storage for the Buying Catalogue data

## Setting up your development environment for the Buying Catalogue

### Requirements
- .NET Core Version 3.0
- Docker
- Data Model repository

> Before you begin please install <b>.NET Core 3.0</b> & <b>Docker</b> on your machine.
> Also download and store the Buying Catalogue Data Model repository along side this repository.


# Running the API


## On a Windows Box
*All scripts are meant to be run in PowerShell from within this directory*

To run the application in a container in development environment, run the following script:

```
 & '.\Launch Environment.ps1'
```

You can now access the API in your browser at 'http://localhost:8080/swagger/index.html'

To stop the application running in a container and to delete the associated images, run the command: 

```
& '.\Tear Down Environment.ps1'
```
To stop the application running in a container and to remove all images, resources and networks associated with it, run the command

```
& '.\Tear Down Environment.ps1' -c
```

### Extra flags
To run the application in a container in development environment, in attached mode run the following script:
```
& '.\Launch Environment.ps1' -a
```

## On a Linux/Mac Box
*All scripts are meant to be run in bash from within this directory*

To run the application in a container in development environment, run the following script:
```
bash launch_environment.sh
```
You can now access the API in your browser at 'http://localhost:8080/swagger/index.html'

To stop the application running in a container and to delete the associated images:
```
bash tear_down_environment.sh
```
To stop the application running in a container and to remove all images, resources and networks associated with it:

```
bash tear_down_environment.sh -c
```
</p>

# Integration Tests

Integration Tests and Persistence Tests run against Docker containers of service and database.
These tests rely on a docker image 'integration_db', this image must be created before running any tests. [How to is listed below.](#integration_db_setup_id)
<br/>

## On a Windows Box

### Before running tests
```
'.\Launch Environment.ps1' i
```
### Running tests
```
& '.\Run Component Tests.ps1'
```
or

Test Explorer in your favourite IDE

### After running tests
```
& '.\Tear Down Environment.ps1' i
```

### Extra flags
To run the application in a container in integration environment, in attached mode run the following script:
```
& '.\Launch Environment.ps1' i -a
```

## On a Linux/Mac Box

### Before running tests
```
bash launch_environment.sh i
```
### Running tests
```
bash run_component_tests.sh
```
or

Test Explorer in your favourite IDE

### After running tests
```
bash tear_down_environment.sh i
```

## <a name="integration_db_setup_id"></a> Integration db setup
In order to speed up the API Integration test execution, a docker image which contains all the data needed has been build.
This docker image needs to be built locally before running the API Integration tests. It only needs to be built once, and then updated every time the DataModel changes.
To build / update the image run `setup-integration-db` script either in PowerShell or Bash

### Running the Script
| CLI | Command |
|---------------|--------------------|
|`bash` | `bash setup-integration-db.sh` |
| `PowerShell` | `.\setup-integration-db.ps1` |

## Troubleshooting
- `./integration-entrypoint.sh: line 2: $'\r': command not found` during the image build
 run `dos2unix` on the integration-entrypoint.sh script

 - Error: "Start Buying Catalogue API failed, could not get a successful health status from 'http://localhost:8080/health/live' after trying for '01:00'"
Have you remembered to run `Launch Environment.ps1 i` :) ?