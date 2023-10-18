---
lab:
    title: 'Monitor Azure AI Services'
    module: 'Module 2 - Developing AI Apps with Azure AI Services'
---

# Monitor Azure AI Services

Azure AI Services can be a critical part of an overall application infrastructure. It's important to be able to monitor activity and get alerted to issues that may need attention.

## Clone the repository for this course in Cloud Shell

Open up a new browser tab to work with Cloud Shell. If you haven't cloned this repository to Cloud Shell recently, follow the steps below to make sure you have the most recent version. Otherwise, open Cloud Shell and navigate to your clone.

1. In the [Azure portal](https://portal.azure.com?azure-portal=true), select the **[>_]** (*Cloud Shell*) button at the top of the page to the right of the search box. A Cloud Shell pane will open at the bottom of the portal.

    ![Screenshot of starting Cloud Shell by clicking on the icon to the right of the top search box.](images/cloudshell-launch-portal.png#lightbox)

2. The first time you open the Cloud Shell, you may be prompted to choose the type of shell you want to use (*Bash* or *PowerShell*). Select **Bash**. If you don't see this option, skip the step.  

3. If you're prompted to create storage for your Cloud Shell, ensure your subscription is specified and select **Create storage**. Then wait a minute or so for the storage to be created.

4. Make sure the type of shell indicated on the top left of the Cloud Shell pane is switched to *Bash*. If it's *PowerShell*, switch to *Bash* by using the drop-down menu.

5. Once the terminal starts, enter the following command to download the sample application and save it to a folder called `labs`.

    ```bash
   git clone https://github.com/MicrosoftLearning/AI-102-AIEngineer labs
    ```
  
6. The files are downloaded to a folder named **labs**. Navigate to the lab files for this exercise using the following command.

    ```bash
   cd labs/03-monitor
    ```

Use the following command to open the lab files in the built-in code editor.

```bash
code .
```

## Provision an Azure AI Services resource

If you don't already have one in your subscription, you'll need to provision an **Azure AI Services** resource.

1. Open the Azure portal at `https://portal.azure.com`, and sign in using the Microsoft account associated with your Azure subscription.
2. In the top search bar, search for *Azure AI services*, select **Azure AI Services**, and create an Azure AI services multi-service account resource with the following settings:
    - **Subscription**: *Your Azure subscription*
    - **Resource group**: *Choose or create a resource group (if you are using a restricted subscription, you may not have permission to create a new resource group - use the one provided)*
    - **Region**: *Choose any available region*
    - **Name**: *Enter a unique name*
    - **Pricing tier**: Standard S0
3. Select any required checkboxes and create the resource.
4. Wait for deployment to complete, and then view the deployment details.
5. When the resource has been deployed, go to it and view its **Keys and Endpoint** page. Make a note of the endpoint URI - you will need it later.

## Configure an alert

Let's start monitoring by defining an alert rule so you can detect activity in your Azure AI services resource.

1. In the Azure portal, go to your Azure AI services resource and view its **Alerts** page (in the **Monitoring** section).
2. Select **+ Create** dropdown, and select **Alert rule**
3. In the **Create an alert rule** page, under **Scope**, verify that the your Azure AI services resource is listed. (Close **Select a signal** pane if open)
4. Select **Condition** tab, and select on the **See all signals** link to show the **Select a signal** pane that appears on the right, where you can select a signal type to monitor.
5. In the **Signal type** list, scroll down to the **Activity Log** section, and then select **List Keys (Cognitive Services API Account)**. Then select **Apply**.
6. Review the activity over the past 6 hours.
7. Select the **Actions** tab. Note that you can specify an *action group*. This enables you to configure automated actions when an alert is fired - for example, sending an email notification. We won't do that in this exercise; but it can be useful to do this in a production environment.
8. In the **Details** tab, set the **Alert rule name** to **Key List Alert**.
9. Select **Review + create**. 
10. Review the configuration for the alert. Select **Create** and wait for the alert rule to be created.
11. Now you can use the following command to get the list of Azure AI services keys, replacing *&lt;resourceName&gt;* with the name of your Azure AI services resource, and *&lt;resourceGroup&gt;* with the name of the resource group in which you created it.

    ```
    az cognitiveservices account keys list --name <resourceName> --resource-group <resourceGroup>
    ```

    The command returns a list of the keys for your Azure AI services resource.

12. Switch back to the browser containing the Azure portal, and refresh your **Alerts page**. You should see a **Sev 4** alert listed in the table (if not, wait up to five minutes and refresh again).
13. Select the alert to see its details.

## Visualize a metric

As well as defining alerts, you can view metrics for your Azure AI services resource to monitor its utilization.

1. In the Azure portal, in the page for your Azure AI services resource, select **Metrics** (in the **Monitoring** section).
2. If there is no existing chart, select **+ New chart**. Then in the **Metric** list, review the possible metrics you can visualize and select **Total Calls**.
3. In the **Aggregation** list, select **Count**.  This will enable you to monitor the total calls to you Azure AI Service resource; which is useful in determining how much the service is being used over a period of time.
4. To generate some requests to your Azure AI service, you will use **curl** - a command line tool for HTTP requests. In your editor, open **rest-test.sh** and edit the **curl** command it contains (shown below), replacing *&lt;yourEndpoint&gt;* and *&lt;yourKey&gt;* with your endpoint URI and **Key1** key to use the Text Analytics API in your Azure AI services resource.

    ```
    curl -X POST "<yourEndpoint>/text/analytics/v3.1/languages?" -H "Content-Type: application/json" -H "Ocp-Apim-Subscription-Key: <yourKey>" --data-ascii "{'documents':           [{'id':1,'text':'hello'}]}"
    ```

5. Save your changes by selecting **CTRL+S**, and then run the following command:

    ```
    sh rest-test.sh
    ```

    The command returns a JSON document containing information about the language detected in the input data (which should be English).

6. Re-run the **rest-test** command multiple times to generate some call activity (you can use the **^** key to cycle through previous commands).
7. Return to the **Metrics** page in the Azure portal and refresh the **Total Calls** count chart. It may take a few minutes for the calls you made using *curl* to be reflected in the chart - keep refreshing the chart until it updates to include them.

## More information

One of the options for monitoring Azure AI services is to use *diagnostic logging*. Once enabled, diagnostic logging captures rich information about your Azure AI services resource usage, and can be a useful monitoring and debugging tool. It can take over an hour after setting up diagnostic logging to generate any information, which is why we haven't explored it in this exercise; but you can learn more about it in the [Azure AI Services documentation](https://docs.microsoft.com/azure/cognitive-services/diagnostic-logging).
