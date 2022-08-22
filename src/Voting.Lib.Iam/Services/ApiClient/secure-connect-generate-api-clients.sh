#!/usr/bin/env bash

set -Eeuo pipefail

echo "you may need to update the swagger files before running this script"
echo "you can download them here:"
echo "https://gitlab.abraxas-tools.ch/secure/apps/secure-connect/identity/-/raw/master/identityAPI/identity.swagger.json"
echo "https://gitlab.abraxas-tools.ch/secure/apps/secure-connect/permission/-/raw/master/permissionAPI/permission.swagger.json"

dotnet tool restore > /dev/null

echo "generating..."

gen() {
  lower_name="$(echo $1 | tr '[:upper:]' '[:lower:]')"
  dotnet tool run Abraxas.Feature.ApiClient.Generator \
    --language "CSharp" \
    --injectHttpClient "true" \
    --useBaseUrl "false" \
    --useHttpClientCreationMethodOption "false" \
    --swaggerfile "secure-connect-${lower_name}.json" \
    --classname "SecureConnect$1ServiceClient" \
    --targetfile "SecureConnect$1ServiceClient.g.cs" \
    --namespace "Voting.Lib.Iam.Services.ApiClient.$1" \
    --excludedParams "x-vrsg-app,x-vrsg-ecm-context,x-vrsg-ecm-client" > /dev/null
}

gen "Permission"
gen "Identity"

echo "generated."
