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
        cog_key = os.getenv('COG_SERVICE_KEY')
        cog_region = os.getenv('COG_SERVICE_REGION')
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

def Translate(text, source_language):
    translation = ''

    # Use the Translator translate function


    # Return the translation
    return translation

if __name__ == "__main__":
    main()