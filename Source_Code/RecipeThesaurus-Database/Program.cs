//using odbc
using System;
using System.Data.Odbc;
class OdbcTest
{
    static void Main()
    {
        //create an odbc connection
        OdbcConnection conn = new OdbcConnection("Server=tcp:recipethesaurus.database.windows.net,1433;Initial Catalog=RecipeThesaurus;Persist SecurityInfo=False;User ID=CloudSA3888a0df;Password=TAKE-AS-SECRET-VAR;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        conn.Open();
        //create an odbc command
        OdbcCommand DbCommand = conn.CreateCommand();
        DbCommand.CommandText = "SELECT * FROM RecipeThesaurus";
        //create an odbc datareader
        OdbcDataReader reader = DbCommand.ExecuteReader();
        //read the data
        while (reader.Read())
        {
            Console.WriteLine(reader["RecipeName"]);
        }
        //close the reader
        reader.Close();
        //close the connection
        conn.Close();
    }
}