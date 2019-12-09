#!/bin/bash

set -e
./launch_environment.sh i &&
dotnet test "tests/NHSD.BuyingCatalogue.API.IntegrationTests/NHSD.BuyingCatalogue.API.IntegrationTests.csproj" -v n &&
./tear_down_environment.sh i