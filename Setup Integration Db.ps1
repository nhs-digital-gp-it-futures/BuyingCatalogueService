
$image="nhsd/buying-catalogue-integration-db:test"
$cwd=$(pwd).Path
Write-Host "[ x ] Checking whether the $image image exists..."

$ErrorActionPreference = "SilentlyContinue"; #This will hide errors
Invoke-Expression "docker image inspect $image" -ErrorVariable NoImage > $null #if we get error from this cmdlet, that means there is no image of that name.
$ErrorActionPreference = "Continue"; #Turning errors back on

if($NoImage.Count -eq 0) {
    Write-Host "[ ! ] The $image image already exists, discarding it and creating a new one ..."
    $id=$(docker image inspect "$image" --format='{{.Id}}')
    $arr=$id.split(':')
    $id_hash=$arr[1]
    docker image rm "$id_hash"
    Write-Host "[ x ] Getting rid of any dangling images..."
    docker rmi -f $(docker images -f "dangling=true" -q)
}
Write-Host "[ x ] Creating new image ..."
cd .\tests\NHSD.BuyingCatalogue.Testing.Data\IntegrationDbSetup
& .\build_image.ps1
Write-Host "[ x ] Created new image."

cd "$cwd"
