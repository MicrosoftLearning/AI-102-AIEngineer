---
lab:
    title: 'Extract Data from Forms'
    module: 'Module 11 - Reading Text in Images and Documents'
---

# Extract Data from Forms 

Suppose a company currently requires employees to manually purchase order sheets and enter the data into a database. They would like you to utilize AI services to improve the data entry process. You decide to build a machine learning model that will read the form and produce structured data that can be used to automatically update a database.

**Form Recognizer** is a cognitive service that enables users to build automated data processing software. This software can extract text, key/value pairs, and tables from form documents using optical character recognition (OCR). Form Recognizer has pre-built models for recognizing invoices, receipts, and business cards. The service also provides the capability to train custom models. In this exercise, we will focus on building custom models.

## Clone the repository for this course

If you have not already done so, you must clone the code repository for this course:

1. Start Visual Studio Code.
2. Open the palette (SHIFT+CTRL+P) and run a **Git: Clone** command to clone the `https://github.com/MicrosoftLearning/AI-102-AIEngineer` repository to a local folder (it doesn't matter which folder).
3. When the repository has been cloned, open the folder in Visual Studio Code.
4. Wait while additional files are installed to support the C# code projects in the repo.

    > **Note**: If you are prompted to add required assets to build and debug, select **Not Now**.

## Create a Form Recognizer resource

To use the Form Recognizer service, you need a Form Recognizer or Cognitive Services resource in your Azure subscription. You'll use the Azure portal to create a resource.

1.  Open the Azure portal at `https://portal.azure.com`, and sign in using the Microsoft account associated with your Azure subscription.

2. Select the **&#65291;Create a resource** button, search for *Form Recognizer*, and create a **Form Recognizer** resource with the following settings:
    - **Subscription**: *Your Azure subscription*
    - **Resource group**: *Choose or create a resource group (if you are using a restricted subscription, you may not have permission to create a new resource group - use the one provided)*
    - **Region**: *Choose any available region*
    - **Name**: *Enter a unique name*
    - **Pricing tier**: F0

    > **Note**: If you already have an F0 form recognizer service in your subscription, select **S0** for this one.

3. When the resource has been deployed, go to it and view its **Keys and Endpoint** page. You will need the **endpoint** and one of the **keys** from this page to manage access from your code later on. 

## Gather documents for training

![An image of an invoice.](../21-custom-form/sample-forms/Form_1.png)  

You'll use the sample forms from the **21-custom-form/sample-forms** folder in this repo, which contain all the files you'll need to train a model without labels and another model with labels.

1. In Visual Studio Code, in the **21-custom-form** folder,  expand the **sample-forms** folder. Notice there are files ending in **.json** and **.png** in the folder.

    You will use the **.png** files to train your model.  

    The **.json** files have been generated for you and contain label information. To train with labels, you need to have the label information files in your blob storage container alongside the forms. 

2. Return to the Azure portal at [https://portal.azure.com](https://portal.azure.com).

3. View the **Resource group** in which you created the Form Recognizer resource previously.

4. On the **Overview** page for your resource group, note the **Subscription ID** and **Location**. You will need these values, along with your **resource group** name in subsequent steps.

![An example of the resource group page.](./images/resource_group_variables.png)

5. In Visual Studio Code, in the Explorer pane, right-click the the **21-custom-form** folder and select **Open in Integrated Terminal**.

6. In the terminal pane, enter the following command to establish an authenticated connection to your Azure subscription.
    
```
az login --output none
```

7. When prompted, sign into your Azure subscription. Then return to Visual Studio Code and wait for the sign-in process to complete.

8. Run the following command to list Azure locations.

```
az account list-locations -o table
```

9. In the output, find the **Name** value that corresponds with the location of your resource group (for example, for *East US* the corresponding name is *eastus*).

    > **Important**: Record the **Name** value and use it in Step 12.

10. In the Explorer pane, in the **21-custom-form** folder, select **setup.cmd**. You will use this batch script to run the Azure command line interface (CLI) commands required to create the other Azure resources you need.

11. In the **setup.cmd** script, review the **rem** commands. These comments outline the program the script will run. The program will: 
    - Create a storage account in your Azure resource group
    - Upload files from your local _sampleforms_ folder to a container called _sampleforms_ in the storage account
    - Print a Shared Access Signature URI

12. Modify the **subscription_id**, **resource_group**, and **location** variable declarations with the appropriate values for the subscription, resource group, and location name where you deployed the Form Recognizer resource. 
Then **save** your changes.

    Leave the **expiry_date** variable as it is for the exercise. This variable is used when generating the Shared Access Signature (SAS) URI. In practice, you will want to set an appropriate expiry date for your SAS. You can learn more about SAS [here](https://docs.microsoft.com/azure/storage/common/storage-sas-overview#how-a-shared-access-signature-works).  

13. In the terminal for the **21-custom-form** folder, enter the following command to run the script:

```
setup
```

14. When the script completes, review the displayed output and note your Azure resource's SAS URI.

> **Important**: Before moving on, paste the SAS URI somewhere you will be able to retrieve it again later (for example, in a new text file in Visual Studio Code).

15. In the Azure portal, refresh the resource group and verify that it contains the Azure Storage account just created. Open the storage account and in the pane on the left, select **Storage Browser (preview)**. Then in Storage Browser, expand **BLOB CONTAINERS** and select the **sampleforms** container to verify that the files have been uploaded from your local **21-custom-form/sample-forms** folder.

## Train a model in the Form Recognizer Studio

You will use the Form Recognizer Studio train a custom model. As you use the tool, your field information files are automatically created and stored in your connected storage account.

1. Open [Form Recognizer Studio](https://formrecognizer.appliedai.azure.com/studio). If you have never used it before, you will be prompted for some additional information: 
    - ***Subscription***
    - ***Resource Group***
    - ***Form Reocgnizer or Cognitive Service Resource***
    - Select **Continue**, then on the Review page, select **Finish**. 

### Create a project 
To create a project, you need the following:
- Azure Form Recognizer or Azure Cognitive Services resource
- Azure Blob Storage container with the training data
- At least five different forms of the same type to help train your custom model. 

Configure the service resource by adding your Storage account and Blob container to Connect your training data source.

1. On the Form Recognizer Studio home page, scroll down and select the **Custom model** card.
2. Under My Projects, select **+ Create a project**.
3. Complete the project details fields: 

**Enter project details**
- ***Project name***: *Select a unique name for your project* 
- ***Description***: A model to extract data from forms. 

**Configure service resource**
- ***Subscription***: *Select the subscription for your Form Recognizer resource* 
- ***Resource group***: *Select the resource group for your Form Recognizer resource*
- ***Form Recognizer or Cognitive Service Resource***: *Select the resource you created or create a new one* 
- ***API version***: 2022-01-30-preview 

**Connect training data source**
- ***Subscription***: *Select the subscription for your Azure Blob* 
- ***Resource Group***: *Select the resource group for your Azure Blob*
- ***Storage account***: *Select the account ai102formxxxxxxxxx*
- ***Blob Container***: sampleforms 
- ***Folder path***: *Leave blank*
**Review and Create**
Review and create your project.

### Label your forms using the Form Recognizer Studio

1. Click on the form 
2. Associate each label with a name 
3. Save and repeat for all five files. 
4. Click train. Write down the Model ID that is generated. 

## Test a model with the Form Recognizer SDK 

Now we will use the Form Recognizer SDK to test the model.  

> **Note**: In this exercise, you can choose to use the API from either the **C#** or **Python** SDK. In the steps below, perform the actions appropriate for your preferred language.

1. In Visual Studio Code, in the **21-custom-form** folder, expand the **C-Sharp** or **Python** folder depending on your language preference.
2. Right-click the **test-model** folder and open an integrated terminal.

3. Install the Form Recognizer package by running the appropriate command for your language preference:

**C#**

```
dotnet add package Azure.AI.FormRecognizer --version 3.0.0 
```

**Python**

```
pip install azure-ai-formrecognizer==3.0.0
```

3. View the contents of the **test-model** folder, and note that it contains a file for configuration settings:
    - **C#**: appsettings.json
    - **Python**: .env

4. Edit the configuration file, modifying the settings to reflect:
    - The **endpoint** for your Form Recognizer resource.
    - A **key** for your Form Recognizer resource.
    - The **Model ID** for your trained model.

    Save your changes. 
5. Note that the **test-model** folder contains a code file for the client application:

    - **C#**: Program.cs
    - **Python**: train-model.py

    Open the code file and review the code it contains, noting the following details:
    - Namespaces from the package you installed are imported
    - The **Main** function retrieves the configuration settings, and uses the key and endpoint to create an authenticated **Client**.
    - The code uses .... 

6. Return the integrated terminal for the **test-model** folder, and enter the following command to run the program:

**C#**

```
dotnet run
```

**Python**

```
python test-model.py
```
 
7. View the output and observe how the output for the model trained **with** labels provides field names like "CompanyPhoneNumber" and "DatedAs".  

The choice of training process can also produce different models, which can in turn affect downstream processes based on what fields the model returns and how confident you are with the returned values. 

## More information

For more information about the Form Recognizer service, see the [Form Recognizer documentation](https://docs.microsoft.com/azure/cognitive-services/form-recognizer/).
