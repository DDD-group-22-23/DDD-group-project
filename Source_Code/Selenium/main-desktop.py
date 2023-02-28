#!/usr/bin/env python3
# import selenium, firefox options, keys to type special keys, by to search elements on the page
from selenium import webdriver
from selenium.webdriver.firefox.options import Options
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.common.by import By
from selenium.common import exceptions

# import environ so we can work with environment variables from setup-envs.sh
# the script simply has two key value pairs 
# PROJECT_USERNAME
# PROJECT_PASSWORD
# and exports them
# to make them available edit setup-envs.sh.example and fill in the username and password used for secrets/backoffice
# rename the file to setup-envs.sh
# run source setup-envs.sh to have the variables become available
from os import environ

def GoToPage(url):
    try:
        driver.get(url)    
    except exceptions.WebDriverException as e:
        print(e)
        print("Page was not found, is it up?")
        print("If you're seeing this, the program will terminate as the site cannot be reached")
        if url == "https://app.recipethesaurus.software/":
            exit(1)

def Click(id, key):
    try:
        elem = driver.find_element(id, key)
        elem.click()
    except exceptions.NoSuchElementException as e:
        print(e)
        print("Element not found")

def Type(id, key, text):
    elem = driver.find_element(id,key)
    elem.send_keys(text)

# initialise options, not headless because this is the desktop testing, we'd like to see what is going on
# and trust self signed certs
# setup the driver with firefox
options = Options()
options.headless = False
options.accept_untrusted_certs = True
driver = webdriver.Firefox(options=options)

GoToPage("https://app.recipethesaurus.software/")
assert "Home Page" in driver.title

Click(By.LINK_TEXT,"Secrets")
assert "Login" in driver.title
try:
    #try fetch the username and password from environment variables
    username = environ['PROJECT_USERNAME']
    password = environ['PROJECT_PASSWORD']

except KeyError as e:
    # if one or more don't exist
    print(e)
    print("Either no username or no password was found. I cannot login")

if username:
    if password:
        #they exist
       Type(By.NAME, "loginId", username)
       Type(By.NAME, "password", password)
       Click(By.XPATH, "//button[contains(text(), 'Submit')]")



input("I'm finished, press enter to quit")
driver.quit()
