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
- .NET Core Version 2.2
- Docker
- Data Model repository

> Before you begin please install <b>Docker</b> on your machine.
> Also download and store the Buying Catalogue Data Model repository along side this repository.

<p>

To run the application in a container in development mode, run the following script:

```
Launch Development Environment.ps1
```

You can now access the API in your browser at 'http://localhost:8080/swagger/index.html'

To stop the application running in a container and to delete all the  associated resources run the command:

```
docker-compose -f ".\docker-compose.yml" -f ".\docker-compose.development.yml" down -v --rmi "all"
```

</p>

# Integration Tests

Integration Tests and Persistence Tests run against Docker images of service and database. These must be re-created before running tests.
<br/>
Alternatively use the supplied powershell scripts "Run Integration Tests.ps1" and "Run Code coverage.ps1" 

## Before running such tests in Visual Studio
```
dotnet publish "src\NHSD.BuyingCatalogue.API\NHSD.BuyingCatalogue.API.csproj" --configuration Release --output "out"
docker-compose -f "docker-compose.yml" -f "docker-compose.integration.yml" up -d
```
(Or run "Launch Integration Environment.ps1")

## After running such tests in Visual Studio

Run "Tear Down Integration Environment.ps1"

## Integration DB docker image
In order to speed up the API Integration test execution, a docker image which contains all the data needed has been build. 
This docker image needs to be built locally before running the API Integration tests. It only needs to be built once, and then updated every time the DataModel changes.
To build / update the image run `setup-integration-db` script either in Powershell or Bash

## Running the Script
| CLI | Command |
|---------------|--------------------|
|`bash` | `bash setup-integration-db.sh` |
| `PowerShell` | `.\setup-integration-db.ps1` |

## Troubleshooting
`./integration-entrypoint.sh: line 2: $'\r': command not found` during the image build - run `dos2unix` on the integration-entrypoint.sh script

## Error: "Start Buying Catalogue API failed, could not get a successful health status from 'http://localhost:8080/health/live' after trying for '01:00'"

Have you remembered to run "Launch Integration Environment.ps1" :) ?