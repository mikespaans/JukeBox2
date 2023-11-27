using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
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
            
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                password = Convert.ToBase64String(hashedBytes);
            }

            using (var dbcontext = new DbdContextClass())
            {
                var user = dbcontext.Users.FirstOrDefault(u => u.Name == username);
                if (user != null && user.Password == password)
                {
                    LogginIn = true;
                    HttpContext.Session.SetString("LoggedUser", user.Name);
                }

                else
                {
                    var Txt = "Username or password is incorrect";
                    ViewData["Txt"] = Txt;
                    return View();
                }
            }

            ViewData["LogginIn"] = LogginIn;

            return RedirectToAction("Index");
        }

        // public IActionResult Playlist(string? SongId, string? SongToAdd)
        // {
        //     var user = HttpContext.Session.GetString("LoggedUser");
        //
        //     using (var dbcontext = new DbdContextClass())
        //     {
        //         var Allsongs = dbcontext.Songs.ToList();
        //         var user1 = dbcontext.Users.FirstOrDefault(u => u.Name == user);
        //         var playlist = dbcontext.Users.FirstOrDefault(u => u.Name == user).PlayList;
        //         var songs = new List<Songs>();
        //         if (playlist != null)
        //         {
        //             var playlistArray = playlist.Split(",");
        //             foreach (var song in playlistArray)
        //             {
        //                 var songId = Int32.Parse(song);
        //                 songs.Add(dbcontext.Songs.FirstOrDefault(s => s.Id == songId));
        //             }
        //         }
        //
        //         if (SongId != null)
        //         {
        //             var songToDelete = songs.FirstOrDefault(s => s.Id.ToString() == SongId);
        //             if (songToDelete != null)
        //             {
        //                 songs.Remove(songToDelete);
        //                 user1.PlayList = string.Join(",", songs.Select(s => s.Id.ToString()));
        //                 dbcontext.SaveChanges();
        //             }
        //         }
        //
        //         if (SongToAdd != null)
        //         {
        //             var songToAddId = Int32.Parse(SongToAdd);
        //             var songToAdd = dbcontext.Songs.FirstOrDefault(s => s.Id == songToAddId);
        //             if (songToAdd != null)
        //             {
        //                 songs.Add(songToAdd);
        //                 user1.PlayList = string.Join(",", songs.Select(s => s.Id.ToString()));
        //                 dbcontext.SaveChanges();
        //             }
        //         }
        //
        //         ViewData["Songs"] = songs;
        //         ViewData["user"] = user;
        //         ViewData["Allsongs"] = Allsongs;
        //         return View();
        //     }
        // }

        public IActionResult Playlist(string? SongId, string? SongToAdd, bool? CreateNewPlaylist, string? PlaylistId, bool? DeleteSong, bool? DeletePlaylist)
        {
            var user = HttpContext.Session.GetString("LoggedUser");
            Console.WriteLine(CreateNewPlaylist);

            using (var dbcontext = new DbdContextClass())
            {
                var Allsongs = dbcontext.Songs.ToList();
                var user1 = dbcontext.Users.FirstOrDefault(u => u.Name == user);
                // var playlist = dbcontext.Playlists.FirstOrDefault(u => u.UserId == user1.Id);
                var playlists = dbcontext.Playlists.Where(u => u.UserId == user1.Id).ToList();
                var songs = new List<Songs>();
                var PlaylistName = "Select a Playlist";
                var PlayListId = 0;
                var PlaylistDuration = 0;
                
                if (PlaylistId != null)
                {
                    PlayListId = Int32.Parse(PlaylistId);
                    PlaylistDuration = dbcontext.Playlists.FirstOrDefault(p => p.Id == PlayListId).DurationInSeconds;
                }
                
                
                // delete a playlist
                if (DeletePlaylist == true && PlaylistId != null)
                {
                    var playlistId = Int32.Parse(PlaylistId);
                    var playlist = dbcontext.Playlists.FirstOrDefault(p => p.Id == playlistId);
                    if (playlist != null)
                    {
                        dbcontext.Playlists.Remove(playlist);
                        dbcontext.SaveChanges();
                    }
                    playlists = dbcontext.Playlists.Where(u => u.UserId == user1.Id).ToList();
                }
                
                //creates a new playlist
                if (CreateNewPlaylist == true)
                {
                    Console.WriteLine("Creating new playlist");
                    dbcontext.Playlists.Add(new Playlist {Name = "New Playlist", UserId = user1.Id});
                    dbcontext.SaveChanges();
                    playlists = dbcontext.Playlists.Where(u => u.UserId == user1.Id).ToList();
                }
                
                // add somg to playlist
                if (SongToAdd != null && PlaylistId != null)
                {
                    var songToAddId = Int32.Parse(SongToAdd);
                    var songToAdd = dbcontext.Songs.FirstOrDefault(s => s.Id == songToAddId);
                    var playlistId = Int32.Parse(PlaylistId);
                    var playlist = dbcontext.Playlists.FirstOrDefault(p => p.Id == playlistId);
                    
                    var SongDuration = songToAdd.Duration.Split(":");
                    
                    
                    
                    
                    
                    if (songToAdd != null && playlist != null)
                    {
                        if (int.TryParse(SongDuration[0], out int minutes) && int.TryParse(SongDuration[1], out int seconds))
                        {
                            playlist.DurationInSeconds += minutes * 60 + seconds;
                            dbcontext.SaveChanges();
                        }
                        
                        dbcontext.PlaylistSongs.Add(new PlaylistSong {PlaylistId = playlist.Id, SongId = songToAdd.Id});
                        dbcontext.SaveChanges();
                        PlaylistDuration = playlist.DurationInSeconds;
                    }
                }
                
                //delete song from playlist
                if (SongId != null && PlaylistId != null && DeleteSong == true)
                {
                    var songToDeleteId = Int32.Parse(SongId);
                    var songToDelete = dbcontext.Songs.FirstOrDefault(s => s.Id == songToDeleteId);
                    var playlistId = Int32.Parse(PlaylistId);
                    var playlist = dbcontext.Playlists.FirstOrDefault(p => p.Id == playlistId);
                    
                    var SongDuration = songToDelete.Duration.Split(":");
                    if (songToDelete != null && playlist != null)
                    {
                        var playlistSong = dbcontext.PlaylistSongs.FirstOrDefault(ps => ps.PlaylistId == playlist.Id && ps.SongId == songToDelete.Id);
                        if (playlistSong != null)
                        {
                            if (int.TryParse(SongDuration[0], out int minutes) && int.TryParse(SongDuration[1], out int seconds))
                            {
                                playlist.DurationInSeconds -= minutes * 60 + seconds;
                                dbcontext.SaveChanges();
                            }
                            
                            dbcontext.PlaylistSongs.Remove(playlistSong);
                            dbcontext.SaveChanges();
                            PlaylistDuration = playlist.DurationInSeconds;
                        }
                    }
                }

                if (PlaylistId != null)
                {
                    var playlistId = Int32.Parse(PlaylistId);
                    var playlist = dbcontext.Playlists.FirstOrDefault(p => p.Id == playlistId);
                    if (playlist != null)
                    {
                        PlaylistName = playlist.Name;
                    }
                    
                    // show songs in playlist
                    if (playlist != null)
                    {
                        var playlistSongs = dbcontext.PlaylistSongs.Where(ps => ps.PlaylistId == playlist.Id).ToList();
                        foreach (var playlistSong in playlistSongs)
                        {
                            songs.Add(dbcontext.Songs.FirstOrDefault(s => s.Id == playlistSong.SongId));
                        }
                    }
                }

               



                


                ViewData["Songs"] = songs;
                ViewData["user"] = user;
                ViewData["Allsongs"] = Allsongs;
                ViewData["Playlists"] = playlists;
                ViewData["PlaylistName"] = PlaylistName;
                ViewData["PlaylistId"] = PlaylistId;
                ViewData["PlaylistDuration"] = PlaylistDuration;
                
                
                return View();
            }
            
            
        }

        public IActionResult PlaylistRename(string? PlaylistId, string? newPlaylistName)
        {
            var user = HttpContext.Session.GetString("LoggedUser");
            using (var dbcontext = new DbdContextClass())
            {
                var user1 = dbcontext.Users.FirstOrDefault(u => u.Name == user);
                var playlists = dbcontext.Playlists.Where(u => u.UserId == user1.Id).ToList();
                
                if (newPlaylistName != null && PlaylistId != null)
                {
                    var playlistId = Int32.Parse(PlaylistId);
                    var playlist = dbcontext.Playlists.FirstOrDefault(p => p.Id == playlistId);
                    if (playlist != null)
                    {
                        playlist.Name = newPlaylistName;
                        dbcontext.SaveChanges();
                    }
                }
                ViewData["PlaylistId"] = PlaylistId;
                ViewData["Playlists"] = playlists;
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

                using (var sha256 = SHA256.Create())
                {
                    byte[] hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                    password = Convert.ToBase64String(hashedBytes);
                }
                
                dbcontext.Users.Add(new Users { Name = username, Password = password});
                dbcontext.SaveChanges();
            }

            return RedirectToAction("Index");
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
