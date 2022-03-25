from dotenv import load_dotenv
import os
import json
from datetime import datetime, timedelta, date
from dateutil.parser import parse as is_date

# Import namespaces
from azure.core.credentials import AzureKeyCredential

from azure.ai.language.conversations import ConversationAnalysisClient
from azure.ai.language.conversations.models import ConversationAnalysisOptions


def main():

    try:
        # Get Configuration Settings
        load_dotenv()
        ls_prediction_endpoint = os.getenv('LS_CONVERSATIONS_ENDPOINT')
        ls_prediction_key = os.getenv('LS_CONVERSATIONS_KEY')

        # Get user input (until they enter "quit")
        userText = ''
        while userText.lower() != 'quit':
            userText = input('\nEnter some text ("quit" to stop)\n')
            if userText.lower() != 'quit':

                # Create a client for the Language service model
                client = ConversationAnalysisClient(ls_prediction_endpoint, AzureKeyCredential(ls_prediction_key))

                # Call the Language service model to get intent and entities
                convInput = ConversationAnalysisOptions(
                    query = userText
                    )
                
                cls_project = 'Clock'
                deployment_slot = 'production'

                with client:
                    result = client.analyze_conversations(
                        convInput, 
                        project_name=cls_project, 
                        deployment_name=deployment_slot
                        )

                # list the prediction results
                top_intent = result.prediction.top_intent
                entities = result.prediction.entities

                print("view top intent:")
                print("\ttop intent: {}".format(result.prediction.top_intent))
                print("\tcategory: {}".format(result.prediction.intents[0].category))
                print("\tconfidence score: {}\n".format(result.prediction.intents[0].confidence_score))

                print("view entities:")
                for entity in entities:
                    print("\tcategory: {}".format(entity.category))
                    print("\ttext: {}".format(entity.text))
                    print("\tconfidence score: {}".format(entity.confidence_score))

                print("query: {}".format(result.query))

                # Apply the appropriate action
                if top_intent == 'GetTime':
                    location = 'local'
                    # Check for entities
                    if len(entities) > 0:
                        # Check for a location entity
                        for entity in entities:
                            if 'Location' == entity.category:
                                # ML entities are strings, get the first one
                                location = entity.text
                    # Get the time for the specified location
                    print(GetTime(location))

                elif top_intent == 'GetDay':
                    date_string = date.today().strftime("%m/%d/%Y")
                    # Check for entities
                    if len(entities) > 0:
                        # Check for a Date entity
                        for entity in entities:
                            if 'Date' == entity.category:
                                # Regex entities are strings, get the first one
                                date_string = entity.text
                    # Get the day for the specified date
                    print(GetDay(date_string))

                elif top_intent == 'GetDate':
                    day = 'today'
                    # Check for entities
                    if len(entities) > 0:
                        # Check for a Weekday entity
                        for entity in entities:
                            if 'Weekday' == entity.category:
                            # List entities are lists
                                day = entity.text
                    # Get the date for the specified day
                    print(GetDate(day))

                else:
                    # Some other intent (for example, "None") was predicted
                    print('Try asking me for the time, the day, or the date.')

    except Exception as ex:
        print(ex)


def GetTime(location):
    time_string = ''

    # Note: To keep things simple, we'll ignore daylight savings time and support only a few cities.
    # In a real app, you'd likely use a web service API (or write  more complex code!)
    # Hopefully this simplified example is enough to get the the idea that you
    # use LU to determine the intent and entities, then implement the appropriate logic

    if location.lower() == 'local':
        now = datetime.now()
        time_string = '{}:{:02d}'.format(now.hour,now.minute)
    elif location.lower() == 'london':
        utc = datetime.utcnow()
        time_string = '{}:{:02d}'.format(utc.hour,utc.minute)
    elif location.lower() == 'sydney':
        time = datetime.utcnow() + timedelta(hours=11)
        time_string = '{}:{:02d}'.format(time.hour,time.minute)
    elif location.lower() == 'new york':
        time = datetime.utcnow() + timedelta(hours=-5)
        time_string = '{}:{:02d}'.format(time.hour,time.minute)
    elif location.lower() == 'nairobi':
        time = datetime.utcnow() + timedelta(hours=3)
        time_string = '{}:{:02d}'.format(time.hour,time.minute)
    elif location.lower() == 'tokyo':
        time = datetime.utcnow() + timedelta(hours=9)
        time_string = '{}:{:02d}'.format(time.hour,time.minute)
    elif location.lower() == 'delhi':
        time = datetime.utcnow() + timedelta(hours=5.5)
        time_string = '{}:{:02d}'.format(time.hour,time.minute)
    else:
        time_string = "I don't know what time it is in {}".format(location)
    
    return time_string

def GetDate(day):
    date_string = 'I can only determine dates for today or named days of the week.'

    weekdays = {
        "monday":0,
        "tuesday":1,
        "wednesday":2,
        "thusday":3,
        "friday":4,
        "saturday":5,
        "sunday":6
    }

    today = date.today()

    # To keep things simple, assume the named day is in the current week (Sunday to Saturday)
    day = day.lower()
    if day == 'today':
        date_string = today.strftime("%m/%d/%Y")
    elif day in weekdays:
        todayNum = today.weekday()
        weekDayNum = weekdays[day]
        offset = weekDayNum - todayNum
        date_string = (today + timedelta(days=offset)).strftime("%m/%d/%Y")

    return date_string

def GetDay(date_string):
    # Note: To keep things simple, dates must be entered in US format (MM/DD/YYYY)
    try:
        date_object = datetime.strptime(date_string, "%m/%d/%Y")
        day_string = date_object.strftime("%A")
    except:
        day_string = 'Enter a date in MM/DD/YYYY format.'
    return day_string

if __name__ == "__main__":
    main()