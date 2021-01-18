from dotenv import load_dotenv
import os
import time
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
        
        


        # Menu for text reading functions
        print('1: Use OCR API\n2: Use Read API\n3: Read handwriting\nAny other key to quit')
        command = input('Enter a number:')
        if command == '1':
            image_file = os.path.join('images','Lincoln.jpg')
            GetTextOcr(image_file)
        elif command =='2':
            image_file = os.path.join('images','Rome.pdf')
            GetTextRead(image_file)
        elif command =='3':
            image_file = os.path.join('images','Note.jpg')
            GetTextRead(image_file)
                

    except Exception as ex:
        print(ex)

def GetTextOcr(image_file):
    print('Reading text in {}\n'.format(image_file))



def GetTextRead(image_file):
    print('Reading text in {}\n'.format(image_file))




if __name__ == "__main__":
    main()