---
lab:
    title: 'Get Started with Azure AI Services'
    module: 'Module 2 - Developing AI Apps with Azure AI Services'
---

# Get Started with Azure AI Services

In this exercise, you'll get started with Azure AI Services by creating an **Azure AI Services** resource in your Azure subscription and using it from a client application. The goal of the exercise is not to gain expertise in any particular service, but rather to become familiar with a general pattern for provisioning and working with Azure AI services as a developer.

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
   cd labs/01-getting-started
    ```

Use the following command to open the lab files in the built-in code editor.

```bash
code .
```

Code for both C# and Python has been provided. Navigate to the folder of your preferred language.

## Provision an Azure AI Services resource

Azure AI Services are cloud-based services that encapsulate artificial intelligence capabilities you can incorporate into your applications. You can provision individual Azure AI services resources for specific APIs (for example, **Language** or **Vision**), or you can provision a single **Azure AI Services** resource that provides access to multiple Azure AI services APIs through a single endpoint and key. In this case, you'll use a single **Azure AI Services** resource.

1. Open the Azure portal at `https://portal.azure.com`, and sign in using the Microsoft account associated with your Azure subscription.
2. In the top search bar, search for *Azure AI services*, select **Azure AI Services**, and create an Azure AI services multi-service account resource with the following settings:
    - **Subscription**: *Your Azure subscription*
    - **Resource group**: *Choose or create a resource group (if you are using a restricted subscription, you may not have permission to create a new resource group - use the one provided)*
    - **Region**: *Choose any available region*
    - **Name**: *Enter a unique name*
    - **Pricing tier**: Standard S0
3. Select the required checkboxes and create the resource.
4. Wait for deployment to complete, and then view the deployment details.
5. Go to the resource and view its **Keys and Endpoint** page. This page contains the information that you will need to connect to your resource and use it from applications you develop. Specifically:
    - An HTTP *endpoint* to which client applications can send requests.
    - Two *keys* that can be used for authentication (client applications can use either key to authenticate).
    - The *location* where the resource is hosted. This is required for requests to some (but not all) APIs.

## Use a REST Interface

The Azure AI services APIs are REST-based, so you can consume them by submitting JSON requests over HTTP. In this example, you'll explore a console application that uses the **Language** REST API to perform language detection; but the basic principle is the same for all of the APIs supported by the Azure AI Services resource.

> **Note**: In this exercise, you can choose to use the REST API from either **C#** or **Python**. In the steps below, perform the actions appropriate for your preferred language.

1. In your editor, expand the **C-Sharp** or **Python** folder depending on your language preference.
2. View the contents of the **rest-client** folder, and note that it contains a file for configuration settings:
    - **C#**: appsettings.json
    - **Python**: .env

    Open the configuration file and update the configuration values it contains to reflect the **endpoint** and an authentication **key** for your Azure AI services resource. Save your changes by pressing **CTRL+S**.
3. Note that the **rest-client** folder contains a code file for the client application:

    - **C#**: Program.cs
    - **Python**: rest-client.py

    Open the code file and review the code it contains, noting the following details:
    - Various namespaces are imported to enable HTTP communication
    - Code in the **Main** function retrieves the endpoint and key for your Azure AI services resource - these will be used to send REST requests to the Text Analytics service.
    - The program accepts user input, and uses the **GetLanguage** function to call the Text Analytics language detection REST API for your Azure AI services endpoint to detect the language of the text that was entered.
    - The request sent to the API consists of a JSON object containing the input data - in this case, a collection of **document** objects, each of which has an **id** and **text**.
    - The key for your service is included in the request header to authenticate your client application.
    - The response from the service is a JSON object, which the client application can parse.
4. In your terminal run the following language-specific commands to open the **rest-client** folder and  run the program:

    **C#**

    ```
    cd C-Sharp/rest-client
    dotnet run
    ```

    **Python**

    ```
    cd Python/rest-client
    pip install python-dotenv
    python rest-client.py
    ```

5. When prompted, enter some text and review the language that is detected by the service, which is returned in the JSON response. For example, try entering "Hello", "Bonjour", and "Gracias".
6. When you have finished testing the application, enter "quit" to stop the program.

## Use an SDK

You can write code that consumes Azure AI services REST APIs directly, but there are software development kits (SDKs) for many popular programming languages, including Microsoft C#, Python, and Node.js. Using an SDK can greatly simplify development of applications that consume Azure AI services.

1. In the editor, expand the **sdk-client** folder under the **C-Sharp** or **Python** folder, depending on your language preference. Then run `cd ../sdk-client` to change into the relevant **sdk-client** folder.

2. Install the Text Analytics SDK package by running the appropriate command for your language preference:

    **C#**

    ```
    dotnet add package Azure.AI.TextAnalytics --version 5.3.0
    ```

    **Python**

    ```
    pip install azure-ai-textanalytics==5.3.0
    ```

3. View the contents of the **sdk-client** folder, and note that it contains a file for configuration settings:
    - **C#**: appsettings.json
    - **Python**: .env

    Open the configuration file and update the configuration values it contains to reflect the **endpoint** and an authentication **key** for your Azure AI services resource. Save your changes.
    
4. Note that the **sdk-client** folder contains a code file for the client application:

    - **C#**: Program.cs
    - **Python**: sdk-client.py

    Open the code file and review the code it contains, noting the following details:
    - The namespace for the SDK you installed is imported
    - Code in the **Main** function retrieves the endpoint and key for your Azure AI services resource - these will be used with the SDK to create a client for the Text Analytics service.
    - The **GetLanguage** function uses the SDK to create a client for the service, and then uses the client to detect the language of the text that was entered.

5. Return to the terminal, and enter for the **sdk-client** folder, and enter the following command to run the program:

    **C#**

    ```
    dotnet run
    ```

    **Python**

    ```
    python sdk-client.py
    ```

6. When prompted, enter some text and review the language that is detected by the service. For example, try entering "Goodbye", "Au revoir", and "Hasta la vista".
7. When you have finished testing the application, enter "quit" to stop the program.

> **Note**: Some languages that require Unicode character sets may not be recognized in this simple console application.

## More information

For more information about Azure AI Services, see the [Azure AI Services documentation](https://docs.microsoft.com/azure/cognitive-services/what-are-cognitive-services).
