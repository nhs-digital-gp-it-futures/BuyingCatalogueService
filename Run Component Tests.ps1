
& ".\Launch Integration Environment.ps1"
dotnet test "tests\NHSD.BuyingCatalogue.API.IntegrationTests\NHSD.BuyingCatalogue.API.IntegrationTests.csproj" -v n
& ".\Tear Down Integration Environment.ps1"