---
lab:
    title: 'Create a language understanding model with the Language service (preview)'
---

# Create a language understanding model with the Language service (preview)

> **Note**: The conversational language understanding feature of the Language service is currently in preview, and subject to change. In some cases, model training may fail - if this happens, try again.  

The Language service enables you to define a *conversational language understanding* model that applications can use to interpret natural language input from users,  predict the users *intent* (what they want to achieve), and identify any *entities* to which the intent should be applied.

For example, a language understanding model for a clock application might be expected to process input such as:

*What is the time in London?*

This kind of input is an example of an *utterance* (something a user might say or type), for which the desired *intent* is to get the time in a specific location (an *entity*); in this case, London.

> **Note**: The task of the language understanding model is to predict the user's intent, and identify any entities to which the intent applies. It is <u>not</u> its job to actually perform the actions required to satisfy the intent. For example, the clock application can use a language app to discern that the user wants to know the time in London; but the client application itself must then implement the logic to determine the correct time and present it to the user.

## Create a Language service resource

To create a conversational language understanding model, you need a **Language service** resource in a supported region. At the time of writing, only the West US 2 and West Europe regions are supported.

1. Open the Azure portal at `https://portal.azure.com`, and sign in using the Microsoft account associated with your Azure subscription.
2. Select the **&#65291;Create a resource** button, search for *language*, and create a **Language service** resource with the following settings.

    - **Subscription**: *Your Azure subscription*
    - **Resource group**: *Choose or create a resource group (if you are using a restricted subscription, you may not have permission to create a new resource group - use the one provided)*
    - **Region**: West US 2 or West Europe
    - **Name**: *Enter a unique name*
    - **Pricing tier**: Free (F0) (*If this tier is not available, select Standard (S)*)
    - **Legal Terms**: _Agree_ 
    - **Responsible AI Notice**: _Agree_
3. Wait for deployment to complete, and then view the deployment details.

## Create a conversational language understanding project

Now that you have created an authoring resource, you can use it to create a conversational language understanding project.

1. In a new browser tab, open the Language Studio portal at `https://language.azure.com` and sign in using the Microsoft account associated with your Azure subscription.
2. If prompted to choose a Language resource, select the following settings:
    - **Azure Directory**: The Azure directory containing your subscription.
    - **Azure subscription**: Your Azure subscription.
    - **Language resource**: The Language resource you created previously.
