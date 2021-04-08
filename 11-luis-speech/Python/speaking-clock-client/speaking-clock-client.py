from dotenv import load_dotenv
import os
import json
from datetime import datetime, timedelta, date
from dateutil.parser import parse as is_date

# Import namespaces


def main():

    try:
        # Get Configuration Settings
        load_dotenv()
        lu_app_id = os.getenv('LU_APP_ID')
        lu_prediction_region = os.getenv('LU_PREDICTION_REGION')
        lu_prediction_key = os.getenv('LU_PREDICTION_KEY')

        # Configure speech service and get intent recognizer


        # Get the model from the AppID and add the intents we want to use


        # Process speech input


    except Exception as ex:
        print(ex)


def GetTime(location):
    time_string = ''

    # Note: To keep things simple, we'll ignore daylight savings time and support only a few cities.
    # In a real app, you'd likely use a web service API (or write  more complex code!)
    # Hopefully this simplified example is enough to get the the idea that you
    # use LU to determine the intent and entitites, then implement the appropriate logic

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