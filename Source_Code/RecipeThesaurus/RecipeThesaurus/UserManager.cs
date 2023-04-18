using Microsoft.Data.SqlClient;

namespace RecipeThesaurus
{

    public class User
    {
        public string username;
        public string firstName;
        public string lastName;
        public string email;

        public string[] userIngredientLikes;
        public string[] userIngredientDislikes;
        public string[] userFridge; //array of ingredients
        public string[] savedRecipes;

    }

    public class UserManager
    {

        SqlConnection conn;


        public UserManager(SqlConnection pConn)
        {

        }


        public User getUserByUsername(string pUsername)
        {

            string sqlGetUser = $"select username, firstname, lastname, email from users where users.username = '{pUsername}';";

            string sqlGetUserIngredientLikes = $"select distinct useringredientlikes.ingredient FROM users INNER JOIN useringredientlikes where useringredientlikes.username = '{0}';";    //gets a users ingredient likes
            string sqlGetUserIngredientDislikes = $"select distinct useringredientdislikes.ingredient FROM users INNER JOIN useringredientdislikes where useringredientdislikes.username = '{0}';";
            string sqlGetUserFridge = $"select distinct userFridge.ingredient FROM users INNER JOIN userFridge where userFridge.username = '{0}';";
            string sqlSavedRecipes = $"select distinct savedrecipes.recipeId FROM users INNER JOIN savedrecipes where savedrecipes.username = '{0}';";



            User newUser = new User();

            SqlCommand command = new SqlCommand(sqlGetUser, conn);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    newUser.username = reader["username"].ToString();
                    newUser.firstName = reader["firstName"].ToString();
                    newUser.lastName = reader["lastName"].ToString();
                    newUser.email = reader["email"].ToString();
                }
            }

            command = new SqlCommand(sqlGetUserIngredientLikes, conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                //load ingr likes
            }



            return null;
        }







    }




}