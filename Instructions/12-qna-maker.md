---
lab:
    title: 'Create a QnA Solution'
    module: 'Module 6 - Building a QnA Solution'
---

# Create a QnA Solution

One of the most common conversational scenarios is providing support through a knowledge base of frequently asked questions (FAQs). Many organizations publish FAQs as documents or web pages, which works well for a small set of question and answer pairs, but large documents can be difficult and time-consuming to search.

QnA Maker is a cognitive service that enables you to create a knowledge base of question and answer pairs that can be queried using natural language input, and is most commonly used as a resource that a bot can use to look up answers to questions submitted by users.

In this lab, we will be using the Managed QnA Maker, which is a feature within Text Analytics. 

## Create a Text Analytics resource 

To create and host a knowledge base using the Managed QnA Maker, you need a Text Analytics resource in your Azure subscription.

1. Open the Azure portal at `https://portal.azure.com`, and sign in using the Microsoft account associated with your Azure subscription.
2. Select the **&#65291;Create a resource** button, search for *Text Analytics*, and create a **Text Analytics** resource. 
3. Click **Select** on the **Custom question answering (preview)** block. Then click **Continue to create your resource**. You will need to enter the following settings:
    
    - **Subscription**: *Your Azure subscription*
    - **Resource group**: *Choose or create a resource group (if you are using a restricted subscription, you may not have permission to create a new resource group - use the one provided)*
    - **Region**: *Choose any available location*
    - **Name**: *Enter a unique name*
    - **Pricing tier**: Standard S
    - **Azure Search location**\*: *Choose a location in the same global region as your QnA Maker resource*.
    - **Azure Search pricing tier**: Free (F) (*If this tier is not available, select Basic (B)*)
    - **Legal Terms**: _Agree_ 
    - **Responsible AI Notice**: _Agree_
    
    \*Custom Question Answering uses Azure Search to index and query the knowledge base of questions and answers.

4. Wait for deployment to complete, and then view the deployment details.

## Create a knowledge base

To create a knowledge base in your Text Analytics resource, you can use the QnA Maker portal. In this case, you'll create a knowledge base containing questions and answers about [Microsoft Learn](https://docs.microsoft.com/learn).

1. In a new browser tab, go to the QnA Maker portal at `https://qnamaker.ai` and sign in using the Microsoft account associated with your Azure subscription.
2. At the top of the portal, select **Create a knowledge base**.
3. You have already created a QnA Maker resource, so you can skip step 1. In the **Step 2** section, select the following settings:
    - **Microsoft Azure Directory ID**: The Azure directory containing your subscription.
    - **Azure subscription name**: Your Azure subscription.
    - **Azure QnA Service**: The Text Analytics resource you created previously.
    - **Language**: English (*by default, this option is only available for the first knowledge base you create*).
4. In the **Step 3** section, enter **Learn FAQ** as the name for your knowledge base.

    You can create a knowledge base from scratch, but it's common to start by importing questions and answers from an existing FAQ page or document.

5. In the **Step 4** section:
    - In the **URL** box type `https://docs.microsoft.com/en-us/learn/support/faq` and click **&#65291; Add URL**.
    - In the **Chit-chat** section, select **Friendly**.
6. In the **Step 5** section, select **Create your KB** and wait for your knowledge base to be created.

## Modify the knowledge base

Your knowledge base has been populated with question and answer pairs from the Microsoft Learn FAQ, supplemented with a set of conversational *chit-chat* question  and answer pairs. You can extend the knowledge base by adding additional question and answer pairs.

1. In the knowledge base, select **&#65291; Add QnA pair**.
2. In the **Question** box, type `What is Microsoft certification?`
3. Select **&#65291; Add alternative phrasing** and type `How can I demonstrate my Microsoft technology skills?`.
4. In the **Answer** box, type `The Microsoft Certified Professional program enables you to validate and prove your skills with Microsoft technologies.`

    In some cases, it makes sense to enable the user to follow up on answer to create a *multi-turn* conversation that enables the user to iteratively refine the question to get to the anseer they need.

