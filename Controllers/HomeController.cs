using Microsoft.AspNetCore.Mvc;



namespace JukeBox;

public class HomeController : Controller
{

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // GET
    public IActionResult Index(string? genre, string? user)
    {
        Console.WriteLine("Fakka");
        // Console.WriteLine(genre);
        Console.WriteLine($"user : {user}");


        using (var context = new DbdContextClass())
        {
            var genres = context.Genres.ToList();
            var songs = context.Songs.Where(s => s.Genre == genre).ToList();



            ViewData["Genres"] = genres;
            ViewData["Songs"] = songs;
            ViewData["user"] = user;
            return View();
        }

    }

    public IActionResult SongInformation(int? SelectedSongId)
    {
        Console.WriteLine(SelectedSongId);
        using (var context = new DbdContextClass())
        {
            var song = context.Songs.FirstOrDefault(s => s.Id == SelectedSongId);
            ViewData["Song"] = song;
            return View();
        }
    }

    public IActionResult Login(string? username, string? password)
    {
        var LogginIn = false;

        Console.WriteLine(username);
        Console.WriteLine(password);
        if (username == null || password == null)
        {
            return View();
        }

        using (var dbcontext = new DbdContextClass())
        {
            var user = dbcontext.Users.FirstOrDefault(u => u.Name == username);
            if (user.Name == username && user.Password == password)
            {
                LogginIn = true;
                // HttpContext.Session.SetString("LoggedUser", user.Name);
                ViewData["user"] = user.Name;
                return RedirectToAction("Index", new { user = user.Name });
            }
            else
            {

                LogginIn = false;
            }

        }

        ViewData["LogginIn"] = LogginIn;


        return View();
    }

    public IActionResult Register(string? username, string? password)
    {


        Console.WriteLine(username);
        Console.WriteLine(password);
        if (username == null || password == null)
        {
            return View();
        }

        using (var dbcontext = new DbdContextClass())
        {
            foreach (var User in dbcontext.Users)
            {
                if (User.Name == username)
                {

                    return RedirectToAction("LoginFailed", new { Failed = "Username already exists" });
                }
            }

            {

            }

            dbcontext.Users.Add(new Users { Name = username, Password = password });

            dbcontext.SaveChanges();
        }





        return View();
    }

    public IActionResult LoginFailed(string? Failed)
    {
        ViewData["Failed"] = Failed;
        return View();
    }




    public IActionResult Privacy()
    {
        return View();
    }

    
}