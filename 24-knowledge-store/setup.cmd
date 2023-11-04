@echo off
SETLOCAL ENABLEDELAYEDEXPANSION

rem Set values for your subscription and resource group
set subscription_id=YOUR_SUBSCRIPTION_ID
set resource_group=YOUR_RESOURCE_GROUP
set location=YOUR_LOCATION_NAME

rem Get random numbers to create unique resource names
set unique_id=!random!!random!

echo Creating storage...
call az storage account create --name ai102str!unique_id! --subscription !subscription_id! --resource-group !resource_group! --location !location! --sku Standard_LRS --encryption-services blob --default-action Allow --allow-blob-public-access true --output none

echo Uploading files...
rem Hack to get storage key
for /f "tokens=*" %%a in ( 
'az storage account keys list --subscription !subscription_id! --resource-group !resource_group! --account-name ai102str!unique_id! --query "[?keyName=='key1'].{keyName:keyName, permissions:permissions, value:value}"' 
) do ( 
set key_json=!key_json!%%a 
) 
set key_string=!key_json:[ { "keyName": "key1", "permissions": "Full", "value": "=!
set AZURE_STORAGE_KEY=!key_string:" } ]=!
call az storage container create --account-name ai102str!unique_id! --name margies --public-access blob --auth-mode key --account-key %AZURE_STORAGE_KEY% --output none
call az storage blob upload-batch -d margies -s data --account-name ai102str!unique_id! --auth-mode key --account-key %AZURE_STORAGE_KEY%  --output none

echo Creating azure ai services account...
call az cognitiveservices account create --kind CognitiveServices --location !location! --name ai102cog!unique_id! --sku S0 --subscription !subscription_id! --resource-group !resource_group! --yes --output none

echo Creating search service...
echo (If this gets stuck at '- Running ..' for more than a minute, press CTRL+C then select N)
call az search service create --name ai102srch!unique_id! --subscription !subscription_id! --resource-group !resource_group! --location !location! --sku basic --output none

echo -------------------------------------
echo Storage account: ai102str!unique_id!
call az storage account show-connection-string --subscription !subscription_id! --resource-group !resource_group! --name ai102str!unique_id!
echo ----
echo Azure AI Services account: ai102cog!unique_id!
call az cognitiveservices account keys list --subscription !subscription_id! --resource-group !resource_group! --name ai102cog!unique_id!
echo ----
echo Search Service: ai102srch
echo  Url: https://ai102srch!unique_id!.search.windows.net
echo  Admin Keys:
call az search admin-key show --subscription !subscription_id! --resource-group !resource_group! --service-name ai102srch!unique_id!
echo  Query Keys:
call az search query-key list --subscription !subscription_id! --resource-group !resource_group! --service-name ai102srch!unique_id!

