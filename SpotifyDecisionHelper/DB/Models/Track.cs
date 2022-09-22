#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace SpotifyDecisionHelper.DB.Models;

public class Track
{
    public string UserId { get; init; }
    public string TrackId { get; init; }

    public int Rating { get; set; }
        
    public string AlbumId { get; init; }
    public Album Album { get; set; }
}
