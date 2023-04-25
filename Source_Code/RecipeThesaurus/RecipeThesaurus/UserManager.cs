using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Identity.Client;
using SQLitePCL;

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
        SqlConnection conn; //change for different connections
        SqliteConnection conn2;
        bool SQL_VER = false;
        public UserManager(SqlConnection pConn = null)
        {
            SQL_VER = true;
            conn = pConn;
        }
        public UserManager(SqliteConnection pConn = null) //change for different connections
        {
            conn2 = pConn;
        }
 

        /// <summary>
        /// Finds the user in the database and returns them in a User object
        /// </summary>
        /// <param name="pUsername"></param>
        /// <returns>a User object storing all the user's data</returns>
        public User getUser(string pField, string type)
        {
            string sqlGetUser;
            if (type == "username")
            {
                sqlGetUser = $"select username, firstname, lastname, email from users where users.username = '{pField}';";
            }
            else
            {
                sqlGetUser = $"select username, firstname, lastname, email from users where users.email = '{pField}';";
            }
            

            string sqlGetUserIngredientLikes = "select distinct userIngredientLikes.ingredient FROM users INNER JOIN userIngredientLikes where userIngredientLikes.username = '{0}';";    //gets a users ingredient likes
            string sqlGetUserIngredientDislikes = "select distinct useringredientdislikes.ingredient FROM users INNER JOIN useringredientdislikes where useringredientdislikes.username = '{0}';";
            string sqlGetUserFridge = "select distinct userFridge.ingredient FROM users INNER JOIN userFridge where userFridge.username = '{0}';";
            string sqlGetUserSavedRecipes = "select distinct savedrecipes.recipeId FROM users INNER JOIN savedrecipes where savedrecipes.username = '{0}';";


            User newUser = new User();
            if (SQL_VER)
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sqlGetUser, conn);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ReadUserSql(reader, newUser);
                }
                command.CommandText = String.Format(sqlGetUserIngredientLikes, newUser.username);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ReadIngredientLikesSql(reader, newUser);
                }
                command.CommandText = String.Format(sqlGetUserIngredientDislikes, newUser.username);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ReadIngredientDisikesSql(reader, newUser);
                }
                command.CommandText = String.Format(sqlGetUserFridge, newUser.username);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ReadFridgeSql(reader, newUser);
                }
                command.CommandText = String.Format(sqlGetUserSavedRecipes, newUser.username);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ReadSavedSql(reader, newUser);
                }
            }
            else
            {
                conn2.Open();
                SqliteCommand command = new SqliteCommand(sqlGetUser, conn2);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    ReadUserSqlite(reader, newUser);
                }
                command.CommandText = String.Format(sqlGetUserIngredientLikes, newUser.username);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    ReadIngredientLikesSqlite(reader, newUser);
                }
                command.CommandText = String.Format(sqlGetUserIngredientDislikes, newUser.username);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    ReadIngredientDisikesSqlite(reader, newUser);
                }
                command.CommandText = String.Format(sqlGetUserFridge, newUser.username);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    ReadFridgeSqlite(reader, newUser);
                }
                command.CommandText = String.Format(sqlGetUserSavedRecipes, newUser.username);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    ReadSavedSqlite(reader, newUser);
                }
            }
            return newUser;
        }



        #region SQL
        public void ReadUserSql(SqlDataReader r, User u)
        {
            u.username = r["username"].ToString();
            u.firstName = r["firstName"].ToString();
            u.lastName = r["lastName"].ToString();
            u.email = r["email"].ToString();
        }

        public void ReadIngredientLikesSql(SqlDataReader r, User u)
        {
            bool more = r.Read();
            while (more)
            {
                u.userIngredientLikes.Add(r["ingredient"].ToString());
                more = r.Read();
            }
        }

        public void ReadIngredientDisikesSql(SqlDataReader r, User u)
        {
            bool more = r.Read(); ;
            while (more)
            {
                u.userIngredientDislikes.Add(r["ingredient"].ToString());
                more = r.Read();
            }
        }

        public void ReadFridgeSql(SqlDataReader r, User u)
        {
            bool more = r.Read();
            while (more)
            {
                u.userFridge.Add(r["ingredient"].ToString());
                more = r.Read();
            }
        }
        public void ReadSavedSql(SqlDataReader r, User u)
        {
            bool more = r.Read();
            while (more)
            {
                u.savedRecipes.Add(r["recipeId"].ToString());
                more = r.Read();
            }
        }
        #endregion

        #region Sqlite
        public void ReadUserSqlite(SqliteDataReader r, User u)
        {
            r.Read();
            u.username = r["username"].ToString();
            u.firstName = r["firstName"].ToString();
            u.lastName = r["lastName"].ToString();
            u.email = r["email"].ToString();
        }

        public void ReadIngredientLikesSqlite(SqliteDataReader r, User u)
        {
            bool more = r.Read();
            while (more)
            {
                u.userIngredientLikes.Add(r["ingredient"].ToString());
                more = r.Read();
            }
        }

        public void ReadIngredientDisikesSqlite(SqliteDataReader r, User u)
        {
            bool more = r.Read(); ;
            while (more)
            {
                u.userIngredientDislikes.Add(r["ingredient"].ToString());
                more = r.Read();
            }
        }

        public void ReadFridgeSqlite(SqliteDataReader r, User u)
        {
            bool more = r.Read();
            while (more)
            {
                u.userFridge.Add(r["ingredient"].ToString());
                more = r.Read();
            }
        }
        public void ReadSavedSqlite(SqliteDataReader r, User u)
        {
            bool more = r.Read();
            while (more)
            {
                u.savedRecipes.Add(r["recipeId"].ToString());
                more = r.Read();
            }
        }
        #endregion sqlite
    }
}

/*

SqlCommand command = new SqlCommand(sqlGetUser, conn);
            var command = conn.CreateCommand(); //for sqlite
            command.CommandText = sqlGetUser;


            //load main fields
            using (sqlitedatareader reader = command.ExecuteReader())
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
*/