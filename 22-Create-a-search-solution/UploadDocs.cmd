@echo off
SETLOCAL ENABLEDELAYEDEXPANSION

rem Set values for your storage account
set subscription_id=YOUR_SUBSCRIPTION_ID
set azure_storage_account=YOUR_AZURE_STORAGE_ACCOUNT_NAME
set azure_storage_key=YOUR_AZURE_STORAGE_KEY


echo Creating container...
call az storage container create --account-name !azure_storage_account! --subscription !subscription_id! --name margies --auth-mode key --account-key !azure_storage_key! --output none

echo Uploading files...
call az storage blob upload-batch -d margies -s data --account-name !azure_storage_account! --auth-mode key --account-key !azure_storage_key!  --output none
