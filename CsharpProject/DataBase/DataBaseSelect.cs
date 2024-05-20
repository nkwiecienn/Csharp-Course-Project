using Microsoft.Data.Sqlite;

public class DataBaseSelect {

    private readonly static string connectionString = "Data Source=db.db";

    public static List<Artist> SelectArtists () {
        var artists = new List<Artist>();

        using (var connection = new SqliteConnection(connectionString)) {
            connection.Open();
            var command = new SqliteCommand("SELECT ArtistID, Name, DebutDate FROM Artists", connection);

            using (var reader = command.ExecuteReader()) {
                while(reader.Read()) {
                    artists.Add(new Artist{
                        ArtistID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        DebutDate = reader.GetString(2)
                    });
                }
            }

        }

        return artists;
    }

    public static List<Album> SelectArtistsAlbums (int artistID) {

        var albums = new List<Album>();

        using (var connection = new SqliteConnection(connectionString)) {
            connection.Open();
            var command = new SqliteCommand("SELECT AlbumID, Name, ArtitsID, ReleaseDate, Genre FROM Albums WHERE ArtistID = '@artistID'", connection);
            command.Parameters.AddWithValue("@artistID", artistID);

            using (var reader = command.ExecuteReader()) {
                while(reader.Read()) {
                    albums.Add(new Album{
                        AlbumID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        ArtistID = reader.GetInt32(2),
                        ReleaseDate = reader.GetString(3),
                        Genre = reader.GetString(4)
                    });
                }
            }

        }

        return albums;

    }

    public static List<Song> SelectAlbumsSongs (int albumID) {

        var songs = new List<Song>();

        using (var connection = new SqliteConnection(connectionString)) {
            connection.Open();
            var command = new SqliteCommand("SELECT SongID, AlbumID, Name FROM Songs WHERE AlbumID = '@albumID'", connection);
            command.Parameters.AddWithValue("@albumID", albumID);

            using (var reader = command.ExecuteReader()) {
                while(reader.Read()) {
                    songs.Add(new Song{
                        SongID = reader.GetInt32(0),
                        AlbumID = reader.GetInt32(1),
                        Name = reader.GetString(2),
                    });
                }
            }

        }

        return songs;        

    }

    public static List<Playlist> SelectUsersPlaylists (int userID) {
        var playlists = new List<Playlist>();

        using (var connection = new SqliteConnection(connectionString)) {
            connection.Open();
            var command = new SqliteCommand("SELECT PlaylistID, UserID, Name FROM Playlists WHERE UserID=@userID", connection);
            command.Parameters.AddWithValue("@userID", userID);

            using (var reader = command.ExecuteReader()) {
                while(reader.Read()) {
                    playlists.Add(new Playlist{
                        PlaylistID = reader.GetInt32(0),
                        UserID = reader.GetInt32(1),
                        Name = reader.GetString(2),
                    });
                }
            }

        }

        return playlists;
    }

    public static List<Song> SelectPlaylistsSongs (int playlistID) {

        var songs = new List<Song>();

        using (var connection = new SqliteConnection(connectionString)) {
            connection.Open();
            var command = new SqliteCommand(
                                            "SELECT s.SongID, s.AlbumID, s.Name"
                                            + "FROM Songs s"
                                            + "INNER JOIN Content c USING SongID"
                                            + "WHERE c.PlaylistID = '@playlistID'", connection);
            command.Parameters.AddWithValue("@playlistID", playlistID);

            using (var reader = command.ExecuteReader()) {
                while(reader.Read()) {
                    songs.Add(new Song{
                        SongID = reader.GetInt32(0),
                        AlbumID = reader.GetInt32(1),
                        Name = reader.GetString(2),
                    });
                }
            }

        }

        return songs;

    }

    public static List<Song> SelectUsersFavorites (int userID) {

        var songs = new List<Song>();

        using (var connection = new SqliteConnection(connectionString)) {
            connection.Open();
            var command = new SqliteCommand(
                                            "SELECT s.SongID, s.AlbumID, s.Name"
                                            + "FROM Users u"
                                            + "INNER JOIN Favorites f USING UserID"
                                            + "INNER JOIN Songs s USING SongID"
                                            + "WHERE u.UserID = '@userID'", connection);
            command.Parameters.AddWithValue("@userID", userID);

            using (var reader = command.ExecuteReader()) {
                while(reader.Read()) {
                    songs.Add(new Song{
                        SongID = reader.GetInt32(0),
                        AlbumID = reader.GetInt32(1),
                        Name = reader.GetString(2),
                    });
                }
            }

        }

        return songs;
    }

    public static List<Song> SelectMostLikedSongInMostPopularGenreInPlaylist(int playlistID) {
        var songs = new List<Song>();

        using (var connection = new SqliteConnection(connectionString)) {
            connection.Open();
            var command = new SqliteCommand(
               "SELECT s.SongID, s.AlbumID, s.Name " +
                "FROM Songs s " +
                    "INNER JOIN Albums a ON s.AlbumID = a.AlbumID " +
                "WHERE a.Genre = ( " +
                    "SELECT aa.Genre " +
                    "FROM Albums aa " +
                    "INNER JOIN Songs ss ON aa.AlbumID = ss.AlbumID " +
                    "INNER JOIN Content c ON ss.SongID = c.SongID " +
                    "WHERE c.PlaylistID = @playlistID " +
                    "GROUP BY aa.Genre " +
                    "ORDER BY COUNT(ss.SongID) DESC " +
                    "LIMIT 1) " +
                "GROUP BY s.SongID " +
                "ORDER BY (SELECT COUNT(f.UserID) FROM Favorites f WHERE f.SongID = s.SongID) DESC " +
                "LIMIT 5;", connection);

            command.Parameters.AddWithValue("@playlistID", playlistID);

            using (var reader = command.ExecuteReader()) {
                while(reader.Read()) {
                    songs.Add(new Song {
                        SongID = reader.GetInt32(0),
                        AlbumID = reader.GetInt32(1),
                        Name = reader.GetString(2),
                    });
                }
            }
        }

        return songs;
    }

}