5. Under the answer you entered for the certification question, select **&#65291; Add follow-up prompt**.
6. In the **Follow-up Prompt** dialog box, enter the following settings:
    - **Display text**: `Learn more about certification.`
    - **Link to QnA**\*: `You can learn more about certification on the [Microsoft certification page](https://docs.microsoft.com/learn/certifications/).`
    - **Context-only**: Selected. *This option ensures that the answer is only ever returned in the context of a follow-up question from the original certification question.*

    \*Typing in the **Link to QnA** box searches the existing answers in the knowledge base. When no match is found, it defaults to creating a new QnA pair. Note that the text you type here is in Markdown format.

## Train and test the knowledge base

Now that you have a knowledge base, you can test it in the QnA Maker portal.

1. At the top right of the page, click **Save and train** to train your knowledge base.
2. After training has completed, click **&larr; Test** to open the test pane.
3. In the test pane, at the top, *deselect* the box that says *Display Short Answer*. Then at the bottom enter the message *Hello*. A suitable response should be returned.
4. In the test pane, at the bottom enter the message *What is Microsoft Learn?*. An appropriate response from the FAQ should be returned.
5. Enter the message *That makes me happy!* An appropriate chit-chat response should be returned.
6. Enter the message *Tell me about certification*. The answer you created should be returned along with a follow-up prompt button.
7. Select the **Learn more about certification** follow-up button. The follow-up answer with a link to the certification page should be returned.
8. When you're done testing the knowledge base, click **&rarr; Test** to close the test pane.

## Publish the knowledge base

The knowledge base provides a back-end service that client applications can use to answer questions. Now you are ready to publish your knowledge base and access its REST interface from a client.

1. At the top of the QnA Maker page, click **Publish**. Then in the **Learn FAQ** page, click **Publish**.
2. When publishing is complete, view the sample code provided to use your knowledge base's REST endpoint. There is an example for *Postman* and an example for *Curl*.
3. View the **Curl** tab and copy the example code.
4. Start Visual Studio Code and open a terminal pane.
5. Paste the code you copied into the terminal, and then edit it to replace **&lt;your question&gt;** with **What is a learning path?**.
6. Enter the command and view the JSON response that is returned from your knowledge base.

## Create a bot for the knowledge base

Most commonly, the client applications used to retrieve answers from a knowledge base are bots.

1. On the page containing the publishing confirmation and sample Curl code, select **Create Bot**. This opens the Azure portal in a new browser tab so you can create a Web App Bot in your Azure subscription.
2. In the Azure portal, create a Web App Bot with the following settings (most of these will be pre-populated for you):

    *If some values are missing, refresh your browser.*  

  - **Bot handle**: *A unique name for your bot*
  - **Subscription**: *Your Azure subscription*
  - **Resource group**: *The resource group containing your QnA Maker resource*
  - **Location**: *The same location as your Text Analytics service*.
  - **Pricing tier**: F0
  - **App name**: *Same as the **Bot handle** with *.azurewebsites.net* appended automatically
  - **SDK language**: *Choose either C# or Node.js*
  - **QnA Auth Key**: *This should automatically be set to the authentication key for your QnA knowledge base*
  - **App service plan/location**: *This may be set automatically to a suitable plan and location if one exists. If not, create a new plan*
  - **Application Insights**: Off
  - **Microsoft App ID and password**: Auto create App ID and password.
3. Wait for your bot to be created (the notification icon at the top right, which looks like a bell, will be animated while you wait). Then in the notification that deployment has completed, click **Go to resource** (or alternatively, on the home page, click **Resource groups**, open the resource group where you created the web app bot, and click it.)
4. In the blade for your bot, view the **Test in Web Chat** page, and wait until the bot displays the message **Hello and welcome!** (it may take a few seconds to initialize).
5. Use the test chat interface to ensure your bot answers questions from your knowledge base as expected. For example, try submitting *What is Microsoft certification?*.

## More information

To learn more about QnA Maker, see the [QnA Maker documentation](https://docs.microsoft.com/azure/cognitive-services/qnamaker/).