3. If you are <u>not</u> prompted to choose a language resource, it may be because you have already assigned a different language resource; in which case:
    1. On the bar at the top if the page, click the **Settings (&#9881;)** button.
    2. On the **Settings** page, view the **Resources** tab.
    3. Select the language resource you just created, and click **Switch resource**.
    4. At the top of the page, click **Language Studio** to return to the Language Studio home page.
4. At the top of the portal, in the **Create new** menu, select **Conversational language understanding**.
5. In the **Create a project** dialog box, in the **Choose project type** page, select **Conversation** and click **Next**.
6. On the **Enter basic information** page, enter the following details and click **Next**:
    - **Name**: `Clock`
    - **Description**: `Natural language clock`
    - **Utterances primary language**: English
    - **Enable multiple languages in project?**: *Unselected*
7. On the **Review and finish** page, click **Create**.

## Create intents

The first thing we'll do in the new project is to define some intents.

> **Tip**: When working on your project, if some tips are displayed, read them and click **Got it** to dismiss them, or click **Skip all**.

1. On the **Build schema** page, on the **Intents** tab, select **&#65291; Add** to add a new intent named **GetTime**.
2. Click the new **GetTime** intent to edit it, add the following utterances as example user input:

    `what is the time?`

    `what's the time?`

    `what time is it?`

    `tell me the time`

3. After you've added these utterances, click **Save changes** and go back to the **Build schema** page.

4. Add another new intent named **GetDay** with the following utterances:

    `what day is it?`

    `what's the day?`

    `what is the day today?`

    `what day of the week is it?`

5. After you've added these utterances and saved them, go back to the **Build schema** page and add another new intent named **GetDate** with the following utterances:

    `what date is it?`

    `what's the date?`

    `what is the date today?`

    `what's today's date?`

6. After you've added these utterances, save them and clear the **GetDate** filter on the utterances page so you can see all of the utterances for all of the intents.

## Train and test the model

Now that you've added some intents, let's train the language model and see if it can correctly predict them from user input.

1. In the pane on the left, select the **Train model** page and select the option to train a new model, naming it **Clock** and ensuring that the option to run evaluation when training is selected. Click **Train** to train the model.
2. When training is complete (which may take some time), view the **View model details** page and select the **Clock** model. Then view the overall and per-intent evaluation metrics (*precision*, *recall*, and *F1 score*) and the *confusion matrix* generated by the evaluation that was performed when training (note that due to the small number of sample utterances, not all intents may be included in the results).

    >**Note**: To learn more about the evaluation metrics, refer to the [documentation](https://docs.microsoft.com/azure/cognitive-services/language-service/conversational-language-understanding/concepts/evaluation-metrics)

3. On the **Deploy model** page, select the **Clock** model and deploy it. This may take some time.
4. When the model has been deployed, on the **Test model** page, select the **Clock** model.
5. Enter the following text, and then click **Run the test**:

    `what's the time now?`

    Review the result that is returned, noting that it includes the predicted intent (which should be **GetTime**) and a confidence score that indicates the probability the model calculated for the predicted intent. The JSON tab shows the comparative confidence for each potential intent (the one with the highest confidence score is the predicted intent)

6. Clear the text box, and then run another test with the following text:

    `tell me the time`

    Again, review the predicted intent and confidence score.

7. Try the following text:

    `what's the day today?`

    Hopefully the model predicts the **GetDay** intent.

## Add entities

So far you've defined some simple utterances that map to intents. Most real applications include more complex utterances from which specific data entities must be extracted to get more context for the intent.

### Add a *learned* entity

The most common kind of entity is a *learned* entity, in which the model learns to identify entity values based on examples.

1. In Language Studio, return to the **Build schema** page and then on the **Entities** tab, select **&#65291; Add** to add a new entity.
2. In the **Add an entity** dialog box, enter the entity name **Location** and ensure that **Learned** is selected. Then click **Add entity**.
3. After the **Location** entity has been created, return to the **Build schema** page and then on the **Intents** tab, select the **GetTime** intent.
4. Enter the following new example utterance:

    `what time is it in London?`

5. When the utterance has been added, select the word ***London***, and in the drop-down list that appears, select **Location** to indicate that "London" is an example of a location.
6. Add another example utterance:

    `Tell me the time in Paris?`

7. When the utterance has been added, select the word ***Paris***, and map it to the **Location** entity.
8. Add another example utterance:

    `what's the time in New York?`

9. When the utterance has been added, select the words ***New York***, and map them to the **Location** entity.

10. Click **Save changes** to save the new utterances.

### Add a *list* entity

In some cases, valid values for an entity can be restricted to a list of specific terms and synonyms; which can help the app identify instances of the entity in utterances.

1. In Language Studio, return to the **Build schema** page and then on the **Entities** tab, select **&#65291; Add** to add a new entity.
2. In the **Add an entity** dialog box, enter the entity name **Weekday** and select the **List** entity type. Then click **Add entity**.
3. in the page for the **Weekday** entity, in the **List** section, click **&#65291; Add new list**. Then enter the following value and synonym and click **Save**:

    | Value | synonyms|
    |-------------------|---------|
    | sunday | sun |

4. Repeat the previous step to add the following list components:

    | Value | synonyms|
    |-------------------|---------|
    | Monday | Mon |
    | Tuesday | Tue, Tues |
    | Wednesday | Wed, Weds |
    | Thursday | Thur, Thurs |
    | Friday | Fri |
    | Saturday | Sat |

5. Return to the **Build schema** page and then on the **Intents** tab, select the **GetDate** intent.
6. Enter the following new example utterance:

    `what date was it on Saturday?`

7. When the utterance has been added, select the word ***Saturday***, and in the drop-down list that appears, select **Weekday**.
8. Add another example utterance:

    `what date will it be on Friday?`

9. When the utterance has been added, map **Friday** to the **Weekday** entity.

10. Add another example utterance:

    `what will the be on Thurs?`

11. When the utterance has been added, map **Thurs** to the **Weekday** entity.

12. Click **Save changes** to save the new utterances.

### Add a *prebuilt* entity

The Language service provides a set of *prebuilt* entities that are commonly used in conversational applications.

1. In Language Studio, return to the **Build schema** page and then on the **Entities** tab, select **&#65291; Add** to add a new entity.
2. In the **Add an entity** dialog box, enter the entity name **Date** and select the **prebuilt** entity type. Then click **Add entity**.
3. On the page for the **Date** entity, in the **Prebuilt** section, click **&#65291; Add new prebuilt**.
4. In the **Select prebuilt** list, select **DateTime** and then click **Save**.
5. Return to the **Build schema** page and then on the **Intents** tab, select the **GetDay** intent.
6. Enter the following new example utterance:

    `what day was 01/01/1901?`

7. When the utterance has been added, select ***01/01/1901***, and in the drop-down list that appears, select **Date**.
8. Add another example utterance:

    `what day will it be on Dec 31st 2099?`

9. When the utterance has been added, map **Dec 31st 2099** to the **Date** entity.

10. Click **Save changes** to save the new utterances.

### Retrain the model

Now that you've modified the schema, you need to retrain and retest the mode.

1. On the **Train model** page, select the option to overwrite an existing model and specify the **Clock** model. Ensure that the option to run evaluation when training is selected and click **Train** to train the model; confirming you want to overwrite the existing model.
2. When training is complete (which may take some time), view the **View model details** page and select the **Clock** model. Then view the overall, per-entity, and per-intent evaluation metrics (*precision*, *recall*, and *F1 score*) and the *confusion matrix* generated by the evaluation that was performed when training (note that due to the small number of sample utterances, not all intents may be included in the results).
3. On the **Deploy model** page, select the **Clock** model and deploy it. This may take some time.
4. When the app is deployed, on the **Test model** page, select the **Clock** model, and then test it with the following text:

    `what's the time in Edinburgh?`

5. Review the result that is returned, which should hopefully predict the **GetTime** intent and a **Location** entity with the text value "Edinburgh".
6. Try testing the following utterances:

    `what time is it in Tokyo?`
    
    `what date is it on Friday?`

    `what's the date on Weds?`

    `what day was 01/01/2020?`

    `what day will Mar 7th 2030 be?`

## Use the model from a client app

In a real project, you'd iteratively refine intents and entities, retrain, and retest until you are satisfied with the predictive performance. Then, when you've tested it and are satisfied with its predictive performance, you can use it in a client app by calling its REST interface. In this exercise, you'll use the *curl* utility to call the REST endpoint for your model.

1. In Language Studio, on the **Deploy model** page, select the **Clock** model. Then click **Get prediction URL**.
2. In the **Get prediction URL** dialog box, note that the URL for the prediction endpoint is shown along with a sample request, which consists of a **curl** command that submits an HTTP POST request to the endpoint, specifying the key for your Language resource in the header and including a query and language in the request data.
3. Copy the sample request, and paste it into your preferred text editor (for example Notepad).
4. Replace the following placeholders:
    - **YOUR_QUERY_HERE**: *What's the time in Sydney*
    - **QUERY_LANGUAGE_HERE**: *EN*

    The command should resemble the following code:

    ```
    curl -X POST "https://some-name.cognitiveservices.azure.com/language/:analyze-conversations?projectName=Clock&deploymentName=production&api-version=2021-11-01-preview" -H "Ocp-Apim-Subscription-Key: 0ab1c23de4f56..."  -H "Apim-Request-Id: 9zy8x76wv5u43...." -H "Content-Type: application/json" -d "{\"verbose\":true,\"query\":\"What's the time in Sydney?\",\"language\":\"EN\"}"
    ```

5. Open a command prompt (Windows) or bash shell (Linux/Mac).
6. Copy and paste the edited curl command to your command line interface and run it.
7. View the resulting JSON, which should include the predicted intent and entities, like this:

    ```
    {"query":"What's the time in Sydney?","prediction":{"topIntent":"GetTime","projectKind":"conversation","intents":[{"category":"GetTime","confidenceScore":0.9998859},{"category":"GetDate","confidenceScore":9.8372206E-05},{"category":"GetDay","confidenceScore":1.5763446E-05}],"entities":[{"category":"Location","text":"Sydney","offset":19,"length":6,"confidenceScore":1}]}}
    ```

8. Review the JSON response returned by your app, which should indicate the top scoring intent predicted for your input (which should be **GetTime**).
9. Change the query in the curl command to `What's today's date?` and then run it and review the resulting JSON.
10. Try the following queries:

    `What day will Jan 1st 2050 be?`

    `What time is it in Glasgow?`

    `What date will next Monday be?`

## Export the project

You can use Language Studio to develop and test your language understanding model, but in a software development process for DevOps, you should maintain a source controlled definition of the project that can be included in continuous integration and delivery (CI/CD) pipelines. While you *can* use the Language REST API in code scripts to create and train the model, a simpler way is to use the portal to create the model schema, and export it as a *.json* file that can be imported and retrained in another Language service instance. This approach enables you to make use of the productivity benefits of the Language Studio visual interface while maintaining portability and reproducibility for the model.

1. In Language Studio, on the **Projects** page, selct the **Clock (Conversation)** project.
2. Click the **&#x2913; Export** button.
3. Save the **Clock.json** file that is generated (anywhere you like).
4. Open the downloaded file in your favorite code editor (for example, Visual Studio Code) to review the JSON definition of your project.

## More information

For more information about using the **Language** service to create conversational language understanding solutions, see the [Language service documentation](https://docs.microsoft.com/azure/cognitive-services/language-service/conversational-language-understanding/overview).
