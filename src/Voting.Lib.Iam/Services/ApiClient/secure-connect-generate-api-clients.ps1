param(
    [string[]] $ClientNames = @("Permission", "Identity")
)

function Start-ReplaceTokenInFiles {
    param(
        [string] $ClientName
    )

    $ClientNameLower = $ClientName.ToLower();

    dotnet Abraxas.Feature.ApiClient.Generator `
        --language "CSharp" `
        --injectHttpClient "true" `
        --useBaseUrl "false" `
        --useHttpClientCreationMethodOption "false" `
        --swaggerfile "secure-connect-${ClientNameLower}.json" `
        --classname "SecureConnect${ClientName}ServiceClient" `
        --targetfile "SecureConnect${ClientName}ServiceClient.g.cs" `
        --namespace "Voting.Lib.Iam.Services.ApiClient.${ClientName}" `
        --excludedParams "x-vrsg-app,x-vrsg-ecm-context,x-vrsg-ecm-client";
}

Set-Location $PSScriptRoot

Write-Host "you may need to update the swagger files before running this script"
Write-Host "you can download them here:"
Write-Host "https://gitlab.abraxas-tools.ch/secure/apps/secure-connect/identity/-/raw/master/identityAPI/identity.swagger.json" -ForegroundColor Magenta
Write-Host "https://gitlab.abraxas-tools.ch/secure/apps/secure-connect/permission/-/raw/master/permissionAPI/permission.swagger.json" -ForegroundColor Magenta

Write-Host "Restoring tools..." -ForegroundColor Green

dotnet tool restore

Write-Host "Generating clients..." -ForegroundColor Green

foreach ($clientName in $ClientNames) {
    Start-ReplaceTokenInFiles -ClientName  $clientName
}

Write-Host "Finished generation!" -ForegroundColor Green
