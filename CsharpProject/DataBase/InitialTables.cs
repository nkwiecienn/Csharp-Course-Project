using System;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CsharpProject.Models;


class InitialTables
{

    public static void Init()
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder();

        connectionStringBuilder.DataSource = "db.db";

        using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
        {
            connection.Open();
            DropTables(connection);
            CreateTables(connection);
            SeedData(connection);
        }
    }

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
                + "ArtistID INTEGER NOT NULL,"
                + "ReleaseDate TEXT NOT NULL,"
                + "Genre TEXT NOT NULL,"
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

            delTableCmd = connection.CreateCommand();
            delTableCmd.CommandText = "DROP TABLE IF EXISTS Users";
            delTableCmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
        return true;
    }


    public static bool SeedData(SqliteConnection connection)
    {
        try
        {
            using (var transaction = connection.BeginTransaction())
            {
                var insertCmd = connection.CreateCommand();

                // Insert Users
                insertCmd.CommandText =
                    "INSERT INTO Users (Username, Password) VALUES " +
                    "('user1', @password1), " +
                    "('user2', @password2), " +
                    "('user3', @password3), " +
                    "('user4', @password4), " +
                    "('user5', @password5);";
                insertCmd.Parameters.AddWithValue("@password1", MD5Hash("password1"));
                insertCmd.Parameters.AddWithValue("@password2", MD5Hash("password2"));
                insertCmd.Parameters.AddWithValue("@password3", MD5Hash("password3"));
                insertCmd.Parameters.AddWithValue("@password4", MD5Hash("password4"));
                insertCmd.Parameters.AddWithValue("@password5", MD5Hash("password5"));
                insertCmd.ExecuteNonQuery();

                // Insert Artists
                insertCmd.CommandText =
                    "INSERT INTO Artists (Name, DebutDate) VALUES " +
                    "('Daria Zawiałow', '2016-03-04'), " +
                    "('The Dumplings', '2013-11-13'), " + 
                    "('Lana Del Rey', '2010-01-01'), " + 
                    "('Beyoncé', '2003-06-24'), " + 
                    "('Myslovitz', '1995-01-01');";
                insertCmd.ExecuteNonQuery();

                // Insert Albums
                insertCmd.CommandText =
                    "INSERT INTO Albums (Name, ArtistID, ReleaseDate, Genre) VALUES " +
                    "('A Kysz!', 1, '2016-03-04', 'Pop'), " +
                    "('Helsinki', 1, '2019-03-08', 'Pop'), " +
                    "('No Bad Days', 2, '2014-05-13', 'Electropop'), " +
                    "('Sea You Later', 2, '2015-11-13', 'Electropop'), " + 
                    "('Born to Die', 3, '2012-01-27', 'Pop'), " +
                    "('Norman Fucking Rockwell!', 3, '2019-08-30', 'Pop'), " + 
                    "('Dangerously in Love', 4, '2003-06-24', 'R&B'), " +
                    "('Lemonade', 4, '2016-04-23', 'R&B'), " + 
                    "('Myslovitz', 5, '1995-01-01', 'Rock'), " +
                    "('Korova Milky Bar', 5, '2002-05-06', 'Rock');"; 
                insertCmd.ExecuteNonQuery();

                // Insert Songs
                insertCmd.CommandText =
                    "INSERT INTO Songs (AlbumID, Name) VALUES " +
                    // Daria Zawiałow - A Kysz!
                    "(1, 'Malinowy Chruśniak'), " +
                    "(1, 'Kundel Bury'), " + 
                    "(1, 'Miłostki'), " +
                    "(1, 'Lwy'), " +
                    "(1, 'Punk Fu!'), " +
                    "(1, 'Nie Dobiję Się Do Ciebie'), " +
                    "(1, 'Skupienie'), " +
                    "(1, 'Wojny i Noce'), " +
                    "(1, 'Dizzy'), " +
                    "(1, 'Na Skróty'), " +
                    "(1, 'Król Lul'), " +
                    // Daria Zawiałow - Helsinki
                    "(2, 'Szarówka'), " +
                    "(2, 'Hej Hej!'), " + 
                    "(2, 'Winter Is Coming'), " +
                    "(2, 'Pistolet'), " +
                    "(2, 'Związek X'), " +
                    "(2, 'Jeszcze W Zielone Gramy'), " +
                    "(2, 'Dolores'), " +
                    "(2, 'Gdybym Miała Serce'), " +
                    "(2, 'Kundel Bury (Helsinki Version)'), " +
                    "(2, 'Iskrzy (Tego Chciałam)'), " +
                    "(2, 'Winter Is Coming (Acoustic)'), " +
                    // The Dumplings - No Bad Days
                    "(3, 'Betonowy Las'), " +
                    "(3, 'Nie Słucham'), " +
                    "(3, 'Słodko-Słony Cios'), " +
                    "(3, 'Technicolor Yawn'), " +
                    "(3, 'Lato 2014'), " +
                    "(3, 'Underwater'), " +
                    "(3, 'Nie Gotujemy'), " +
                    "(3, 'Fastrygi'), " +
                    "(3, 'Zanim Zjesz'), " +
                    "(3, 'Backpack'), " +
                    // The Dumplings - Sea You Later
                    "(4, 'Kocham Być z Tobą'), " +
                    "(4, 'Zabawa'), " +
                    "(4, 'Odyseusz'), " +
                    "(4, 'Możliwość Wyspy'), " +
                    "(4, 'Samoloty'), " +
                    "(4, 'Kieszenie'), " +
                    "(4, 'Nie Nie Nie'), " +
                    "(4, 'Tysiąc Miejsc'), " +
                    "(4, 'Słowa'), " +
                    "(4, 'Ach Nie Mnie Jednej'), " +
                    // Lana Del Rey - Born to Die
                    "(5, 'Born to Die'), " +
                    "(5, 'Off to the Races'), " +
                    "(5, 'Blue Jeans'), " +
                    "(5, 'Video Games'), " +
                    "(5, 'Diet Mountain Dew'), " +
                    "(5, 'National Anthem'), " +
                    "(5, 'Dark Paradise'), " +
                    "(5, 'Radio'), " +
                    "(5, 'Carmen'), " +
                    "(5, 'Million Dollar Man'), " +
                    "(5, 'Summertime Sadness'), " +
                    "(5, 'This Is What Makes Us Girls'), " +
                    // Lana Del Rey - Norman Fucking Rockwell!
                    "(6, 'Norman f***ing Rockwell'), " +
                    "(6, 'Mariners Apartment Complex'), " +
                    "(6, 'Venice Bitch'), " +
                    "(6, 'F*** it I love you'), " +
                    "(6, 'Doin Time'), " +
                    "(6, 'Love song'), " +
                    "(6, 'Cinnamon Girl'), " +
                    "(6, 'How to disappear'), " +
                    "(6, 'California'), " +
                    "(6, 'The Next Best American Record'), " +
                    "(6, 'The greatest'), " +
                    "(6, 'Bartender'), " +
                    "(6, 'Happiness is a butterfly'), " +
                    "(6, 'hope is a dangerous thing for a woman like me to have - but I have it'), " +
                    // Beyoncé - Dangerously in Love
                    "(7, 'Crazy in Love'), " +
                    "(7, 'Naughty Girl'), " +
                    "(7, 'Baby Boy'), " +
                    "(7, 'Hip Hop Star'), " +
                    "(7, 'Be With You'), " +
                    "(7, 'Me, Myself and I'), " +
                    "(7, 'Yes'), " +
                    "(7, 'Signs'), " +
                    "(7, 'Speechless'), " +
                    "(7, 'Thats How You Like It'), " +
                    "(7, 'The Closer I Get to You'), " +
                    "(7, 'Dangerously in Love 2'), " +
                    "(7, 'Beyonce Interlude'), " +
                    "(7, 'Gift from Virgo'), " +
                    "(7, 'Daddy'), " +
                    // Beyoncé - Lemonade
                    "(8, 'Pray You Catch Me'), " +
                    "(8, 'Hold Up'), " +
                    "(8, 'Dont Hurt Yourself'), " +
                    "(8, 'Sorry'), " +
                    "(8, '6 Inch'), " +
                    "(8, 'Daddy Lessons'), " +
                    "(8, 'Love Drought'), " +
                    "(8, 'Sandcastles'), " +
                    "(8, 'Forward'), " +
                    "(8, 'Freedom'), " +
                    "(8, 'All Night'), " +
                    "(8, 'Formation')," +
                    // Myslovitz - Myslovitz
                    "(9, 'Peggy Brown'), " +
                    "(9, 'Z Twarzą Marilyn Monroe'), " +
                    "(9, 'Myslovitz'), " +
                    "(9, 'Krótka Piosenka o Miłości'), " +
                    "(9, 'Maj'), " +
                    "(9, 'Nienawiść'), " +
                    "(9, 'Mieć czy być'), " +
                    "(9, 'Chłopcy'), " +
                    "(9, 'Jeden Wyścig'), " +
                    "(9, 'Ostatnia Noc'), " +
                    "(9, 'Good Day My Angel'), " +
                    // Myslovitz - Korova Milky Bar
                    "(10, 'Acidland'), " +
                    "(10, 'Długość Dźwięku Samotności'), " +
                    "(10, 'Sprzedawcy Marzeń'), " +
                    "(10, '21 Dni'), " +
                    "(10, 'Alexander'), " +
                    "(10, 'Polowanie na Wielbłąda'), " +
                    "(10, 'Dla Ciebie'), " +
                    "(10, 'To Nie Był Film'), " +
                    "(10, 'Zginę, jeśli Zginiesz Ty'), " +
                    "(10, 'Uciekinier'), " +
                    "(10, 'Pocztówka z Lotniska');"; 
                    insertCmd.ExecuteNonQuery();

                    insertCmd.CommandText =
                    "INSERT INTO Playlist (UserID, Name) VALUES " +
                    "(1, 'Motyle i Cmy')," +
                    "(1, 'Road trip')," +
                    "(2, 'its all good')," +
                    "(2, 'i didnt know i had a dream');";
                    insertCmd.ExecuteNonQuery();

                    insertCmd.CommandText = 
                    "INSERT INTO Content (PlaylistID, SongID) VALUES "+
                    "(1, 1), " +
                    "(1, 6), " +
                    "(1, 19), " +
                    "(1, 33), " +
                    "(1, 42), " +
                    "(1, 108), " +
                    "(1, 96), " +
                    "(1, 111), " +
                    "(1, 113), " +
                    "(1, 36), " +
                    "(1, 62), " +
                    "(1, 103), " +
                    "(1, 57), " +

                    "(2, 13), " +
                    "(2, 69), " +
                    "(2, 80), " +
                    "(2, 82), " +
                    "(2, 85), " +
                    "(2, 23), " +
                    "(2, 8), " +
                    "(2, 46), " +
                    "(2, 50), " +
                    "(2, 44);";
                    insertCmd.ExecuteNonQuery();

                    insertCmd.CommandText =
                    "INSERT INTO Favorites (SongID, UserID) VALUES " +
                    "(6, 1), (42, 1), (57, 1), (69, 1), (101, 1), (102, 1), (10, 1), (31, 1), (5, 1), (18, 1), (48, 1), (115, 1), (11, 1), (12, 1), (13, 1), (14, 1), (15, 1), (16, 1), (17, 1), (19, 1), " +
                    "(6, 2), (42, 2), (57, 2), (69, 2), (101, 2), (102, 2), (31, 2), (1, 2), (67, 2), (8, 2), (47, 2), (115, 2), (32, 2), (33, 2), (34, 2), (35, 2), (36, 2), (37, 2), (38, 2), (39, 2), " +
                    "(6, 3), (42, 3), (5, 3), (69, 3), (101, 3), (102, 3), (51, 3), (31, 3), (19, 3), (13, 3), (46, 3), (114, 3), (53, 3), (54, 3), (55, 3), (56, 3), (57, 3), (58, 3), (59, 3), (60, 3), " +
                    "(6, 4), (42, 4), (57, 4), (69, 4), (101, 4), (102, 4), (71, 4), (91, 4), (15, 4), (1, 4), (45, 4), (114, 4), (74, 4), (75, 4), (76, 4), (77, 4), (78, 4), (79, 4), (80, 4), (81, 4), " +
                    "(6, 5), (42, 5), (57, 5), (69, 5), (102, 5), (117, 5), (91, 5), (92, 5), (15, 5), (43, 5), (44, 5), (114, 5), (95, 5), (96, 5), (97, 5), (98, 5), (99, 5), (100, 5), (1, 5), (103, 5);";
                    insertCmd.ExecuteNonQuery();


                    
                transaction.Commit();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
        return true;
    }

    private static string MD5Hash(string input)
    {
        using (var md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();

            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }

}
