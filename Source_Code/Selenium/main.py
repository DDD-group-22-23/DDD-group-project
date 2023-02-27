from selenium import webdriver
from selenium.webdriver.firefox.options import Options

options = Options()
options.headless = True
driver = webdriver.Firefox(options=options)
driver.get("https://app.recipethesaurus.software/")
print ("Headless Firefox Initialized")
driver.quit()