#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace SpotifyDecisionHelper.DB.Models;

public class Artist
{
    public string UserId { get; init; }
    public string ArtistId { get; init; }
    public int Rating { get; set; }

    public ICollection<Album>? Albums { get; set; }
}