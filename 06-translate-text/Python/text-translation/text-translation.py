from dotenv import load_dotenv
import os
import requests, json

def main():
    global translator_endpoint
    global cog_key
    global cog_region

    try:
        # Get Configuration Settings
        load_dotenv()
        cog_key = os.getenv('449a474bfa594b30ace7d45e228e9d14')
        cog_region = os.getenv('eastus')
        translator_endpoint = 'https://api.cognitive.microsofttranslator.com'

        # Analyze each text file in the reviews folder
        reviews_folder = 'reviews'
        for file_name in os.listdir(reviews_folder):
            # Read the file contents
            print('\n-------------\n' + file_name)
            text = open(os.path.join(reviews_folder, file_name), encoding='utf8').read()
            print('\n' + text)

            # Detect the language
            language = GetLanguage(text)
            print('Language:',language)

            # Translate if not already English
            if language != 'en':
                translation = Translate(text, language)
                print("\nTranslation:\n{}".format(translation))
                
    except Exception as ex:
        print(ex)

def GetLanguage(text):
    # Default language is English
    language = 'en'

    # Use the Translator detect function


    # Return the language
    return language


    # Use the Translator translate function
def Translate(text, source_language):
    translation = ''

    # Use the Translator translate function
    path = '/translate'
    url = translator_endpoint + path

    # Build the request
    params = {
        'api-version': '3.0',
        'from': source_language,
        'to': ['en']
    }

    headers = {
        'Ocp-Apim-Subscription-Key': cog_key,
        'Ocp-Apim-Subscription-Region': cog_region,
        'Content-type': 'application/json'
    }

    body = [{
        'text': text
    }]

    # Send the request and get response
    request = requests.post(url, params=params, headers=headers, json=body)
    response = request.json()

    # Parse JSON array and get translation
    translation = response[0]['translations'][0]['text']

    # Return the translation
    return translation

if __name__ == "__main__":
    main()
