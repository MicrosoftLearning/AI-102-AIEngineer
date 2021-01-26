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
        # To train a model you need your Blob URI to access your training files
        trainingDataUrl = os.getenv('STORAGE_URL')

        # Create client using endpoint and key
        form_recognizer_client = FormRecognizerClient(form_endpoint, AzureKeyCredential(form_key))
        form_training_client = FormTrainingClient(form_endpoint, AzureKeyCredential(form_key))

        # Use Training Labels = False 
        poller = form_training_client.begin_training(trainingDataUrl, use_training_labels=False)
        model = poller.result()

        print("Model ID: {}".format(model.model_id))
        print("Status: {}".format(model.status))
        print("Training started on: {}".format(model.training_started_on))
        print("Training completed on: {}".format(model.training_completed_on))

        print("\nRecognized fields:")
        for submodel in model.submodels:
            print(
                "The submodel with form type '{}' has recognized the following fields: {}".format(
                    submodel.form_type,
                    ", ".join(
                        [
                            field.label if field.label else name
                            for name, field in submodel.fields.items()
                        ]
                    ),
                )
            )

        # Training result information
        for doc in model.training_documents:
            print("Document name: {}".format(doc.name))
            print("Document status: {}".format(doc.status))
            print("Document page count: {}".format(doc.page_count))
            print("Document errors: {}".format(doc.errors))

    except Exception as ex:
        print(ex)

if __name__ == '__main__': 
    main()