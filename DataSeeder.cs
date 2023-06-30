namespace JukeBox;

internal class DataSeeder{
    
    public static void SeedData(IApplicationBuilder app)
    {
        using var dbContext = new DbdContextClass();
        
        if (!dbContext.Genres.Any())
        {
            dbContext.Genres.AddRange(
                new Genres { Name = "Pop", GenreDescription = "Pop genre description" },
                new Genres { Name = "Rock", GenreDescription = "Rock genre description" },
                new Genres { Name = "Rap", GenreDescription = "Hip hop genre description" },
                new Genres { Name = "Country", GenreDescription = "Country genre description" },
                new Genres { Name = "Electronic Dance Music", GenreDescription = "Electronic Dance Music genre description" }

                // Add more music genres here
            );

            dbContext.SaveChanges();
        }
        if (!dbContext.Songs.Any())
        {
            dbContext.Songs.AddRange(
                // Pop songs
    new Songs { Name = "Shape of You", Artist = "Ed Sheeran", Genre = "Pop", Album = "÷", Duration = "3:53" },
                new Songs { Name = "Bad Guy", Artist = "Billie Eilish", Genre = "Pop", Album = "When We All Fall Asleep, Where Do We Go?", Duration = "3:14" },
                new Songs { Name = "Blinding Lights", Artist = "The Weeknd", Genre = "Pop", Album = "After Hours", Duration = "3:20" },
                new Songs { Name = "Dance Monkey", Artist = "Tones and I", Genre = "Pop", Album = "The Kids Are Coming", Duration = "3:29" },
                new Songs { Name = "Love Yourself", Artist = "Justin Bieber", Genre = "Pop", Album = "Purpose", Duration = "3:53" },

                // Rock songs
                new Songs { Name = "Sweet Child o' Mine", Artist = "Guns N' Roses", Genre = "Rock", Album = "Appetite for Destruction", Duration = "5:56" },
                new Songs { Name = "Hotel California", Artist = "Eagles", Genre = "Rock", Album = "Hotel California", Duration = "6:30" },
                new Songs { Name = "Bohemian Rhapsody", Artist = "Queen", Genre = "Rock", Album = "A Night at the Opera", Duration = "5:55" },
                new Songs { Name = "Smells Like Teen Spirit", Artist = "Nirvana", Genre = "Rock", Album = "Nevermind", Duration = "5:01" },
                new Songs { Name = "Stairway to Heaven", Artist = "Led Zeppelin", Genre = "Rock", Album = "Led Zeppelin IV", Duration = "8:02" },

                // Rap songs
                new Songs { Name = "Lose Yourself", Artist = "Eminem", Genre = "Rap", Album = "8 Mile Soundtrack", Duration = "5:20" },
                new Songs { Name = "Sicko Mode", Artist = "Travis Scott", Genre = "Rap", Album = "Astroworld", Duration = "5:12" },
                new Songs { Name = "God's Plan", Artist = "Drake", Genre = "Rap", Album = "Scorpion", Duration = "3:18" },
                new Songs { Name = "HUMBLE.", Artist = "Kendrick Lamar", Genre = "Rap", Album = "DAMN.", Duration = "2:57" },
                new Songs { Name = "Old Town Road", Artist = "Lil Nas X ft. Billy Ray Cyrus", Genre = "Rap", Album = "7", Duration = "2:37" },

                // Electronic Dance Music songs
                new Songs { Name = "Wake Me Up", Artist = "Avicii", Genre = "Electronic Dance Music", Album = "True", Duration = "4:09" },
                new Songs { Name = "Don't You Worry Child", Artist = "Swedish House Mafia ft. John Martin", Genre = "Electronic Dance Music", Album = "Until Now", Duration = "3:32" },
                new Songs { Name = "Clarity", Artist = "Zedd ft. Foxes", Genre = "Electronic Dance Music", Album = "Clarity", Duration = "4:31" },
                new Songs { Name = "Animals", Artist = "Martin Garrix", Genre = "Electronic Dance Music", Album = "Gold Skies", Duration = "5:03" },
                new Songs { Name = "Lean On", Artist = "Major Lazer & DJ Snake ft. MØ", Genre = "Electronic Dance Music", Album = "Peace Is the Mission", Duration = "2:56" },

                // Country songs
                new Songs { Name = "Wagon Wheel", Artist = "Old Crow Medicine Show", Genre = "Country", Album = "O.C.M.S.", Duration = "4:58" },
                new Songs { Name = "Cruise", Artist = "Florida Georgia Line", Genre = "Country", Album = "Here's to the Good Times", Duration = "3:29" },
                new Songs { Name = "The House That Built Me", Artist = "Miranda Lambert", Genre = "Country", Album = "Revolution", Duration = "3:57" },
                new Songs { Name = "Chicken Fried", Artist = "Zac Brown Band", Genre = "Country", Album = "The Foundation", Duration = "3:58" },
                new Songs { Name = "Before He Cheats", Artist = "Carrie Underwood", Genre = "Country", Album = "Some Hearts", Duration = "3:20" }
            );

            dbContext.SaveChanges();
        }
    }
}