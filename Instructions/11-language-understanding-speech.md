---
lab:
    title: 'Use the Speech and Language Understanding Services'
---

# Use the Speech and Language Understanding Services

You can integrate the Speech service with the Language Understanding service to create applications that can intelligently determine user intents from spoken input.

## Clone the repository for this course

If you have not already done so, you must clone the code repository for this course:

1. Start Visual Studio Code.
2. Open the palette (SHIFT+CTRL+P) and run a **Git: Clone** command to clone the **https://github.com/MicrosoftLearning/AI-102-AIEngineer** repository to a local folder.
3. When the repository has been cloned, open the folder in Visual Studio Code.
4. Wait while additional files are installed to support the C# code projects in the repo.

## Create Language Understanding resources

To use the Language Understanding service, you need two kinds of resource:

- An *authoring* resource: used to define, train, and test the language understanding app. This must be a **Language Understanding - Authoring** resource in your Azure subscription.
- A *prediction* resource: used to publish your language understanding app and handle requests from client applications that use it. This can be either a **Language Understanding** or **Cognitive Services** resource in your Azure subscription.

> **Note**: If you already have Language Understanding authoring and prediction resources in your Azure subscription, you can use them in this exercise. Otherwise, follow these instructions to create them.

1. Open the Azure portal at [https://portal.azure.com](https://portal.azure.com), and sign in using the Microsoft account associated with your Azure subscription.
2. Select the **&#65291;Create a resource** button, search for *language understanding*, and create a **Language Understanding** resource with the following settings:
    - **Create option**: Both
    - **Subscription**: *Your Azure subscription*
    - **Resource group**: *Choose or create a resource group (if you are using a restricted subscription, you may not have permission to create a new resource group - use the one provided)*
    - **Name**: *Enter a unique name*
    - **Authoring location**: *Select your preferred location*
    - **Authoring pricing tier**: F0
    - **Prediction location**: *Choose the <u>same location</u> as your authoring location*
    - **Prediction pricing tier**: F0\*

    \**If F0 is not available, choose S0*

3. Wait for the resources to be created, and note that two Language Understanding resources are provisioned; one for authoring, and another for prediction. You can view both of these by navigating to the resource group where you created them.

## Prepare a Language Understanding app

In this exercise, you'll use an app that contains a language model for clock-related intents. For example, the user input *what is the time?* predicts an intent named **GetTime**.

> **Note**: If you already have a **Clock** app from a previous exercise, open it in the Language Udnerstanding portal. Otherwise, follow these instructions to create it.

1. In a new browser tab, open the Language Understanding portal for the location where you created your authoring resource:
    - US: [https://www.luis.ai](https://www.luis.ai)
    - Europe: [https://eu.luis.ai](https://eu.luis.ai)
    - Australia: [https://au.luis.ai](https://au.luis.ai)
 2. Sign in using the Microsoft account associated with your Azure subscription. If this is the first time you have signed into the Language Understanding portal, you may need to grant the app some permissions to access your account details. Then complete the *Welcome* steps by selecting your Azure subscription and the authoring resource you just created.
3. Open the **Conversation Apps** page, next to **&#65291;New app**, view the drop-down list and select **Import As LU**.
Browse to the **11-luis-speech** subfolder in the project folder containing the lab files for this exercise, and select **Clock&period;lu**. Then specify a unique name for the clock app.
4. If a panel with tips for creating an effective Language Understanding app is displayed, close it.

## Train and publish the app with *Speech Priming*

1. At the top of the Language Understanding portal, select **Train** to train the app.
2. At the top right of the Language Understanding portal, select **Publish**. Then select the **Production slot** and modify the settings to enable **Speech Priming** (this will result in better performance for speech recognition).
3. After publishing is complete, at the top of the Language Understanding portal, select **Manage**.
4. On the **Settings** page, note the **App ID**. Client applications need this to use your app.
5. On the **Azure Resources** page, under **Prediction resources**, if no prediction resource is listed, add the prediction resource in your Azure subscription.
6. Note the **Primary Key**, **Secondary Key**, and **Location** for the prediction resource. Speech SDK client applications need the location and one of the keys to connect to the prediction resource and be authenticated.

## Prepare to use the Speech SDK with Language Understanding

In this exercise, you'll complete a partially implemented client application that uses the clock Language Understanding app to predict intents from spoken user input.

> **Note**: You can choose to use the SDK for either **C#** or **Python**. In the steps below, perform the actions appropriate for your preferred language.

1. In Visual Studio Code open the **AI-102** project, and in the **Explorer** pane, browse to the **11-luis-speech** folder and expand the **C-Sharp** or **Python** folder depending on your language preference.
2. Right-click the **speaking-clock-client** folder and open an integrated terminal. Then install the Language Understanding SDK package by running the appropriate command for your language preference:

   **C#**

    ```
    dotnet add package Microsoft.CognitiveServices.Speech --version 1.14.0
    ```

   **Python**

   ```
   pip install azure-cognitiveservices-speech==1.14.0
   ```

3. View the contents of the **speaking-clock-client** folder, and note that it contains a file for configuration settings:
    - **C#**: appsettings.json
    - **Python**: .env

    Open the configuration file and update the configuration values it contains to include the **App ID** for your Language Understanding app, and the **Location** (<u>not</u> the full endpoint - for example, *eastus*) and one of the **Keys** for its prediction resource (from the **Manage** page for your app in the Language Understanding portal).

4. Note that the **speaking-clock-client** folder contains a code file for the client application:

    - **C#**: Program.cs
    - **Python**: speaking-clock-client&period;py

    Open the code file and at the top, under the existing namespace references, find the comment **Import namespaces**. Then, under this comment, add the following language-specific code to import the namespaces you will need to use the Speech SDK:

    **C#**

    ```C#
    // Import namespaces
    using Microsoft.CognitiveServices.Speech;
    using Microsoft.CognitiveServices.Speech.Intent;
    ```

    **Python**

    ```Python
    # Import namespaces
    import azure.cognitiveservices.speech as speech_sdk
    ```

## Get a predicted intent from spoken input

Now you're ready to implement code that uses the Speech SDK to get a predicted intent from spoken input.

1. In the **Main** function, note that code to load the App ID, prediction region, and key from the configuration file has already been provided. Then find the comment **Configure speech service and get intent recognizer** and add the following code to create a Speech SDK **SpeechConfig** and **IntentRecognizer** using your Language Understanding prediction resource details:

    **C#**

    ```C#
    // Configure speech service and get intent recognizer
    SpeechConfig speechConfig = SpeechConfig.FromSubscription(predictionKey, predictionRegion);
    IntentRecognizer recognizer = new IntentRecognizer(speechConfig);
    ```

    **Python**

    ```Python
    # Configure speech service and get intent recognizer
    speech_config = speech_sdk.SpeechConfig(subscription=lu_prediction_key, region=lu_prediction_region)
    recognizer = speech_sdk.intent.IntentRecognizer(speech_config)
    ```
2. Immediately beneath the code you just added, find the comment **Get the model from the AppID and add the intents we want to use** and add the following code to get your Language Understanding model (based on its App ID) and specify the intents that we want the recognizer to identify.

    **C#**

    ```C#
    // Get the model from the AppID and add the intents we want to use
    var model = LanguageUnderstandingModel.FromAppId(luAppId);
    recognizer.AddIntent(model, "GetTime", "time");
    recognizer.AddIntent(model, "GetDate", "date");
    recognizer.AddIntent(model, "GetDay", "day");
    recognizer.AddIntent(model, "None", "none");
    ```

    *Note that you can specify a string-based ID for each intent*

    **Python**

    ```Python
    # Get the model from the AppID and add the intents we want to use
    model = speech_sdk.intent.LanguageUnderstandingModel(app_id=lu_app_id)
    intents = [
        (model, "GetTime"),
        (model, "GetDate"),
        (model, "GetDay"),
        (model, "None")
    ]
    recognizer.add_intents(intents)
    ```

3. Note that the code in the **Main** loops continually until the user says "stop". Within this loop, find the comment **Process speech input** and add the following code, which uses the recognizer to asynchronously call the Language Understanding service with spoken input, and retrieve response. If the response includes a predicted intent, the spoken query, predicted intent, and full JSON response are displayed. Otherwise the code handles the response based on the reason returned.

    **C#**

    ```C
    // Process speech input
    var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);
    if (result.Reason == ResultReason.RecognizedIntent)
    {
        // Intent was identified
        intent = result.IntentId;
        Console.WriteLine($"Query: {result.Text}");
        Console.WriteLine($"Intent Id: {intent}.");
        string jsonResponse = result.Properties.GetProperty(PropertyId.LanguageUnderstandingServiceResponse_JsonResult);
        Console.WriteLine($"JSON Response:\n{jsonResponse}\n");
        
        // Get the first entity (if any)

        // Apply the appropriate action
        
    }
    else if (result.Reason == ResultReason.RecognizedSpeech)
    {
        // Speech was recognized, but no intent was identified.
        intent = result.Text;
        Console.Write($"I don't know what {intent} means.");
    }
    else if (result.Reason == ResultReason.NoMatch)
    {
        // Speech wasn't recognized
        Console.WriteLine($"Sorry. I didn't understand that.");
    }
    else if (result.Reason == ResultReason.Canceled)
    {
        // Something went wrong
        var cancellation = CancellationDetails.FromResult(result);
        Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

        if (cancellation.Reason == CancellationReason.Error)
        {
            Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
            Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
        }
    }
    ```

    **Python**

    ```Python
    # Process speech input
    result = recognizer.recognize_once_async().get()
    if result.reason == speech_sdk.ResultReason.RecognizedIntent:
        intent = result.intent_id
        print("Query: {}".format(result.text))
        print("Intent: {}".format(intent))
        json_response = json.loads(result.intent_json)
        print("JSON Response:\n{}\n".format(json.dumps(json_response, indent=2)))
        
        # Get the first entity (if any)
        
        # Apply the appropriate action
        
    elif result.reason == speech_sdk.ResultReason.RecognizedSpeech:
        # Speech was recognized, but no intent was identified.
        intent = result.text
        print("I don't know what {} means.".format(intent))
    elif result.reason == speech_sdk.ResultReason.NoMatch:
        # Speech wasn't recognized
        print("Sorry. I didn't understand that.")
    elif result.reason == speech_sdk.ResultReason.Canceled:
        # Something went wrong
        print("Intent recognition canceled: {}".format(result.cancellation_details.reason))
        if result.cancellation_details.reason == speech_sdk.CancellationReason.Error:
            print("Error details: {}".format(result.cancellation_details.error_details))
    ```

    The code you've added so far identifies the *intent*, but some intents can reference *entities*, so you must add code to extract the entity information from the JSON returned by the service.

4. In the code you just added, find the comment **Get the first entity (if any)** and add the following code beneath it:

    **C#**

    ```C
    // Get the first entity (if any)
    JObject jsonResults = JObject.Parse(jsonResponse);
    string entityType = "";
    string entityValue = "";
    if (jsonResults["entities"].HasValues)
    {
        JArray entities = new JArray(jsonResults["entities"][0]);
        entityType = entities[0]["type"].ToString();
        entityValue = entities[0]["entity"].ToString();
        Console.WriteLine(entityType + ": " + entityValue);
    }
    ```

    **Python**

    ```Python
    # Get the first entity (if any)
    entity_type = ''
    entity_value = ''
    if len(json_response["entities"]) > 0:
        entity_type = json_response["entities"][0]["type"]
        entity_value = json_response["entities"][0]["entity"]
        print(entity_type + ': ' + entity_value)
    ```
    
    Your code now uses the Language Understanding app to predict an intent as well as any entities that were detected in the input utterance. Your client application must now use that prediction to determine and perform the appropriate action.

5. Beneath the code you just added, find the comment **Apply the appropriate action**, and add the following code, which checks for intents supported by the application (**GetTime**, **GetDate**, and **GetDay**) and determines if any relevant entities have been detected, before calling an existing function to produce an appropriate response.

    **C#**

    ```C#
    // Apply the appropriate action
    switch (intent)
    {
        case "time":
            var location = "local";
            // Check for entities
            if (entityType == "Location")
            {
                location = entityValue;
            }
            // Get the time for the specified location
            var getTimeTask = Task.Run(() => GetTime(location));
            string timeResponse = await getTimeTask;
            Console.WriteLine(timeResponse);
            break;
        case "day":
            var date = DateTime.Today.ToShortDateString();
            // Check for entities
            if (entityType == "Date")
            {
                date = entityValue;
            }
            // Get the day for the specified date
            var getDayTask = Task.Run(() => GetDay(date));
            string dayResponse = await getDayTask;
            Console.WriteLine(dayResponse);
            break;
        case "date":
            var day = DateTime.Today.DayOfWeek.ToString();
            // Check for entities
            if (entityType == "Weekday")
            {
                day = entityValue;
            }

            var getDateTask = Task.Run(() => GetDate(day));
            string dateResponse = await getDateTask;
            Console.WriteLine(dateResponse);
            break;
        default:
            // Some other intent (for example, "None") was predicted
            Console.WriteLine("You said " + result.Text.ToLower());
            if (result.Text.ToLower().Replace(".", "") == "stop")
            {
                intent = result.Text;
            }
            else
            {
                Console.WriteLine("Try asking me for the time, the day, or the date.");
            }
            break;
    }
    ```

    **Python**

    ```Python
    # Apply the appropriate action
    if intent == 'GetTime':
        location = 'local'
        # Check for entities
        if entity_type == 'Location':
            location = entity_value
        # Get the time for the specified location
        print(GetTime(location))

    elif intent == 'GetDay':
        date_string = date.today().strftime("%m/%d/%Y")
        # Check for entities
        if entity_type == 'Date':
            date_string = entity_value
        # Get the day for the specified date
        print(GetDay(date_string))

    elif intent == 'GetDate':
        day = 'today'
        # Check for entities
        if entity_type == 'Weekday':
            # List entities are lists
            day = entity_value
        # Get the date for the specified day
        print(GetDate(day))

    else:
        # Some other intent (for example, "None") was predicted
        print('You said {}'.format(result.text))
        if result.text.lower().replace('.', '') == 'stop':
            intent = result.text
        else:
            print('Try asking me for the time, the day, or the date.')
    ```

6. Save your changes and return to the integrated terminal for the **speaking-clock-client** folder, and enter the following command to run the program:

    **C#**

    ```
    dotnet run
    ```

    **Python**

    ```
    python speaking-clock-client.py
    ```
7. When prompted, enter utterances to test the application. For example, try:

    *What's the time?*
    
    *What time is it?*

    *What day is it?*

    *What is the time in London?*

    *What's the date?*

    *What date is Sunday?*

    > **Note**: The logic in the application is deliberately simple, and has a number of limitations, but should serve the purpose of testing the ability for the Language Understanding model to predict intents from spoken input using the Speech SDK. You may have trouble recognizing the **GetDay** intent with a specific date entity due to the difficulty in verbalizing a date in *MM/DD/YYYY* format!

8. When you have finished testing, say "stop".
