using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

public class PlaylistsController : Controller
{
    public IActionResult Index() {
        ViewData["Username"] = HttpContext.Session.GetString("Username");
        int userID = (int)HttpContext.Session.GetInt32("UserID");
        var playlists = DataBaseSelect.SelectUsersPlaylists(userID);

        return View(playlists);
    }

    public IActionResult PlaylistDetails(int id) {
        ViewData["Username"] = HttpContext.Session.GetString("Username");
        var playlist = DataBaseSelect.SelectPlaylist(id);
        return View(playlist);
    }

    public IActionResult AddToFavorites(int id, int playlistID) {
        ViewData["Username"] = HttpContext.Session.GetString("Username");
        int userID = (int)HttpContext.Session.GetInt32("UserID");
        DataBaseInsertStatements.InsertIntoFavorites(id, userID);

        return RedirectToAction("PlaylistDetails", new { id =  playlistID});
    }   
    
}
