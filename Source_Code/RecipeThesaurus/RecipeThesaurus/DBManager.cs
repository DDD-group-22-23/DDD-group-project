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
                recipeInstructions varchar(1000),
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
                    istCmd.CommandText = "INSERT INTO recipes VALUES(2,'Dark Jamaican Gingerbread','This cake, originally from the sugar-and-spice island of Jamaica, has sadly become a factory-made clone, but made at home it’s dark, sticky, fragrant with ginger – the real thing.', 'Begin by placing the tin of black treacle (without a lid) in a saucepan of barely simmering water to warm it and make it easier to measure. Sift the flour and spices into a large bowl, then mix the bicarbonate of soda with the milk and set it on one side. Now measure the black treacle, golden syrup, sugar and butter into a saucepan with 75ml of water, heat and gently stir until thoroughly melted and blended – don’t let it come anywhere near the boil and don’t go off and leave it! Next add the syrup mixture to the flour and spices, beating vigorously with a wooden spoon, and when the mixture is smooth, beat in the egg a little at a time, followed by the bicarbonate of soda and milk. Now pour the mixture into the prepared tin and bake on a lower shelf so that the top of the tin is aligned with the centre of the oven for 1¼–1½ hours until it’s well-risen and firm to the touch. Remove the cake from the oven and allow to cool in the tin for 5 minutes before turning out.', 0, null, 'Delia')";
                    istCmd.ExecuteNonQuery();




                    //adding some example recipes
                    //istCmd.CommandText = "INSERT INTO recipes VALUES(ID,NAME,DESCRIPTION,STEPS, LIKES, null, AUTHOR)";
                    //istCmd.ExecuteNonQuery();

                    istCmd.CommandText = "INSERT INTO recipes VALUES(3,'Beans on toast',null,'1.Put beans in a bowl.\n2.Microwave them\n3.toast bread\n4.Pour Beans on toast.\n5.eat the beans on toast', 66, null, 'Fred')";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(3,'baked beans');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(3,'bread');";
                    istCmd.ExecuteNonQuery();



                    istCmd.CommandText = "INSERT INTO recipes VALUES(4,      'Roasted tomato, basil and parmesan quiche'         ,          'A taste of summer, this quiche is full of Italian flavours and is perfect for dinner in the garden'           ,        'STEP 1 To make the pastry, tip the flour and butter into a bowl, then rub together with your fingertips until completely mixed and crumbly.Add 8 tbsp cold water, then bring everything together with your hands until just combined. Roll into a ball and use straight away or chill for up to 2 days.The pastry can also be frozen for up to a month.\nSTEP 2 Roll out the pastry on a lightly floured surface to a round about 5cm larger than a 25cm tin.Use your rolling pin to lift it up, then drape over the tart case so there is an overhang of pastry on the sides.Using a small ball of pastry scraps, push the pastry into the corners of the tin.Chill in the fridge or freezer for 20 mins.Heat oven to 200C / fan 180C / gas 6.\nSTEP 3 In a small roasting tin, drizzle the tomatoes with olive oil and season with salt and pepper.Put the tomatoes in a low shelf of the oven.'               ,       126        ,         null       ,      'Barney Desmazery');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'olive oil');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'permesan cheese');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'eggs');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'double cream');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'cherry tomatoes');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'basil');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'flour');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(4,'butter');";
                    istCmd.ExecuteNonQuery();



                    istCmd.CommandText = "INSERT INTO recipes VALUES(5,'Vegan mushroom and potato hash',  'Make this mushroom and potato hash for a healthier breakfast. Mushrooms are a great addition in a vegan diet, as theyre one of the few plant-based sources of vitamin D'  ,   'STEP 1\nTip the oats and soya milk into a large bowl and blitz using a hand blender to break down the oats to a less coarse texture. Set aside for 10 mins to soak.\n\nSTEP 2\nMeanwhile, boil the potatoes for 5 mins, then drain.Heat the oil in a large non - stick frying pan over a medium heat, and cook the mushrooms, onion and paprika for a few minutes until softened.Tip in the potatoes and cook for 10 mins, turning the mixture over every now and then.Stir in the halved tomatoes and leave to cook for 5 mins.\n\nSTEP 3\nThe oat mixture should now be stiff.Work in the baking powder using your hands, then halve the mixture.With wet hands, press out one half of the mixture on a plastic chopping board to make a thin disc, like a pancake.Carefully lift it off with a palette knife and cook in a dry non - stick frying pan for 2 mins on each side.Remove to a plate, and repeat with the other half.Put the oat thins on two plates and top with the hash to serve.'   , 23, null, 'Sara Buenfeld');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'oats');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'soya milk');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'mushrooms');";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipeIngredients VALUES(5,'potato');";
                    istCmd.ExecuteNonQuery();


                    istCmd.CommandText = "INSERT INTO savedRecipes VALUES ('david', 0);";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO savedRecipes VALUES ('david', 2);";
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

            User dav = userManager.getUserByUsername("jasper");

            recipesManager.GetRecipes();
            Output();
        }
    }
}
