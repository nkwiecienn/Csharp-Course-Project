using Microsoft.Data.Sqlite;

InitialTables.Init();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=login}/{id?}");
app.UseStatusCodePagesWithReExecute("/");

app.Use(async (ctx, next) =>
{
    await next();

    if ((ctx.Response.StatusCode == 404 || ctx.Response.StatusCode == 400) && !ctx.Response.HasStarted)
    {
        string originalPath = ctx.Request.Path.Value ?? "";
        ctx.Items["originalPath"] = originalPath;
        ctx.Request.Path = "/login/";
        ctx.Response.Redirect("/login/");
        await next();
    }
});

app.Run();

// var connectionStringBuilder = new SqliteConnectionStringBuilder();
// connectionStringBuilder.DataSource = "db.db";

// using var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
// connection.Open();
// var Command = connection.CreateCommand();

// string createTable = "CREATE TABLE IF NOT EXISTS Users (id INTEGER PRIMARY KEY AUTOINCREMENT, Username TEXT NOT NULL, Password TEXT NOT NULL);";
// Command.CommandText = createTable;
// Command.ExecuteNonQuery();

// string dataTable = "CREATE TABLE IF NOT EXISTS Data (id INTEGER PRIMARY KEY AUTOINCREMENT, Content TEXT NOT NULL, UserId INTEGER NOT NULL REFERENCES Users(id));";

// Command.CommandText = dataTable;
// Command.ExecuteNonQuery();

// connection.Close();


