using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace RecipeThesaurus
{


    /// <summary>
    /// the user class just stores a user's info. It doesnt have any functioonality
    /// </summary>
    public class User
    {
        public string username;
        public string firstName;
        public string lastName;
        public string email;

        public List<string> userIngredientLikes;
        public List<string> userIngredientDislikes;
        public List<string> userFridge; //array of ingredients
        public List<string> savedRecipes;

        public User()
        {
            userIngredientLikes = new List<string>();
            userIngredientDislikes = new List<string>();
            userFridge = new List<string>();
            savedRecipes = new List<string>();
        }

    }








    /// <summary>
    /// The UserManager class has a method getUserByUsername which returns a User object containing all the user's information
    /// </summary>
    public class UserManager
    {

        SqliteConnection conn; //change for different connections


        public UserManager(SqliteConnection pConn) //change for different connections
        {
            conn = pConn;
        }


        /// <summary>
        /// Finds the user in the database and returns them in a User object
        /// </summary>
        /// <param name="pUsername"></param>
        /// <returns>a User object storing all the user's data</returns>
        public User getUserByUsername(string pUsername)
        {

            string sqlGetUser = $"select username, firstname, lastname, email from users where users.username = '{pUsername}';";

            string sqlGetUserIngredientLikes = "select distinct userIngredientLikes.ingredient FROM users INNER JOIN userIngredientLikes where userIngredientLikes.username = '{0}';";    //gets a users ingredient likes
            string sqlGetUserIngredientDislikes = "select distinct useringredientdislikes.ingredient FROM users INNER JOIN useringredientdislikes where useringredientdislikes.username = '{0}';";
            string sqlGetUserFridge = "select distinct userFridge.ingredient FROM users INNER JOIN userFridge where userFridge.username = '{0}';";
            string sqlGetUserSavedRecipes = "select distinct savedrecipes.recipeId FROM users INNER JOIN savedrecipes where savedrecipes.username = '{0}';";

            conn.Open();

            User newUser = new User();

            //SqlCommand command = new SqlCommand(sqlGetUser, conn); //for sql
            var command = conn.CreateCommand(); //for sqlite
            command.CommandText = sqlGetUser;


            //load main fields
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    newUser.username = reader["username"].ToString();
                    newUser.firstName = reader["firstName"].ToString();
                    newUser.lastName = reader["lastName"].ToString();
                    newUser.email = reader["email"].ToString();
                }

            }

            //load ingr likes
            //command = new SqlCommand(sqlGetUserIngredientLikes, conn); //for sql
            command.CommandText = String.Format(sqlGetUserIngredientLikes, newUser.username); //for sqlite
            using (var reader = command.ExecuteReader())
            {
                bool more = reader.Read();
                while (more)
                {
                    newUser.userIngredientLikes.Add(reader["ingredient"].ToString());
                    more = reader.Read();
                }
                
            }


            //load ingr dislikes
            //command = new SqlCommand(sqlGetUserIngredientDislikes, conn); //for sql
            command.CommandText = String.Format(sqlGetUserIngredientDislikes, newUser.username); //for sqlite
            using (var reader = command.ExecuteReader())
            {
                bool more = reader.Read(); ;
                while (more)
                {
                    newUser.userIngredientDislikes.Add(reader["ingredient"].ToString());
                    more = reader.Read();
                }

            }

            //load users fridge
            //command = new SqlCommand(sqlGetUserFridge, conn); //for sql
            command.CommandText = String.Format(sqlGetUserFridge, newUser.username); //for sqlite
            using (var reader = command.ExecuteReader())
            {
                bool more = reader.Read();
                while (more)
                {
                    newUser.userFridge.Add(reader["ingredient"].ToString());
                    more = reader.Read();
                }

            }

            //load users saved recipes
            //command = new SqlCommand(sqlGetUserSavedRecipes, conn); //for sql
            command.CommandText = String.Format(sqlGetUserSavedRecipes, newUser.username); //for sqlite
            using (var reader = command.ExecuteReader())
            {
                bool more = reader.Read();
                while (more)
                {
                    newUser.savedRecipes.Add(reader["recipeId"].ToString());
                    more = reader.Read();
                }

            }

            conn.Close();

            return newUser;
        }







    }




}