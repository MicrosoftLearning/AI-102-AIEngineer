---
lab:
    title: 'Analyze Text'
    module: 'Module 3 - Getting Started with Natural Language Processing'
---

# Analyze Text

**Azure Language** supports analysis of text, including language detection, sentiment analysis, key phrase extraction, and entity recognition.

For example, suppose a travel agency wants to process hotel reviews that have been submitted to the company's web site. By using the Azure AI Language, they can determine the language each review is written in, the sentiment (positive, neutral, or negative) of the reviews, key phrases that might indicate the main topics discussed in the review, and named entities, such as places, landmarks, or people mentioned in the reviews.

## Provision an Azure AI Language resource

If you don't already have one in your subscription, you'll need to provision an **Azure AI Language service** resource in a supported region.

1. Open the Azure portal at `https://portal.azure.com`, and sign in using the Microsoft account associated with your Azure subscription.
1. In the search field at the top, search for **Azure AI services**. Then, in the results, select **Create** under **Language Service**.
1. Select **Continue to create your resource**.
1. Provision the resource using the following settings:
    - **Subscription**: *Your Azure subscription*.
    - **Resource group**: *Choose or create a resource group (if you are using a restricted subscription, you may not have permission to create a new resource group - use the one provided)*.
    - **Region**:*Choose any available region*
    - **Name**: *Enter a unique name*.
    - **Pricing tier**: Select either **Free (F0)** or **Standard (S)* *tier if Free is not available.
    - **Responsible AI Notice**: Agree.
1. Select **Review + create**.
1. Wait for deployment to complete, and then view the deployment details.

## Clone the repository for this course in Cloud Shell

Open up a new browser tab to work with Cloud Shell. If you haven't cloned this repository to Cloud Shell recently, follow the steps below to make sure you have the most recent version. Otherwise, open Cloud Shell and navigate to your clone.

