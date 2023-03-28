CREATE DATABASE recipeAppDb;
USE recipeAppDb;

CREATE TABLE users (
username varchar(20) NOT NULL,
firstname varchar(20),
lastname varchar(20),
email varchar(50),
profilePic varchar(100),
PRIMARY KEY (username)
);

CREATE TABLE recipes (
recipeId int NOT NULL,
recipeName varchar(100) NOT NULL,
recipeDescription varchar(200),
recipeInstructions varchar(1000),
recipeLikes int,
imageURL varchar(100),
recipeAuthor varchar(20),
PRIMARY KEY (recipeId),
FOREIGN KEY (recipeAuthor) REFERENCES users(username)
);

CREATE TABLE userIngredientLikes (
username varchar(20) NOT NULL,
ingredient varchar(20) NOT NULL,
FOREIGN KEY (username) REFERENCES users(username)
);

CREATE TABLE userIngredientDislikes (
username varchar(20) NOT NULL,
ingredient varchar(20) NOT NULL,
FOREIGN KEY (username) REFERENCES users(username)
);

CREATE TABLE userFridge (
username varchar(20) NOT NULL,
ingredient varchar(20) NOT NULL,
FOREIGN KEY (username) REFERENCES users(username)
);

CREATE TABLE savedRecipes (
username varchar(20) NOT NULL,
recipeId int NOT NULL,
FOREIGN KEY (username) REFERENCES users(username),
FOREIGN KEY (recipeId) REFERENCES recipes(recipeId)
);

CREATE TABLE recipeIngredients (
recipeId int NOT NULL,
FOREIGN KEY (recipeId) REFERENCES recipes(recipeId)
);












