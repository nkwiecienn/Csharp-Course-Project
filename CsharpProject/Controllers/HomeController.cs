using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CsharpProject.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;

namespace CsharpProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

     [Route("/")]
    public IActionResult Index()
    {
        ViewData["Username"] = HttpContext.Session.GetString("Username");

        return View();
    }

    [Route("/profile")]
    public IActionResult Profile()
    {
        ViewData["Username"] = HttpContext.Session.GetString("Username");
        return View();
    }

    [Route("/privacy")]
    public IActionResult Privacy()
    {
        ViewData["Username"] = HttpContext.Session.GetString("Username");
        return View();
    }

    [Route("/logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    [Route("/login")]
    public IActionResult Login()
    {
        ViewData["Username"] = HttpContext.Session.GetString("Username");
        return View();
    }

    [HttpPost]
    [Route("/login")]
    public IActionResult Login(IFormCollection form)
    {
        if (form is null)
            return View();

        using (var connection = new SqliteConnection("Data Source=db.db"))
        {
            String username = form["username"].ToString();
            String password = form["password"].ToString();

            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password;";
            command.Parameters.AddWithValue("@Username", form["username"].ToString());
            command.Parameters.AddWithValue("@Password", MD5Hash(form["password"].ToString()));

            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                ViewData["Username"] = username;
                HttpContext.Session.SetString("Username", username);
                int userID = DataBaseSelect.SelectUser(username).UserID;
                HttpContext.Session.SetInt32("UserID", userID);
                return RedirectToAction("Profile");
            }
            else
            {
                ViewData["Message"] = "Invalid username or password";
            }

        }
        return View();
    }

    [Route("/register")]
    public IActionResult Register()
    {
        ViewData["Username"] = HttpContext.Session.GetString("Username");
        return View();
    }

    [HttpPost]
    [Route("/register")]
    public IActionResult Register(IFormCollection form)
    {
        if (form is null)
            return View();

        using (var connection = new SqliteConnection("Data Source=db.db"))

        {

            String username = form["username"].ToString();
            String password = form["password1"].ToString();

            connection.Open();
            if (password != form["password2"].ToString())
            {
                ViewData["Message"] = "Passwords do not match";
                return View();
            }

            var isRegistered = connection.CreateCommand();
            isRegistered.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = @Username;";
            isRegistered.Parameters.AddWithValue("@Username", username);
            var reader = isRegistered.ExecuteReader();
            if (reader.Read())
            {
                if (reader.GetInt32(0) > 0)
                {
                    ViewData["Message"] = "Username already exists";
                    return View();
                }
            }

            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password);";
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", MD5Hash(password));

            command.ExecuteNonQuery();

            ViewData["Message"] = "Registration successful";
        }

        return View();
    }



    private string MD5Hash(string input)
    {
        using (var md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();

            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }

    // [Route("/data")]
    // public IActionResult Data()
    // {
    //     ViewData["Username"] = HttpContext.Session.GetString("Username");

    //     if (ViewData["Username"] is null)
    //     {
    //         return RedirectToAction("Login");
    //     }

    //     using (var connection = new SqliteConnection("Data Source=db.db"))

    //     {
    //         connection.Open();
    //         var command = connection.CreateCommand();
    //         command.CommandText = "SELECT * FROM Data WHERE UserId = (SELECT Id FROM Users WHERE Username = @Username);";
    //         command.Parameters.AddWithValue("@Username", ViewData["Username"].ToString());
    //         var reader = command.ExecuteReader();
    //         List<String> dataList = new List<String>();
    //         while (reader.Read())
    //         {
    //             dataList.Add(new String(reader.GetString(1)));
    //         }
    //         ViewData["DataList"] = dataList;
    //     }
    //     return View();
    // }

    // [HttpPost]
    // [Route("/data")]
    // public IActionResult Data(IFormCollection form)
    // {
    //     ViewData["Username"] = HttpContext.Session.GetString("Username");

    //     if (ViewData["Username"] is null)
    //     {
    //         return RedirectToAction("Login");
    //     }

    //     using (var connection = new SqliteConnection("Data Source=db.db"))

    //     {
    //         String content = form["data"].ToString();


    //         String username = HttpContext.Session.GetString("Username");
            

    //         connection.Open();
    //         var command = connection.CreateCommand();
    //         command.CommandText = "INSERT INTO Data (Content, UserId) VALUES (@Content, (SELECT Id FROM Users WHERE Username = @Username));";
    //         command.Parameters.AddWithValue("@Username", username);
    //         command.Parameters.AddWithValue("@Content", content);
    //         command.ExecuteNonQuery();

    //         ViewData["Message"] = "Data added successfully";

    //     }

    //     return Data();
    // }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
