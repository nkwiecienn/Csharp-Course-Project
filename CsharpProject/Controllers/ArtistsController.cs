using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

public class ArtistsController : Controller
{
    public IActionResult Index() {
        ViewData["Username"] = HttpContext.Session.GetString("Username");

        return View(DataBaseSelect.SelectArtists());
    }

    public IActionResult Details(int id) {
        ViewData["Username"] = HttpContext.Session.GetString("Username");

        var artist = DataBaseSelect.SelectArtist(id);
        return View(artist);
    }

    public IActionResult AlbumDetails(int id) {
        ViewData["Username"] = HttpContext.Session.GetString("Username");

        var album = DataBaseSelect.SelectAlbum(id);
        return View(album);
    }

    public IActionResult AddToPlaylist(int id) {
        ViewData["Username"] = HttpContext.Session.GetString("Username");
        int userID = (int)HttpContext.Session.GetInt32("UserID");
        var playlists = DataBaseSelect.SelectUsersPlaylists(userID);

        ViewBag.SongID = id;

        return View(playlists);
    }

    [HttpPost]
    public IActionResult Add (int playlistID, int songID) {
        ViewData["Username"] = HttpContext.Session.GetString("Username");
        DataBaseInsertStatements.InsertIntoContent(playlistID, songID);
        return RedirectToAction("Index");
    }
}