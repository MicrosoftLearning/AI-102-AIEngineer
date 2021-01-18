from dotenv import load_dotenv
import os
from PIL import Image, ImageDraw
from matplotlib import pyplot as plt

# Import namespaces



def main():

    global cv_client

    try:
        # Get Configuration Settings
        load_dotenv()
        cog_endpoint = os.getenv('COG_SERVICE_ENDPOINT')
        cog_key = os.getenv('COG_SERVICE_KEY')

        # Authenticate Computer Vision client


        # Detect faces in an image
        image_file = os.path.join('images','people.jpg')
        AnalyzeFaces(image_file)

    except Exception as ex:
        print(ex)

def AnalyzeFaces(image_file):
    print('Analyzing', image_file)

    # Specify features to be retrieved (faces)
    

    # Get image analysis



if __name__ == "__main__":
    main()