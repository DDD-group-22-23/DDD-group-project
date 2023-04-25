using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Identity.Client;

namespace RecipeThesaurus
{
    public class DBManager
    {
        public SqlConnection conn;
        public SqliteConnection conn2;
        public RecipesManager recipesManager;
        public UserManager userManager;
        public bool SQL_VER;
        public bool RUN_WEBPAGE;

        public DBManager(bool sql)
        {
            SQL_VER = sql;
            if (sql)
            {
                // Add code for sql connection
            }
            else
            {
                var connBuilder = new SqliteConnectionStringBuilder();
                connBuilder.DataSource = "./sqliteDB.db";
                conn2 = new SqliteConnection(connBuilder.ConnectionString);
                userManager = new UserManager(conn2);
                recipesManager = new RecipesManager(conn2);
            }
        }

        public void SetConnection(SqlConnection con)
        {
            conn = con;
            recipesManager.conn = con;
        }
        public void SetConnection(SqliteConnection con)
        {
            conn2 = con;
            recipesManager.conn2 = con;
        }

        // Creates sqlite database for use
        public void createDb()
        {
            using (conn2)
            {
                conn2.Open();

                var delCmd = conn2.CreateCommand();
                delCmd.CommandText = "DROP TABLE IF EXISTS userIngredientLikes;" +
                    "DROP TABLE IF EXISTS userIngredientDislikes;" +
                    "DROP TABLE IF EXISTS userFridge;" +
                    "DROP TABLE IF EXISTS savedRecipes;" +
                    "DROP TABLE IF EXISTS recipeIngredients;" +
                    "DROP TABLE IF EXISTS recipes;" +
                    "DROP TABLE IF EXISTS users;";
                delCmd.ExecuteNonQuery();

                var createDatabase = conn2.CreateCommand();
                createDatabase.CommandText = @"CREATE TABLE users(
                username varchar(20) NOT NULL,
                firstname varchar(20),
                lastname varchar(20),
                email varchar(50),
                profilePic varchar(100),
                PRIMARY KEY(username)
                );

                CREATE TABLE recipes(
                recipeId int NOT NULL,
                recipeName varchar(100) NOT NULL,
                recipeDescription varchar(200),
                recipeInstructions varchar(2000),
                recipeLikes int,
                imageURL varchar(100),
                recipeAuthor varchar(20),
                PRIMARY KEY(recipeId)
                );

                CREATE TABLE userIngredientLikes(
                username varchar(20) NOT NULL,
                ingredient varchar(20) NOT NULL,
                FOREIGN KEY(username) REFERENCES users(username)
                );

                CREATE TABLE userIngredientDislikes(
                username varchar(20) NOT NULL,
                ingredient varchar(20) NOT NULL,
                FOREIGN KEY(username) REFERENCES users(username)
                );

                CREATE TABLE userFridge(
                username varchar(20) NOT NULL,
                ingredient varchar(20) NOT NULL,
                FOREIGN KEY(username) REFERENCES users(username)
                );

                CREATE TABLE savedRecipes(
                username varchar(20) NOT NULL,
                recipeId int NOT NULL,
                FOREIGN KEY(username) REFERENCES users(username),
                FOREIGN KEY(recipeId) REFERENCES recipes(recipeId)
                );

                CREATE TABLE recipeIngredients(
                recipeId int NOT NULL,
                ingredient varchar(20) NOT NULL,
                FOREIGN KEY(recipeId) REFERENCES recipes(recipeId)
                );
                ";
                createDatabase.ExecuteNonQuery();

                var fillDatabase = conn2.CreateCommand();
                fillDatabase.CommandText = @"INSERT INTO users VALUES ('jasper', 'jasper', 'Johnson', 'jasper@RecipeThesaurus.software', null);
                INSERT INTO users VALUES ('tanika', 'Tankia', 'Astley', 'tanika@RecipeThesaurus.software', null);
                INSERT INTO users VALUES ('fernando', 'fernando', 'Ansley', 'fernando@RecipeThesaurus.software', null);
                INSERT INTO users VALUES ('caitlin', 'Caitlin', 'Ashpool', 'caitlin@RecipeThesaurus.software', null);
                INSERT INTO users VALUES ('david', 'David', 'Cain', 'd.p.cain-2021@hull.ac.uk', null);
                INSERT INTO users VALUES ('nikolai', 'Nikolai', 'Valkamo', 'n.valkamo-2021@hull.ac.uk', null);
                INSERT INTO users VALUES ('rowan', 'Rowan', 'Clark', 'matthew.clark-2021@hull.ac.uk', null);
                INSERT INTO users VALUES ('lawrence', 'Lawrence', 'Gibson', 'l.gibson-2021@hull.ac.uk', null);
                INSERT INTO users VALUES ('chris', 'Christopher', 'Boczko', 'c.j.boczko-2020@hull.ac.uk', null);

                /* user preferences */

                INSERT INTO userIngredientLikes VALUES ('jasper', 'salmon');
                INSERT INTO userIngredientLikes VALUES ('jasper', 'egg');
                INSERT INTO userIngredientLikes VALUES ('jasper', 'pasta');


                INSERT INTO userIngredientDislikes VALUES ('jasper', 'melon');
                INSERT INTO userIngredientDislikes VALUES ('jasper', 'cucumber');


                INSERT INTO userIngredientLikes VALUES ('tanika', 'hotdog');
                INSERT INTO userIngredientLikes VALUES ('tanika', 'chilli');


                INSERT INTO userIngredientDislikes VALUES ('tanika', 'mashed potato');


                INSERT INTO userIngredientLikes VALUES ('david', 'egg');
                INSERT INTO userIngredientLikes VALUES ('david', 'sweetcorn');


                INSERT INTO userIngredientLikes VALUES ('nikolai', 'rice krispies');";

                fillDatabase.ExecuteNonQuery();

                using (var transaction = conn2.BeginTransaction())
                {
                    var istCmd = conn2.CreateCommand();
                    istCmd.CommandText = "INSERT INTO recipes VALUES(0,'Korean fried chicken','Cook an exotic yet easy dinner like these spicy and sticky Korean chicken wings. They make ideal finger food for a buffet, but dont forget the napkins','To make the sauce, put all the ingredients in a saucepan and simmer gently until syrupy, so around 3-4 mins. Take off the heat and set aside. Season the chicken wings with salt, pepper and the grated ginger. Toss the chicken with the cornflour until completely coated. Heat about 2cm of vegetable oil in a large frying pan over a medium/high heat. Fry the chicken wings for 8-10 mins until crisp, turning halfway. Remove from the oil and place on kitchen paper. Leave to cool slightly (around 2 mins). Reheat the sauce, and toss the crispy chicken wings in it. Tip into a bowl and top with the sesame seeds and sliced spring onions.',0,'url','Elena Silcock')";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(0,'chicken');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(0,'soy sauce');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipes VALUES(1,'Wok-cooked fragrant mussels', 'Jamie Oliver had this idea while eating mussels in New York. It takes literally minutes to cook and tastes absolutely fabulous.', 'Place the mussels with a couple of lugs of olive oil in a large, very hot wok or pot. Shake around and add the rest of the ingredients, apart from the lime juice and coconut milk. Keep turning over until all the mussels have opened - throw away any that remain closed. Squeeze in the lime juice and add the coconut milk. Bring to the boil and serve immediately.',0,'url', 'Jamie Oliver')";
                    istCmd.ExecuteNonQuery();

                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(1,'Mussels');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(1,'Garlic');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(1,'Chillies');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(1,'Ginger');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(1,'Coriander');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(1,'Sesame seed oil');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(1,'Spring Onions');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(1,'Lime juice');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(1,'Coconut milk');";
                    istCmd.ExecuteNonQuery();

                    istCmd.CommandText = "INSERT INTO recipes VALUES(2,'Dark Jamaican Gingerbread','This cake, originally from the sugar-and-spice island of Jamaica, has sadly become a factory-made clone, but made at home it’s dark, sticky, fragrant with ginger – the real thing.', 'Begin by placing the tin of black treacle (without a lid) in a saucepan of barely simmering water to warm it and make it easier to measure. Sift the flour and spices into a large bowl, then mix the bicarbonate of soda with the milk and set it on one side. Now measure the black treacle, golden syrup, sugar and butter into a saucepan with 75ml of water, heat and gently stir until thoroughly melted and blended – don’t let it come anywhere near the boil and don’t go off and leave it! Next add the syrup mixture to the flour and spices, beating vigorously with a wooden spoon, and when the mixture is smooth, beat in the egg a little at a time, followed by the bicarbonate of soda and milk. Now pour the mixture into the prepared tin and bake on a lower shelf so that the top of the tin is aligned with the centre of the oven for 1¼–1½ hours until it’s well-risen and firm to the touch. Remove the cake from the oven and allow to cool in the tin for 5 minutes before turning out.', 0, null, 'Delia')";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(2, '175g Plain flour')";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(2, '1 tbps Ground giner')";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(2, '1 tsp Ground cinnamon')";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(2, '1/4 nutmeg')";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(2, '1/2 tsp bicaronate of soda')";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(2, '2 tbps milk')";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(2, '75g treacle')";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(2, '75g golden syrup')";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(2, '75g dark brown sugar')";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(2, '75g butter')";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(2, '1 large egg')";
                    istCmd.ExecuteNonQuery();

                    istCmd.CommandText = "INSERT INTO recipes VALUES(3, 'Spaghetti Carbonara', 'A classic Italian pasta dish', 'Bring a large pot of salted water to a boil. Add the spaghetti and cook until al dente, according to the package instructions. While the pasta is cooking, heat a large skillet over medium heat. Add the diced pancetta and cook until crispy and browned. Remove from the heat and set aside. In a medium bowl, whisk together the Parmesan cheese, egg yolks, and egg until well combined. In the same skillet that you used to cook the pancetta, add the minced garlic and cook for about 30 seconds, stirring constantly. Drain the spaghetti and reserve 1 cup of the pasta cooking water. Add the spaghetti to the skillet with the garlic and toss to coat. Remove the skillet from the heat. Add the pancetta to the skillet with the spaghetti and toss to combine. Slowly pour the egg and Parmesan mixture over the spaghetti, stirring constantly to coat the pasta. The heat from the pasta will cook the eggs and create a creamy sauce. If the sauce seems too thick, add some of the reserved pasta cooking water to thin it out. Season the pasta with salt and freshly ground black pepper to taste. Serve the spaghetti carbonara immediately, garnished with chopped fresh parsley. Enjoy!', 0, null, 'John Smith')";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(3,'1 pound spaghetti');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(3,'1/2 pound pancetta or bacon, diced');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(3,'1/2 cup grated Parmesan cheese');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(3,'3 large egg yolks');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(3,'1 large egg');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(3,'3 cloves garlic, minced');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(3,'Salt and freshly ground black pepper');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(3,'2 tablespoons chopped fresh parsley leaves');";
                    istCmd.ExecuteNonQuery();

                    istCmd.CommandText = "INSERT INTO recipes VALUES(4, 'Char Siu Bao', 'Char Siu Bao are steamed pork buns. Soft, fluffy steamed buns filled with Chinese BBQ pork or char siu. Easy, authentic and the best char siu bao recipe!', 'In the bowl of an electric mixer fitted with a dough hook, dissolve the yeast in the warm water. Sift together the flour and cornstarch, and add it to the yeast mixture along with the sugar and oil. Turn on the mixer to the lowest setting and let it go until a smooth dough ball is formed. Cover with a damp cloth and let it rest for 2 hours. While the dough is resting, make the meat filling. Heat the oil in a wok over medium high heat. Add the onion and stir-fry for a minute. Turn heat down to medium-low, and add the sugar, soy sauce, oyster sauce, sesame oil, and dark soy. Stir and cook until the mixture starts to bubble up. Add the chicken stock and flour, cooking for a couple minutes until thickened. Remove from the heat and stir in the roast pork. Set aside to cool. If you make the filling ahead of time, cover and refrigerate to prevent it from drying out. After your dough has rested for 2 hours, add the baking powder to the dough and turn the mixer on to the lowest setting. At this point, if the dough looks dry or you are having trouble incorporating the baking powder, add 1-2 teaspoons water. Gently knead the dough until it becomes smooth again. Cover with a damp cloth and let it rest for another 15 minutes. In the meantime, get a large piece of parchment paper and cut it into ten 4x4 inch squares. Prepare your steamer by bringing the water to a boil. Now we are ready to assemble the buns: roll the dough into a long tube and divide it into 10 equal pieces. Press each piece of dough into a disc about 4 1/2 inches in diameter. Add some filling and pleat the buns until they are closed on top. Place each bun on a parchment paper square, and steam. I steamed the buns in two separate batches using a bamboo steamer. Once the water boils, place the buns in the steamer and steam each batch for 12 minutes over high heat.', 0, null, 'Judy')";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'1 tsp yeast');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'180ml cups water');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'270g flour');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'120g cornstarch');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'65g sugar');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'60ml oil');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'10g baking powder');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'15ml oil');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'20g shallots');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'12g sugar');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'15ml soy sauce');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'1 tsp oyster sauce');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'2 tsp sesame seed oil');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'120ml chicken stock');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'16g flour');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'225g diced chicken');";
                    istCmd.ExecuteNonQuery();

                    istCmd.CommandText = "INSERT INTO recipes VALUES (5 ,'Dan dan noodles', 'Famous Szechuan style Dan dan noodles (担担面) is one of the most popular Chinese street foods.', 'Heat oil in wok and then fry star anise and Sichuan peppercorn with slowest fire until aromatic. Then remove the spices. Add minced pork and fry for several minutes until slightly browned. Drizzle cooking wine around the edges. Then place sugar and light soy sauce in. Add Ya cai in, continue frying for 3-5 minutes until dry and golden brown. Mix all the seasonings in serving bowls. Combine well. Cook noodles in boiling water according to the instructions on the package and blanch vegetables in the last minutes when the noodles are almost ready. Transfer the noodles to the serving bowl.Top with pork topping, chopped scallion and toasted peanuts. Pour around ½ cup hot pork stock or chicken stock along with the edges. Mix well before eating.', 0, null, 'Elaine')";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'3 Servings noodles');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'Chopped scallion');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'Blanched vegtables');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'Chicken stock');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'Toasted peanuts');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'400g Minced pork');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'2 tbsp cooking oil');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'1 tsp Sichuan peppercorn');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'1 star anise');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'1/2 tbsp minced garlic');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'1/2 tbsp cooking wine');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'1 tsp sugar');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'1 cup ya-cai');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'1 tbsp soy sauce');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'1 tsp sesame paste');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'1 tsp sesame oil');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'1 tbsp chili oil');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'1 tsp black vinegar');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'chopped scallion');";
                    istCmd.ExecuteNonQuery();

                    istCmd.CommandText = "INSERT INTO recipes VALUES(6,'Beans on toast',null,'1.Put beans in a bowl.\n2.Microwave them\n3.toast bread\n4.Pour Beans on toast.\n5.eat the beans on toast', 66, null, 'Fred')";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(6,'baked beans');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(6,'bread');";
                    istCmd.ExecuteNonQuery();



                    istCmd.CommandText = "INSERT INTO recipes VALUES(7,'Roasted tomato, basil and parmesan quiche','A taste of summer, this quiche is full of Italian flavours and is perfect for dinner in the garden','STEP 1 To make the pastry, tip the flour and butter into a bowl, then rub together with your fingertips until completely mixed and crumbly.Add 8 tbsp cold water, then bring everything together with your hands until just combined. Roll into a ball and use straight away or chill for up to 2 days.The pastry can also be frozen for up to a month.\nSTEP 2 Roll out the pastry on a lightly floured surface to a round about 5cm larger than a 25cm tin.Use your rolling pin to lift it up, then drape over the tart case so there is an overhang of pastry on the sides.Using a small ball of pastry scraps, push the pastry into the corners of the tin.Chill in the fridge or freezer for 20 mins.Heat oven to 200C / fan 180C / gas 6.\nSTEP 3 In a small roasting tin, drizzle the tomatoes with olive oil and season with salt and pepper.Put the tomatoes in a low shelf of the oven.',126,null,'Barney Desmazery');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(7,'olive oil');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(7,'permesan cheese');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(7,'eggs');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(7,'double cream');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(7,'cherry tomatoes');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(7,'basil');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(7,'flour');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(7,'butter');";
                    istCmd.ExecuteNonQuery();



                    istCmd.CommandText = "INSERT INTO recipes VALUES(8,'Vegan mushroom and potato hash',  'Make this mushroom and potato hash for a healthier breakfast. Mushrooms are a great addition in a vegan diet, as theyre one of the few plant-based sources of vitamin D'  ,   'STEP 1\nTip the oats and soya milk into a large bowl and blitz using a hand blender to break down the oats to a less coarse texture. Set aside for 10 mins to soak.\n\nSTEP 2\nMeanwhile, boil the potatoes for 5 mins, then drain.Heat the oil in a large non - stick frying pan over a medium heat, and cook the mushrooms, onion and paprika for a few minutes until softened.Tip in the potatoes and cook for 10 mins, turning the mixture over every now and then.Stir in the halved tomatoes and leave to cook for 5 mins.\n\nSTEP 3\nThe oat mixture should now be stiff.Work in the baking powder using your hands, then halve the mixture.With wet hands, press out one half of the mixture on a plastic chopping board to make a thin disc, like a pancake.Carefully lift it off with a palette knife and cook in a dry non - stick frying pan for 2 mins on each side.Remove to a plate, and repeat with the other half.Put the oat thins on two plates and top with the hash to serve.'   , 23, null, 'Sara Buenfeld');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(8,'oats');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(8,'soya milk');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(8,'mushrooms');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(8,'potato');";
                    istCmd.ExecuteNonQuery();


                    istCmd.CommandText = "INSERT INTO savedRecipes VALUES ('david', 5);";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO savedRecipes VALUES ('david', 8);";
                    istCmd.ExecuteNonQuery();

                    istCmd.CommandText = "INSERT INTO savedRecipes VALUES ('jasper', 1);";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO savedRecipes VALUES ('jasper', 2);";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO savedRecipes VALUES ('tanika', 0);";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO savedRecipes VALUES ('tanika', 5);";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO savedRecipes VALUES ('tanika', 3);";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO savedRecipes VALUES ('fernando', 3);";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO savedRecipes VALUES ('caitlin', 2);";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO savedRecipes VALUES ('caitlin', 3);";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO savedRecipes VALUES ('nikolai', 4);";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO savedRecipes VALUES ('nikolai', 5);";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO savedRecipes VALUES ('rowan', 0);";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO savedRecipes VALUES ('lawrence', 1);";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO savedRecipes VALUES ('lawrence', 2);";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO savedRecipes VALUES ('lawrence', 3);";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO savedRecipes VALUES ('chris', 4);";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO savedRecipes VALUES ('chris', 0);";
                    istCmd.ExecuteNonQuery();





                    transaction.Commit();
                }
            }
        }

        // Displays recipe information
        public void Output()
        {
            foreach (Recipe recipe in recipesManager.recipes)
            {
                recipe.Log();
                Console.WriteLine();
            }
        }

        // Used for testing databases and connection
        public void run()
        {
            if (SQL_VER)
            {
                // Needs to setup db connection
                SqlConnection conn = null;
                recipesManager = new RecipesManager(conn);
                userManager = new UserManager(conn);
            }
            else
            {
                createDb();
            }
            
            //recipesManager.CreateRecipe("Nikolai"); // Changed to deal with request.form

            Console.WriteLine("user test:");

            User dav = userManager.getUser("n.valkamo-2021@hull.ac.uk", "email");

            recipesManager.GetRecipes();
            Output();
        }
    }
}
