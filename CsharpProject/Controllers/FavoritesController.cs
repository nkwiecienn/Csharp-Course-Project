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
}
