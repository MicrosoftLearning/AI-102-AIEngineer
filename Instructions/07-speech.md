---
lab:
    title: 'Recognize and Synthesize Speech'
---

# Recognize and Synthesize Speech

The **Speech** service is an Azure cognitive service that provides speech-related functionality, including:

- A *speech-to-text* API that enables you to implement speech recognition (converting audible spoken words into text).
- A *text-to-speech* API that enables you to implement speech synthesis (converting text into audible speech).

In this exercise, you'll use both of these APIs to implement a speaking clock application.

> **Note**: This exercise requires that you are using a computer with a microphone and speakers/headphones.

## Clone the repository for this course

If you have not already cloned **AI-102-AIEngineer** code repository to the environment where you're working on this lab, follow these steps to do so. Otherwise, open the cloned folder in Visual Studio Code.

1. Start Visual Studio Code.
2. Open the palette (SHIFT+CTRL+P) and run a **Git: Clone** command to clone the `https://github.com/MicrosoftLearning/AI-102-AIEngineer` repository to a local folder (it doesn't matter which folder).
3. When the repository has been cloned, open the folder in Visual Studio Code.
4. Wait while additional files are installed to support the C# code projects in the repo.

    > **Note**: If you are prompted to add required assets to build and debug, select **Not Now**.

## Provision a Cognitive Services resource

If you don't already have one in your subscription, you'll need to provision a **Cognitive Services** resource.

1. Open the Azure portal at `https://portal.azure.com`, and sign in using the Microsoft account associated with your Azure subscription.
2. Select the **&#65291;Create a resource** button, search for *cognitive services*, and create a **Cognitive Services** resource with the following settings:
    - **Subscription**: *Your Azure subscription*
    - **Resource group**: *Choose or create a resource group (if you are using a restricted subscription, you may not have permission to create a new resource group - use the one provided)*
    - **Region**: *Choose any available region*
    - **Name**: *Enter a unique name*
    - **Pricing tier**: Standard S0
3. Select the required checkboxes and create the resource.
4. Wait for deployment to complete, and then view the deployment details.
5. When the resource has been deployed, go to it and view its **Keys and Endpoint** page. You will need one of the keys and the location in which the service is provisioned from this page in the next procedure.

## Prepare to use the Speech service

In this exercise, you'll complete a partially implemented client application that uses the Speech SDK to recognize and synthesize speech.

> **Note**: You can choose to use the SDK for either **C#** or **Python**. In the steps below, perform the actions appropriate for your preferred language.

1. In Visual Studio Code, in the **Explorer** pane, browse to the **07-speech** folder and expand the **C-Sharp** or **Python** folder depending on your language preference.
2. Right-click the **speaking-clock** folder and open an integrated terminal. Then install the Speech SDK package by running the appropriate command for your language preference:

**C#**

```
dotnet add package Microsoft.CognitiveServices.Speech --version 1.14.0
```

**Python**

```
pip install azure-cognitiveservices-speech==1.14.0
```

3. View the contents of the **speaking-clock** folder, and note that it contains a file for configuration settings:
    - **C#**: appsettings.json
    - **Python**: .env

    Open the configuration file and update the configuration values it contains to include an authentication **key** for your cognitive services resource, and the **location** where it is deployed. Save your changes.
4. Note that the **speaking-clock** folder contains a code file for the client application:

    - **C#**: Program.cs
    - **Python**: speaking-clock&period;py

    Open the code file and at the top, under the existing namespace references, find the comment **Import namespaces**. Then, under this comment, add the following language-specific code to import the namespaces you will need to use the Speech SDK:

**C#**

```C#
// Import namespaces
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
```

**Python**

```Python
# Import namespaces
import azure.cognitiveservices.speech as speech_sdk
```

5. In the **Main** function, note that code to load the cognitive services key and region from the configuration file has already been provided. You must use these variables to create a **SpeechConfig** for your cognitive services resource. Add the following code under the comment **Configure speech service**:

**C#**

```C#
// Configure speech service
speechConfig = SpeechConfig.FromSubscription(cogSvcKey, cogSvcRegion);
Console.WriteLine("Ready to use speech service in " + speechConfig.Region);
```

**Python**

```Python
# Configure speech service
speech_config = speech_sdk.SpeechConfig(cog_key, cog_region)
print('Ready to use speech service in:', speech_config.region)
```

6. Save your changes and return to the integrated terminal for the **speaking-clock** folder, and enter the following command to run the program:

**C#**

```
dotnet run
```

**Python**

```
python speaking-clock.py
```

7. If you are using C#, you can ignore any warnings about using the **await** operator in asynchronous methods - we'll fix that later. The code should display the region of the speech service resource the application will use.

## Recognize speech

Now that you have a **SpeechConfig** for the speech service in your cognitive services resource, you can use the **Speech-to-text** API to recognize speech and transcribe it to text.

1. In the **Main** function for your program, note that the code uses the **TranscribeCommand** function to accept spoken input.
2. In the **TranscribeCommand** function, under the comment **Configure speech recognition**, add the following code to create a **SpeechRecognizer** client that can be used to recognize and transcribe speech using the default system microphone for input:

**C#**

```C#
// Configure speech recognition
using AudioConfig audioConfig = AudioConfig.FromDefaultMicrophoneInput();
using SpeechRecognizer speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);
```

**Python**

```Python
# Configure speech recognition
audio_config = speech_sdk.AudioConfig(use_default_microphone=True)
speech_recognizer = speech_sdk.SpeechRecognizer(speech_config, audio_config)
```

> **Note**: *You can also recognize speech input from an audio file by modifying the **AudioConfig** object to reference an file path.*

3. In the **TranscribeCommand** function, under the comment **Process speech input**, add the following code to listen for spoken input, being careful not to replace the code at the end of the function that returns the command:

**C#**

```C#
// Process speech input
Console.WriteLine("Say 'stop' to end...");
SpeechRecognitionResult speech = await speechRecognizer.RecognizeOnceAsync();
if (speech.Reason == ResultReason.RecognizedSpeech)
{
    command = speech.Text;
    Console.WriteLine(command);
}
else
{
    Console.WriteLine(speech.Reason);
    if (speech.Reason == ResultReason.Canceled)
    {
        var cancellation = CancellationDetails.FromResult(speech);
        Console.WriteLine(cancellation.Reason);
        Console.WriteLine(cancellation.ErrorDetails);
    }
}
```

**Python**

```Python
# Process speech input
print('Say "stop" to end...')
speech = speech_recognizer.recognize_once_async().get()
if speech.reason == speech_sdk.ResultReason.RecognizedSpeech:
    command = speech.text
    print(command)
else:
    print(speech.reason)
    if speech.reason == speech_sdk.ResultReason.Canceled:
        cancellation = speech.cancellation_details
        print(cancellation.reason)
        print(cancellation.error_details)
```

4. Save your changes and return to the integrated terminal for the **speaking-clock** folder, and enter the following command to run the program:

**C#**

```
dotnet run
```

**Python**

```
python speaking-clock.py
```

5. When prompted, speak clearly into the microphone and say "what time is it?". The program should transcribe your spoken input and display the time (based on the local time of the computer where the code is running, which may not be the correct time where you are). When prompted again, say "stop" to end the program.

    > **Note** *The SpeechRecognizer gives you around 5 seconds to speak. If it detects no spoken input, it produces a "No match" result. Since the code in the application uses a default command of "stop.", the program will then end.*
    > 
    > *If the SpeechRecognizer encounters an error, it produces a result of "Cancelled". The code in the application will then display the error message. The most likely cause is an incorrect key or region in the configuration file.*

## Synthesize speech

Your speaking clock application accepts spoken input, but it doesn't actually speak! Let's fix that by adding code to synthesize speech.

1. In the **Main** function for your program, note that the code uses the **TellTime** function to tell the user the current time.
2. In the **TellTime** function, under the comment **Configure speech synthesis**, add the following code to create a **SpeechSynthesizer** client that can be used to generate spoken output:

**C#**

```C#
// Configure speech synthesis
using SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer(speechConfig);
```

**Python**

```Python
# Configure speech synthesis
speech_synthesizer = speech_sdk.SpeechSynthesizer(speech_config)
```

> **Note**: *The default audio configuration uses the default system audio device for output, so you don't need to explicitly provide an **AudioConfig**. If you need to redirect audio output to a file, you can use an **AudioConfig** with a filepath to do so.*

3. In the **TellTime** function, under the comment **Synthesize spoken output**, add the following code to generate spoken output, being careful not to replace the code at the end of the function that prints the response:

**C#**

```C#
// Synthesize spoken output
SpeechSynthesisResult speak = await speechSynthesizer.SpeakTextAsync(responseText);
if (speak.Reason != ResultReason.SynthesizingAudioCompleted)
{
    Console.WriteLine(speak.Reason);
}
```

**Python**

```Python
# Synthesize spoken output
speak = speech_synthesizer.speak_text_async(response_text).get()
if speak.reason != speech_sdk.ResultReason.SynthesizingAudioCompleted:
    print(speak.reason)
```

4. Save your changes and return to the integrated terminal for the **speaking-clock** folder, and enter the following command to run the program:

**C#**

```
dotnet run
```

**Python**

```
python speaking-clock.py
```

5. When prompted, speak clearly into the microphone and say "what time is it?". The program should speak, telling you the time. When prompted again, say "stop" to end the program.

## Use a different voice

Your speaking clock application uses a default voice, which you can change. The Speech service supports a range of *standard* voices as well as more human-like *neural* voices. You can also create *custom* voices.

> **Note**: For a list of neural and standard voices, see [Language and voice support](https://docs.microsoft.com/azure/cognitive-services/speech-service/language-support#text-to-speech) in the Speech service documentation.  Availability of standard, neural, and custom voices varies by region. See [Speech service supported regions](https://docs.microsoft.com/azure/cognitive-services/speech-service/regions#standard-and-neural-voices) for more details.

1. In the **TellTime** function, under the comment **Configure speech synthesis**, modify the code as follows to specify an alternative voice before creating the **SpeechSynthesizer** client:

   **C#**

```C#
// Configure speech synthesis
speechConfig.SpeechSynthesisVoiceName = "en-GB-George"; // add this
using SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer(speechConfig);
```

**Python**

```Python
# Configure speech synthesis
speech_config.speech_synthesis_voice_name = 'en-GB-George' # add this
speech_synthesizer = speech_sdk.SpeechSynthesizer(speech_config)
```

2. Save your changes and return to the integrated terminal for the **speaking-clock** folder, and enter the following command to run the program:

**C#**

```
dotnet run
```

**Python**

```
python speaking-clock.py
```

3. When prompted, speak clearly into the microphone and say "what time is it?". The program should speak in the specified voice, telling you the time. When prompted again, say "stop" to end the program.

## More information

For more information about using the **Speech-to-text** and **Text-to-speech** APIs, see the [Speech-to-text documentation](https://docs.microsoft.com/azure/cognitive-services/speech-service/index-speech-to-text) and [Text-to-speech documentation](https://docs.microsoft.com/azure/cognitive-services/speech-service/index-text-to-speech).
