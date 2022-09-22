#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace SpotifyDecisionHelper.DB.Models;

public class Album
{
    public string UserId { get; init; }
    public string AlbumId { get; init; }
    public int Rating { get; set; }
        
    public string ArtistId { get; init; }
    public Artist Artist { get; set;}

    public ICollection<Track>? Tracks { get; set; }
}