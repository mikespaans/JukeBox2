using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JukeBox
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string? genre)
        {
            using (var context = new DbdContextClass())
            {
                var genres = context.Genres.ToList();
                var songs = context.Songs.Where(s => s.Genre == genre).ToList();

                // Set the user in ViewData
                ViewData["Genres"] = genres;
                ViewData["Songs"] = songs;
                ViewData["user"] = HttpContext.Session.GetString("LoggedUser");

                return View();
            }
        }


        public IActionResult SongInformation(int? SelectedSongId)
        {
            var user = HttpContext.Session.GetString("LoggedUser");

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

            if (username == null || password == null)
            {
                return View();
            }

            using (var dbcontext = new DbdContextClass())
            {
                var user = dbcontext.Users.FirstOrDefault(u => u.Name == username);
                if (user != null && user.Password == password)
                {
                    LogginIn = true;
                    HttpContext.Session.SetString("LoggedUser", user.Name);
                }
            }

            ViewData["LogginIn"] = LogginIn;

            return RedirectToAction("Index");
        }

        public IActionResult Playlist(string? SongId, string? SongToAdd)
        {
            var user = HttpContext.Session.GetString("LoggedUser");

            using (var dbcontext = new DbdContextClass())
            {
                var Allsongs = dbcontext.Songs.ToList();
                var user1 = dbcontext.Users.FirstOrDefault(u => u.Name == user);
                var playlist = dbcontext.Users.FirstOrDefault(u => u.Name == user).PlayList;
                var songs = new List<Songs>();
                if (playlist != null)
                {
                    var playlistArray = playlist.Split(",");
                    foreach (var song in playlistArray)
                    {
                        var songId = Int32.Parse(song);
                        songs.Add(dbcontext.Songs.FirstOrDefault(s => s.Id == songId));
                    }
                }

                if (SongId != null)
                {
                    var songToDelete = songs.FirstOrDefault(s => s.Id.ToString() == SongId);
                    if (songToDelete != null)
                    {
                        songs.Remove(songToDelete);
                        user1.PlayList = string.Join(",", songs.Select(s => s.Id.ToString()));
                        dbcontext.SaveChanges();
                    }
                }

                if (SongToAdd != null)
                {
                    var songToAddId = Int32.Parse(SongToAdd);
                    var songToAdd = dbcontext.Songs.FirstOrDefault(s => s.Id == songToAddId);
                    if (songToAdd != null)
                    {
                        songs.Add(songToAdd);
                        user1.PlayList = string.Join(",", songs.Select(s => s.Id.ToString()));
                        dbcontext.SaveChanges();
                    }
                }

                ViewData["Songs"] = songs;
                ViewData["user"] = user;
                ViewData["Allsongs"] = Allsongs;
                return View();
            }
        }

        public IActionResult Register(string? username, string? password)
        {
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

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("LoggedUser");
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
