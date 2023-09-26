---
lab:
    title: 'Create a language understanding model with the Azure AI Language service'
    module: 'Module 5 - Create language understanding solutions'
---

# Create a language understanding model with the Language service

> [!NOTE]
> The conversational language understanding feature of the Azure AI Language service is currently in preview, and subject to change. In some cases, model training may fail - if this happens, try again.  

The Azure AI Language service enables you to define a *conversational language understanding* model that applications can use to interpret natural language input from users,  predict the users *intent* (what they want to achieve), and identify any *entities* to which the intent should be applied.

For example, a conversational language model for a clock application might be expected to process input such as:

*What is the time in London?*

This kind of input is an example of an *utterance* (something a user might say or type), for which the desired *intent* is to get the time in a specific location (an *entity*); in this case, London.

> [!NOTE]
> The task of a conversational language model is to predict the user's intent and identify any entities to which the intent applies. It is <u>not</u> the job of a conversational language model to actually perform the actions required to satisfy the intent. For example, a clock application can use a conversational language model to discern that the user wants to know the time in London; but the client application itself must then implement the logic to determine the correct time and present it to the user.

## Create an Azure AI Language resource

To create a conversational language model, you need a **Azure AI Language service** resource in a supported region.

1. Open the Azure portal at `https://portal.azure.com`, and sign in using the Microsoft account associated with your Azure subscription.
1. In the search field at the top, search for **Azure AI services**. Then, in the results, select **Create** under **Language Service**.
1. Select **Continue to create your resource**.
1. Provision the resource using the following settings:
    - **Subscription**: *Your Azure subscription*.
    - **Resource group**: *Choose or create a resource group (if you are using a restricted subscription, you may not have permission to create a new resource group - use the one provided)*.
    - **Region**: West US 2 or West Europe.
    - **Name**: *Enter a unique name*.
    - **Pricing tier**: Select either **Free (F0)** or **Standard (S)** tier if Free is not available.
    - **Responsible AI Notice**: Agree.
1. Select **Review + create**.
1. Wait for deployment to complete, and then view the deployment details.

## Create a conversational language understanding project

Now that you have created an authoring resource, you can use it to create a conversational language understanding project.

1. In a new browser tab, open the Language Studio portal at `https://language.cognitive.azure.com/` and sign in using the Microsoft account associated with your Azure subscription.

1. If prompted to choose a Language resource, select the following settings:

    - **Azure Directory**: The Azure directory containing your subscription.
    - **Azure subscription**: Your Azure subscription.
    - **Resource type**: Language.
    - **Language resource**: The Azure AI Language resource you created previously.

