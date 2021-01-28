import os 
from dotenv import load_dotenv

from azure.core.exceptions import ResourceNotFoundError
from azure.ai.formrecognizer import FormRecognizerClient
from azure.ai.formrecognizer import FormTrainingClient
from azure.core.credentials import AzureKeyCredential

def main(): 
        
    try: 
    
        # Get configuration settings 
        load_dotenv()
        form_endpoint = os.getenv('FORM_ENDPOINT')
        form_key = os.getenv('FORM_KEY')
        
        # Create client using endpoint and key
        form_recognizer_client = FormRecognizerClient(form_endpoint, AzureKeyCredential(form_key))
        form_training_client = FormTrainingClient(form_endpoint, AzureKeyCredential(form_key))

        # Model ID from when you trained your model.
        model_id = os.getenv('MODEL_ID')

        # Test trained model with a new form 
        with open('test1.jpg', "rb") as f: 
            poller = form_recognizer_client.begin_recognize_custom_forms(
                model_id=model_id, form=f)

        result = poller.result()

        for recognized_form in result:
            print("Form type: {}".format(recognized_form.form_type))
            for name, field in recognized_form.fields.items():
                print("Field '{}' has label '{}' with value '{}' and a confidence score of {}".format(
                    name,
                    field.label_data.text if field.label_data else name,
                    field.value,
                    field.confidence
                ))

    except Exception as ex:
        print(ex)

if __name__ == '__main__': 
    main()