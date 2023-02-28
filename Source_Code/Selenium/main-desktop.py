# import selenium, firefox options, keys to type special keys, by to search elements on the page
from selenium import webdriver
from selenium.webdriver.firefox.options import Options
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.common.by import By


# initialise options, not headless because this is the desktop testing, we'd like to see what is going on
# setup the driver with firefox
options = Options()
options.headless = False
driver = webdriver.Firefox(options=options)

# go to our homepage and check that we're at the homepage
driver.get("https://app.recipethesaurus.software/")
assert "Home Page" in driver.title

# go to secrets login page
elem = driver.find_element(By.LINK_TEXT, "Secrets")
elem.click()
assert "Login" in driver.title



input()
driver.quit()