using System.ComponentModel.DataAnnotations;

namespace JukeBox;

public class Genres
{
    public string Name { get; set; }
    public string GenreDescription { get; set; }
}

public class Songs
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Artist { get; set; }
    public string Genre { get; set; }
    public string Album { get; set; }   
    public string Duration { get; set; }
}

public class Users
{

    public int Id { get; set; }
    public string Name { get; set; }

    public string Password { get; set; }
    public string PlayList { get; set; }
}