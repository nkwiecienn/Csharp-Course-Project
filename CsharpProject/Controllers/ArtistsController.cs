using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

public class ArtistsController : Controller
{
    public IActionResult Index() {
        return View(DataBaseSelect.SelectArtists());
    }

    public IActionResult Details(int id) {
        var artist = DataBaseSelect.SelectArtist(id);
        return View(artist);
    }
}