from dotenv import load_dotenv
import os
from PIL import Image, ImageDraw
from matplotlib import pyplot as plt

# Import namespaces


def main():

    global face_client

    try:
        # Get Configuration Settings
        load_dotenv()
        cog_endpoint = os.getenv('COG_SERVICE_ENDPOINT')
        cog_key = os.getenv('COG_SERVICE_KEY')

        # Authenticate Face client


        # Menu for face functions
        print('1: Detect faces\n2: Compare faces\n3: Train a facial recognition model\n4: Recognize faces\n5: Verify a face\nAny other key to quit')
        command = input('Enter a number:')
        if command == '1':
            DetectFaces(os.path.join('images','people.jpg'))
        elif command =='2':
            person_image = os.path.join('images','person1.jpg') # Also try person2.jpg
            CompareFaces(person_image, os.path.join('images','people.jpg'))
        elif command =='3':
            names = ['Aisha', 'Sama']
            TrainModel('employees_group', 'employees', names)
        elif command =='4':
            RecognizeFaces(os.path.join('images','people.jpg'), 'employees_group')
        elif command == '5':
            VerifyFace(os.path.join('images','person1.jpg'), 'Aisha', 'employees_group')
                

    except Exception as ex:
        print(ex)

def DetectFaces(image_file):
    print('Detecting faces in', image_file)

    # Specify facial features to be retrieved


    # Get faces



def CompareFaces(image_1, image_2):
    print('Comparing faces in ', image_1, 'and', image_2)




def TrainModel(group_id, group_name, image_folders):
    print('Creating model for', group_id)



def RecognizeFaces(image_file, group_id):
    print('Recognizing faces in', image_file)



def VerifyFace(person_image, person_name, group_id):
    print('Verifying the person in', person_image, 'is', person_name)

    result = "Not verified"

    # Get the ID of the person from the people group

                        
    # print the result
    print(result)
    

if __name__ == "__main__":
    main()