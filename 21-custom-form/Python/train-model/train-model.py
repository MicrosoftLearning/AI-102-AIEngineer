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
        trainingDataUrl = os.getenv('STORAGE_URL')

        # Authenticate Form Training Client
        form_recognizer_client = FormRecognizerClient(form_endpoint, AzureKeyCredential(form_key))
        form_training_client = FormTrainingClient(form_endpoint, AzureKeyCredential(form_key))

        # Train model 
        poller = form_training_client.begin_training(trainingDataUrl, use_training_labels=False)
        model = poller.result()

        print("Model ID: {}".format(model.model_id))
        print("Status: {}".format(model.status))
        print("Training started on: {}".format(model.training_started_on))
        print("Training completed on: {}".format(model.training_completed_on))

    except Exception as ex:
        print(ex)

if __name__ == '__main__': 
    main()