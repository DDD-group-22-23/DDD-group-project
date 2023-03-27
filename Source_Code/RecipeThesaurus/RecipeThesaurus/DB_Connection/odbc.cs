//https://www.easysoft.com/developer/languages/csharp/ado-net-odbc.html


using System.Data.Odbc;

namespace RecipeThesaurus.DB_Connection
{
    public class odbc
    {
        OdbcConnection DbConnection = new OdbcConnection("DSN=CONNECTION_STRING_HERE");
        DbConnection.Open();
        OdbcCommand DbCommand = DbConnection.CreateCommand();

        DbCommand.CommandText = "SELECT * FROM NATION";
        OdbcDataReader DbReader = DbCommand.ExecuteReader();
        int fCount = DbReader.FieldCount;
        Console.Write(":");

        for (int i = 0; i< fCount; i ++ )
        {
            String fName = DbReader.GetName(i);
            Console.Write(fName + ":");
        }
        Console.WriteLine();
    }

    while (DbReader.Read())
    {
        Console.Write(":");
        for (int i = 0; i < fCount; i++)
        {
            String col = DbReader.GetString(i);

            Console.Write(col + ":");
        }
        Console.WriteLine();
    }

    DbReader.Close();
    DbCommand.Dispose();
    DbConnection.Close();
}
