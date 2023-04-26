using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

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

        public RecipesManager(SqlConnection con)
        {
            conn = con;
            SQL_VER = true;
        }
        public RecipesManager(SqliteConnection con)
        {
            conn2 = con;
        }

        public bool SaveRecipe(int id, User user)
        {
            if (user.savedRecipes.Contains(id.ToString()))
            {
                return true; // Returns to delete save
            }
            

            string saveRecipe = $"INSERT INTO savedRecipes VALUES('{user.username}', {id});";
            if(SQL_VER)
            {
                SqlCommand command = new SqlCommand(saveRecipe, conn);
                command.ExecuteNonQuery();
            }
            else
            {
                SqliteCommand command = new SqliteCommand(saveRecipe, conn2);
                command.ExecuteNonQuery();
            }
            return false;
        }




        public void LikeRecipe(int id)
        {
            string getRecipe = $"UPDATE recipes SET recipeLikes = (recipeLikes + 1) WHERE recipeID = {id};";
            if (SQL_VER)
            {
            }
            else
            {
                conn2.Open();
                SqliteCommand command = new SqliteCommand(getRecipe, conn2);
                command.ExecuteNonQuery();
            }
        }




        public void UnsaveRecipe(int id, User user)
        {
            if (user.savedRecipes.Contains(id.ToString()))
            {
            


            string saveRecipe = $"DELETE FROM savedRecipes WHERE username = '{user.username}' AND recipeId = {id};";
            if (SQL_VER)
            {
                SqlCommand command = new SqlCommand(saveRecipe, conn);
                command.ExecuteNonQuery();
            }
            else
            {
                SqliteCommand command = new SqliteCommand(saveRecipe, conn2);
                command.ExecuteNonQuery();
            }
            }
        }







        // Searches all recipes for a string matching the query
        public List<Recipe>? GetRecipesLike(string like)
        {
            if (like == null) return null;

            List<Recipe> likeRecipes = new List<Recipe>();

            foreach (Recipe recipe in recipes)
            {
                if (recipe.name.Contains(like))
                {
                    likeRecipes.Add(recipe);
                }
                else if (recipe.author.Contains(like))
                {
                    likeRecipes.Add(recipe);
                }
                else if (recipe.description.Contains(like))
                {
                    likeRecipes.Add(recipe);
                }
                else if (recipe.instructions.Contains(like)) 
                { 
                likeRecipes.Add(recipe);
                }
                foreach (string ingredient in recipe.ingredientsList)
                {
                    if (ingredient.Contains(like))
                    {
                        likeRecipes.Add(recipe);
                        break;
                    }
                }
                
            }
            return likeRecipes;
        }

        // Gets ids for each recipe to identify saved ids
        public List<Recipe> GetRecipesIds(List<string> ids)
        {
            List<Recipe> like = new List<Recipe>();
            foreach (Recipe recipe in recipes)
            {
                if (ids.Contains(recipe.id.ToString()))
                {
                    like.Add(recipe);
                }
            }
            return (like);
        }

        // Creates a recipe based of the form submitted
        public void CreateRecipe(string t, string d, string ing, string ist, string url, string use)
        {
            int id = recipes.Count();

            string fields = $"(recipeId, recipeName, recipeDescription, recipeInstructions, recipeLikes, imageURL, recipeAuthor)";
            string values = $"({id}, '{t}', '{d}', '{ist}', 0, '{url}', '{use}')";
            string saveRecipe = $"INSERT INTO recipes {fields} VALUES {values}";
            string ingredients = "INSERT INTO recipeIngredients VALUES({0},'{1}');";
            string saveIngredients = "";
            foreach (string sub in ing.Split(','))
            {
                saveIngredients += String.Format(ingredients, id, sub.Trim());
            }

            if(SQL_VER)
            {
                conn.Open();
                SqlCommand command = new SqlCommand(saveRecipe, conn);
                command.ExecuteNonQuery();
                command.CommandText = saveIngredients;
                command.ExecuteNonQuery();
            }
            else
            {
                conn2.Open();
                SqliteCommand command = new SqliteCommand(saveRecipe, conn2);
                command.ExecuteNonQuery();
                command.CommandText = saveIngredients;
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
                ReadSql(getRecipe);
            }
            else
            {
                conn2.Open();
                ReadSqlite(getRecipe);
            }
        }

        // Reads in recipe sql / sqlite versions
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
