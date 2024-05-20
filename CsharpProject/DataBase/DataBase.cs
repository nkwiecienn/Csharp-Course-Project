using System;
using Microsoft.Data.Sqlite;

class InitialTables
{

    public static bool CreateTables(SqliteConnection connection)
    {

        try
        {
            SqliteCommand createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText =
                "CREATE TABLE Users ("
                + "ClientId INTEGER NOT NULL,"
                + "UserName TEXT NOT NULL,"
                + "Password TEXT NOT NULL,"
                + "CONSTRAINT PK_Users PRIMARY KEY(Id AUTOINCREMENT));";
            createTableCmd.ExecuteNonQuery();

            SqliteCommand createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText =
                "CREATE TABLE Artists ("
                + "";
            createTableCmd.ExecuteNonQuery();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
        return true;

    }

}