1. If you are <u>not</u> prompted to choose a language resource, it may be because you have already assigned a different Azure AI Language resource; in which case:

    1. On the bar at the top if the page, select the **Settings (&#9881;)** button.
    2. On the **Settings** page, view the **Resources** tab.
    3. Select the language resource you just created, and select **Switch resource**.
    4. At the top of the page, select **Language Studio** to return to the Language Studio home page.

1. At the top of the portal, in the **Create new** menu, select **Conversational language understanding**.

1. In the **Create a project** dialog box, on the **Enter basic information** page, enter the following details and then select **Next**:
    - **Name**: `Clock`
    - **Description**: `Natural language clock`
    - **Utterances primary language**: English
    - **Enable multiple languages in project?**: *Unselected*

1. On the **Review and finish** page, select **Create**.

## Create intents

The first thing we'll do in the new project is to define some intents.

> **Tip**: When working on your project, if some tips are displayed, read them and select **Got it** to dismiss them, or select **Skip all**.

1. On the **Schema definition** page, on the **Intents** tab, select **&#65291; Add** to add a new intent named **GetTime**.

1. Select the new **GetTime** intent to edit it, add the following utterances as example user input:

    `what is the time?`

    `what's the time?`

    `what time is it?`

    `tell me the time`

1. After you've added these utterances, select **Save changes** and go back to the **Schema definition** page.

1. Add another new intent named **GetDay** with the following utterances:

    `what day is it?`

    `what's the day?`

    `what is the day today?`

    `what day of the week is it?`

1. After you've added these utterances and saved them, go back to the **Schema definition** page and add another new intent named **GetDate** with the following utterances:

    `what date is it?`

    `what's the date?`

    `what is the date today?`

    `what's today's date?`

1. After you've added these utterances, save them and clear the **GetDate** filter on the utterances page so you can see all of the utterances for all of the intents. To do this select the filter button on the top right of the Training set tab then unselect **GetDate**.

## Train and test the model

Now that you've added some intents, let's train the language model and see if it can correctly predict them from user input.

1. In the pane on the left, select **Training jobs**. Then select **+ Start a training job**.

1. On the **Start a training job** dialog, select the option to train a new model, name it **Clock**.

1. To begin the process of training your model, select **Train**.

1. When training is complete (which may take several minutes) the job **Status** will change to **Training succeeded**.

1. Select the **Model performance** page, and then select the **Clock** model. Review the overall and per-intent evaluation metrics (*precision*, *recall*, and *F1 score*) and the *confusion matrix* generated by the evaluation that was performed when training (note that due to the small number of sample utterances, not all intents may be included in the results).

    > [!NOTE]
> > To learn more about the evaluation metrics, refer to the [documentation](https://learn.microsoft.com/azure/ai-services/language-service/conversational-language-understanding/concepts/evaluation-metrics)

1. Go to the **Deploying a model** page, then select **Add deployment**.

1. On the **Add deployment** dialog, select **Create a new deployment name**, and then enter **production**.

1. Select the **Clock** model in the **Model** field then select **Deploy**. The deployment may take some time.

1. When the model has been deployed, select the **Testing deployments** page, then select the **production** deployment in the **Deployment name** field.

1. Enter the following text in the empty textbox, and then select **Run the test**:

    `what's the time now?`

    Review the result that is returned, noting that it includes the predicted intent (which should be **GetTime**) and a confidence score that indicates the probability the model calculated for the predicted intent. The JSON tab shows the comparative confidence for each potential intent (the one with the highest confidence score is the predicted intent)

1. Clear the text box, and then run another test with the following text:

    `tell me the time`

    Again, review the predicted intent and confidence score.

1. Try the following text:

    `what's the day today?`

    Hopefully the model predicts the **GetDay** intent.

## Add entities

So far you've defined some simple utterances that map to intents. Most real applications include more complex utterances from which specific data entities must be extracted to get more context for the intent.

### Add a learned entity

The most common kind of entity is a *learned* entity, in which the model learns to identify entity values based on examples.

1. In Language Studio, return to the **Schema definition** page and then on the **Entities** tab, select **&#65291; Add** to add a new entity.

1. In the **Add an entity** dialog box, enter the entity name **Location** and ensure that the **Learned** tab is selected. Then select **Add entity**.

1. After the **Location** entity has been created, return to the **Schema definition** page and then on the **Intents** tab, select the **GetTime** intent.

1. Enter the following new example utterance:

    `what time is it in London?`

1. When the utterance has been added, select the word **London**, and in the drop-down list that appears, select **Location** to indicate that "London" is an example of a location.

1. Add another example utterance:

    `Tell me the time in Paris?`

1. When the utterance has been added, select the word **Paris**, and map it to the **Location** entity.

1. Add another example utterance:

    `what's the time in New York?`

1. When the utterance has been added, select the words **New York**, and map them to the **Location** entity.

1. Select **Save changes** to save the new utterances.

### Add a *list* entity

In some cases, valid values for an entity can be restricted to a list of specific terms and synonyms; which can help the app identify instances of the entity in utterances.

1. In Language Studio, return to the **Schema definition** page and then on the **Entities** tab, select **&#65291; Add** to add a new entity.

1. In the **Add an entity** dialog box, enter the entity name **Weekday** and select the **List** entity tab. Then select **Add entity**.

1. On the page for the **Weekday** entity, in the **List** section, select **&#65291; Add new list**. Then enter the following value and synonym and select **Save**:

    | List key | synonyms|
    |-------------------|---------|
    | Sunday | Sun |

1. Repeat the previous step to add the following list components:

    | Value | synonyms|
    |-------------------|---------|
    | Monday | Mon |
    | Tuesday | Tue, Tues |
    | Wednesday | Wed, Weds |
    | Thursday | Thur, Thurs |
    | Friday | Fri |
    | Saturday | Sat |

1. Return to the **Schema definition** page and then on the **Intents** tab, select the **GetDate** intent.

1. Enter the following new example utterance:

    `what date was it on Saturday?`

1. When the utterance has been added, select the word ***Saturday***, and in the drop-down list that appears, select **Weekday**.

1. Add another example utterance:

    `what date will it be on Friday?`

1. When the utterance has been added, map **Friday** to the **Weekday** entity.

1. Add another example utterance:

    `what will the date be on Thurs?`

1. When the utterance has been added, map **Thurs** to the **Weekday** entity.

1. select **Save changes** to save the new utterances.

### Add a *prebuilt* entity

The Azure AI Language service provides a set of *prebuilt* entities that are commonly used in conversational applications.

1. In Language Studio, return to the **Schema definition** page and then on the **Entities** tab, select **&#65291; Add** to add a new entity.

1. In the **Add an entity** dialog box, enter the entity name **Date** and select the **Prebuilt** entity tab. Then select **Add entity**.

1. On the page for the **Date** entity, in the **Prebuilt** section, select **&#65291; Add new prebuilt**.

1. In the **Select prebuilt** list, select **DateTime** and then select **Save**.

1. Return to the **Schema definition** page and then on the **Intents** tab, select the **GetDay** intent.

1. Enter the following new example utterance:

    `what day was 01/01/1901?`

1. When the utterance has been added, select ***01/01/1901***, and in the drop-down list that appears, select **Date**.

1. Add another example utterance:

    `what day will it be on Dec 31st 2099?`

1. When the utterance has been added, map **Dec 31st 2099** to the **Date** entity.

1. Select **Save changes** to save the new utterances.

### Retrain the model

Now that you've modified the schema, you need to retrain and retest the model.

1. On the **Training jobs** page, select **Start a training job**.

1. On the **Start a training job** dialog,  select  **overwrite an existing model** and specify the **Clock** model. Select **Train** to train the model. If prompted, confirm you want to overwrite the existing model.

1. When training is complete the job **Status** will update to **Training succeeded**.

1. Select the **Model performance** page and then select the **Clock** model. Review the evaluation metrics (*precision*, *recall*, and *F1 score*) and the *confusion matrix* generated by the evaluation that was performed when training (note that due to the small number of sample utterances, not all intents may be included in the results).

1. On the **Deploying a model** page, select **Add deployment**.

1. On the **Add deployment** dialog, select **Override an existing deployment name**, and then select **production**.

1. Select the **Clock** model in the **Model** field and then select **Deploy** to deploy it. This may take some time.

1. When the model is deployed, on the **Testing deployments** page, select the **production** deployment under the **Deployment name** field, and then test it with the following text:

    `what's the time in Edinburgh?`

1. Review the result that is returned, which should hopefully predict the **GetTime** intent and a **Location** entity with the text value "Edinburgh".

1. Try testing the following utterances:

    `what time is it in Tokyo?`

    `what date is it on Friday?`

    `what's the date on Weds?`

    `what day was 01/01/2020?`

    `what day will Mar 7th 2030 be?`

## Use the model from a client app

In a real project, you'd iteratively refine intents and entities, retrain, and retest until you are satisfied with the predictive performance. Then, when you've tested it and are satisfied with its predictive performance, you can use it in a client app by calling its REST interface. In this exercise, you'll use the *curl* utility to call the REST endpoint for your model.

1. In Language Studio, on the **Deploying a model** page, select the **production** deployment. Then select **Get prediction URL**.

1. In the **Get prediction URL** dialog box, note that the URL for the prediction endpoint is shown along with a sample request, which consists of a **curl** command that submits an HTTP POST request to the endpoint, specifying the key for your Azure AI Language resource in the header and including a query and language in the request data.

## Call the API from the Azure Cloud Shell

Open up a new internet browser tab to work with Cloud Shell.

1. In the [Azure portal](https://portal.azure.com?azure-portal=true), select the **[>_]** (*Cloud Shell*) button at the top of the page to the right of the search box. A Cloud Shell pane will open at the bottom of the portal.

    ![Screenshot of starting Cloud Shell by clicking on the icon to the right of the top search box.](images/cloudshell-launch-portal.png#lightbox)

1. The first time you open the Cloud Shell, you may be prompted to choose the type of shell you want to use (*Bash* or *PowerShell*). Select **Bash**. If you don't see this option, skip the step.  

1. If you're prompted to create storage for your Cloud Shell, ensure your subscription is specified and select **Create storage**. Then wait a minute or so for the storage to be created.

1. Make sure the type of shell indicated on the top left of the Cloud Shell pane is switched to *Bash*. If it's *PowerShell*, switch to *Bash* by using the drop-down menu in the top left.

1. Once the terminal starts, run the following commands to download a copy of the repo into your Cloud Shell:

    ```bash
    rm -r azure-ai-eng -f
    git clone https://github.com/MicrosoftLearning/AI-102-AIEngineer azure-ai-eng
    ```

1. The files have been downloaded into a folder called **azure-ai-eng**. Let's change into that folder by running:

    ```bash
    cd azure-ai-eng/09-language-app
    ```

1. Then run `code send-call.sh` to open the file in the Cloud Shell editor. This file contains a script that will call the service with the question: "What's the time in Sydney?".
1. Replace the following values from the corresponding values in the sample request from Language Studio:

    - **<ENDPOINT_URL>**: Your endpoint URL.Looks like: `https://my-service.cognitiveservices.azure.com/language/:analyze-conversations?api-version=2022-10-01-preview`
    - **<YOUR_KEY>**: Your key. Looks like: `b11bcsbd50a149dfb6626791d20f514b`
    - <**REQUEST_ID>**: Your request ID. Looks like: `4vfdad1c-b2fc-48ba-bd7d-b59d2242395b`

1. Press **CTRL + Save** to save your changes.
1. Make a call by running `sh send-call.sh`.
1. View the resulting JSON, which should include the predicted intent and entities, like this:

    ```json
    {
      "kind": "ConversationResult",
      "result": {
        "query": "What's the time in Sydney",
        "prediction": {
          "topIntent": "GetTime",
          "projectKind": "Conversation",
          "intents": [
            {
              "category": "GetTime",
              "confidenceScore": 0.9135122
            },
            {
              "category": "GetDay",
              "confidenceScore": 0.61633164
            },
            {
              "category": "GetDate",
              "confidenceScore": 0.601757
            },
            {
              "category": "None",
              "confidenceScore": 0
            }
          ],
          "entities": [
            {
              "category": "Location",
              "text": "Sydney",
              "offset": 19,
              "length": 6,
              "confidenceScore": 1
            }
          ]
        }
      }
    }
    ```

1. Review the JSON response returned by your model to ensure that the top scoring intent predicted is **GetTime**.

1. Change the query in the curl command to `What's today's date?` and then run it and review the resulting JSON.

1. Try the following queries:

    `What day will Jan 1st 2050 be?`

    `What time is it in Glasgow?`

    `What date will next Monday be?`

## Export the project

You can use Language Studio to develop and test your language understanding model, but in a software development process for DevOps, you should maintain a source controlled definition of the project that can be included in continuous integration and delivery (CI/CD) pipelines. While you can use the Azure AI Language REST API in code scripts to create and train the model, a simpler way is to use the portal to create the model schema, and export it as a **.json** file that can be imported and retrained in another Azure AI Language  instance. This approach enables you to make use of the productivity benefits of the Language Studio visual interface while maintaining portability and reproducibility for the model.

1. Select the**Projects** tab, select the circle icon to select the **Clock** project.

1. Select the **&#x2913; Export** button.

1. Save the **Clock.json** file that is generated (anywhere you like).

1. Open the downloaded file in your favorite code editor (for example, Visual Studio Code) to review the JSON definition of your project.

## More information

For more information about using the **Azure AI Language** service to create language understanding solutions, see the [Azure AI Language documentation](/azure/ai-services/language-service/conversational-language-understanding/overview).
