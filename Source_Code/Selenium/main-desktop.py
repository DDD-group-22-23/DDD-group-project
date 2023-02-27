from selenium import webdriver
from selenium.webdriver.firefox.options import Options

options = Options()
options.headless = False
driver = webdriver.Firefox(options=options)
driver.get("https://app.recipethesaurus.software/")
# print ("Headless Firefox Initialized")
input()
driver.quit()