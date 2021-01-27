---
lab:
    title: 'Create an Azure Cognitive Search solution'
---

# Create an Azure Cognitive Search solution

All organizations rely on information to make decisions, answer questions, and function efficiently. The problem for most organizations is not a lack of information, but the challenge of finding and  extracting the information from the massive set of documents, databases, and other sources in which the information is stored.

For example, suppose *Margie's Travel* is a travel agency that specializes in organizing trips to cities around the world. Over time, the company has amassed a huge amount of information in documents such as brochures, as well as reviews of hotels submitted by customers. This data is a valuable source of insights for travel agents and customers as they plan trips, but the sheer volume of data can make it difficult to find relevant information to answer a specific customer question.

To address this challenge, Margie's Travel can use Azure Cognitive Search to implement a solution in which the documents are indexed and enriched by using AI-based cognitive skills to make them easier to search.

## Clone the repository for this course

If you have not already done so, you must clone the code repository for this course:

1. Start Visual Studio Code.
2. Open the palette (SHIFT+CTRL+P) and run a **Git: Clone** command to clone the `https://github.com/MicrosoftLearning/AI-102-AIEngineer` repository to a local folder (it doesn't matter which folder).
3. When the repository has been cloned, open the folder in Visual Studio Code.
4. Wait while additional files are installed to support the C# code projects in the repo.

    > **Note**: If you are prompted to add required assets to build and debug, select **Not Now**.

## Create Azure resources

The solution you will create for Margie's Travel requires the following resources in your Azure subscription:

- A **Storage account** with a blob container in which the documents to be searched are stored.
- A **Cognitive Services** resource, which provides AI services for skills that your search solution can use to enrich the data in the data source with AI-generated insights.
- An **Azure Cognitive Search** resource, which will manage indexing and querying.

### Create a storage account and upload files

1. Open the Azure portal at [https://portal.azure.com](https://portal.azure.com), and sign in using the Microsoft account associated with your Azure subscription.
2. Select the **&#65291;Create a resource** button, search for *storage*, and create a **Storage account** resource with the following settings:
    - **Subscription**: *Your Azure subscription*
    - **Resource group**: *Choose or create a resource group (if you are using a restricted subscription, you may not have permission to create a new resource group - use the one provided)*
    - **Storage account name**: *Enter a unique name*
    - **Location**: *Choose any available location*
    - **Performance**: Standard
    - **Account kind**: Storage V2
    - **Replication**: Locally-redundant storage (LRS)
3. Wait for deployment to complete, and then go to the deployed resource.
4. In the blade for your storage account, in the pane on the left, select **Storage Explorer**.
5. In Storage Explorer, right-click **BLOB CONTAINERS** and create a blob container named **margies** with **Blob** level public access (the container will host documents that Margie's Travel makes publicly available in their web site).
6. Expand **BLOB CONTAINERS** and select your new **margies** container.
7. In the **margies** container, use the **&#65291;New Folder** button to create a new folder named **collateral**.
8. In the **margies > collateral** folder, select **Upload**, and in the **Upload blob** pane, select *all* of the files in the local **ai-102/21-create-a-search-solution/data/collateral** folder (in the folder where you cloned the repo) and upload them.
9. Use the **&uarr;** button to navigate back up to the root of the **margies** container. Then create a new folder named **reviews**, alongside the existing **collateral** folder.
10. In the **margies > reviews** folder, select **Upload**, and in the **Upload blob** pane, select *all* of the files in the local **ai-102/21-create-a-search-solution/data/reviews** folder and upload them.

    You should end up with a blob container structure like this:
    
    - **margies** (container)
        - **collateral** (folder)
            - 6 brochures (PDF files)
        - **reviews** (folder)
            - 66 hotel reviews (PDF files)

### Create a Cognitive Services resource

If you don't already have on in your subscription, you'll need to provision a **Cognitive Services** resource. Your search solution will use this to enrich the data in the datastore with AI-generated insights.

1. Return to the home page of the Azure portal, and then select the **&#65291;Create a resource** button, search for *cognitive services*, and create a **Cognitive Services** resource with the following settings:
    - **Subscription**: *Your Azure subscription*
    - **Resource group**: *The same resource group as your storage account*
    - **Region**: *Choose any available region*
    - **Name**: *Enter a unique name*
    - **Pricing tier**: Standard S0
2. Select the required checkboxes and create the resource.
3. Wait for deployment to complete, and then view the deployment details.

### Create an Azure Cognitive Search resource

1. Return to the home page of the Azure portal, and then select the **&#65291;Create a resource** button, search for *search*, and create a **Azure Cognitive Search** resource with the following settings:
    - **Subscription**: *Your Azure subscription*
    - **Resource group**: *The same resource group as your storage account and cognitive services resource*
    - **URL**: *Enter a unique name*
    - **Location**: *The same location as your storage account*
    - **Pricing tier**: Basic

2. Wait for deployment to complete, and then go to the deployed resource.
3. Review the **Overview** page on the blade for your Azure Cognitive Search resource in the Azure portal. Here, you can use a visual interface to create, test, manage, and monitor the various components of a search solution; including data sources, indexes, indexers, and skillsets.

## Index the documents

Now that you have the necessary Azure resources in place, you can create a search solution by indexing the documents.

1. On the **Overview** page for your Azure Cognitive Search resource, select **import data**.
2. On the **Connect to your data** page, in the **Data Source** list, select **Azure Blob Storage**. Then complete the data store details with the following values:
    - **Data Source**: Azure Blob Storage
    - **Data source name**: margies-data
    - **Data to extract**: Content and metadata
    - **Parsing mode**: Default
    - **Connection string**: *Select **Choose an existing connection**. Then select your storage account, and finally select the **margies** container you created previously.*
    - **Authenticate using managed identity**: Unselected
    - **Blob folder**: *Leave this blank*
    - **Description**: Brochures and reviews in Margie's Travel web site.
3. Proceed to the next step (*Add cognitive skills*).
4. in the **Attach Cognitive Services** section, select your cognitive services resource.
5. In the **Add enrichments** section:
    - Change the **Skillset name** to **margies-skillset**.
    - Select the option **Enable OCR and merge all text into merged_content field**.
    - Set the **Source data field** to **merged_content**.
    - Leave the **Enrichment granularity level** as **Source field**, which is set the enture contents of the document being indexed; but note that you can change this to extract information at more granular levels, like pages or sentences.
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
9. Make the following changes to the index fields, leaving all other fields with their default settings:

    | Field name | Retrievable | Filterable | Sortable | Facetable | Searchable |
    | ---------- | ----------- | ---------- | -------- | --------- | ---------- |
    | metadata_storage_size | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | | |
    | metadata_storage_last_modified | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | | |
    | metadata_storage_name | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; |
    | metadata_author | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; |
    | locations | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | | | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; |
    | language | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10004; | | | |

11. Proceed to the next step (*Create an indexer*).
12. Change the **Indexer name** to **margies-indexer**.
13. Leave the **Schedule** set to **Once**.
14. Expand the **Advanced** options, and ensure that the **Base-64 encode keys** option is selected (generally encoding keys make the index more efficient).
15. Select **Submit** to create the data source, skillset, index, and indexer. The indexer is run automatically and runs the indexing pipeline, which:
    1. Extracts the document metadata fields and content from the data source
    2. Runs the skillset of cognitive skills to generate additional enriched fields
    3. Maps the extracted fields to the index.
16. View the **Indexers** tab of the blade for your Azure Cognitive Search resource, which should show the newly created **margies-indexer**. Wait a few minutes, and click **&orarr; Refresh** until the **Status** indicates success.

## Search the index

Now that you have an index, you can search it.

1. At the top of the blade for your Azure Cognitive Search resource, select **Search explorer**.
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

1. In the Azure portal, return to the **Overview** page on the blade for your Azure Cognitive Search resource, and select the **Data sources** tab. The **margies-data** data source should be listed.
2. Select the **margies-data** data source to view its settings. Then select the **Data Source Definition (JSON)** tab to view the JSON definition of the data source. This includes the information it uses to connect to the blob container in your storage account.
3. Close the **margies-data** page to return to the blade for your Azure Cognitive Search resource.

### Review and modify the skillset

1. On the blade for your Azure Cognitive Search resource, select the **Skillsets** tab, where **margies-skillset** should be listed.
2. Select **margies-skillset** and view the **Skillset Definition (JSON)** page. This shows a JSON definition that includes the six skills you specified in the user interface previously.
3. On the right side of the page, note that there are templates for additional skills you might want to add to the skillset. For example, it would be good to identify the *sentiment* of the documents being indexed - particularly the hotel reviews, so we can easily find reviews that are positive or negative.
4. In the **Skills** list, select **Sentiment Skill** to show a JSON template for this skill.
5. Copy the template to the clipboard, and then on the left side, in the JSON for your skillset definition, paste the copied skill in a newly inserted line immediately after the following line (which should be line 6) - be careful not to overwrite the **{** marking the beginning of the first existing skill:


    "skills": [


6. Add a comma immediately after the newly inserted skill, and reformat the JSON indentation to make it more readable. It should look like this:

```
{
"@odata.context": "https://....",
"@odata.etag": "\"....\"",
"name": "margies-skillset",
"description": "Skillset created from the portal....",
"skills": [
    /* Insert the new skill here */
    {
        "@odata.type": "#Microsoft.Skills.Text.SentimentSkill",
        "defaultLanguageCode": "",
        "name": "",
        "description": "",
        "context": "",
        "inputs": [
            {
                "name": "text",
                "source": ""
            },
            {
                "name": "languageCode",
                "source": ""
            }
        ],
        "outputs": [
            {
                "name": "score",
                "targetName": "score"
            }
        ]
    },
    /* Add a comma after the new skill, before the first existing skill */
    {
        "@odata.type": "#Microsoft.Skills.Text.EntityRecognitionSkill",
        "name": "#1",
        ...
}
```

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

The new skill is named **get-sentiment**, and will evaluate the text found in the **merged_content** field of the document being indexed (which includes the source content as well as any text extracted from images in the content). It uses the extracted **language** of the document (with a default of English), and evaluates a score for the sentiment of the content. This score is then  output as a new field named **sentimentScore** at the **document** level of the object that represents the indexed document.

8. Select **Save** to save the skillset with the new skill.
9. Close the **margies-skillset** page to return to the blade for your Azure Cognitive Search resource.

### Review and modify the index

1. On the blade for your Azure Cognitive Search resource, select the **Indexes** tab (<u>not</u> *Indexers*), where **margies-index** should be listed.
2. Select **margies-index** and view the **Index Definition (JSON)** page. This shows a JSON definition for your index, including definitions for each field. Some fields are based on metadata and content in the source document, and others are the results of skills in the skillset.
3. You added a skill to the skillset to extract a sentiment score for the document. Now you must add a corresponding field in the index to which this value can be mapped. At the bottom of the **fields** list (before the closing **]**, which is followed by index properties such as **suggesters**), add the following field (being sure to include the comma at the beginning, after the previous field):

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
3. In the **fieldMappings** section, after the existing mapping for the **metadata_storage_path** value to the base-54 encoded key field, add another mapping to map the same value to the **url** field, so that the entire **fieldMappings** section looks like this (be sure to include the comma between the existing mapping and the new one):

```
"fieldMappings": [
    {
    "sourceFieldName": "metadata_storage_path",
    "targetFieldName": "metadata_storage_path",
    "mappingFunction": {
        "name": "base64Encode",
        "parameters": null
        }
    },
    {
        "sourceFieldName" : "metadata_storage_path",
        "targetFieldName" : "url"
    }
],
```

All of the other metadata and content field in the source document are implicitly mapped to fields of the same name in the index.

4. At the end of the **ouputFieldMappings** section, add the following mapping to map the **sentimentScore** value extracted by your sentiment skill to the **sentiment** field you added to the index:

```
,
{
    "sourceFieldName": "/document/sentimentScore",
    "targetFieldName": "sentiment"
}
```

5. Select **Save** to save the indexer with the new mappings.
6. Select **Reset** to reset the index, and confirm that you want to do this when prompted. You've added new fields to an already-populated index, so you'll need to reset and reindex to update the existing index records with the new field values.
7. Select **Run** to run the updated indexer, confirming that you want to run it now when prompted.

    *Note that in a free-tier resource, you can only run the indexer once every three minutes; so if you have already run the indexer recently, you may need to wait before running it again.*

8. Close the **margies-indexer** page to return to the blade for your Azure Cognitive Search resource, and select **Refresh** to track the progress of the indexing operation. It may take a minute or so to complete.

    *There may be some warnings for a few documents that are too large to evaluate sentiment. Often sentiment analysis is performed at the page or sentence level rather than the full document; but in this case scenario, most of the documents - particularly the hotel reviews, are short enough for useful document-level sentiment scores to be evaluated.*

### Query the modified index

1. At the top of the blade for your Azure Cognitive Search resource, select **Search explorer**.
2. In Search explorer, in the **Query string** box, enter the following query string, and then select **Search**.

    ```
    search=London&$select=url,sentiment,keyphrases&$filter=metadata_author eq 'Reviewer' and sentiment gt 0.5
    ```

    This query retrieves the **url**, **sentiment**, and **keyphrases** for all documents that mention *London* authored by *Reviewer* that have a **sentiment** score greater than *0.5* (in other words, positive reviews that mention London)

## Create a search client application

Now that you have a useful index, you can use it from a client application. You can do this by consuming the REST interface, submitting requests and receiving responses in JSON format over HTTP; or you can use the software development kit (SDK) for your preferred programming language. In this exercise, we'll use the SDK.

> **Note**: You can choose to use the SDK for either **C#** or **Python**. In the steps below, perform the actions appropriate for your preferred language.

### Get the endpoint and keys for your search resource

1. In the Azure portal, return to the blade for your Azure Cognitive Search resource and on the **Overview** page, note the **Url** value, which should be similar to **https://*your_resource_name*.search.windows.net**. This is the endpoint for your search resource.
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

### Add code to search an index

The **margies-travel** folder contains code files for a web application (a Microsoft C# *ASP&period;NET Razor* web application or a Python *Flask* application), which you will update to include search functionality.

1. Open the following code file in the web application, depending on your choice of programming language:
    - **C#**:Pages/Index.cshtml.cs
    - **Python**: app&period;py
2. Near the top of the code file, find the comment **Import namespaces**, and add the following code below this comment:

**C#**

```C#
// Import namespaces
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
```

**Python**

```Python
# Import namespaces
from azure.core.credentials import AzureKeyCredential
from azure.search.documents import SearchClient
```

3. In the **search_query** function, find the comment **Create a search client**, and add the following code:

**C#**

```C#
// Create a search client
AzureKeyCredential credential = new AzureKeyCredential(QueryKey);
SearchClient searchClient = new SearchClient(SearchEndpoint, IndexName, credential);
```

**Python**

```Python
# Create a search client
azure_credential = AzureKeyCredential(search_key)
search_client = SearchClient(search_endpoint, search_index, azure_credential)
```

4. In the **search_query** function, find the comment **Submit search query**, and add the following code to submit a search for the specified text with the following options:
    - A *search mode* that requires **all** of the individual words in the search text are found.
    - The total number of documents found by the search is included in the results.
    - The results are filtered to include only documents that match the provided filter expression.
    - The results are sorted into the specified sort order.
    - Each discrete value of the **metadata_author** field is returned as a *facet* that can be used to display pre-defined values for filtering.
    - Up to three extracts of the **merged_content** and **imageCaption** fields with the search terms highlighted are included in the results.
    - The results include only the fields specified.

**C#**

```C#
// Submit search query
var options = new SearchOptions{
    IncludeTotalCount = true,
    SearchMode = SearchMode.All,
    Filter = FilterExpression,
    OrderBy = {SortOrder},
    Facets = {"metadata_author"},
    HighlightFields = {"merged_content-3","imageCaption-3"} 
};
options.Select.Add("url");
options.Select.Add("metadata_storage_name");
options.Select.Add("metadata_author");
options.Select.Add("metadata_storage_size");
options.Select.Add("metadata_storage_last_modified");
options.Select.Add("language");
options.Select.Add("sentiment");
options.Select.Add("merged_content");
options.Select.Add("keyphrases");
options.Select.Add("locations");
options.Select.Add("imageTags");
options.Select.Add("imageCaption");
SearchResults<SearchResult> results = searchClient.Search<SearchResult>(SearchTerms, options);
return results;
```

**Python**

```Python
# Submit search query
results =  search_client.search(search_text,
                                search_mode="all",
                                include_total_count=True,
                                filter=filter_by,
                                order_by=sort_order,
                                facets=['metadata_author'],
                                highlight_fields='merged_content-3,imageCaption-3',
                                # select fields on a single line
                                select = "url,metadata_storage_name,metadata_author,metadata_storage_last_modified,language,sentiment,merged_content,keyphrases,locations,imageTags,imageCaption")
return results
```

5. Save your changes.

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

2. In the message that is displayed when the app starts successfully, follow the link to the running web application (*https://localhost:5000/* or *https://127.0.0.1:5000/*) to open the Margies Travel site in a web browser.
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
