using System;
using Microsoft.Data.Sqlite;

class InitialTables
{

    public static bool CreateTables(SqliteConnection connection)
    {

        try
        {
            SqliteCommand createTableCmd = connection.CreateCommand();

            // Create Users table
            createTableCmd.CommandText =
                "CREATE TABLE Users ("
                + "ClientId INTEGER PRIMARY KEY AUTOINCREMENT,"
                + "Username TEXT NOT NULL,"
                + "Password TEXT NOT NULL);";
            createTableCmd.ExecuteNonQuery();

            // Create Artists table
            createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText =
                "CREATE TABLE Artists ("
                + "ArtistID INTEGER PRIMARY KEY AUTOINCREMENT,"
                + "Name TEXT NOT NULL,"
                + "DebutDate TEXT NOT NULL);";
            createTableCmd.ExecuteNonQuery();

            // Create Albums table
            createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText =
                "CREATE TABLE Albums ("
                + "AlbumID INTEGER PRIMARY KEY AUTOINCREMENT,"
                + "Name TEXT NOT NULL,"
                + "ArtistID INTEGER NOT NULL"
                + "ReleaseDate TEXT NOT NULL,"
                + "Genre TEXT NOT NULL"
                + "FOREIGN KEY (ArtistID) REFERENCES Artists(ArtistID));";
            createTableCmd.ExecuteNonQuery();

            // Create Songs table
            createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText =
                "CREATE TABLE Songs ("
                + "SongID INTEGER PRIMARY KEY AUTOINCREMENT,"
                + "AlbumID INTEGER NOT NULL,"
                + "Name TEXT NOT NULL,"
                + "FOREIGN KEY (AlbumID) REFERENCES Albums(AlbumID));";
            createTableCmd.ExecuteNonQuery();

            // Create Favorites table
            createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText =
                "CREATE TABLE Favorites ("
                + "SongID INTEGER NOT NULL,"
                + "UserID INTEGER NOT NULL,"
                + "PRIMARY KEY (SongID, UserID),"
                + "FOREIGN KEY (SongID) REFERENCES Songs(SongID),"
                + "FOREIGN KEY (UserID) REFERENCES Users(ClientId));";
            createTableCmd.ExecuteNonQuery();

            // Create Playlist table
            createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText =
                "CREATE TABLE Playlist ("
                + "PlaylistID INTEGER PRIMARY KEY AUTOINCREMENT,"
                + "UserID INTEGER NOT NULL,"
                + "Name TEXT NOT NULL,"
                + "FOREIGN KEY (UserID) REFERENCES Users(ClientId));";
            createTableCmd.ExecuteNonQuery();

            // Create Content table
            createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText =
                "CREATE TABLE Content ("
                + "PlaylistID INTEGER NOT NULL,"
                + "SongID INTEGER NOT NULL,"
                + "PRIMARY KEY (PlaylistID, SongID),"
                + "FOREIGN KEY (PlaylistID) REFERENCES Playlist(PlaylistID),"
                + "FOREIGN KEY (SongID) REFERENCES Songs(SongID));";
            createTableCmd.ExecuteNonQuery();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
        return true;

    }

    public static Boolean DropTables(SqliteConnection connection)
    {
        try
        {
            SqliteCommand delTableCmd;

            delTableCmd = connection.CreateCommand();
            delTableCmd.CommandText = "DROP TABLE IF EXISTS Users";
            delTableCmd.ExecuteNonQuery();

            delTableCmd = connection.CreateCommand();
            delTableCmd.CommandText = "DROP TABLE IF EXISTS Content";
            delTableCmd.ExecuteNonQuery();

            delTableCmd = connection.CreateCommand();
            delTableCmd.CommandText = "DROP TABLE IF EXISTS Playlist";
            delTableCmd.ExecuteNonQuery();

            delTableCmd = connection.CreateCommand();
            delTableCmd.CommandText = "DROP TABLE IF EXISTS Favorites";
            delTableCmd.ExecuteNonQuery();

            delTableCmd = connection.CreateCommand();
            delTableCmd.CommandText = "DROP TABLE IF EXISTS Songs";
            delTableCmd.ExecuteNonQuery();

            delTableCmd = connection.CreateCommand();
            delTableCmd.CommandText = "DROP TABLE IF EXISTS Albums";
            delTableCmd.ExecuteNonQuery();

            delTableCmd = connection.CreateCommand();
            delTableCmd.CommandText = "DROP TABLE IF EXISTS Artists";
            delTableCmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
        return true;
    }


    public static void Init()
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder();

        connectionStringBuilder.DataSource = "db.db";

        using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
        {
            connection.Open();
            DropTables(connection);
            CreateTables(connection);
        }
    }

}
