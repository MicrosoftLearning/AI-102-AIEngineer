---
lab:
    title: 'Create an Azure Cognitive Search solution'
    module: 'Module 12 - Creating a Knowledge Mining Solution'
---

# Create an Azure Cognitive Search solution

All organizations rely on information to make decisions, answer questions, and function efficiently. The problem for most organizations is not a lack of information, but the challenge of finding and  extracting the information from the massive set of documents, databases, and other sources in which the information is stored.

For example, suppose *Margie's Travel* is a travel agency that specializes in organizing trips to cities around the world. Over time, the company has amassed a huge amount of information in documents such as brochures, as well as reviews of hotels submitted by customers. This data is a valuable source of insights for travel agents and customers as they plan trips, but the sheer volume of data can make it difficult to find relevant information to answer a specific customer question.

To address this challenge, Margie's Travel can use Azure Cognitive Search to implement a solution in which the documents are indexed and enriched by using AI-based cognitive skills to make them easier to search.

## Clone the repository for this course

If you have not already cloned **AI-102-AIEngineer** code repository to the environment where you're working on this lab, follow these steps to do so. Otherwise, open the cloned folder in Visual Studio Code.

1. Start Visual Studio Code.
2. Open the palette (SHIFT+CTRL+P) and run a **Git: Clone** command to clone the `https://github.com/MicrosoftLearning/AI-102-AIEngineer` repository to a local folder (it doesn't matter which folder).
3. When the repository has been cloned, open the folder in Visual Studio Code.
4. Wait while additional files are installed to support the C# code projects in the repo.

    > **Note**: If you are prompted to add required assets to build and debug, select **Not Now**.

## Create Azure resources

The solution you will create for Margie's Travel requires the following resources in your Azure subscription:

- An **Azure Cognitive Search** resource, which will manage indexing and querying.
- A **Cognitive Services** resource, which provides AI services for skills that your search solution can use to enrich the data in the data source with AI-generated insights.
- A **Storage account** with a blob container in which the documents to be searched are stored.

> **Important**: Your Azure Cognitive Search and Cognitive Services resources must be in the same location!

### Create an Azure Cognitive Search resource

1. Open the Azure portal at `https://portal.azure.com`, and sign in using the Microsoft account associated with your Azure subscription.
2. Select the **&#65291;Create a resource** button, search for *search*, and create a **Azure Cognitive Search** resource with the following settings:
    - **Subscription**: *Your Azure subscription*
    - **Resource group**: *Create a new resource group (if you are using a restricted subscription, you may not have permission to create a new resource group - use the one provided)*
    - **Service name**: *Enter a unique name*
    - **Location**: *Select a location - note that your Azure Cognitive Search and Cognitive Services resources must be in the same location*
    - **Pricing tier**: Basic

3. Wait for deployment to complete, and then go to the deployed resource.
4. Review the **Overview** page on the blade for your Azure Cognitive Search resource in the Azure portal. Here, you can use a visual interface to create, test, manage, and monitor the various components of a search solution; including data sources, indexes, indexers, and skillsets.

### Create a Cognitive Services resource

If you don't already have one in your subscription, you'll need to provision a **Cognitive Services** resource. Your search solution will use this to enrich the data in the datastore with AI-generated insights.

1. Return to the home page of the Azure portal, and then select the **&#65291;Create a resource** button, search for *cognitive services*, and create a **Cognitive Services** resource with the following settings:
    - **Subscription**: *Your Azure subscription*
    - **Resource group**: *The same resource group as your Azure Cognitive Search resource*
    - **Region**: *The same location as your Azure Cognitive Search resource*
    - **Name**: *Enter a unique name*
    - **Pricing tier**: Standard S0
2. Select the required checkboxes and create the resource.
3. Wait for deployment to complete, and then view the deployment details.

### Create a storage account

1. Return to the home page of the Azure portal, and then select the **&#65291;Create a resource** button, search for *storage account*, and create a **Storage account** resource with the following settings:
    - **Subscription**: *Your Azure subscription*
    - **Resource group**: **The same resource group as your Azure Cognitive Search and Cognitive Services resources*
    - **Storage account name**: *Enter a unique name*
    - **Location**: *Choose any available location*
    - **Performance**: Standard
    - **Account kind**: Storage V2
    - **Replication**: Locally-redundant storage (LRS)
2. Wait for deployment to complete, and then go to the deployed resource.
3. On the **Overview** page, note the **Subscription ID** -this identifies the subscription in which the storage account is provisioned.
4. On the **Access keys** page, note that two keys have been generated for your storage account. Then select **Show keys** to view the keys.

    > **Tip**: Keep the **Storage Account** blade open - you will need the subscription ID and one of the keys in the next procedure.

## Upload Documents to Azure Storage

Now that you have the required resources, you can upload some documents to your Azure Storage account.

1. In Visual Studio Code, in the **Explorer** pane, expand the ****22-create-a-search-solution** folder and select **UploadFiles.cmd**.
2. Edit the batch file to replace the **YOUR_SUBSCRIPTION_ID**, **YOUR_AZURE_STORAGE_ACCOUNT_NAME**, and **YOUR_AZURE_STORAGE_KEY** placeholders with the appropriate subscription ID, Azure storage account name, and Azure storage account key values for the storage account you created previously.
3. Save your changes, and then right-click the **22-create-a-search-solution** folder and open an integrated terminal.
4. Enter the following command to sign into your Azure subscription by using the Azure CLI.

    ```
    az login
    ```

A web browser tab will open and prompt you to sign into Azure. Do so, and then close the browser tab and return to Visual Studio Code.

5. Enter the following command to run the batch file. This will create a blob container in your storage account and upload the documents in the **data** folder to it.

    ```
    UploadDocs
    ```

## Index the documents

Now that you have the documents in place, you can create a search solution by indexing them.

1. In the Azure portal, browse to your Azure Cognitive Search resource. Then, on its **Overview** page, select **import data**.
2. On the **Connect to your data** page, in the **Data Source** list, select **Azure Blob Storage**. Then complete the data store details with the following values:
    - **Data Source**: Azure Blob Storage
    - **Data source name**: margies-data
    - **Data to extract**: Content and metadata
    - **Parsing mode**: Default
    - **Connection string**: *Select **Choose an existing connection**. Then select your storage account, and finally select the **margies** container that was created by the UploadDocs.cmd script.*
    - **Authenticate using managed identity**: Unselected
    - **Container name**: margies
    - **Blob folder**: *Leave this blank*
    - **Description**: Brochures and reviews in Margie's Travel web site.
3. Proceed to the next step (*Add cognitive skills*).
4. in the **Attach Cognitive Services** section, select your cognitive services resource.
5. In the **Add enrichments** section:
    - Change the **Skillset name** to **margies-skillset**.
    - Select the option **Enable OCR and merge all text into merged_content field**.
    - Set the **Source data field** to **merged_content**.
    - Leave the **Enrichment granularity level** as **Source field**, which is set the entire contents of the document being indexed; but note that you can change this to extract information at more granular levels, like pages or sentences.
    - Select the following enriched fields:

        | Cognitive Skill | Parameter | Field name |
        | --------------- | ---------- | ---------- |
        | Extract location names | | locations |
        | Extract key phrases | | keyphrases |
        | Detect language | | language |
        | Generate tags from images | | imageTags |
        | Generate captions from images | | imageCaptions |

6. Proceed to the next step (*Customize target index*).
7. Change the **Index name** to **margies-index**.
8. Set the **Key** set to **metadata_storage_path** and leave the **Suggester name** and **Search mode** blank.
9. Make the following changes to the index fields, leaving all other fields with their default settings (you may need to scroll to the right to see the entire table):

    | Field name | Retrievable | Filterable | Sortable | Facetable | Searchable |
    | ---------- | ----------- | ---------- | -------- | --------- | ---------- |
    | metadata_storage_size | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | | |
    | metadata_storage_last_modified | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | | |
    | metadata_storage_name | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; |
    | metadata_author | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; |
    | locations | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | | | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; |
    | keyphrases | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | | | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; |
    | language | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | | | |

11. Proceed to the next step (*Create an indexer*).
12. Change the **Indexer name** to **margies-indexer**.
13. Leave the **Schedule** set to **Once**.
14. Expand the **Advanced** options, and ensure that the **Base-64 encode keys** option is selected (generally encoding keys make the index more efficient).
15. Select **Submit** to create the data source, skillset, index, and indexer. The indexer is run automatically and runs the indexing pipeline, which:
    1. Extracts the document metadata fields and content from the data source
    2. Runs the skillset of cognitive skills to generate additional enriched fields
    3. Maps the extracted fields to the index.
16. In the bottom half of the **Overview** page for your Azure Cognitive Search resource, view the **Indexers** tab, which should show the newly created **margies-indexer**. Wait a few minutes, and click **&orarr; Refresh** until the **Status** indicates success.

## Search the index

Now that you have an index, you can search it.

1. At the top of the **Overview** page for your Azure Cognitive Search resource, select **Search explorer**.
2. In Search explorer, in the **Query string** box, enter `*` (a single asterisk), and then select **Search**.

    This query retrieves all documents in the index in JSON format. Examine the results and note the fields for each document, which contain document content, metadata, and enriched data extracted by the cognitive skills you selected.

3. Modify the query string to `search=*&$count=true` and submit the search.

    This time, the results include a **@odata.count** field at the top of the results that indicates the number of documents returned by the search.

4. Try the following query string:

    ```
    search=*&$count=true&$select=metadata_storage_name,metadata_author,locations
    ```

    This time the results include only the file name, author, and any locations mentioned in the document content. The file name and author are in the **metadata_content_name** and **metadata_author** fields, which were extracted from the source document. The **locations** field was generated by a cognitive skill.

5. Now try the following query string:

    ```
    search="New York"&$count=true&$select=metadata_storage_name,keyphrases
    ```

    This search finds documents that mention "New York" in any of the searchable fields, and returns the file name and key phrases in the document.

6. Let's try one more query string:

    ```
    search="New York"&$count=true&$select=metadata_storage_name&$filter=metadata_author eq 'Reviewer'
    ```

    This query returns the filename of any documents authored by *Reviewer* that mention "New York".

## Explore and modify definitions of search components

The components of the search solution are based on JSON definitions, which you can view and edit in the Azure portal.

### Review the data source

1. In the Azure portal, return to the **Overview** page for your Azure Cognitive Search resource; and in the bottom half of the page, select the **Data sources** tab. The **margies-data** data source should be listed.
2. Select the **margies-data** data source to view its settings. Then select the **Data Source Definition (JSON)** tab to view the JSON definition of the data source. This includes the information it uses to connect to the blob container in your storage account.
3. Close the **margies-data** page to return to the **Overview** page.

### Review and modify the skillset

1. In the bottom half of the **Overview** page,  select the **Skillsets** tab, where **margies-skillset** should be listed.
2. Select **margies-skillset** and view the **Skillset Definition (JSON)** page. This shows a JSON definition that includes the six skills you specified in the user interface previously.

    The JSON structure can be difficult to understand at first, but the important thing to understand is that the **"skills"** element is a list (enclosed by **[...]** brackets) that contains all of the skills in your enrichment pipeline. Each skill is enclosed in **{...}** braces, and they are separated by commas - like this:

    ```
    "skills": [
        {
            "@odata.type": "skill_1_type",
            "name" : "#1",
            ...
        },
        {
            "@odata.type": "skill_2_type",
            "name" : "#2",
            ...
        },
        {
            "@odata.type": "skill_3_type",
            "name" : "#3",
            ...
        }
    ]
    ```

3. On the right side of the page, note that there are templates for additional skills you might want to add to the skillset. For example, it would be good to identify the *sentiment* of the documents being indexed - particularly the hotel reviews, so we can easily find reviews that are positive or negative.
4. In the **Skills** list, select **Sentiment Skill** to show a JSON template for this skill.
5. Copy the template to the clipboard, and then on the left side, in the JSON for your skillset definition, paste the copied skill in a newly inserted line immediately after the following line (which should be line 6) - be careful not to overwrite the **{** marking the beginning of the first existing skill:


    "skills": [


6. Add a comma immediately after the newly inserted skill to separate it from the next skill (which was previously the first skill in the list).
7. Update the new skill definition like this:

    ```
        {
            "@odata.type": "#Microsoft.Skills.Text.SentimentSkill",
            "defaultLanguageCode": "en",
            "name": "get-sentiment",
            "description": "Evaluate sentiment",
            "context": "/document",
            "inputs": [
                {
                    "name": "text",
                    "source": "/document/merged_content"
                },
                {
                    "name": "languageCode",
                    "source": "/document/language"
                }
            ],
            "outputs": [
                {
                    "name": "score",
                    "targetName": "sentimentScore"
                }
            ]
        },
    ```

The new skill is named **get-sentiment**, and will evaluate the text found in the **merged_content** field of the document being indexed (which includes the source content as well as any text extracted from images in the content). It uses the extracted **language** of the document (with a default of English), and evaluates a score for the sentiment of the content. This score is then output as a new field named **sentimentScore** at the **document** level of the object that represents the indexed document.

8. Select **Save** to save the skillset with the new skill.
9. Close the **margies-skillset** page to return to the **Overview** page** for your Azure Cognitive Search resource.

### Review and modify the index

1. On the **Overview** page** for your Azure Cognitive Search resource, select the **Indexes** tab (<u>not</u> *Indexers*), where **margies-index** should be listed.
2. Select **margies-index** and view the **Index Definition (JSON)** page. This shows a JSON definition for your index, including definitions for each field. Some fields are based on metadata and content in the source document, and others are the results of skills in the skillset.
3. You added a skill to the skillset to extract a sentiment score for the document. Now you must add a corresponding field in the index to which this value can be mapped. At the bottom of the **fields** list (before the **]** that denotes the end of the list, which is followed by index properties such as **suggesters**), add the following field (being sure to include the comma at the beginning, to separate the new field from the previous field):

    ```
    ,
    {
        "name": "sentiment",
        "type": "Edm.Double",
        "facetable": false,
        "filterable": true,
        "key": false,
        "retrievable": true,
        "searchable": false,
        "sortable": true,
        "analyzer": null,
        "indexAnalyzer": null,
        "searchAnalyzer": null,
        "synonymMaps": [],
        "fields": []
    }
    ```

4. The index includes the **metadata_storage_path** field (the URL of the document), which is currently used as the index key. The key is Base-64 encoded, making it efficient as a key but requiring client applications to decode it if they want to use the actual URL value as a field. We'll resolve this by adding another field that will be mapped to the unencoded value. Add the following field definition immediately after the **sentiment** field you just added:

    ```
    ,
    {
        "name": "url",
        "type": "Edm.String",
        "facetable": false,
        "filterable": true,
        "key": false,
        "retrievable": true,
        "searchable": false,
        "sortable": false,
        "analyzer": null,
        "indexAnalyzer": null,
        "searchAnalyzer": null,
        "synonymMaps": [],
        "fields": []
    }
    ```

5. Select **Save** to save the index with the new fields.
6. Close the **margies-index** page to return to the blade for your Azure Cognitive Search resource.

### Review and modify the indexer

1. On the blade for your Azure Cognitive Search resource, select the **Indexers** tab (<u>not</u> *Indexes*), where **margies-indexer** should be listed.
2. Select **margies-indexer** and view the **Indexer Definition (JSON)** page. This shows a JSON definition for your indexer, which maps fields extracted from document content and metadata (in the **fieldMappings** section), and values extracted by skills in the skillset (in the **outputFieldMappings** section), to fields in the index.
3. In the **fieldMappings** list, add a comma after the existing mapping for the **metadata_storage_path** value to the base-64 encoded key field; and then add the following JSON to map the same value to the **url** field, but without the Base-64 encoding:

    ```
    {
        "sourceFieldName" : "metadata_storage_path",
        "targetFieldName" : "url"
    }
    
    ```

All of the other metadata and content field in the source document are implicitly mapped to fields of the same name in the index.

4. At the end of the **ouputFieldMappings** section, add the following mapping to map the **sentimentScore** value extracted by your sentiment skill to the **sentiment** field you added to the index (remember to separate it from the previous mapping with a comma!):

    ```
    {
        "sourceFieldName": "/document/sentimentScore",
        "targetFieldName": "sentiment"
    }
    ```

5. Select **Save** to save the indexer with the new mappings.
6. Select **Reset** to reset the index, and confirm that you want to do this when prompted. You've added new fields to an already-populated index, so you'll need to reset and reindex to update the existing index records with the new field values.
7. Select **Run** to run the updated indexer, confirming that you want to run it now when prompted.
8. Close the **margies-indexer** page to return to the **Overview** page** for your Azure Cognitive Search resource, and select **Refresh** to track the progress of the indexing operation. It may take a minute or so to complete.

    *There may be some warnings for a few documents that are too large to evaluate sentiment. Often sentiment analysis is performed at the page or sentence level rather than the full document; but in this case scenario, most of the documents - particularly the hotel reviews, are short enough for useful document-level sentiment scores to be evaluated.*

### Query the modified index

1. At the top of the blade for your Azure Cognitive Search resource, select **Search explorer**.
2. In Search explorer, in the **Query string** box, enter the following query string, and then select **Search**.

    ```
    search=London&$select=url,sentiment,keyphrases&$filter=metadata_author eq 'Reviewer' and sentiment gt 0.5
    ```

    This query retrieves the **url**, **sentiment**, and **keyphrases** for all documents that mention *London* authored by *Reviewer* that have a **sentiment** score greater than *0.5* (in other words, positive reviews that mention London)

3. Close the **Search explorer** page to return to the **Overview** page.

## Create a search client application

Now that you have a useful index, you can use it from a client application. You can do this by consuming the REST interface, submitting requests and receiving responses in JSON format over HTTP; or you can use the software development kit (SDK) for your preferred programming language. In this exercise, we'll use the SDK.

> **Note**: You can choose to use the SDK for either **C#** or **Python**. In the steps below, perform the actions appropriate for your preferred language.

### Get the endpoint and keys for your search resource

1. In the Azure portal, on the **Overview** page for your Azure Cognitive Search resource, note the **Url** value, which should be similar to **https://*your_resource_name*.search.windows.net**. This is the endpoint for your search resource.
2. On the **Keys** page, note that there are two **admin** keys, and a single **query** key. An *admin* key is used to create and manage search resources; a *query* key is used by client applications that only need to perform search queries.

    *You will need the endpoint and query key for your client application.*

### Prepare to use the Azure Cognitive Search SDK

1. In Visual Studio Code, in the **Explorer** pane, browse to the **22-create-a-search-solution** folder and expand the **C-Sharp** or **Python** folder depending on your language preference.
2. Right-click the **margies-travel** folder and open an integrated terminal. Then install the Azure Cognitive Search SDK package by running the appropriate command for your language preference:

    **C#**
    
    ```
    dotnet add package Azure.Search.Documents --version 11.1.1
    ```
    
    **Python**
    
    ```
    pip install azure-search-documents==11.0.0
    ```
    
3. View the contents of the **margies-travel** folder, and note that it contains a file for configuration settings:
    - **C#**: appsettings.json
    - **Python**: .env

    Open the configuration file and update the configuration values it contains to reflect the **endpoint** and **query key** for your Azure Cognitive Search resource. Save your changes.

### Explore code to search an index

The **margies-travel** folder contains code files for a web application (a Microsoft C# *ASP&period;NET Razor* web application or a Python *Flask* application), which you will update to include search functionality.

1. Open the following code file in the web application, depending on your choice of programming language:
    - **C#**:Pages/Index.cshtml.cs
    - **Python**: app&period;py
2. Near the top of the code file, find the comment **Import search namespaces**, and note the namespaces that have been imported to work with the Azure Cognitive Search SDK:
3. In the **search_query** function, find the comment **Create a search client**, and note that the code creates a **SearchClient** object using the endpoint and query key for your Azure Cognitive Search resource:
4. In the **search_query** function, find the comment **Submit search query**, and review the code to submit a search for the specified text with the following options:
    - A *search mode* that requires **all** of the individual words in the search text are found.
    - The total number of documents found by the search is included in the results.
    - The results are filtered to include only documents that match the provided filter expression.
    - The results are sorted into the specified sort order.
    - Each discrete value of the **metadata_author** field is returned as a *facet* that can be used to display pre-defined values for filtering.
    - Up to three extracts of the **merged_content** and **imageCaption** fields with the search terms highlighted are included in the results.
    - The results include only the fields specified.

### Explore code to render search results

The web app already includes code to process and render the search results.

1. Open the following code file in the web application, depending on your choice of programming language:
    - **C#**:Pages/Index.cshtml
    - **Python**: templates/search&period;html
2. Examine the code, which renders the page on which the search results are displayed. Observe that:
    - The page begins with a search form that the user can use to submit a new search (in the Python version of the application, this form is defined in the **base&period;html** template), which is referenced at the beginning of the page.
    - A second form is then rendered, enabling the user to refine the search results. The code for this form:
        - Retrieves and displays the count of documents from the search results.
        - Retrieves the facet values for the **metadata_author** field and displays them as an option list for filtering.
        - Creates a drop-down list of sort options for the results.
    - The code then iterates through the search results, rendering each result as follows:
        - Display the **metadata_storage_name** (file name) field as a link to the address in the **url** field.
        - Displaying *highlights* for search terms found in the **merged_content** and **imageCaption** fields to help show the search terms in context.
        - Display the **metadata_author**, **metadata_storage_size**, **metadata_storage_last_modified**, and **language** fields.
        - Indicate the **sentiment** using an emoticon (&#128578; for scores of 0.5 or higher, and &#128577; for scores less than 0.5).
        - Display the first five **keyphrases** (if any).
        - Display the first five **locations** (if any).
        - Display the first five **imageTags** (if any).

### Run the web app

 1. return to the integrated terminal for the **margies-travel** folder, and enter the following command to run the program:

    **C#**
    
    ```
    dotnet run
    ```
    
    **Python**
    
    ```
    flask run
    ```

2. In the message that is displayed when the app starts successfully, follow the link to the running web application (*http://localhost:5000/* or *http://127.0.0.1:5000/*) to open the Margies Travel site in a web browser.
3. In the Margie's Travel website, enter **London hotel** into the search box and click **Search**.
4. Review the search results. They include the file name (with a hyperlink to the file URL), an extract of the file content with the search terms (*London* and *hotel*) emphasized, and other attributes of the file from the index fields.
5. Observe that the results page includes some user interface elements that enable you to refine the results. These include:
    - A *filter* based on a facet value for the **metadata_author** field. This demonstrates how you can use *facetable* fields to return a list of *facets* - fields with a small set of discrete values that can displayed as potential filter values in the user interface.
    - The ability to *order* the results based on a specified field and sort direction (ascending or descending). The default order is based on *relevancy*, which is calculated as a **search.score()** value based on a *scoring profile* that evaluates the frequency and importance of search terms in the index fields.
6. Select the **Reviewer** filter and the **Positive to negative** sort option, and then select **Refine Results**.
7. Observe that the results are filtered to include only reviews, and sorted into descending order of sentiment.
8. In the **Search** box, enter a new search for **quiet hotel in New York** and review the results.
9. Try the following search terms:
    - **Tower of London** (observe that this term is identified as a *key phrase* in some documents).
    - **skyscraper** (observe that this word doesn't appear in the actual content of any documents, but is found in the *image captions* and *image tags* that were generated for images in some documents).
    - **Mojave desert** (observe that this term is identified as a *location* in some documents).
10. Close the browser tab containing the Margie's Travel web site and return to Visual Studio Code. Then in the Python terminal for the **margies-travel** folder (where the dotnet or flask application is running), enter Ctrl+C to stop the app.

## More information

To learn more about Azure Cognitive Search, see the [Azure Cognitive Search documentation](https://docs.microsoft.com/azure/search/search-what-is-azure-search).
