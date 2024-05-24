using Microsoft.Data.Sqlite;

public class DataBaseDeleteStatements {
    private readonly static string connectionString = "Data Source=db.db";

    public static void DeleteFromContent(int playlistID, int songID) {
        using (var connection = new SqliteConnection(connectionString)) {
            connection.Open();
            var command = new SqliteCommand("DELETE FROM Content WHERE PlaylistID = @playlistID AND SongID = @songID;", connection);
            command.Parameters.AddWithValue("@playlistID", playlistID);
            command.Parameters.AddWithValue("@songID", songID);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public static void DeleteFromFavorites(int userID, int songID) {
        using (var connection = new SqliteConnection(connectionString)) {
            connection.Open();
            var command = new SqliteCommand("DELETE FROM Favorites WHERE UserID = @userID AND SongID = @songID;", connection);
            command.Parameters.AddWithValue("@userID", userID);
            command.Parameters.AddWithValue("@songID", songID);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}