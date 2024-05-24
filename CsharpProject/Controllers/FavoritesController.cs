using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

public class FavoritesController : Controller
{
    public IActionResult Index() {
        ViewData["Username"] = HttpContext.Session.GetString("Username");
        int userID = (int)HttpContext.Session.GetInt32("UserID");
        var favorites = DataBaseSelect.SelectUsersFavorites(userID);

        return View(favorites);
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
