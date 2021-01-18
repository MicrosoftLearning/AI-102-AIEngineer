from dotenv import load_dotenv
import os

# Import namespaces


def main():
    try:
        # Get Configuration Settings
        load_dotenv()
        cog_endpoint = os.getenv('COG_SERVICE_ENDPOINT')
        cog_key = os.getenv('COG_SERVICE_KEY')

        # Create client using endpoint and key


        # Analyze each text file in the reviews folder
        reviews_folder = 'reviews'
        for file_name in os.listdir(reviews_folder):
            # Read the file contents
            print('\n-------------\n' + file_name)
            text = open(os.path.join(reviews_folder, file_name), encoding='utf8').read()
            print('\n' + text)

            # Get language


            # Get sentiment


            # Get key phrases


            # Get entities


            # Get linked entities



    except Exception as ex:
        print(ex)


if __name__ == "__main__":
    main()