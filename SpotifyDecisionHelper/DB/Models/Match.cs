#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace SpotifyDecisionHelper.DB.Models;

public class Match
{
    public string UserId { get; init; }
    public string TrackId1 { get; init; }
    public string TrackId2 { get; init; }
    
    public int Result { get; set; }
}