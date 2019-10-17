
Write-Host "Check the report generator is installed"

$dotnetToolOutput = dotnet tool list -g | Select-String -pattern "dotnet-reportgenerator-globaltool" -SimpleMatch

if ([string]::IsNullOrEmpty($dotnetToolOutput)) {
    Write-Warning "Report generator could not be found"
    dotnet tool install -g dotnet-reportgenerator-globaltool
}
else {
    Write-Host "Report generator is already installed"
}

Write-Host ""
Write-Host "Run code coverage tests"
Write-Host "----- Code Coverage --------"

dotnet test "tests\NHSD.BuyingCatalogue.Application.UnitTests\NHSD.BuyingCatalogue.Application.UnitTests.csproj" --no-build --nologo /p:CollectCoverage=true /p:CoverletOutput=../../OpenCover/
dotnet test "tests\NHSD.BuyingCatalogue.Domain.Tests\NHSD.BuyingCatalogue.Domain.Tests.csproj" --no-build --nologo /p:CollectCoverage=true /p:CoverletOutput=../../OpenCover/ /p:MergeWith="../../OpenCover/coverage.json"
dotnet test "tests\NHSD.BuyingCatalogue.Persistence.DatabaseTests\NHSD.BuyingCatalogue.Persistence.DatabaseTests.csproj" --no-build --nologo /p:CollectCoverage=true /p:CoverletOutput=../../OpenCover/ /p:MergeWith="../../OpenCover/coverage.json" /p:CoverletOutputFormat="opencover"

Write-Host "----- Code Coverage --------"
Write-Host ""
Write-Host "Generate code coverage reports"
Write-Host "----- Report Output--------"

reportgenerator "-reports:OpenCover\coverage.opencover.xml" "-targetdir:OpenCover\Report\"

Write-Host "----- Report Output--------"