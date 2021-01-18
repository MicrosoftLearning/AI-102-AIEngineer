@echo off

rem Set values for your Language Understanding app
set app_id=YOUR_APP_ID
set endpoint=YOUR_ENDPOINT
set key=YOUR_KEY

rem Get parameter and encode spaces for URL
set input=%1
set query=%input: =+%

rem Use cURL to call the REST API
curl -X GET "%endpoint%/luis/prediction/v3.0/apps/%app_id%/slots/production/predict?subscription-key=%key%&log=true&query=%query%"