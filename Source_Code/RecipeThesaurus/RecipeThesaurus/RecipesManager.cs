using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
namespace RecipeThesaurus
{
    public class Recipe
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ID { get; set; }
        public string Instructions { get; set; }

        public Recipe(int id, string name, string desc, string inst)
        {
            ID = id;
            Name = name;
            Description = desc;
            Instructions = inst;
        }
    }

    public class RecipesManager
    {
        List<Recipe> recipes = new List<Recipe>();
        SqlConnection conn;


        public RecipesManager(SqlConnection con)
        {
            conn = con;
        }

        // Adds a recipe to the currently loaded recipes
        public void AddRecipe()
        {

            // Recipe newRec = new Recipe();
        }

        // Gets recipe information then stores
        public void CreateRecipe()
        {
            string name = string.Empty;
            string desc = string.Empty;
            string inst = string.Empty;
            int ID = 0; // Set to next open num
            // string username = current user open

            string command = string.Empty;
            // command = "INSERT INTO recipes VALUES (ID, name, desc, inst, 0, "url", username)"
        }

        // Opens a recipe with its whole description
        public void ViewRecipe()
        {

        }

        // Basic view of a recipe during search etc
        public void ViewOverview()
        {

        }

        public void ClearRecipes()
        {
            recipes.Clear();
        }

    }
}
