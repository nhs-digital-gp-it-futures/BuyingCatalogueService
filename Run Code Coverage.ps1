# Get all directories in "tests" directory
[System.Collections.ArrayList]$directoriesToTest = $(Get-ChildItem "tests" -Directory)
# Declare which ones to exlude in code coverage report
[System.Collections.ArrayList]$exludedDirectories = "NHSD.BuyingCatalogue.Testing.Data","NHSD.BuyingCatalogue.Testing.Tools","NHSD.BuyingCatalogue.API.IntegrationTests"

# convert the objects from DirectoryInfo to String type.
$directoriesToTest = $directoriesToTest.ForEach({$_.ToString()})

# remove excluded directories
foreach ($dir in $exludedDirectories) {
    $directoriesToTest.Remove($dir)
}

function CheckRequirements() {
    Write-Host "Check the report generator is installed" -ForegroundColor Yellow

    $dotnetToolOutput = dotnet tool list -g | Select-String -pattern "dotnet-reportgenerator-globaltool" -SimpleMatch

    if ([string]::IsNullOrEmpty($dotnetToolOutput)) {
        Write-Warning "Report generator could not be found, installing it" -ForegroundColor Yellow
        dotnet tool install -g dotnet-reportgenerator-globaltool
    }
    else {
        Write-Host "Report generator is already installed" -ForegroundColor Green
    }
}

function ClearOutputFolder() {
    if(Test-Path "OpenCover") {    
        Remove-Item 'OpenCover' -Recurse -Force
    }
}

# makes sure the projects are built in debug mode, then runs the tests and collects code coverage reports
function RunCodeCoverageTests() { 

    Write-Host ""
    Write-Host "Run code coverage tests" -ForegroundColor Yellow
    Write-Host "----- Code Coverage --------" -ForegroundColor Yellow

    For($i = 0; $i -lt $directoriesToTest.Count; $i++)
    {
        cd "tests\$($directoriesToTest[$i])"

        if( -NOT (Test-Path "bin\Debug")) {    
            dotnet build -c "Debug"
        }

        RunTests -dir "$($directoriesToTest[$i])"
        cd "..\.."
        Write-Progress -Activity "Running tests" `
        -Status "$($i+1) out of $($directoriesToTest.Count) finished"`

    }
}

function RunTests([string]$dir){

    $RunTestCommand= 'dotnet test --no-build --nologo /p:CollectCoverage=true /p:CoverletOutput=../../OpenCover/ /p:Exclude=[*.Testing.*]* /p:MergeWith="../../OpenCover/coverage.json"'
    $OutputParam='';
    if ($dir -eq $($directoriesToTest[$directoriesToTest.Count-1])) {
        $OutputParam='/p:CoverletOutputFormat="opencover"'   
    }
    Invoke-Expression "$RunTestCommand $OutputParam"
}

function GenerateCodeCoverageReport() {

    Write-Host "----- Code Coverage --------" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Generate code coverage reports" -ForegroundColor Yellow
    Write-Host "----- Report Output--------" -ForegroundColor Yellow

    reportgenerator "-reports:OpenCover\coverage.opencover.xml" "-targetdir:OpenCover\Report\"

    Write-Host "----- Report Output--------" -ForegroundColor Yellow
}

CheckRequirements

& ".\Launch Environment.ps1" -env i

ClearOutputFolder

RunCodeCoverageTests

GenerateCodeCoverageReport

& ".\Tear Down Environment.ps1" -env i