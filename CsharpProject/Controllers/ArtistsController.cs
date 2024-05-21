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
}