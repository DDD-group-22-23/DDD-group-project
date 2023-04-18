//using odbc
using System;
using System.Data.Odbc;
using QC = Microsoft.Data.SqlClient;
class OdbcTest
{
    static void Main()
    {
        
        //create an odbc connection
        OdbcConnection conn = new OdbcConnection("Driver={ODBC Driver 18 for SQL Server}Server=tcp:recipethesaurus.database.windows.net,1433;Initial Catalog=RecipeThesaurus;Persist SecurityInfo=False;User ID=CloudSA3888a0df;Password=password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        conn.Open();
        //create an odbc command
        OdbcCommand DbCommand = conn.CreateCommand();
        DbCommand.CommandText = "SELECT * FROM RecipeThesaurus";
        //create an odbc datareader
        OdbcDataReader reader = DbCommand.ExecuteReader();
        //read the data
        while (reader.Read())
        {
            Console.WriteLine(reader["STUFF HERE, DON'T KNOW WHAT YET"]);
        }
        //close the reader
        reader.Close();
        //close the connection
        conn.Close();
        
        /*
        using (var connection = new QC.SqlConnection(
                "Server=tcp:recipethesaurus.database.windows.net,1433;Initial Catalog=RecipeThesaurus;User ID=CloudSA3888a0df;Password=password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
                ))
        {
            connection.Open();
            Console.WriteLine("Connected successfully.");

            Console.WriteLine("Press any key to finish...");
            Console.ReadKey(true);
        }
        */
    }
}