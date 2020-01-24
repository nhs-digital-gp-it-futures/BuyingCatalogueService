param (
    [int]$port = 1430,
    [string]$username = "NHSD",
    [string]$password = "DisruptTheMarket1!"
)

$db_name="buyingcataloguegpit"

function containsSymbol() {
    $SymbolArray = "!@#$^&*()-+={}[]\|/:;<>?,.".ToCharArray()
    foreach ($char in $password.ToCharArray()){
        if ($char -in $SymbolArray) {
        return 1
        }
    }
}

function validate() {
    $message = "The password does not meet SQL Server password policy requirements because it is not complex enough.`nThe password must be at least 8 characters long and contain characters from three of the following four sets: Uppercase letters, Lowercase letters, Base 10 digits, and Symbols"
    $count=0

    if ($port -gt 65535) {
    Write-Host "You can't have your database on port $port chief, that's outside the range.`nPlease choose a port between 0 and 65535 and try again." -ForegroundColor Red
    exit
    }

    if ($password.Length -lt 8){
        Write-Host $message -ForegroundColor Red
        exit
    }

    if (($password -match '\d')){$count++}
    if (($password -cmatch “[A-Z]”)) {$count++}
    if (($password -cmatch “[a-z]”)) {$count++}
    if (containsSymbol -eq 1) {$count++}
    
    if ($count -lt 3) {
        Write-Host $message -ForegroundColor Red
        exit
    }
    
}

function setEnvVariables() {
    $env:NHSD_LOCAL_DB_PORT=$port
    $env:NHSD_LOCAL_DB_USERNAME=$username
    $env:NHSD_LOCAL_DB_PASSWORD=$password
    $env:NHSD_LOCAL_DB_NAME=$db_name
}

function tearDown() {
    docker rm nhsd_debug_db -f
    docker volume rm nsd_debug_volume
    docker network rm nhsd_debug_network
    docker image rm nhsd/buying-catalogue-debug-db
}

tearDown

validate 

setEnvVariables

docker-compose -f "docker/docker-compose.debug.yml" up -d

$connectionString = "Data Source=127.0.0.1,$port;Initial Catalog=$db_name;MultipleActiveResultSets=True;User Id=$username;Password=$password" 

Write-Host "`nYour connection string for BuyingCatalogue is:`n"

Write-Host "$connectionString`n" -ForegroundColor Yellow

Write-Host "Your connection string has been copied to your clipboard.`nPlease make sure to update your 'secrets.json' file before running the API" -ForegroundColor Green

Set-Clipboard -Value $connectionString

