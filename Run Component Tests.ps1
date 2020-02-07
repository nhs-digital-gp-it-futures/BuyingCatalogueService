
& ".\Launch Environment.ps1" -env i -q
dotnet test "tests\NHSD.BuyingCatalogue.API.IntegrationTests\NHSD.BuyingCatalogue.API.IntegrationTests.csproj" -v n
& ".\Tear Down Environment.ps1" -env i -q