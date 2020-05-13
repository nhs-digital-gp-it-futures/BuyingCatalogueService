# BuyingCatalogueService - Service architecture for the NHS Digital Buying Catalogue

.Net Core application, based on a service architecture.

## IMPORTANT NOTES

**You can use either the latest version of Visual Studio or .NET CLI for Windows, Mac and Linux**.

### Architecture overview

This application uses **.NET core** to provide an API capable of running on Linux or Windows.

> For the frontend web application see <https://github.com/nhs-digital-gp-it-futures/public-browse>

### Overview of the application code

This repo consists of one service to provide multiple resource endpoints for the NHS Digitial Buying Catalogue application using **.NET Core** and **Docker**.

The application is broken down into the following project libraries:

- API – defines and exposes the available Buying Catalogue resources to the frontend
- Application – provides the different use cases and functionality for the Buying Catalogue
- Domain – defines the entities and business logic for the application
- Persistence Layer – provides access and storage for the Buying Catalogue data

## Setting up your development environment for the Buying Catalogue

### Requirements

- .NET Core Version 3.0
- Docker

> Before you begin please install **.NET Core 3.0** & **Docker** on your machine.

## Running the API

### On a Windows Box

*All scripts are meant to be run in PowerShell from within this directory.*

To run the application in a container in development environment, run the following script:

```powershell
 & '.\Launch Environment.ps1'
```

You can now access the API in your browser at <http://localhost:8080/swagger/index.html>.

To stop the application running in a container and to delete the associated images, run the command:

```powershell
& '.\Tear Down Environment.ps1'
```

#### Extra flags

- Attached mode – directs docker-compose output to your CLI

    ```powershell
    & '.\Launch Environment.ps1' -a
    ```

- Clean mode – removes all images, resources and networks

    ```powershell
    & '.\Tear Down Environment.ps1' -c
    ```

- Quiet mode – doesn't do a `docker ps -a` after finishing

    ```powershell
    & '.\Launch Environment.ps1' -q
    ```

    or

    ```powershell
    & '.\Tear Down Environment.ps1' -q
    ```

### On a Linux/Mac Box

*All scripts are meant to be run in bash from within this directory.*

To run the application in a container in development environment, run the following script:

```bash
bash launch_environment.sh
```

You can now access the API in your browser at <http://localhost:8080/swagger/index.html>.

To stop the application running in a container and to delete the associated images:

```bash
bash tear_down_environment.sh
```

#### Extra flags

- Attached mode – directs docker-compose output to your CLI

    ```bash
    bash launch_environment.sh -a
    ```

- Clean mode – removes all images, resources and networks

    ```bash
    bash tear_down_environment.sh -c
    ```

- Quiet mode – doesn't do a `docker ps -a` after finishing

    ```bash
    bash launch_environment.sh -q
    ```

    or

    ```bash
    bash tear_down_environment.sh -q
    ```

## Local Debugging

Launch the environment as described above in [Running the API](#running-the-api).

Secondly, copy and paste the connection string into your [User Secrets file](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=windows#how-the-secret-manager-tool-works). Your file should have this format:

```json
{
  "ConnectionStrings": {
    "BuyingCatalogue": "Data Source=.,1433;Initial Catalog=buyingcatalogue;MultipleActiveResultSets=True;User Id=NHSD;Password=DisruptTheMarket1!"
  }
}
```

Lastly, run the API – either through your favourite IDE or using your favourite shell. From the solution root directory run

```shell
dotnet run --project ./src/NHSD.BuyingCatalogue.API
```

## Integration Tests

Integration tests and persistence Tests run against Docker containers of the service, a [mock of the document API](tests/NHSD.BuyingCatalogue.Documents.API.WireMock/README.md), and the database.

### On a Windows Box

#### Before running tests

```powershell
'.\Launch Environment.ps1' i
```

#### Running tests

```powershell
& '.\Run Component Tests.ps1'
```

or via the test runner in your favourite IDE.

#### After running tests

```powershell
& '.\Tear Down Environment.ps1' i
```

#### Extra flags

- Attached mode – directs docker-compose output to your CLI

    ```powershell
    & '.\Launch Environment.ps1' i -a
    ```

- Quiet mode – doesn't do a `docker ps -a` after finishing

    ```powershell
    & '.\Launch Environment.ps1' i -q
    ```

    or 

    ```powershell
    & '.\Tear Down Environment.ps1' i -q
    ```

### On a Linux/Mac Box

#### Before running tests

```bash
bash launch_environment.sh i
```

#### Running tests

```bash
bash run_component_tests.sh
```

or via the test runner in your favourite IDE.

#### After running tests

```bash
bash tear_down_environment.sh i
```

#### Extra flags

- Attached mode – directs docker-compose output to your CLI

    ```bash
    bash launch_environment.sh i -a
    ```

- Quiet mode – doesn't do a `docker ps -a` after finishing

    ```bash
    bash launch_environment.sh i -q
    ```

    or

    ```bash
    bash tear_down_environment.sh i -q
    ```

## Troubleshooting

### SQL Server is running but there is no database

The `dacpac` deployment takes a few seconds to initialize and complete so it is not unusual for there to be a slight delay between SQL server initializing and the database being ready for use; upon completion `Database setup complete` is logged to the console.

### SQL Server is running but the database is not deployed after a couple of minutes

1. View the logs of the db_deploy container.
2. If the logs contain `standard_init_linux.go:211: exec user process caused "no such file or directory"`, then run `dos2unix` on the src/NHSD.BuyingCatalogue.Database.Deployment/entrypoint.sh script.

### "Start Buying Catalogue API failed, could not get a successful health status from 'http://localhost:8080/health/live' after trying for '01:00'"

Have you remembered to run `Launch Environment.ps1 i` :) ?
