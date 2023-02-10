using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;


namespace RecipeThesaurus.Models.DB.LoginUsernamePassword
{
    #region Login by username and password store procedure method.  

    public class LoginByUsernamePassword
    {
   

        /// <summary>  
        /// Login by username and password store procedure method.  
        /// </summary>  
        /// <param name="usernameVal">Username value parameter</param>  
        /// <param name="passwordVal">Password value parameter</param>  
        /// <returns>Returns - List of logins by username and password</returns>  
        public async Task<List<LoginByUsernamePassword>> LoginByUsernamePasswordMethodAsync(string usernameVal, string passwordVal)
        {
            // Initialization.  
            List<LoginByUsernamePassword> lst = new List<LoginByUsernamePassword>();
            using (var context = new RecipeThesaurusContext())
                try
                {
                    // Settings.  
                    var sqlParams = new[]{
                    new SqlParameter("@username", usernameVal ?? (object)DBNull.Value),
                    new SqlParameter("@password", passwordVal ?? (object)DBNull.Value)
                };

            // Processing.  
           string sqlQuery = $"EXEC [dbo].[LoginByUsernamePassword] @username @password";
                    lst = await context.Database.SqlQueryRaw<LoginByUsernamePassword>(
                        sqlQuery,
                        sqlParams
                        ).ToListAsync();

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            // Info.  
            return lst;
        }

        #endregion
    }
    }
