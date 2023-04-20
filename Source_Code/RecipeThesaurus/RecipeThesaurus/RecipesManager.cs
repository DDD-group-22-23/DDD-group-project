using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RecipeThesaurus
{
    public class Recipe
    {
        public int id;
        public string name;
        public string description;
        public string instructions;
        public int likes;
        public string url;
        public string author;
        public List<string> ingredientsList = new List<string>();

        public Recipe(int pid, string pname, string pdesc, string pinst, int plikes, string purl, string pauthor, List<string> pingredientsList = null)
        {
            id = pid;
            name = pname;
            description = pdesc;
            instructions = pinst;
            likes = plikes;
            url = purl;
            author = pauthor;

            if(pingredientsList != null) ingredientsList = pingredientsList;
        }

        public void Log()
        {
            string outline = $"id={id}\nname={name}\ndesc={description}\ninstructions={instructions}\nlikes={likes}\nurl={url}\nauthor={author}";
            Console.WriteLine(outline);
        }
    }

    public class RecipesManager
    {
        public List<Recipe> recipes = new List<Recipe>();
        public SqlConnection conn;
        public SqliteConnection conn2;
        bool SQL_VER = false;

        public RecipesManager(SqlConnection con = null)
        {
            conn = con;
            SQL_VER = true;
        }
        public RecipesManager(SqliteConnection con = null)
        {
            conn2 = con;

        }

        // Gets recipe information then stores
        public void CreateRecipe(string pUsername)
        {
            int id = 4;
            string name = "CreateRecipe()";
            string desc = "Function to create a recipe";
            string inst = "In the code";
            string url = "null";
            string username = "Nikolai";
            string ingredients = "None";
            // need to add ingredients


            string fields = $"(recipeId, recipeName, recipeDescription, recipeInstructions, recipeLikes, imageURL, recipeAuthor)";
            string values = $"({id}, '{name}', '{desc}', '{inst}', 0, '{url}', '{username}')";
            string saveRecipe = $"INSERT INTO recipes {fields} VALUES {values}";

            if(SQL_VER)
            {
                conn.Open();
                SqlCommand command = new SqlCommand(saveRecipe, conn);
                command.ExecuteNonQuery();
            }
            else
            {
                conn2.Open();
                SqliteCommand command = new SqliteCommand(saveRecipe, conn2);
                command.ExecuteNonQuery();
            }            
        }

        // Reads recipes stored and loads them into a list
        // As the total amount of recipes stored is small, all can be loaded in at once
        public void GetRecipes()
        {
            // Need to also select 'recipeIngredients' when its been added to the sql

            string getRecipe = $"SELECT recipeId, recipeName, recipeDescription, recipeInstructions, recipeLikes, imageURL, recipeAuthor FROM recipes";

            if (SQL_VER)
            {
                
                conn.Open();
                //SqlCommand command = new SqlCommand(getRecipe, conn);
                //using (SqlDataReader reader = command.ExecuteReader())
                //{
                    ReadSql(getRecipe);
                //}
            }
            else
            {
                conn2.Open();
                
                ReadSqlite(getRecipe);
                
            }
        }

        public void ReadSql(string getRecipe)
        {

            SqlCommand command = new SqlCommand(getRecipe, conn);
            using (SqlDataReader reader = command.ExecuteReader())
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
                    recipes.Add(recipe);
                }
            }


            string sqlGetRecipeIngredients = "select distinct recipeIngredients.ingredient FROM recipeIngredients where recipeIngredients.recipeId = {0};";
            foreach (Recipe i in recipes)
            {
                //getRecipeIngredients.CommandText = String.Format(sqlGetRecipeIngredients, i.id); //getting ingredients query results for the recipe id
                SqlCommand getRecipeIngredients = new SqlCommand(String.Format(sqlGetRecipeIngredients, i.id), conn);

                using (var ingredientsReader = getRecipeIngredients.ExecuteReader())
                {
                    bool more = ingredientsReader.Read();
                    while (more)
                    {
                        i.ingredientsList.Add(ingredientsReader["ingredient"].ToString());
                        more = ingredientsReader.Read();
                    }
                }
            }


        }




        public void ReadSqlite(string getRecipe)
        {
            SqliteCommand command = new SqliteCommand(getRecipe, conn2);
            using (SqliteDataReader reader = command.ExecuteReader())
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
                    recipes.Add(recipe);
                }
            }

            string sqlGetRecipeIngredients = "select distinct recipeIngredients.ingredient FROM recipeIngredients where recipeIngredients.recipeId = {0};";
            foreach (Recipe i in recipes)
            {
                //getRecipeIngredients.CommandText = String.Format(sqlGetRecipeIngredients, i.id); //getting ingredients query results for the recipe id
                SqliteCommand getRecipeIngredients = new SqliteCommand(String.Format(sqlGetRecipeIngredients, i.id), conn2);

                using (var ingredientsReader = getRecipeIngredients.ExecuteReader())
                {
                    bool more = ingredientsReader.Read();
                    while (more)
                    {
                        i.ingredientsList.Add(ingredientsReader["ingredient"].ToString());
                        more = ingredientsReader.Read();
                    }
                }
            }
        }



        // Clears all stored recipes 
        public void ClearRecipes()
        {
            recipes.Clear();
        }
    }
}
