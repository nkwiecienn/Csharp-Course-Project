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
}