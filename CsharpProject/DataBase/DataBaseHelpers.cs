using System.Data;
using Microsoft.Data.Sqlite;

public class DataBaseHelpers {
    private readonly static string connectionString = "Data Source=db.db";

    public static Boolean IsSongAddedToFavorites(int SongID, int UserID) {
        using (var connection = new SqliteConnection(connectionString)) {
            connection.Open();
            var command = new SqliteCommand("SELECT SongID, UserID FROM Favorites WHERE SongID = @SongID AND UserID = @UserID", connection);
            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@SongID", SongID);

            using (var reader = command.ExecuteReader()) {
                if(reader.Read()) {
                    return true;
                }
                return false;
            }
        }
    }

    public static int SelectSongsAlbumID (int songID) {
        using (var connection = new SqliteConnection(connectionString)) {
            connection.Open();
            var command = new SqliteCommand("SELECT AlbumID FROM Songs WHERE SongID = @SongID", connection);
            command.Parameters.AddWithValue("@SongID", songID);

            using (var reader = command.ExecuteReader()) {
                if(reader.Read()) {
                    return reader.GetInt32(0);
                }
                return 0;
            }
        }
    }
}