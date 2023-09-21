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

## Clone the repository for this course to Azure Cloud Shell

Open up a new browser tab to work with Cloud Shell. If you haven't cloned this repository to Cloud Shell recently, follow the steps below to make sure you have the most recent version. Otherwise, open Cloud Shell and navigate to your clone.

1. In the [Azure portal](https://portal.azure.com?azure-portal=true), select the **[>_]** (*Cloud Shell*) button at the top of the page to the right of the search box. A Cloud Shell pane will open at the bottom of the portal.

    ![Screenshot of starting Cloud Shell by clicking on the icon to the right of the top search box.](images/cloudshell-launch-portal.png#lightbox)

2. The first time you open the Cloud Shell, you may be prompted to choose the type of shell you want to use (*Bash* or *PowerShell*). Select **Bash**. If you don't see this option, skip the step.  

3. If you're prompted to create storage for your Cloud Shell, ensure your subscription is specified and select **Create storage**. Then wait a minute or so for the storage to be created.

4. Make sure the type of shell indicated on the top left of the Cloud Shell pane is switched to *Bash*. If it's *PowerShell*, switch to *Bash* by using the drop-down menu.

5. Once the terminal starts, run the following commands to download a copy of the repo into your Cloud Shell:

    ```bash
    rm -r azure-ai-eng -f
   git clone https://github.com/MicrosoftLearning/AI-102-AIEngineer azure-ai-eng
    ```

6. The files have been downloaded into a folder called **azure-ai-eng**. Let's change into that folder by running:

    ```bash
    cd azure-ai-eng/12-qna
    ```

## Create a Azure AI Language resource

To create and host a knowledge base for question answering, you need an **Azure AI Language** resource in your Azure subscription.

1. Open the Azure portal at `https://portal.azure.com`, and sign in using the Microsoft account associated with your Azure subscription.
1. In the search field at the top enter **Azure AI services**, then press **Enter**.
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
    > Custom Question Answering uses Azure Search to index and query the knowledge base of questions and answers.

1. Wait for deployment to complete, and then view the deployment details.

## Create a question answering project

To create a knowledge base for question answering in your Azure AI Language resource, you can use the Language Studio portal to create a question answering project. In this case, you'll create a knowledge base containing questions and answers about [Microsoft Learn](https://docs.microsoft.com/learn).

1. In a new browser tab, go to the [Language Studio portal](https://language.cognitive.azure.com/) and sign in using the Microsoft account associated with your Azure subscription.
1. If you're prompted to choose a Language resource, select the following settings:
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

## Add a source to the knowledge base

You can create a knowledge base from scratch, but it's common to start by importing questions and answers from an existing FAQ page or document. In this case, you'll import data from an existing FAQ web page for Microsoft learn, and you'll also import some pre-defined "chit chat" questions and answers to support common conversational exchanges.

1. On the **Manage sources** page for your question answering project, in the **&#9547; Add source** list, select **URLs**. Then in the **Add URLs** dialog box, select **&#9547; Add url** and set the following name and URL  before you select **Add all** to add it to the knowledge base:
    - **Name**: `Learn FAQ Page`
    - **URL**: `https://docs.microsoft.com/en-us/learn/support/faq`
1. On the **Manage sources** page for your question answering project, in the **&#9547; Add source** list, select **Chitchat**. The in the **Add chit chat** dialog box, select **Friendly** and select **Add chit chat**.

## Edit the knowledge base

Your knowledge base has been populated with question and answer pairs from the Microsoft Learn FAQ, supplemented with a set of conversational *chit-chat* question  and answer pairs. You can extend the knowledge base by adding additional question and answer pairs.

1. In your **LearnFAQ** project in Language Studio, select the **Edit knowledge base** page to see the existing question and answer pairs (if some tips are displayed, read them and choose **Got it** to dismiss them, or select **Skip all**)
1. In the knowledge base, on the **Question answer pairs** tab, select **&#65291;**, and create a new question answer pair with the following settings:
    - **Source**: `https://docs.microsoft.com/en-us/learn/support/faq`
    - **Question**: `What is Microsoft certification?`
    - **Answer**: `The Microsoft Certified Professional program enables you to validate and prove your skills with Microsoft technologies.`
1. Select **Done**.
1. In the page for the **What is Microsoft certification?** question that is created, expand **Alternate questions**. Then add the alternate question `How can I demonstrate my Microsoft technology skills?`.

    In some cases, it makes sense to enable the user to follow up on an answer by creating a *multi-turn* conversation that enables the user to iteratively refine the question to get to the answer they need.

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
1. Enter the message `Tell me about Microsoft certification`. The answer you created should be returned along with a follow-up prompt link.
1. Select the **Learn more about certification** follow-up link. The follow-up answer with a link to the certification page should be returned.
1. When you're done testing the knowledge base, close the test pane.

## Deploy and test the knowledge base

The knowledge base provides a back-end service that client applications can use to answer questions. Now you are ready to publish your knowledge base and access its REST interface from a client.

1. In the **LearnFAQ** project in Language Studio, select the **Deploy knowledge base** page.
1. At the top of the page, select **Deploy**. Then select **Deploy** to confirm you want to deploy the knowledge base.
1. When deployment is complete, select **Get prediction URL** to view the REST endpoint for your knowledge base, and copy it to the clipboard (but don't close the dialog box yet).
1. In your Azure Cloud Shell, in the **12-qna** folder, open **ask-question.sh** by running `code ask-question.sh`. This script uses *Curl* to call the REST interface of a question answering endpoint.
1. In the script, replace ***YOUR_PREDICTION_ENDPOINT*** with the prediction endpoint you copied (ensuring it is enclosed in the quotation marks). Select **CTRL+ Save** to save your changes.
1. Return to the browser and in the **Get prediction URL** dialog box, note that the sample request includes a value for the **Ocp-Apim-Subscription-Key** parameter, which looks similar to *ab12c345de678fg9hijk01lmno2pqrs34*. This is the authorization key for your resource. Copy it to the clipboard, save it somewhere, and then select **Close** to close the dialog box.
1. Return to your Cloud Shell, and in the **ask-question.sh** script, replace *YOUR_KEY* with the key you copied (ensuring it is enclosed in the quotation marks). Select **CTRL+ Save** to save your changes.
1. Note that the Curl command in the script submits a **question** parameter with the value **What is a Learning Path?**.
1. Verify that the entire script looks similar to the following code, then save the file.

    ```bash
    prediction_url="https://my-example-resource.cognitiveservices.azure.com/language/:query-knowledgebases?projectName=LearnFAQ&api-version=2021-10-01&deploymentName=production"
    key="123ca1b012ec4e4456dab367fefdf178"
    
    curl -X POST $prediction_url -H "Ocp-Apim-Subscription-Key: $key" -H "Content-Type: application/json" -d "{'question': 'What is a learning Path?' }"
    ```

1. In the terminal pane, enter the command `ask-question.sh` to run the script and view the JSON response that is returned by the service, which should contain an appropriate answer to the question *What is a learning path?*.

## Create a bot for the knowledge base

Most commonly, the client applications used to retrieve answers from a knowledge base are bots.

1. Return to Language Studio in the browser, and in the **Deploy knowledge base** page, select **Create Bot**. This opens the Azure portal in a new browser tab so you can create a bot in your Azure subscription (if prompted, sign in).
1. In the Azure portal, create a bot with the following settings (most of these will be pre-populated for you):

    *If some values are missing, refresh your browser.*  

    - **Bot handle**: *A unique name for your bot*
    - **Subscription**: *Your Azure subscription*
    - **Resource group**: *The resource group containing your Language resource*
    - **Pricing tier**: Standard.
    - **Creation type**: Create new User-assigned managed identity.

1. Select **Next**, then set the following if not already populated automatically:

    - **App name**: *Same as the **Bot handle** with a unique ID and *.azurewebsites.net* appended automatically.*
    - **SDK language**: *Choose either C# or Node.js*
    - **Creation type**: *This may be set automatically to a suitable plan if one exists. If not, select **Create a new app service plan***.
    - **Language Resource Key**: *They key you copied earlier*.
1. Select **Review + create**. Then select **Create**.

1. Wait for your bot to be created. Then select **Go to resource group** (or alternatively, on the home page, select **Resource groups**).
1. To open the  bot, select the Azure Bot resource in the resources list.
1. In the overview pane for your bot, select the **Test in Web Chat** page, and wait until the bot displays the message **Hello and welcome!** (it may take a few seconds to initialize).
1. Use the test chat interface to ensure your bot answers questions from your knowledge base as expected. For example, try submitting `What is Microsoft certification?`.

## More information

To learn more about question answering in  Azure AI Language, see the [Azure AI Language documentation](azure/ai-services/language-service/question-answering/overview).
