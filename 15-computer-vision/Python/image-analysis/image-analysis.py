from dotenv import load_dotenv
import os
from array import array
from PIL import Image, ImageDraw
import sys
import time
from matplotlib import pyplot as plt
import numpy as np

# Import namespaces


def main():
    global cv_client

    try:
        # Get Configuration Settings
        load_dotenv()
        cog_endpoint = os.getenv('COG_SERVICE_ENDPOINT')
        cog_key = os.getenv('COG_SERVICE_KEY')

        # Get image
        image_file = 'images/street.jpg'
        if len(sys.argv) > 1:
            image_file = sys.argv[1]

        # Authenticate Computer Vision client


        # Analyze image
        AnalyzeImage(image_file)

        # Generate thumbnail
        GetThumbnail(image_file)

    except Exception as ex:
        print(ex)

def AnalyzeImage(image_file):
    print('Analyzing', image_file)

    # Specify features to be retrieved
    
    
    # Get image analysis

        

def GetThumbnail(image_file):
    print('Generating thumbnail')

    # Generate a thumbnail


if __name__ == "__main__":
    main()
