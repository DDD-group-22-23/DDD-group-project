using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace RecipeThesaurus
{


    /// <summary>
    /// the rcipeNew class just stores a user's info. It doesnt have any functioonality
    /// </summary>
    public class RecipeNew
    {
        public int id;
        public string name;
        public string description;
        public string instructions;
        public int likes;
        public string recipeAuthor;

        public List<string> ingredients;

        public RecipeNew()
        {
            ingredients = new List<string>();
        }

    }








    /// <summary>
    /// The UserManager class has a method getUserByUsername which returns a User object containing all the user's information
    /// </summary>
    public class RecipeManagerNew
    {

        SqliteConnection conn; //change for different connections


        public RecipeManagerNew(SqliteConnection pConn) //change for different connections
        {
            conn = pConn;
        }




        /// <summary>
        /// Finds the recipe in the database and returns them in a recipe object
        /// </summary>
        /// <param name="pUsername"></param>
        /// <returns>a recipe object storing all the recipe info</returns>
        public List<RecipeNew> getRecipesByPhrase(string pPhrase = null)
        {
            List<RecipeNew> returnList = new List<RecipeNew>();

            string plusKeyPhrase = null;

            if (pPhrase != null)
            {
                plusKeyPhrase = $" WHERE recipes.recipeName LIKE '{pPhrase}' OR recipes.recipeDescription LIKE '{pPhrase}' OR recipes.recipeInstructions LIKE '{pPhrase}' OR recipes.recipeAuthor LIKE '{pPhrase}'"; //this adds a filter for the keyphrase being withing the recipe fields. add to the end of the recipe get query
            }

            string sqlGetRecipe = $"select recipeId, recipeName, recipeDescription, recipeInstructions, recipeLikes, recipeAuthor from recipes{plusKeyPhrase};";

            string sqlGetRecipeIngredients = "select distinct recipeIngredients.ingredient FROM recipeIngredients where recipeIngredients.recipeId = {0};";    //gets all the ingredients in a recipe
            

            conn.Open();

            

            var command = conn.CreateCommand(); //for sqlite
            command.CommandText = sqlGetRecipe;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {

                    RecipeNew newRecipe = new RecipeNew();

                    //load main fields without ingredients list
                       
                    newRecipe.id = int.Parse(reader["recipeId"].ToString());
                    newRecipe.name = reader["recipeName"].ToString();
                    newRecipe.description = reader["recipeDescription"].ToString();
                    newRecipe.instructions = reader["recipeInstructions"].ToString();
                    newRecipe.likes = int.Parse(reader["recipeLikes"].ToString());
                    newRecipe.recipeAuthor = reader["recipeAuthor"].ToString();

                    returnList.Add(newRecipe);

                }
            }

            

            foreach (RecipeNew i in returnList)
            {
                command.CommandText = String.Format(sqlGetRecipeIngredients, i.id); //getting ingredients query results for the recipe id
                using (var reader = command.ExecuteReader())
                {
                    bool more = reader.Read();
                    while (more)
                    {
                        i.ingredients.Add(reader["ingredient"].ToString());
                        more = reader.Read();
                    }
                }

                
            }


            conn.Close();


            return returnList;
        }







    }




}