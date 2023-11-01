using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    
}

public class Playlist
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    public int DurationInSeconds { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }

    public List<PlaylistSong> PlaylistSongs { get; set; }
}


public class PlaylistSong
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Playlist")]
    public int PlaylistId { get; set; }
    public Playlist Playlist { get; set; }

    [ForeignKey("Song")]
    public int SongId { get; set; }
    public Songs Song { get; set; }
}


