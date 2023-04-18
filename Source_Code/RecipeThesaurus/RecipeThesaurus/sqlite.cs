using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace RecipeThesaurus
{
    public class sqlite
    {
        RecipesManager manager = new RecipesManager();

        // Creates sqlite database for use
        static public void createDb()
        {
            var connBuilder = new SqliteConnectionStringBuilder();
            connBuilder.DataSource = "./sqliteDB.db";
            using (SqliteConnection conn = new SqliteConnection(connBuilder.ConnectionString))
            {
                conn.Open();
                var delCmd = conn.CreateCommand();
                delCmd.CommandText = "DROP TABLE IF EXISTS recipes";
                delCmd.ExecuteNonQuery();
                delCmd.CommandText = "DROP TABLE IF EXISTS users";
                delCmd.ExecuteNonQuery();

                var crtCmd = conn.CreateCommand();
                // FOREIGN KEY has been removed to bypass integrity
                crtCmd.CommandText = "CREATE TABLE recipes (recipeId int NOT NULL, recipeName varchar(100) NOT NULL, recipeDescription varchar(200), recipeInstructions varchar(1000), recipeLikes int, imageURL varchar(100), recipeAuthor varchar(20), PRIMARY KEY (recipeId))";
                crtCmd.ExecuteNonQuery();
                crtCmd.CommandText = "CREATE TABLE users (username varchar(20) NOT NULL, firstname varchar(20), lastname varchar(20), email varchar(50), profilePic varchar(100), PRIMARY KEY (username))";
                crtCmd.ExecuteNonQuery();

                using (var transaction = conn.BeginTransaction())
                {
                    var istCmd = conn.CreateCommand();
                    istCmd.CommandText = "INSERT INTO recipes VALUES(0,'Korean fried chicken','Cook an exotic yet easy dinner like these spicy and sticky Korean chicken wings. They make ideal finger food for a buffet, but dont forget the napkins','To make the sauce, put all the ingredients in a saucepan and simmer gently until syrupy, so around 3-4 mins. Take off the heat and set aside. Season the chicken wings with salt, pepper and the grated ginger. Toss the chicken with the cornflour until completely coated. Heat about 2cm of vegetable oil in a large frying pan over a medium/high heat. Fry the chicken wings for 8-10 mins until crisp, turning halfway. Remove from the oil and place on kitchen paper. Leave to cool slightly (around 2 mins). Reheat the sauce, and toss the crispy chicken wings in it. Tip into a bowl and top with the sesame seeds and sliced spring onions.',0,'url','Elena Silcock')";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipes VALUES(1,'Wok-cooked fragrant mussels', 'Jamie Oliver had this idea while eating mussels in New York. It takes literally minutes to cook and tastes absolutely fabulous.', 'Place the mussels with a couple of lugs of olive oil in a large, very hot wok or pot. Shake around and add the rest of the ingredients, apart from the lime juice and coconut milk. Keep turning over until all the mussels have opened - throw away any that remain closed. Squeeze in the lime juice and add the coconut milk. Bring to the boil and serve immediately.',0,'url', 'Jamie Oliver')";
                    istCmd.ExecuteNonQuery();
                    istCmd.CommandText = "INSERT INTO recipes VALUES(2,'Dark Jamaican Gingerbread','This cake, originally from the sugar-and-spice island of Jamaica, has sadly become a factory-made clone, but made at home it’s dark, sticky, fragrant with ginger – the real thing.', 'Begin by placing the tin of black treacle (without a lid) in a saucepan of barely simmering water to warm it and make it easier to measure. Sift the flour and spices into a large bowl, then mix the bicarbonate of soda with the milk and set it on one side. Now measure the black treacle, golden syrup, sugar and butter into a saucepan with 75ml of water, heat and gently stir until thoroughly melted and blended – don’t let it come anywhere near the boil and don’t go off and leave it! Next add the syrup mixture to the flour and spices, beating vigorously with a wooden spoon, and when the mixture is smooth, beat in the egg a little at a time, followed by the bicarbonate of soda and milk. Now pour the mixture into the prepared tin and bake on a lower shelf so that the top of the tin is aligned with the centre of the oven for 1¼–1½ hours until it’s well-risen and firm to the touch. Remove the cake from the oven and allow to cool in the tin for 5 minutes before turning out.', 0, null, 'Delia')";
                    istCmd.ExecuteNonQuery();

                    transaction.Commit();
                }
            }
        }

        // Copy of recipemanager.GetRecipes() modified for sqlite
        public void GetRecipes()
        {
            string getRecipe = $"SELECT recipeId, recipeName, recipeDescription, recipeInstructions, recipeLikes, imageURL, recipeAuthor FROM recipes";

            var connBuilder = new SqliteConnectionStringBuilder();
            connBuilder.DataSource = "./sqliteDB.db";
            using (SqliteConnection conn = new SqliteConnection(connBuilder.ConnectionString))
            {
                conn.Open();
                var selCmd = conn.CreateCommand();
                selCmd.CommandText = getRecipe;
                using (var reader = selCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["recipeId"]);
                        string name = reader["recipeName"].ToString();
                        string desc = reader["recipeDescription"].ToString();
                        string inst = reader["recipeInstructions"].ToString();
                        int like = Convert.ToInt32(reader["recipeLikes"]);
                        string url = reader["imageURL"].ToString();
                        string auth = reader["recipeAuthor"].ToString();
                        // string ingr = reader["recipeIngredients"].ToString();

                        Recipe recipe = new Recipe(id, name, desc, inst, like, url, auth/*, ingr*/);
                        manager.recipes.Add(recipe);
                    }
                }
            }
        }

        // Copy of recipemanager.CreateRecipe() modified for sqlite
        public void CreateRecipe(string pUsername)
        {
            int id = 4;
            string name = " Ab";
            string desc = " Ac";
            string inst = " Ad";
            string url = " Ae";
            string username = pUsername;
            string ingredients = " A";
            string fields = $"(recipeId, recipeName, recipeDescription, recipeInstructions, recipeLikes, imageURL, recipeAuthor)";
            string values = $"({id}, '{name}', '{desc}', '{inst}', 0, '{url}', '{username}')";

            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            //Use DB in project directory.  If it does not exist, create it:
            connectionStringBuilder.DataSource = "./SqliteDB.db";
            using (SqliteConnection connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var saveRecipe = connection.CreateCommand(); 
                    saveRecipe.CommandText = $"INSERT INTO recipes {fields} VALUES {values}";
                    saveRecipe.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
        }

        // Displays recipe information
        public void Output()
        {
            foreach (var recipe in manager.recipes)
            {
                recipe.Log();
                Console.WriteLine();
            }
        }

        // To run remove lines 7-13 in Program.cs
        public void run()
        {
            createDb();
            CreateRecipe("Nikolai");
            GetRecipes();
            Output();
            while (true)
            {
                // Stops webpage from running
            }
        }

        /*
        USE recipeappdb;

        INSERT INTO recipes VALUES(0,
        "Korean fried chicken",
        "Cook an exotic yet easy dinner like these spicy and sticky Korean chicken wings. They make ideal finger food for a buffet, but don't forget the napkins"
        "To make the sauce, put all the ingredients in a saucepan and simmer gently until syrupy, so around 3-4 mins. Take off the heat and set aside. Season the chicken wings with salt, pepper and the grated ginger. Toss the chicken with the cornflour until completely coated. Heat about 2cm of vegetable oil in a large frying pan over a medium/high heat. Fry the chicken wings for 8-10 mins until crisp, turning halfway. Remove from the oil and place on kitchen paper. Leave to cool slightly (around 2 mins). Reheat the sauce, and toss the crispy chicken wings in it. Tip into a bowl and top with the sesame seeds and sliced spring onions.",
        0,
        null, 
        "Elena Silcock"
        );

        INSERT INTO recipes VALUES(1,
        "Dark Jamaican Gingerbread",
        "This cake, originally from the sugar-and-spice island of Jamaica, has sadly become a factory-made clone, but made at home it’s dark, sticky, fragrant with ginger – the real thing.",
        "Begin by placing the tin of black treacle (without a lid) in a saucepan of barely simmering water to warm it and make it easier to measure. Sift the flour and spices into a large bowl, then mix the bicarbonate of soda with the milk and set it on one side. Now measure the black treacle, golden syrup, sugar and butter into a saucepan with 75ml of water, heat and gently stir until thoroughly melted and blended – don’t let it come anywhere near the boil and don’t go off and leave it! Next add the syrup mixture to the flour and spices, beating vigorously with a wooden spoon, and when the mixture is smooth, beat in the egg a little at a time, followed by the bicarbonate of soda and milk. Now pour the mixture into the prepared tin and bake on a lower shelf so that the top of the tin is aligned with the centre of the oven for 1¼–1½ hours until it’s well-risen and firm to the touch. Remove the cake from the oven and allow to cool in the tin for 5 minutes before turning out.",
        0,
        null,
        "Delia");

        INSERT INTO recipes VALUES(2, 
        "Wok-cooked fragrant mussels", 
        "Jamie Oliver had this idea while eating mussels in New York. It takes literally minutes to cook and tastes absolutely fabulous.",
        "Place the mussels with a couple of lugs of olive oil in a large, very hot wok or pot. Shake around and add the rest of the ingredients, apart from the lime juice and coconut milk. Keep turning over until all the mussels have opened - throw away any that remain closed. Squeeze in the lime juice and add the coconut milk. Bring to the boil and serve immediately.",
        0, 
        null, 
        "Jamie Oliver");
         */
    }
}
