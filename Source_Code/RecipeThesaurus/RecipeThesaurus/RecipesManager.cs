using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
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
        public List<int> ingredientsIdList = new List<int>();

        public Recipe(int pid, string pname, string pdesc, string pinst, int plikes, string purl, string pauthor, List<int> pingredientsIdList=null)
        {
            id = pid;
            name = pname;
            description = pdesc;
            instructions = pinst;
            likes = plikes;
            url = purl;
            author = pauthor;
            ingredientsIdList = pingredientsIdList;
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
        SqlConnection conn;

        public RecipesManager(SqlConnection con = null)
        {
            conn = con;
        }

        // Gets recipe information then stores
        public void CreateRecipe(string pUsername)
        {
            int id = 0;
            string name = " A";
            string desc = " A";
            string inst = " A";
            string url = " A";
            string username = " A";
            string ingredients = " A";
            // need to add ingredients
            string fields = $"(recipeId, recipeName, recipeDescription, recipeInstructions, recipeLikes, imageURL, recipeAuthor)";
            string values = $"({id}, '{name}', '{desc}', '{inst}', 0, '{url}', '{username}')";
            string saveRecipe = $"INSERT INTO recipes {fields} VALUES {values}";
            SqlCommand command = new SqlCommand(saveRecipe, conn);
            command.ExecuteNonQuery();
        }

        // Reads recipes stored and loads them into a list
        // As the total amount of recipes stored is small, all can be loaded in at once
        public void GetRecipes()
        {
            // Need to also select 'recipeIngredients' when its been added to the sql

            string getRecipe = $"SELECT recipeId, recipeName, recipeDescription, recipeInstructions, recipeLikes, imageURL, recipeAuthor FROM recipes";
            
            SqlCommand command = new SqlCommand(getRecipe, conn);
            //bool valid = false; Only needed if getting a specific recipe
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    //valid = true;

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
            // return valid; 
        }

        // Clears all stored recipes 
        public void ClearRecipes()
        {
            recipes.Clear();
        }
    }
}
