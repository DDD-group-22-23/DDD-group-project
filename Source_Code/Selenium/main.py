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

# initialise options, not headless because this is the desktop testing, we'd like to see what is going on
# setup the driver with firefox
options = Options()
options.headless = True
driver = webdriver.Firefox(options=options)

# go to our homepage and check that we're at the homepage
try:
    driver.get("https://app.recipethesaurus.software/")    
except exceptions.WebDriverException as e:
    print(e)
    print("Site was not found, is it up?")
    print("If you're seeing this, the program will terminate as the site cannot be reached")
    exit(1)
assert "Home Page" in driver.title
# go to secrets login page
elem = driver.find_element(By.LINK_TEXT, "Secrets")
elem.click()
assert "Login" in driver.title
try:
    #try fetch the username and password from environment variables
    username = environ['PROJECT_USERNAME']
    password = environ['PROJECT_PASSWORD']
except KeyError as e:
    # if one or more don't exist
    print(e)
    print("Either no username or no password was found. I cannot login")
driver.quit()