1. In the [Azure portal](https://portal.azure.com?azure-portal=true), select the **[>_]** (*Cloud Shell*) button at the top of the page to the right of the search box. A Cloud Shell pane will open at the bottom of the portal.

    ![Screenshot of starting Cloud Shell by clicking on the icon to the right of the top search box.](images/cloudshell-launch-portal.png#lightbox)

2. The first time you open the Cloud Shell, you may be prompted to choose the type of shell you want to use (*Bash* or *PowerShell*). Select **Bash**. If you don't see this option, skip the step.  

3. If you're prompted to create storage for your Cloud Shell, ensure your subscription is specified and select **Create storage**. Then wait a minute or so for the storage to be created.

4. Make sure the type of shell indicated on the top left of the Cloud Shell pane is switched to *Bash*. If it's *PowerShell*, switch to *Bash* by using the drop-down menu.

5. Once the terminal starts, enter the following command to download the sample application and save it to a folder called `azure-ai-eng`.

    ```bash
   rm -r azure-ai-eng -f
   git clone https://github.com/MicrosoftLearning/AI-102-AIEngineer azure-ai-eng
    ```
  
6. The files are downloaded to a folder named **azure-ai-eng**. Navigate to the lab files for this exercise using the following command.

    ```bash
   cd azure-ai-eng/05-analyze-text
    ```

Applications for both C# and Python have been provided, as well as a supporting files you'll use to test the feature. Both apps feature the same functionality. Navigate to the folder of your preferred language.

Open the built-in code editor, and observe the text files in the `text-analysis` folder. Use the following command to open the lab files in the code editor.

```bash
code .
```

## Prepare to use the Azure AI Language SDK for text analytics

In this exercise, you'll complete a partially implemented client application that uses the Azure AI Language text analytics SDK to analyze hotel reviews.

> [!NOTE]
> > You can choose to use the SDK for either **C#** or **Python**. In the steps below, perform the actions appropriate for your preferred language.

1. In Cloud Shell, ensure you are in the **05-analyze-text** folder and have navigated to the **C-Sharp** or **Python** folder depending on your language preference.
1. Install the Text Analytics SDK package by running the appropriate command for your language preference:

    **C#**

    ```bash
    dotnet add package Azure.AI.TextAnalytics --version 5.3.0
    ```

    **Python**

    ```bash
    pip install azure-ai-textanalytics==5.3.0
    pip install python-dotenv
    ```

1. View the contents of the **text-analysis** folder in the code window, and note that it contains a file for configuration settings:
    - **C#**: appsettings.json
    - **Python**: .env

    Open the configuration file and update the configuration values it contains to reflect the **endpoint** and an authentication **key** for your Azure AI Language service resource. Save your changes.

1. Note that the **text-analysis** folder contains a code file for the client application:

    - **C#**: Program.cs
    - **Python**: text-analysis.py

    Open the code file and at the top, under the existing namespace references, find the comment **Import namespaces**. Then, under this comment, add the following language-specific code to import the namespaces you will need to use the Text Analytics SDK:

    **C#**

    ```csharp
    // import namespaces
    using Azure;
    using Azure.AI.TextAnalytics;
    ```

    **Python**

    ```python
    # import namespaces
    from azure.core.credentials import AzureKeyCredential
    from azure.ai.textanalytics import TextAnalyticsClient
    ```

1. In the **Main** function, note that code to load the Azure AI Language service endpoint and key from the configuration file has already been provided. Then find the comment **Create client using endpoint and key**, and add the following code to create a client for the Text Analysis API:

    **C#**

    ```C#
    // Create client using endpoint and key
    AzureKeyCredential credentials = new AzureKeyCredential(cogSvcKey);
    Uri endpoint = new Uri(cogSvcEndpoint);
    TextAnalyticsClient CogClient = new TextAnalyticsClient(endpoint, credentials);
    ```

    **Python**

    ```Python
    # Create client using endpoint and key
    credential = AzureKeyCredential(cog_key)
    cog_client = TextAnalyticsClient(endpoint=cog_endpoint, credential=credential)
    ```

1. Save your changes and return to the integrated terminal for the **text-analysis** folder, and enter the following command to run the program:

    **C#**

    ```bash
    dotnet run
    ```

    **Python**

    ```bash
    python text-analysis.py
    ```

1. Observe the output as the code should run without error, displaying the contents of each review text file in the **reviews** folder. The application successfully creates a client for the Text Analytics API but doesn't make use of it. We'll fix that in the next procedure.

## Detect language

Now that you have created a client for the API, let's use it to detect the language in which each review is written.

1. In the **Main** function for your program, find the comment **Get language**. Then, under this comment, add the code necessary to detect the language in each review document:

    **C#**

    ```csharp
    // Get language
    DetectedLanguage detectedLanguage = CogClient.DetectLanguage(text);
    Console.WriteLine($"\nLanguage: {detectedLanguage.Name}");
    ```

    **Python**

    ```python
    # Get language
    detectedLanguage = cog_client.detect_language(documents=[text])[0]
    print('\nLanguage: {}'.format(detectedLanguage.primary_language.name))
    ```

    > **Note**: *In this example, each review is analyzed individually, resulting in a separate call to the service for each file. An alternative approach is to create a             collection of documents and pass them to the service in a single call. In both approaches, the response from the service consists of a collection of documents; which is why     in the Python code above, the index of the first (and only) document in the response ([0]) is specified.*

1. Save your changes and return to the integrated terminal for the **text-analysis** folder, and enter the following command to run the program:

    **C#**

    ```bash
    dotnet run
    ```

    **Python**

    ```bash
    python text-analysis.py
    ```

1. Observe the output, noting that this time the language for each review is identified.

## Evaluate sentiment

*Sentiment analysis* is a commonly used technique to classify text as *positive* or *negative* (or possible *neutral* or *mixed*). It's commonly used to analyze social media posts, product reviews, and other items where the sentiment of the text may provide useful insights.

1. In the **Main** function for your program, find the comment **Get sentiment**. Then, under this comment, add the code necessary to detect the sentiment of each review document:

    **C#**

    ```csharp
    // Get sentiment
    DocumentSentiment sentimentAnalysis = CogClient.AnalyzeSentiment(text);
    Console.WriteLine($"\nSentiment: {sentimentAnalysis.Sentiment}");
    ```

    **Python**

    ```python
    # Get sentiment
    sentimentAnalysis = cog_client.analyze_sentiment(documents=[text])[0]
    print("\nSentiment: {}".format(sentimentAnalysis.sentiment))
    ```

1. Save your changes and return to the integrated terminal for the **text-analysis** folder, and enter the following command to run the program:

    **C#**

    ```bash
    dotnet run
    ```

    **Python**

    ```bash
    python text-analysis.py
    ```

1. Observe the output, noting that the sentiment of the reviews is detected.

## Identify key phrases

It can be useful to identify key phrases in a body of text to help determine the main topics that it discusses.

1. In the **Main** function for your program, find the comment **Get key phrases**. Then, under this comment, add the code necessary to detect the key phrases in each review document:

    **C#**

    ```csharp
    // Get key phrases
    KeyPhraseCollection phrases = CogClient.ExtractKeyPhrases(text);
    if (phrases.Count > 0)
    {
        Console.WriteLine("\nKey Phrases:");
        foreach(string phrase in phrases)
        {
            Console.WriteLine($"\t{phrase}");
        }
    }
    ```

    **Python**

    ```python
    # Get key phrases
    phrases = cog_client.extract_key_phrases(documents=[text])[0].key_phrases
    if len(phrases) > 0:
        print("\nKey Phrases:")
        for phrase in phrases:
            print('\t{}'.format(phrase))
    ```

1. Save your changes and return to the integrated terminal for the **text-analysis** folder, and enter the following command to run the program:

    **C#**

    ```bash
    dotnet run
    ```

    **Python**

    ```bash
    python text-analysis.py
    ```

1. Observe the output, noting that each document contains key phrases that give some insights into what the review is about.

## Extract entities

Often, documents or other bodies of text mention people, places, time periods, or other entities. The text Analytics API can detect multiple categories (and subcategories) of entity in your text.

1. In the **Main** function for your program, find the comment **Get entities**. Then, under this comment, add the code necessary to identify entities that are mentioned in each review:

    **C#**

    ```csharp
    // Get entities
    CategorizedEntityCollection entities = CogClient.RecognizeEntities(text);
    if (entities.Count > 0)
    {
        Console.WriteLine("\nEntities:");
        foreach(CategorizedEntity entity in entities)
        {
            Console.WriteLine($"\t{entity.Text} ({entity.Category})");
        }
    }
    ```

    **Python**

    ```python
    # Get entities
    entities = cog_client.recognize_entities(documents=[text])[0].entities
    if len(entities) > 0:
        print("\nEntities")
        for entity in entities:
            print('\t{} ({})'.format(entity.text, entity.category))
    ```

1. Save your changes and return to the integrated terminal for the **text-analysis** folder, and enter the following command to run the program:

    **C#**

    ```bash
    dotnet run
    ```

    **Python**

    ```bash
    python text-analysis.py
    ```

1. Observe the output, noting the entities that have been detected in the text.

## Extract linked entities

In addition to categorized entities, the Text Analytics API can detect entities for which there are known links to data sources, such as Wikipedia.

1. In the **Main** function for your program, find the comment **Get linked entities**. Then, under this comment, add the code necessary to identify linked entities that are mentioned in each review:

    **C#**

    ```csharp
    // Get linked entities
    LinkedEntityCollection linkedEntities = CogClient.RecognizeLinkedEntities(text);
    if (linkedEntities.Count > 0)
    {
        Console.WriteLine("\nLinks:");
        foreach(LinkedEntity linkedEntity in linkedEntities)
        {
            Console.WriteLine($"\t{linkedEntity.Name} ({linkedEntity.Url})");
        }
    }
    ```

    **Python**

    ```python
    # Get linked entities
    entities = cog_client.recognize_linked_entities(documents=[text])[0].entities
    if len(entities) > 0:
        print("\nLinks")
        for linked_entity in entities:
            print('\t{} ({})'.format(linked_entity.name, linked_entity.url))
    ```

1. Save your changes and return to the integrated terminal for the **text-analysis** folder, and enter the following command to run the program:

    **C#**

    ```bash
    dotnet run
    ```

    **Python**

    ```bash
    python text-analysis.py
    ```

1. Observe the output, noting the linked entities that are identified.

## More information

For more information about using **Azure AI Language**, see the [documentation](https://learn.microsoft.com/en-us/azure/ai-services/language-service/overview).
