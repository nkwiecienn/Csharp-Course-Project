using Microsoft.Data.Sqlite;

public class DataBaseInsertStatements {
    private readonly static string connectionString = "Data Source=db.db";

    public static void InsertIntoContent(int playlistID, int songID) {
        using (var connection = new SqliteConnection(connectionString)) {
            connection.Open();
            var command = new SqliteCommand("INSERT INTO Content (PlaylistID, SongID) VALUES (@playlistID, @songID);", connection);
            command.Parameters.AddWithValue("@playlistID", playlistID);
            command.Parameters.AddWithValue("@songID", songID);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public static void InsertIntoFavorites(int songID, int userID) {
        using (var connection = new SqliteConnection(connectionString)) {
            connection.Open();
            var command = new SqliteCommand("INSERT INTO Favorites (SongID, UserID) VALUES (@songID, @userID);", connection);
            command.Parameters.AddWithValue("@songID", songID);
            command.Parameters.AddWithValue("@userID", userID);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public static void InsertIntoPlaylists(int userID, string name) {
        using (var connection = new SqliteConnection(connectionString)) {
            connection.Open();
            var command = new SqliteCommand("INSERT INTO Playlists (UserID, Name) VALUES (@userID, @name);", connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@userID", userID);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}