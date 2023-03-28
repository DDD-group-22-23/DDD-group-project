/* INSERT INTO users VALUES (username, firstname, lastname, email, profilePic);
INSERT INTO userIngredientLikes VALUES (username, ingredient);
INSERT INTO userIngredientDislikes VALUES (username, ingredient);
INSERT INTO userFridge VALUES (username, ingredient); */

/* users */
USE recipeappdb;

INSERT INTO users VALUES ("jasper", "jasper", "Johnson", "jasper@RecipeThesaurus.software", null);
INSERT INTO users VALUES ("tanika", "Tankia", "Astley", "tanika@RecipeThesaurus.software", null);
INSERT INTO users VALUES ("fernando", "fernando", "Ansley", "fernando@RecipeThesaurus.software", null);
INSERT INTO users VALUES ("caitlin", "Caitlin", "Ashpool", "caitlin@RecipeThesaurus.software", null);
INSERT INTO users VALUES ("david", "David", "Cain", "d.p.cain-2021@hull.ac.uk", null);
INSERT INTO users VALUES ("nikolai", "Nikolai", "Valkamo", "n.valkamo-2021@hull.ac.uk", null);
INSERT INTO users VALUES ("rowan", "Rowan", "Clark", "matthew.clark-2021@hull.ac.uk", null);
INSERT INTO users VALUES ("lawrence", "Lawrence", "Gibson", "l.gibson-2021@hull.ac.uk", null);
INSERT INTO users VALUES ("chris", "Christopher", "Boczko", "c.j.boczko-2020@hull.ac.uk", null);

/* user preferences */

INSERT INTO userIngredientLikes VALUES ("jasper", "salmon");
INSERT INTO userIngredientLikes VALUES ("jasper", "egg");
INSERT INTO userIngredientLikes VALUES ("jasper", "pasta");

INSERT INTO userIngredientDislikes VALUES ("jasper", "melon");
INSERT INTO userIngredientDislikes VALUES ("jasper", "cucumber");



INSERT INTO userIngredientLikes VALUES ("tanika", "hotdog");
INSERT INTO userIngredientLikes VALUES ("tanika", "chilli");

INSERT INTO userIngredientDislikes VALUES ("tanika", "mashed potato");




INSERT INTO userIngredientLikes VALUES ("david", "egg");
INSERT INTO userIngredientLikes VALUES ("david", "sweetcorn");




INSERT INTO userIngredientLikes VALUES ("nikolai", "rice krispies");
