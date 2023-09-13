---
lab:
    title: 'Create a Question Answering Solution'
    module: 'Module 6 - Create question answering solutions with Azure AI Language'
---

# Create a Question Answering Solution

One of the most common conversational scenarios is providing support through a knowledge base of frequently asked questions (FAQs). Many organizations publish FAQs as documents or web pages, which works well for a small set of question and answer pairs, but large documents can be difficult and time-consuming to search.

**Azure AI Language** includes a *question answering* capability that enables you to create a knowledge base of question and answer pairs that can be queried using natural language input, and is most commonly used as a resource that a bot can use to look up answers to questions submitted by users.

> [!NOTE]
> The question answering capability in Azure AI Language is a newer version of the QnA Maker service - which is still available as a separate service, but to be retired in the near future.

## Clone the repository for this course

If you have not already cloned **AI-102-AIEngineer** code repository to the environment where you're working on this lab, follow these steps to do so. Otherwise, open the cloned folder in Visual Studio Code.

1. Start Visual Studio Code.
1. Open the palette (SHIFT+CTRL+P) and run a **Git: Clone** command to clone the `https://github.com/MicrosoftLearning/AI-102-AIEngineer` repository to a local folder (it doesn't matter which folder).
1. When the repository has been cloned, open the folder in Visual Studio Code.
1. Wait while additional files are installed to support the C# code projects in the repo.

    > [!NOTE]
    > If you are prompted to add required assets to build and debug, select **Not Now**.

## Create a Azure AI Language resource

To create and host a knowledge base for question answering, you need an **Azure AI Language** resource in your Azure subscription.

1. Open the Azure portal at `https://portal.azure.com`, and sign in using the Microsoft account associated with your Azure subscription.
1. In the search folder at the top search enter **Azure AI services**, then select **Enter**.
1. Select **Create** under the **Language Service** resource in the results.
1. **Select** the **Custom question answering** block. Then select **Continue to create your resource**. You will need to enter the following settings:

    - **Subscription**: *Your Azure subscription*
    - **Resource group**: *Choose or create a resource group (if you are using a restricted subscription, you may not have permission to create a new resource group - use the one provided)*
    - **Region**: *Choose any available location*
    - **Name**: *Enter a unique name*
    - **Pricing tier**: Standard S
    - **Azure Search region**: *Choose a location in the same global region as your Language resource*
    - **Azure Search pricing tier**: Free (F) (*If this tier is not available, select Basic (B)*)
    - **Responsible AI Notice**: *Agree*

1. Select **Create + review**, then select **Create**.
    > [!NOTE]
    > Custom Question Answering uses Azure AI Search to index and query the knowledge base of questions and answers.

1. Wait for deployment to complete, and then view the deployment details.

## Create a question answering project

To create a knowledge base for question answering in your Azure AI Language resource, you can use the Language Studio portal to create a question answering project. In this case, you'll create a knowledge base containing questions and answers about [Microsoft Learn](https://docs.microsoft.com/learn).

1. In a new browser tab, go to the *Language Studio* portal at `https://language.cognitive.azure.com` and sign in using the Microsoft account associated with your Azure subscription.
1. If prompted to choose a Language resource, select the following settings:
    - **Azure Directory**: The Azure directory containing your subscription.
    - **Azure subscription**: Your Azure subscription.
    - **Language resource**: The Azure AI Language resource you created previously.
1. Select **Done**.
1. If you are <u>not</u> prompted to choose a language resource, it may be because you have multiple Language resources in your subscription; in which case:
    1. On the bar at the top if the page, select the **Settings (&#9881;)** button.
    2. On the **Settings** page, view the **Resources** tab.
    3. Select the language resource you just created, and click **Switch resource**.
    4. At the top of the page, click **Language Studio** to return to the Language Studio home page.
1. At the top of the portal, in the **Create new** menu, select **Custom question answering**.
1. In the ***Create a project** wizard, on the **Choose language setting** page, select the option to **Set the language for all projects in this resource**, and select **English** as the language. Then select **Next**.
1. On the **Enter basic information** page, enter the following details:
    - **Name** LearnFAQ
    - **Description**: FAQ for Microsoft Learn
    - **Default answer when no answer is returned**: Sorry, I don't understand the question
1. Select **Next**.
1. On the **Review and finish** page, select **Create project**.

## Add a sources to the knowledge base

You can create a knowledge base from scratch, but it's common to start by importing questions and answers from an existing FAQ page or document. In this case, you'll import data from an existing FAQ web page for Microsoft learn, and you'll also import some pre-defined "chit chat" questions and answers to support common conversational exchanges.

1. On the **Manage sources** page for your question answering project, in the **&#9547; Add source** list, select **URLs**. Then in the **Add URLs** dialog box, select **&#9547; Add url** and add the following URL, and then select **Add all** to add it to the knowledge base:
    - **Name**: `Learn FAQ Page`
    - **URL**: `https://docs.microsoft.com/en-us/learn/support/faq`
1. On the **Manage sources** page for your question answering project, in the **&#9547; Add source** list, select **Chitchat**. The in the **Add chit chat** dialog box, select **Friendly** and select **Add chit chat**.

## Edit the knowledge base

Your knowledge base has been populated with question and answer pairs from the Microsoft Learn FAQ, supplemented with a set of conversational *chit-chat* question  and answer pairs. You can extend the knowledge base by adding additional question and answer pairs.

1. In your **LearnFAQ** project in Language Studio, select the **Edit knowledge base** page to see the existing question and answer pairs (if some tips are displayed, read them and click **Got it** to dismiss them, or select **Skip all**)
1. In the knowledge base, on the **Question answer pairs** tab, select **&#65291;**, and create a new question answer pair with the following settings:
    - **Source**: `https://docs.microsoft.com/en-us/learn/support/faq`
    - **Question**: `What is Microsoft certification?`
    - **Answer**: `The Microsoft Certified Professional program enables you to validate and prove your skills with Microsoft technologies.`
1. Select **Done**.
1. In the page for the **What is Microsoft certification?** question that is created, expand **Alternate questions**. Then add the alternate question `How can I demonstrate my Microsoft technology skills?`.

    In some cases, it makes sense to enable the user to follow up on answer to create a *multi-turn* conversation that enables the user to iteratively refine the question to get to the answer they need.

1. Under the answer you entered for the certification question, expand **Follow-up prompts** and add  the following follow-up prompt:
    - **Text displayed in the prompt to the user**: `Learn more about certification`.
    - Select the **Create link to new pair** tab, and enter this text: `You can learn more about certification on the [Microsoft certification page](https://docs.microsoft.com/learn/certifications/).`
    - Select **Show in contextual flow only**. This option ensures that the answer is only ever returned in the context of a follow-up question from the original certification question.
1. Select **Add prompt**.

## Train and test the knowledge base

Now that you have a knowledge base, you can test it in Language Studio.

1. Save the changes to your knowledge base by selecting the **Save** button under the **Question answer pairs** tab on the left.
1. After the changes have been saved, select the **Test** button to open the test pane.
1. In the test pane, at the top, deselect **Include short answer response** (if not already unselected). Then at the bottom enter the message `Hello`. A suitable response should be returned.
1. In the test pane, at the bottom enter the message `What is Microsoft Learn?`. An appropriate response from the FAQ should be returned.
1. Enter the message `Thanks!` An appropriate chit-chat response should be returned.
1. Enter the message `Tell me about certification`. The answer you created should be returned along with a follow-up prompt link.
1. Select the **Learn more about certification** follow-up link. The follow-up answer with a link to the certification page should be returned.
1. When you're done testing the knowledge base, close the test pane.

## Deploy and test the knowledge base

The knowledge base provides a back-end service that client applications can use to answer questions. Now you are ready to publish your knowledge base and access its REST interface from a client.

1. In the **LearnFAQ** project in Language Studio, select the **Deploy knowledge base** page.
1. At the top of the page, select **Deploy**. Then select **Deploy** to confirm you want to deploy the knowledge base.
1. When deployment is complete, select **Get prediction URL** to view the REST endpoint for your knowledge base, and copy it to the clipboard (but don't close the dialog box yet).
1. In Visual Studio Code, in the **12-qna** folder, open **ask-question.cmd**. This script uses *Curl* to call the REST interface of a question answering endpoint.
1. In the script, replace ***YOUR_PREDICTION_ENDPOINT*** with the prediction endpoint you copied (ensuring it is enclosed in the quotation marks).
1. Return to the browser and in the **Get prediction URL** dialog box, note that the sample request includes a value for the **Ocp-Apim-Subscription-Key** parameter, which looks similar to *ab12c345de678fg9hijk01lmno2pqrs34*. This is the authorization key for your resource. Copy it to the clipboard, save it somewhere, and then select **Close** to close the dialog box.
1. Return to Visual Studio Code, and in the **ask-question.cmd** script, replace *YOUR_KEY* with the key you copied (ensuring it is enclosed in the quotation marks).
1. Note that the Curl command in the script submits a **question** parameter with the value **What is a Learning Path?**.
1. Verify that the entire script looks similar to the following code, then save the file.

    ```bash
    @echo off
    SETLOCAL ENABLEDELAYEDEXPANSION

    rem Set variables
    set prediction_url="https://some-name.cognitiveservices.azure.com/language/......."
    set key="ab12c345de678fg9hijk01lmno2pqrs34"

    curl -X POST !prediction_url! -H "Ocp-Apim-Subscription-Key: !key!" -H "Content-Type: application/json" -d "{'question': 'What is a learning Path?' }"
    ```

1. In Visual Studio Code in the Explorer pane, right-click the **ask-question.cmd** script and select **Open in Integrated Terminal**.
1. In the terminal pane, enter the command `.\ask-question.cmd` to run the script and view the JSON response that is returned by the service, which should contain an appropriate answer to the question *What is a learning path?*.

## Create a bot for the knowledge base

Most commonly, the client applications used to retrieve answers from a knowledge base are bots.

1. Return to Language Studio in the browser, and in the **Deploy knowledge base** page, select **Create Bot**. This opens the Azure portal in a new browser tab so you can create a Web App Bot in your Azure subscription (if prompted, sign in).
1. In the Azure portal, create a Web App Bot with the following settings (most of these will be pre-populated for you):

    *If some values are missing, refresh your browser.*  

    - **Bot handle**: *A unique name for your bot*
    - **Subscription**: *Your Azure subscription*
    - **Resource group**: *The resource group containing your Language resource*
    - **Location**: *The same location as your Text Analytics service*.
    - **Pricing tier**: Standard.
    - **Creation type**: Create new User-assigned managed identity.

1. Select **Next**, then set the following if not already populated automatically:

    - **App name**: *Same as the **Bot handle** with a unique ID and *.azurewebsites.net* appended automatically*
    - **SDK language**: *Choose either C# or Node.js*
    - **QnA Auth Key**: *This should automatically be set to the authentication key for your QnA knowledge base*
    - **Creation type**: *This may be set automatically to a suitable plan if one exists. If not, select **Create a new app service plan***.
    - **Language resource Key**: *They key you copied earlier*. 
1. Select **Review + create**. Then select **Create**.

1. Wait for your bot to be created. Then select **Go to resource group** (or alternatively, on the home page, select **Resource groups**), to open the resource group where you created the web app bot, and select it.
1. In the blade for your bot, view the **Test in Web Chat** page, and wait until the bot displays the message **Hello and welcome!** (it may take a few seconds to initialize).
1. Use the test chat interface to ensure your bot answers questions from your knowledge base as expected. For example, try submitting `What is Microsoft certification?`.

## More information

To learn more about question answering in  Azure AI Language, see the [Azure AI Language documentation](azure/ai-services/language-service/question-answering/overview).
